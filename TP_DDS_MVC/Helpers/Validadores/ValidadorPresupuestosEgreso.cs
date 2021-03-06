﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_DDS_MVC.Models.Compras;
using TP_DDS_MVC.Models.Otros;

using TP_DDS_MVC.Models.Proyectos;

using TP_DDS_MVC.Helpers.DB;
using TP_DDS_MVC.DAOs;


namespace TP_DDS_MVC.Helpers.Validadores
{
    public class ValidadorPresupuestosEgreso
    {
        public static async Task validar(Compra comp)
        {

            using (MyDBContext context = new MyDBContext())
            {
                Compra compra = CompraDAO.getInstancia().getCompraConEgresoYDocumentos(comp.idCompra);
                if (compra.cantMinimaPresupuestos > 0)
                {
                    compra.compraValidada = true;
                    if (cantidadIndicadaPresupuestos(compra) && esMenorPresupuesto(compra) && compraUsaPresupuesto(compra))
                    {

                        enviarMensajes(compra.revisores, "La compra: " + compra.descripcion + " , fue validada");

                    }
                    else
                    {

                        enviarMensajes(compra.revisores, "Hubo un error con la validacion de la compra: " + compra.descripcion);
                    }
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("La compra no requiere presupuestos");
                }
            }
        }


        public static async Task validar(ProyectoFinanciamiento proyecto)
        {
            //TASK 6

            if (esMenorALimite(proyecto) && validarPresupuestos(proyecto) && validarIngresosEgresos(proyecto))
            {
                enviarMensajes(proyecto.director, "El proyecto " + proyecto.propuesta + " fue validado");
            } else
            {
                enviarMensajes(proyecto.director, "Hubo un error");
            }
        }

        public static bool validarIngresosEgresos(ProyectoFinanciamiento proyecto)
        {
            float montoEgresos = proyecto.compras.Sum(x => x.egreso.montoTotal);
            float montoIngresos = proyecto.ingresos.Sum(x => x.monto);
            return montoEgresos <= montoIngresos;

        }   


        public static bool esMenorALimite(ProyectoFinanciamiento proyecto)
        {
            float montoErogacion = proyecto.compras.Sum(x => x.egreso.montoTotal);
            return montoErogacion <= proyecto.limiteErogacion;
        }

        public static bool validarPresupuestos(ProyectoFinanciamiento proyecto)
        {
            int cantPresupuestosCompras = proyecto.compras.Sum(x => x.presupuestos.Count());
            return cantPresupuestosCompras >= proyecto.cantidadPresupuestos;
        }

        public static void enviarMensajes(List<Usuario> usuarios, string mensaje) //sobrecarga +1 usuarios
        {
            foreach (Usuario usuario in usuarios)
            {
                UsuarioDAO.getInstancia().enviarNotificacion(new Notificacion(mensaje, DateTime.Now, usuario.idUsuario));
            }
        }

        public static void enviarMensajes(Usuario usuario, string mensaje) { //sobrecarga 1 usuario
        UsuarioDAO.getInstancia().enviarNotificacion(new Notificacion(mensaje, DateTime.Now, usuario.idUsuario));
        }

        private static bool cantidadIndicadaPresupuestos(Compra compra)
        {
            return compra.cantMinimaPresupuestos <= compra.getCantPresupuestos();
        }

        private static bool esMenorPresupuesto(Compra compra)
        {
            //return !(compra.presupuestos.Any(presupuesto => compra.getEgreso().getPresupuestoElegido()!= presupuesto && presupuesto.getMontoTotal() <= compra.getEgreso().getPresupuestoElegido().getMontoTotal()));
            return compra.presupuestos.OrderBy(p => p.montoTotal).First() == compra.egreso.getPresupuestoElegido();
        }

        private static bool compraUsaPresupuesto(Compra compra)
        {
            return compra.presupuestos.Contains(compra.egreso.getPresupuestoElegido());
            //buscamos en la lista de presupuestos de la compra, si esta el presupuesto elegido
        }
    }
}
