﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP_DDS_MVC.Models.Compras;
using TP_DDS_MVC.Helpers.DB;
using System.Data.Entity;

namespace TP_DDS_MVC.DAOs
{
    public class PresupuestoDAO
    {
        public static PresupuestoDAO instancia = null;
        public List<Presupuesto> Presupuestos = new List<Presupuesto>();

        private PresupuestoDAO() { }

        public static PresupuestoDAO getInstancia()
        {

            if (instancia == null)
            {
                instancia = new PresupuestoDAO();
            }
            return instancia;
        }

        public List<Presupuesto> getPresupuestos()
        {

            using (MyDBContext context = new MyDBContext())
            {
                return context.DocumentosComerciales.OfType<Presupuesto>().ToList();
            }
        }

        public Presupuesto getPresupuesto(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.DocumentosComerciales.OfType<Presupuesto>().Include("items").Include("prestadorDeServicios").Include("medioDePago").Where(s => s.idDocComercial == id).FirstOrDefault<Presupuesto>();
                //return (Presupuesto)context.DocumentosComerciales.OfType<Presupuesto>().Include(p => p.items).Where(p => p.idDocComercial == id);
            }
        }

        public Presupuesto add(Presupuesto presupuesto)
        {
            Presupuesto added;
            using (MyDBContext context = new MyDBContext())
            {
                added = (Presupuesto)context.DocumentosComerciales.Add(presupuesto);
                context.SaveChanges();
            }

            return added;
        }

        public void deletePresupuesto(int idPresupuesto)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var itemToRemove = context.DocumentosComerciales.SingleOrDefault(x => x.idDocComercial == idPresupuesto); //returns a single item.

                if (itemToRemove != null)
                {
                    context.DocumentosComerciales.Remove(itemToRemove);

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("El presupuesto que quiere eliminar, no existe");
                }
            }

        }
    }
}