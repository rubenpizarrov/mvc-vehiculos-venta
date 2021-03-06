﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluacionVehiculosMVC.Controllers
{
    public class VentaController : Controller
    {
       
        public ActionResult Index()
        {
            if (TempData["MensajeVentaError"] != null)
            {
                ViewBag.MensajeVentaError = TempData["MensajeVentaError"].ToString();
            }
            else
            {
                ViewBag.MensajeVentaError = string.Empty;
            }
            
            return View();
        }
        public PartialViewResult BuscaDuenoVehiculo(string rut)
        {
            DataModels.ManagerDuenos manD = new DataModels.ManagerDuenos();
            DataModels.Dueno dueno = manD.BuscarDuenoVista(rut);
            if (!string.IsNullOrEmpty(dueno.RUT) && ModelState.IsValid)
            {
                return PartialView("_TablaDueno", dueno);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El rut ingresado no existe");
                return PartialView("_AjaxError", dueno);
            }
            
        }

        
        public PartialViewResult ComboVehiculos(int id) 
        {
            DataModels.ManagerVenta manVenta = new DataModels.ManagerVenta();
            List<Models.VehiculosDuenos> listaVehiculos = manVenta.LlenaComboVehiculos(id);
            ViewBag.cboVehiculos = new SelectList(listaVehiculos, "IdVehiculo", "Marca");
            return PartialView("_ComboVehiculos", listaVehiculos);
        }
       
        public PartialViewResult DetallesVehiculo(int id)
        {
            DataModels.ManagerVenta manVenta = new DataModels.ManagerVenta();
            Models.VehiculosDuenos vehiculo = manVenta.BuscarVehiculoVenta(id);
            return PartialView("_TablaVehiculo", vehiculo);
        }
       
        public ActionResult DetalleVenta(Models.VehiculosDuenos model)
        {
            DataModels.ManagerVenta man = new DataModels.ManagerVenta();
            DataModels.Ventas venta = man.CrearVistaVenta(model.IdDueno, model.IdVehiculo);
            if (venta.TotalVenta != 0)
            {
                return View(venta);
            }
            else
            {
                TempData["MensajeVentaError"] = "Hay Problemas con el servicio inténtelo más tarde";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult ConfirmarVenta(DataModels.Ventas model)
        {
            DataModels.ManagerVenta man = new DataModels.ManagerVenta();
            string result = string.Empty;
            if (Session["usercode"] != null)
            {
                string userCode = Session["usercode"].ToString();
                result = man.InsertarVenta(model, userCode);
                if (!string.IsNullOrEmpty(result))
                {
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hay Problemas");
                    return View(model);
                }
            }
            return View(model);
        }



    }
}
