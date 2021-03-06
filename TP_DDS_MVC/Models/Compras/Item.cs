﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_DDS_MVC.Models.Compras
{
    [Table("items")]
    public abstract class Item
    {
        [Key]
        public int idItem { get; set; }

        public int cant { get; set; }

        [StringLength(200)]
        public string descripcion { get; set; }

        public float valor { get; set; }

        public List<Categoria> categorias { get; set; }

        public Item() { }

        public Item(int cant, string descripcion, float valor)
        {
            this.cant = cant;
            this.descripcion = descripcion;
            this.valor = valor;
        }

        public string categoriasString()
        {
            foreach(Categoria cat in categorias)
            {
                Console.WriteLine(cat.nombre);
            }

            return String.Join(", ", categorias.Select(c=>c.nombre));
        }

    }
}