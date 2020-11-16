﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP_DDS.Model.Compras;
using TP_DDS_MVC.Helpers.DB;

namespace TP_DDS_MVC.DAOs
{
    public class ItemEgresoDAO
    {
        public static ItemEgresoDAO instancia = null;
        public List<ItemEgreso> itemsEgresos = new List<ItemEgreso>();

        private ItemEgresoDAO() { }

        public static ItemEgresoDAO getInstancia()
        {

            if (instancia == null)
            {
                instancia = new ItemEgresoDAO();
            }
            return instancia;
        }

        public List<Item> getItemsEgresos()
        {

            using (MyDBContext context = new MyDBContext())
            {
                return context.ItemsEgresos.ToList();
            }
        }

        public Item getItemEgreso(int id)
        {
            using (MyDBContext context = new MyDBContext())
            {
                return context.ItemsEgresos.Find(id);
            }
        }

        public Item add(Item itemEgreso)
        {
            Item added;
            using (MyDBContext context = new MyDBContext())
            {
                added = context.ItemsEgresos.Add(itemEgreso);
            }

            return added;
        }
    }
}