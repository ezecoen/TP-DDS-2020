﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_DDS_MVC.Models.Entidades;
using Newtonsoft.Json.Linq;
using TP_DDS_MVC.Models.Otros;
using TP_DDS_MVC.Helpers.DB;

namespace TP_DDS_MVC.Helpers.Validadores
{

    public class ValidadorDireccionPostal
    {

        public static bool validarDireccionPostal(DireccionPostal dirPos)
        {
            Pais pais = validarPais(dirPos);

            if (pais != null){
                Provincia prov = validarProvincia(dirPos);
                if (prov != null)
                {
                    return validarCiudad(dirPos)!=null;
                }
            }
            return false;
        }

        private static Pais validarPais(DireccionPostal dirPos)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.Paises.Where(p => p.nombre == dirPos.pais).FirstOrDefault();
            }

        }

        private static Provincia validarProvincia(DireccionPostal dirPos)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.Provincias.Where(p => p.nombre == dirPos.provincia).FirstOrDefault();
            }

        }


        private static Ciudad validarCiudad(DireccionPostal dirPos)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.Ciudades.Where(c => c.nombre == dirPos.ciudad).FirstOrDefault();
            }
        }
    }
}
