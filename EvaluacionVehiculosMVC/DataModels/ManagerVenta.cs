using EvaluacionVehiculosMVC.WSEvaluacionInacap;
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

        public DataModels.Ventas CrearVistaVenta(int idDueno, int idVehiculo)
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
                    if (venta != null && !string.IsNullOrEmpty(userCode))
                    {

                        db.Ventas.Add(venta);
                        db.SaveChanges();
                        string service = "Venta Patente: " + venta.Patente.ToString() + " Rut Dueño: " + venta.RUTDueno.ToString() + " Total Venta: " + string.Format("{0:C}", venta.TotalVenta);
                        bool seEliminoVehiculo = EliminarVehiculoDueno(venta);
                        if (seEliminoVehiculo)
                        {
                            var logged = ws.LoggedPurchase(userCode, service);
                            logStatus = logged.LogStatus;
                            logMessage = logged.LogMessage;
                        }
                        else
                        {
                            return string.Empty;
                        }


                    }
                }
                return (logStatus + " " + logMessage);
            }
            catch (Exception)
            {

                return string.Empty;
            }

        }

        public bool EliminarVehiculoDueno(DataModels.Ventas venta)
        {
            DataModels.Vehiculo vehiculo = new Vehiculo();
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var query = (from oVehiculo in db.Vehiculo
                                 join oVenta in db.Ventas
                                 on oVehiculo.Patente equals oVenta.Patente
                                 where oVehiculo.Patente == venta.Patente
                                 select oVehiculo).FirstOrDefault();
                    vehiculo = query;
                    db.Vehiculo.Attach(vehiculo);
                    db.Entry(vehiculo).State = System.Data.EntityState.Deleted;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        #region Metodos Privados
        private string ObtenerRutDueno(int idDueno)
        {
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var query = (from oDueno in db.Dueno
                                 where oDueno.IdDueno == idDueno
                                 select oDueno).FirstOrDefault();
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
                                 select oVehiculo).FirstOrDefault();
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
                    if (resultUF.IsValudOrMessage.Contains("[GET_OK]") && resultUF.UFValues != null)
                    {
                        decimal ValorUF = decimal.Parse(resultUF.UFValues, CultureInfo.InvariantCulture);
                        decimal ValorBruto = (decimal)vehiculo.PrecioEnUF * ValorUF;
                        return ValorBruto; 
                    }
                    else
                    {
                        return 0;
                    }
                    
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
        #endregion
    }
}