using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluacionVehiculosMVC.Controllers
{
    public class VehiculoController : Controller
    {

        public ActionResult Index()
        {
            if (TempData["MensajeErrorVehiculo"] != null)
            {
                ViewBag.MensajeBusquedaError = TempData["MensajeErrorVehiculo"] = "Vehículo no encontrado";
            }
            else
            {
                ViewBag.MensajeBusquedaError = "";
            }
            DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
            List<Models.VehiculosDuenos> listaVehiculos = managerVehiculo.ListarVehiculos();
            return View(listaVehiculos);
        }

        public ActionResult ListaDueno(int id)
        {
            DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
            List<Models.VehiculosDuenos> listaVehiculosDueno = managerVehiculo.ListarVehiculosDuenos(id);
            return View(listaVehiculosDueno);
        }
        /// <summary>
        /// Busca un Vehiculo Para la Vista
        /// </summary>
        /// <param name=string "patente"></param>
        /// <returns>VehiculoDueno Model</returns>
        public ActionResult BuscarVehiculo(string patente)
        {
            DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
            Models.VehiculosDuenos vehiculoDueno = managerVehiculo.BuscarVehiculoVista(patente);
            if (vehiculoDueno.IdVehiculo != 0)
            {
                return View(vehiculoDueno);
            }
            else
            {
                TempData["MensajeErrorVehiculo"] = "Vehículo no encontrado";
                return RedirectToAction("Index");
            }

        }

        public ActionResult NoEncontrado()
        {
            return View();
        }

        public ActionResult DetallesVehiculo(int id)
        {
            DataModels.ManagerVehiculos managerVehiculos = new DataModels.ManagerVehiculos();
            DataModels.Vehiculo vehiculo = managerVehiculos.BuscarVehiculo(id);
            Models.VehiculosDuenos detallesVehiculosDueno = managerVehiculos.NombreDueno(vehiculo.IdVehiculo, vehiculo.IdDueno);
            return View(detallesVehiculosDueno);
        }

        public ActionResult CrearVehiculo()
        {
            DataModels.Vehiculo vehiculo = new DataModels.Vehiculo();
            return View();
        }

        public ActionResult _ComboDuenosPartial()
        {
            DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
            List<Models.VehiculosDuenos> listaNombresDuenos = managerVehiculo.LlenarComboDuenos();
            return PartialView(listaNombresDuenos);
        }


        [HttpPost]
        public ActionResult CrearVehiculo(DataModels.Vehiculo model)
        {
            try
            {
                if (model.IdDueno == 0)
                {
                    ModelState.AddModelError(string.Empty, "Aún no hay Dueños Disponibles o no ha Elegido un Dueño para Asignar");
                }

                DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
                bool patenteExistente = managerVehiculo.PatenteExiste(model.Patente);
                if (patenteExistente)
                {
                    ModelState.AddModelError(string.Empty, "La patente que está ingresando ya existe");
                }
                if (ModelState.IsValid)
                {
                    bool result = managerVehiculo.CrearVehiculo(model);
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


        public ActionResult EditarVehiculo(int id)
        {
            DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
            DataModels.Vehiculo vehiculo = managerVehiculo.BuscarVehiculo(id);
            return View(vehiculo);
        }

        [HttpPost]
        public ActionResult EditarVehiculo(int id, DataModels.Vehiculo model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
                    managerVehiculo.ActualizarVehiculo(model);
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                throw;
            }
            return View(model);
        }


        public ActionResult EliminarVehiculo(int id)
        {
            DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
            return View(managerVehiculo.BuscarVehiculo(id));
        }


        [HttpPost]
        public ActionResult EliminarVehiculo(int id, DataModels.Vehiculo model)
        {
            try
            {
                DataModels.ManagerVehiculos managerVehiculo = new DataModels.ManagerVehiculos();
                bool result = managerVehiculo.EliminarVehiculo(id);
                return RedirectToAction("Index", "Dueno");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
