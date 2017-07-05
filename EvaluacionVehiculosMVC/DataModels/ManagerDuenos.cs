using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EvaluacionVehiculosMVC.DataModels
{
    public class ManagerDuenos
    {
        public bool CreateDueno(DataModels.Dueno dueno)
        {
            try
            {
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    dueno.RUT = FormatoRut(dueno.RUT);
                    
                    if (!string.IsNullOrEmpty(dueno.RUT))
                    {
                        db.Dueno.Add(dueno);
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

        public bool ActualizarDueno(DataModels.Dueno dueno)
        {
            try
            {
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    db.Dueno.Attach(dueno);
                    db.Entry(dueno).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool RutExiste(string rut) 
        {
            bool rutExiste = false;
            string newpatente = string.Empty;

            try
            {
                using (var db = new BDEvaluacionVehiculosEntities())
                {
                    rut = FormatoRut(rut);
                    rutExiste = db.Dueno.Any(r => r.RUT == rut);
                    if (rutExiste)
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

        public DataModels.Dueno BuscarDueno(int idDueno)
        {
            DataModels.Dueno dueno = new DataModels.Dueno();

            try
            {
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    dueno = db.Dueno.Single(d => d.IdDueno == idDueno);
                }
            }
            catch (Exception)
            {
                return new DataModels.Dueno();
            }
            return dueno;
        }

        public DataModels.Dueno BuscarDuenoVista(string rut)
        {
            DataModels.Dueno dueno = new DataModels.Dueno();

            try
            {
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    dueno = db.Dueno.Single(d => d.RUT == rut);
                }

                return dueno;
            }
            catch (Exception)
            {
                return new DataModels.Dueno();
            }
         
        }

        public bool EliminarDueno(int id)
        {
            try
            {
                DataModels.Dueno dueno = new DataModels.Dueno();
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                   
                    dueno = db.Dueno.Single(d => d.IdDueno == id);
                    if (dueno.IdDueno != 0)
                    {
                        db.Dueno.Attach(dueno);
                        db.Entry(dueno).State = System.Data.EntityState.Deleted;
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool DuenoConVehiculos(int id) 
        {
            int tieneVehiculos = 0;
            try
            {
                DataModels.Dueno dueno = new DataModels.Dueno();
                using (var db = new DataModels.BDEvaluacionVehiculosEntities())
                {
                    dueno = db.Dueno.Single(d => d.IdDueno == id);
                    tieneVehiculos = dueno.Vehiculo.Count();
                    if (tieneVehiculos > 0)
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

        public List<DataModels.Dueno> ListarDueno()
        {
            List<DataModels.Dueno> listaDuenos = new List<DataModels.Dueno>();

            using (var db = new DataModels.BDEvaluacionVehiculosEntities())
            {
                listaDuenos = db.Dueno.ToList();
            }
            return listaDuenos;

        }

        public List<Models.VehiculosDuenos> ListarVehiculosDueno(int id)
        {
            List<Models.VehiculosDuenos> listaVehiculosDueno = new List<Models.VehiculosDuenos>();

            using (var db = new DataModels.BDEvaluacionVehiculosEntities())
            {
                var listaVehiculosDuenosQuery = (from oDueno in db.Dueno
                                                 join oVehiculo in db.Vehiculo
                                                 on oDueno.IdDueno equals oVehiculo.IdDueno
                                                 where id == oVehiculo.IdDueno
                                                 orderby oDueno.IdDueno
                                                 select new Models.VehiculosDuenos()
                                                 {
                                                     IdDueno = oDueno.IdDueno,
                                                     Nombre = oDueno.Nombre,
                                                     Patente = oVehiculo.Patente,
                                                     Marca = oVehiculo.Marca,
                                                     Modelo = oVehiculo.Modelo

                                                 }).ToList();
                listaVehiculosDueno = listaVehiculosDuenosQuery;
            }
            return listaVehiculosDueno;

        }

        private string FormatoRut(string rut)
        {
            rut = rut.Replace(".", "");
            rut = rut.Replace("-", "");
            string patron = "^([1-9]{1,2})([0-9]{3})([0-9]{3})([Kk0-9]{1})$";
            Regex expresion = new Regex(patron);
            if (rut.Length > 9)
            {
                char[] charsrut = rut.ToCharArray(0, 9);
                rut = new string(charsrut);
            }
            if (expresion.IsMatch(rut))
            {
                return rut.ToUpper();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}