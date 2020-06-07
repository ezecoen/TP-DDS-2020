﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TP_DDS__consola_
{
    class EntidadJuridica : Entidad
    {
        public string condInscripDefinitiva { get; set; }
        public string CUIT { get; set; }
        public string direccionPostal { get; set; }
        public List<EntidadBase> listaEntidadesBase;
        public string razonSocial { get; set; }
        public TipoOrganizacion tipoOrganizacion { get; set; }

        public EntidadJuridica (string nombreFicticio, string condInscripDefinitiva, string CUIT, string direccionPostal, List<EntidadBase> listaEntidadesBase, string razonSocial, string actividad):base(nombreFicticio)
        {
            this.condInscripDefinitiva = condInscripDefinitiva;
            this.CUIT = CUIT;
            this.direccionPostal = direccionPostal;
            this.listaEntidadesBase = listaEntidadesBase;
            this.razonSocial = razonSocial;
            this.tipoOrganizacion = new OSC(actividad);
        }

        public EntidadJuridica(string nombreFicticio, string condInscripDefinitiva, string CUIT, string direccionPostal, List<EntidadBase> listaEntidadesBase, string razonSocial, string actividad, string sector, float promVentas, int cantPersonal) : base(nombreFicticio)
        {
            this.condInscripDefinitiva = condInscripDefinitiva;
            this.CUIT = CUIT;
            this.direccionPostal = direccionPostal;
            this.listaEntidadesBase = listaEntidadesBase;
            this.razonSocial = razonSocial;
            this.tipoOrganizacion = CategorizadorOrg.categorizar(new Empresa(actividad, sector, promVentas, cantPersonal));
        }
    }
}