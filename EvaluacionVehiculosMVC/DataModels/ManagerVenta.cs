﻿using EvaluacionVehiculosMVC.WSEvaluacionInacap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EvaluacionVehiculosMVC.DataModels
{
    public class ManagerVenta
    {
        WSEvaluacionInacapSoapClient ws = new WSEvaluacionInacapSoapClient();

        public List<Models.VehiculosDuenos> LlenaComboVehiculos(int idDueno)
        {
            List<Models.VehiculosDuenos> listaVehiculosD = new List<Models.VehiculosDuenos>();

            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var queryVehiculo = (from oVehiculo in db.Vehiculo
                                         join oDueno in db.Dueno
                                         on oVehiculo.IdDueno equals oDueno.IdDueno
                                         where idDueno == oVehiculo.IdDueno
                                         select new Models.VehiculosDuenos()
                                         {
                                             IdVehiculo = oVehiculo.IdVehiculo,
                                             Patente = oVehiculo.Patente,
                                             Marca = oVehiculo.Marca,
                                             Modelo = oVehiculo.Modelo,
                                         }).ToList();
                    listaVehiculosD = queryVehiculo;
                }
                return listaVehiculosD;
            }
            catch (Exception)
            {
                return new List<Models.VehiculosDuenos>();
            }

        }

        public Models.VehiculosDuenos BuscarVehiculoVenta(int id)
        {
            Models.VehiculosDuenos vehiculo = new Models.VehiculosDuenos();
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var queryvehiculo = (from oVehiculo in db.Vehiculo
                                         join oDueno in db.Dueno
                                         on oVehiculo.IdDueno equals oDueno.IdDueno
                                         where oVehiculo.IdVehiculo == id
                                         select new Models.VehiculosDuenos()
                                         {
                                             IdVehiculo = oVehiculo.IdVehiculo,
                                             IdDueno = oDueno.IdDueno,
                                             Patente = oVehiculo.Patente,
                                             Marca = oVehiculo.Marca,
                                             Modelo = oVehiculo.Modelo,
                                             Anno = oVehiculo.Anno,
                                             PrecioEnUF = oVehiculo.PrecioEnUF
                                         }).FirstOrDefault();

                    vehiculo = queryvehiculo;
                }
                return vehiculo;
            }
            catch (Exception)
            {
                return new Models.VehiculosDuenos();
            }
        }

        public DataModels.Ventas CrearVenta(int idDueno, int idVehiculo)
        {
            DataModels.Ventas venta = new DataModels.Ventas();
            venta.Patente = ObtenerPatente(idVehiculo);
            venta.FechaVenta = DateTime.Now;
            venta.TotalBruto = ValorBruto(idVehiculo);
            venta.IVA = ValorIva(venta);
            venta.TotalVenta = venta.IVA + venta.TotalBruto;
            venta.RUTDueno = ObtenerRutDueno(idDueno);
            return venta;
        }

        public string InsertarVenta(DataModels.Ventas venta, string userCode)
        {
            string logStatus = string.Empty;
            string logMessage = string.Empty;
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    if (venta != null)
                    {

                        db.Ventas.Add(venta);
                        db.SaveChanges();
                        var logged = ws.LoggedPurchase(userCode, "Venta Patente: " + venta.Patente.ToString() + "\n Rut Dueño: " + venta.RUTDueno.ToString() + "\n Total Venta: " + venta.TotalVenta.ToString());
                        logStatus = logged.LogStatus;
                        logMessage = logged.LogMessage;
                        EliminarVehiculoDueno(venta);
                    }
                }
                return (logStatus + " " + logMessage);
            }
            catch (Exception)
            {

                return string.Empty;
            }

        }

        private void EliminarVehiculoDueno(DataModels.Ventas venta)
        {

            DataModels.Ventas ventas = venta;
            DataModels.Vehiculo vehiculo = new Vehiculo();
            using (var db = new BDEvaluacionVehiculosEntities())
            {
                var query = (from oVehiculo in db.Vehiculo
                             join oVenta in db.Ventas
                             on oVehiculo.Patente equals oVenta.Patente
                             where oVehiculo.IdDueno != 0
                             select oVehiculo).Single();
                vehiculo = query;
                vehiculo.IdDueno = 0;
                db.Vehiculo.Attach(vehiculo);
                db.Entry(vehiculo).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }

        private string ObtenerRutDueno(int idDueno)
        {
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var query = (from oDueno in db.Dueno
                                 where oDueno.IdDueno == idDueno
                                 select oDueno).Single();
                    string RUT = query.RUT;
                    return RUT;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string ObtenerPatente(int idVehiculo)
        {
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var query = (from oVehiculo in db.Vehiculo
                                 where oVehiculo.IdVehiculo == idVehiculo
                                 select oVehiculo).Single();
                    string patente = query.Patente;
                    return patente;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal ValorBruto(int idVehiculo)
        {
            try
            {
                DataModels.Vehiculo vehiculo = new Vehiculo();
                using (var db = new BDEvaluacionVehiculosEntities())
                {

                    var queryVehiculo = db.Vehiculo.Single(uf => uf.IdVehiculo == idVehiculo);
                    vehiculo = queryVehiculo;

                    DateTime fecha = DateTime.Today;
                    string fecha_consulta = fecha.ToString("yyyy-MM-dd");
                    var resultUF = ws.GetUFValue(fecha_consulta);
                    decimal ValorUF = decimal.Parse(resultUF.UFValues, CultureInfo.InvariantCulture);
                    decimal ValorBruto = (decimal)vehiculo.PrecioEnUF * ValorUF;
                    return ValorBruto;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal ValorIva(DataModels.Ventas venta)
        {
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    decimal IVA = 0.19m;
                    decimal ValorBruto = (decimal)venta.TotalBruto;
                    return ValorBruto * IVA;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}