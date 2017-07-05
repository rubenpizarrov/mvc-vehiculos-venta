using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EvaluacionVehiculosMVC.DataModels
{
    public class ManagerVehiculos
    {
        public List<Models.VehiculosDuenos> ListarVehiculos()
        {
            List<Models.VehiculosDuenos> listaVehiculos = new List<Models.VehiculosDuenos>();

            using (var db = new DataModels.BDEvaluacionVehiculosEntities())
            {
                var listaVehiculosDuenosQuery = (from oDueno in db.Dueno
                                                 join oVehiculo in db.Vehiculo
                                                 on oDueno.IdDueno equals oVehiculo.IdDueno
                                                 orderby oDueno.IdDueno
                                                 select new Models.VehiculosDuenos()
                                                   {
                                                       IdVehiculo = oVehiculo.IdVehiculo,
                                                       Nombre = oDueno.Nombre,
                                                       Apellido = oDueno.Apellido,
                                                       Patente = oVehiculo.Patente,
                                                       Marca = oVehiculo.Marca,
                                                       Modelo = oVehiculo.Modelo,
                                                       Anno = oVehiculo.Anno,
                                                       PrecioEnUF = oVehiculo.PrecioEnUF
                                                   }).ToList();
                listaVehiculos = listaVehiculosDuenosQuery;
            }
            return listaVehiculos;
        }

        public List<Models.VehiculosDuenos> ListarVehiculosDuenos(int id)
        {
            List<Models.VehiculosDuenos> listaVehiculosDueno = new List<Models.VehiculosDuenos>();

            using (var db = new DataModels.BDEvaluacionVehiculosEntities())
            {
                var listaVehiculosDuenosQuery = (from oDueno in db.Dueno
                                                 join oVehiculo in db.Vehiculo
                                                 on oDueno.IdDueno equals oVehiculo.IdDueno
                                                 where id == oVehiculo.IdDueno
                                                 orderby oVehiculo.IdVehiculo
                                                 select new Models.VehiculosDuenos()
                                                 {
                                                     IdVehiculo = oVehiculo.IdVehiculo,
                                                     IdDueno = oDueno.IdDueno,
                                                     Nombre = oDueno.Nombre,
                                                     Apellido = oDueno.Apellido,
                                                     Patente = oVehiculo.Patente,
                                                     Marca = oVehiculo.Marca,
                                                     Modelo = oVehiculo.Modelo,
                                                     Anno = oVehiculo.Anno,
                                                     PrecioEnUF = oVehiculo.PrecioEnUF
                                                 }).ToList();
                listaVehiculosDueno = listaVehiculosDuenosQuery;
            }
            return listaVehiculosDueno;
        }
        public bool CrearVehiculo(DataModels.Vehiculo vehiculo)
        {
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    vehiculo.Patente = FormatoPatente(vehiculo.Patente);
                    if (vehiculo.IdDueno != 0)
                    {
                        db.Vehiculo.Add(vehiculo);
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool PatenteExiste(string patente) 
        {
            bool patenteExiste = false;
            string newpatente = string.Empty;

            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    patente = FormatoPatente(patente);
                    patenteExiste = db.Vehiculo.Any(p => p.Patente == patente);
                    if (patenteExiste)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Models.VehiculosDuenos> LlenarComboDuenos()
        {
            try
            {
                List<Models.VehiculosDuenos> listaNombresDuenos = new List<Models.VehiculosDuenos>();
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var nombresDuenos = (from oDueno in db.Dueno
                                         select new Models.VehiculosDuenos()
                                          {
                                              IdDueno = oDueno.IdDueno,
                                              Nombre = oDueno.Nombre,
                                              Apellido = oDueno.Apellido
                                          }).Distinct().ToList();
                    listaNombresDuenos = nombresDuenos;
                }
                return listaNombresDuenos;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public DataModels.Vehiculo BuscarVehiculo(int idVehiculo)
        {
            DataModels.Vehiculo vehiculo = new DataModels.Vehiculo();
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    vehiculo = db.Vehiculo.SingleOrDefault(v => v.IdVehiculo == idVehiculo);
                }
                return vehiculo;
            }
            catch (Exception)
            {
                return new DataModels.Vehiculo();
            }
        }

        
        public Models.VehiculosDuenos BuscarVehiculoVista(string patente)
        {
            Models.VehiculosDuenos vehiculo = new Models.VehiculosDuenos();

            try
            {
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    var query = (from oVehiculo in db.Vehiculo
                                 join oDueno in db.Dueno
                                 on oVehiculo.IdDueno equals oDueno.IdDueno
                                 where oVehiculo.Patente == patente
                                 select new Models.VehiculosDuenos()
                                 {
                                     IdVehiculo = oVehiculo.IdVehiculo,
                                     IdDueno = oDueno.IdDueno,
                                     Nombre = oDueno.Nombre,
                                     Apellido = oDueno.Apellido,
                                     Patente = oVehiculo.Patente,
                                     Marca = oVehiculo.Marca,
                                     Modelo = oVehiculo.Modelo,
                                     Anno = oVehiculo.Anno,
                                     PrecioEnUF = oVehiculo.PrecioEnUF
                                 }).First();
                    vehiculo = query;
                }

                return vehiculo;
            }
            catch (Exception)
            {
                return new Models.VehiculosDuenos();
            }

        }

        public Models.VehiculosDuenos NombreDueno(int idVehiculo, int idDueno)
        {
            Models.VehiculosDuenos vehiculoDueno = new Models.VehiculosDuenos();
            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    var nombreDueno = (from oVehiculo in db.Vehiculo
                                       join oDueno in db.Dueno
                                       on oVehiculo.IdDueno equals oDueno.IdDueno
                                       where oVehiculo.IdDueno == idDueno && oVehiculo.IdVehiculo == idVehiculo
                                       select new Models.VehiculosDuenos()
                                        {
                                            IdVehiculo = oVehiculo.IdVehiculo,
                                            IdDueno = oDueno.IdDueno,
                                            Nombre = oDueno.Nombre,
                                            Apellido = oDueno.Apellido,
                                            Patente = oVehiculo.Patente,
                                            Marca = oVehiculo.Marca,
                                            Modelo = oVehiculo.Modelo,
                                            Anno = oVehiculo.Anno,
                                            PrecioEnUF = oVehiculo.PrecioEnUF
                                        }).FirstOrDefault();
                    vehiculoDueno = nombreDueno;
                }
                return vehiculoDueno;
            }
            catch (Exception)
            {
                return new Models.VehiculosDuenos();
            }
        }


        public bool ActualizarVehiculo(DataModels.Vehiculo vehiculo)
        {

            try
            {
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    
                    if (vehiculo.Patente != null && vehiculo.IdDueno != 0)
                    {
                        db.Vehiculo.Attach(vehiculo);
                        db.Entry(vehiculo).State = System.Data.EntityState.Modified;
                        db.SaveChanges();

                        return true;
                    }
                    
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool EliminarVehiculo(int id) 
        {
            try
            {
                DataModels.Vehiculo vehiculo = new Vehiculo();
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    vehiculo = db.Vehiculo.Single(v => v.IdVehiculo == id);

                    db.Vehiculo.Attach(vehiculo);
                    db.Entry(vehiculo).State = System.Data.EntityState.Deleted;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string FormatoPatente(string patente)
        {
            string nuevaPatente = string.Empty;
            nuevaPatente = patente.Replace("-", "");
            for (int i = 2; i < 8; i = i + 3)
            {
                nuevaPatente = nuevaPatente.Insert(i, "-");
            }
            Regex expresion = new Regex(@"^([a-zA-Z]{2})-?([a-zA-Z]{2})-?(\d{2})$");
            if (expresion.IsMatch(nuevaPatente))
            {
                return nuevaPatente.ToUpper();
            }
            else
            {
                return string.Empty;
            }
        }

    }


}