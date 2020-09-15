﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TP_DDS__consola_.Model.Entidades
{
    public class Empresa : TipoOrganizacion
    {
        public int idEmpresa { get; set; }
        public string sector { get; set; }
        public float promedioVentas { get; set; }
        public int cantPersonal { get; set; }

        public Empresa(string actividad, string sector, float promVentas, int cantPersonal) : base(actividad)
        {
            this.sector = sector;
            this.promedioVentas = promVentas;
            this.cantPersonal = cantPersonal;
        }

    }
}