﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TP_DDS__consola_.Model.Compras
{
    public class PrestadorDeServicios
    {
        public int idPrestador { get; set; }
        public string direccionPostal { get; set; }
        public string numDoc { get; set; }
        public string razonSocial { get; set; }
        public string tipoDoc { get; set; }

        public PrestadorDeServicios(string direccionPostal, string razonSocial, string tipoDoc, string numDoc)
        {
            this.direccionPostal = direccionPostal;
            this.numDoc = numDoc;
            this.razonSocial = razonSocial;
            this.tipoDoc = tipoDoc;
        }

    }
}