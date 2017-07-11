using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluacionVehiculosMVC.Controllers
{
    public class DuenoController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["MensajeBusquedaError"] != null)
            {
                ViewBag.MensajeBusquedaError = TempData["MensajeBusquedaError"].ToString();
            }
            else
            {
                ViewBag.MensajeBusquedaError = string.Empty;
            }
            
            DataModels.ManagerDuenos managerDuenos = new DataModels.ManagerDuenos();
            List<DataModels.Dueno> listaDuenos = managerDuenos.ListarDueno();
            return View(listaDuenos);
        }

        public ActionResult ListaDueno(int id)
        {
            DataModels.ManagerDuenos managerVehiculosDuenos = new DataModels.ManagerDuenos();
            List<Models.VehiculosDuenos> listaVehiculosDuenos = managerVehiculosDuenos.ListarVehiculosDueno(id);
            return View(listaVehiculosDuenos);
        }

        [HttpPost]
        public ActionResult BuscarDueno(string rut)
        {
            DataModels.ManagerDuenos manager = new DataModels.ManagerDuenos();
            DataModels.Dueno dueno = manager.BuscarDuenoVista(rut);
            if (dueno.IdDueno != 0)
            {
                return View(dueno);
            }
            else
            {
                TempData["MensajeBusquedaError"] = "No se encontró el Dueño en la base de datos";
                return RedirectToAction("Index");
            }


        }

        public ActionResult Details(int id)
        {
            DataModels.ManagerDuenos mangerDuenos = new DataModels.ManagerDuenos();
            DataModels.Dueno dueno = mangerDuenos.BuscarDueno(id);
            return View(dueno);
        }

        public ActionResult Create()
        {
            DataModels.Dueno dueno = new DataModels.Dueno();
            dueno.FechaRegistro = DateTime.Now;
            return View(dueno);
        }

        [HttpPost]
        public ActionResult Create(DataModels.Dueno model)
        {
            try
            {

                DataModels.ManagerDuenos managerDueno = new DataModels.ManagerDuenos();
                bool rutExiste = managerDueno.RutExiste(model.RUT);
                if (rutExiste)
                {
                    ModelState.AddModelError(string.Empty, "El RUT que está ingresando ya existe");
                }
                if (ModelState.IsValid)
                {
                    bool result = managerDueno.CreateDueno(model);
                    if (result)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }
                }

            }
            catch
            {
                return View(model);
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            DataModels.ManagerDuenos mangerDuenos = new DataModels.ManagerDuenos();
            DataModels.Dueno dueno = mangerDuenos.BuscarDueno(id);
            return View(dueno);
        }

        [HttpPost]
        public ActionResult Edit(int id, DataModels.Dueno model)
        {
            try
            {
                DataModels.ManagerDuenos managerDuenos = new DataModels.ManagerDuenos();
                bool result = managerDuenos.ActualizarDueno(model);
                if (!result)
                {
                    ModelState.AddModelError("", "El RUT ingresado no es válido");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            DataModels.ManagerDuenos managerDuenos = new DataModels.ManagerDuenos();
            return View(managerDuenos.BuscarDueno(id));
        }

        [HttpPost]
        public ActionResult Delete(int id, DataModels.Dueno model)
        {
            bool tieneVehiculos = false;
            try
            {
                DataModels.ManagerDuenos managerDuenos = new DataModels.ManagerDuenos();
                tieneVehiculos = managerDuenos.DuenoConVehiculos(id);
                if (tieneVehiculos)
                {
                    ModelState.AddModelError(string.Empty, "Este Usuario tiene vehículos asignados y no puede eliminarse Presione en Regresar");
                    return View();
                }


                bool result = managerDuenos.EliminarDueno(id);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }


            }
            catch
            {
                return View(model);
            }

        }
    }
}
