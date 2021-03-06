﻿using EvaluacionVehiculosMVC.WSEvaluacionInacap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvaluacionVehiculosMVC.DataModels
{
    public class ManagerLogin
    {
        WSEvaluacionInacapSoapClient ws = new WSEvaluacionInacapSoapClient();

        //public bool DisponibilidadServicio() 
        //{
        //    string resultado = string.Empty;
        //    try
        //    {
        //        resultado = ws.TestConexion();
        //        if (!string.IsNullOrEmpty(resultado))
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return false;
        //    }
        //    return false;
        //}

        public Models.User UserAuth(string userAlumno, string passwordAlumno) 
        {
            Models.User user = new Models.User();
            var result = ws.Login(userAlumno, passwordAlumno);

            user.UserAlumno = result.UserAlumno;
            user.UserName = result.UserName;
            user.UserCode = result.UserCode;
            user.IsValudOrMessage = result.IsValudOrMessage;

            if (user.UserCode.Equals("0E"))
            {
                return user;
            }
            return user;
        }
    }
}