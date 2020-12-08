﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP_DDS_MVC.Models.Compras;
using TP_DDS_MVC.Helpers.DB;

namespace TP_DDS_MVC.DAOs
{
    public class PrestadorDeServiciosDAO
    {
        public static PrestadorDeServiciosDAO instancia = null;
        public List<PrestadorDeServicios> PrestadoresDeServicios = new List<PrestadorDeServicios>();

        private PrestadorDeServiciosDAO() { }

        public static PrestadorDeServiciosDAO getInstancia()
        {

            if (instancia == null)
            {
                instancia = new PrestadorDeServiciosDAO();
            }
            return instancia;
        }

        public List<PrestadorDeServicios> getPrestadoresDeServicios()
        {

            using (MyDBContext context = new MyDBContext())
            {
                return context.PrestadoresDeServicios.ToList();
            }
        }

        public PrestadorDeServicios getPrestadorDeServicios(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.PrestadoresDeServicios.Find(id);
            }
        }

        public PrestadorDeServicios add(PrestadorDeServicios prestadorDeServicios)
        {
            PrestadorDeServicios added;
            using (MyDBContext context = new MyDBContext())
            {

                added = context.PrestadoresDeServicios.Add(prestadorDeServicios);
                context.SaveChanges();
            }
            

            return added;
        }

        public void deletePrestador(int idPrestador)
        {
            using (MyDBContext context = new MyDBContext())
            {
                var itemToRemove = context.PrestadoresDeServicios.SingleOrDefault(x => x.idPrestador == idPrestador); //returns a single item.

                if (itemToRemove != null)
                {
                    context.PrestadoresDeServicios.Remove(itemToRemove);

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("El prestador de servicios que quiere eliminar, no está registrado");
                }
            }
        }
    }
}