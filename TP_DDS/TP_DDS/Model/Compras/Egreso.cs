﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_DDS.Model.Entidades;
using TP_DDS.Model.Ingresos;

namespace TP_DDS.Model.Compras
{
    [Table("egreso")]
    public class Egreso
    {
        [Key]
        [Column("idegreso")]
        public int idEgreso { get; set; }

        [Column("fechaDeOperacion")]
        public DateTime fechaEgreso { get; set; }

        [Column("idMedioDePago")]
        public int idMedioDePago { get; set; }
        public MedioDePago medioDePago { get; set; }

        [Column("monto")]
        public float montoTotal { get; set; }

        [Column("idPrestadorDeServicios")]
        public int idPrestadorDeServicios { get; set; }
        public PrestadorDeServicios prestadorDeServicios { get; set; }

        [Column("idIngresoAsociado")]
        public Ingreso ingresoAsociado { get; set; }

        public List<Item> detalle { get; set; }
        public List<DocumentoComercial> docsComerciales { get; set; }

        public Egreso() { }
        public Egreso(List<Item> detalle, List<DocumentoComercial> docsComerciales, Entidad entidad, DateTime fechaEgreso, MedioDePago medioDePago, PrestadorDeServicios prestadorDeServicios, Ingreso ingresoAsociado)
        {
            this.detalle = detalle;
            this.docsComerciales = docsComerciales;
            this.entidad = entidad;
            this.fechaEgreso = fechaEgreso;
            this.medioDePago = medioDePago;
            this.montoTotal = detalle.Sum(i => i.valor * i.cant);
            this.prestadorDeServicios = prestadorDeServicios;
            this.ingresoAsociado = ingresoAsociado;
        }


        public Presupuesto getPresupuestoElegido()
        {
            return (Presupuesto)docsComerciales.Find(doc => doc.tipo_enlace == "Presupuesto");
        }

       public bool TieneIngresoAsociado() {
            return this.ingresoAsociado != null;
        }
    }

}
