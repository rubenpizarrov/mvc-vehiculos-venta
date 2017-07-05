using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvaluacionVehiculosMVC.Controllers
{
    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Autenticacion(Models.User model) 
        {
            if (!string.IsNullOrEmpty(model.Usuario) || !string.IsNullOrEmpty(model.Password))
            {
                DataModels.ManagerLogin man = new DataModels.ManagerLogin();
                Models.User user = man.UserAuth(model.Usuario, model.Password);

                if (user.UserCode.Equals("0E"))
                {
                    ViewBag.Mensaje = "El usuario Ingresado no es válido";
                 
                    return View("Index");
                }
                else
                {
                    Session["usercode"] = user.UserCode;
                    Session["username"] = user.UserName;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View(model);
            }
            
        }

        public ActionResult Logout() 
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

    }
}
