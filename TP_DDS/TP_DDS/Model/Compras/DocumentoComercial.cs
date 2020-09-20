﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_DDS.Model.Compras
{
    public class DocumentoComercial
    {
        [Key]
        [Column("idDocComercial")]
        public int idDocComercial { get; set; }

        [Column("nroIdentificador")]
        public string nroIdentificacion { get; set; }

        [Column("tipo-enlace")]
        public string tipo_enlace { get; set; }

        public DocumentoComercial(string nroIdentificacion, string tipo_enlace)
        {
            this.nroIdentificacion = nroIdentificacion;
            this.tipo_enlace = tipo_enlace;
        }

        public string getTipo_Enlace() { return tipo_enlace; }
    }
}