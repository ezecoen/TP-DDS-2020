﻿using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TP_DDS_MVC.Models.Compras;
using TP_DDS_MVC.Models.Entidades;
using TP_DDS_MVC.DAOs;
using TP_DDS_MVC.Helpers;
using TP_DDS_MVC.Mongo;
using MongoDB.Driver;
using MongoDB.Bson;
using TP_DDS_MVC.Models.Otros;

namespace TP_DDS_MVC.Controllers
{
    [CustomAuthenticationFilter]
    public class CompraController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        ///////////////////////////////////////////////
        ///         Prestador de servicios          ///
        ///////////////////////////////////////////////

        public ActionResult ListPrestadorDeServicios()
        {
            int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
            List<PrestadorDeServicios> pres = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios(idEntidad);
            return View(pres);
        }

        public ActionResult AddPrestadorDeServicios()
        {
            ViewBag.paises = PaisDAO.getInstancia().getPaises();
            ViewBag.provincias = ProvinciaDAO.getInstancia().getProvincias();
            ViewBag.ciudades = CiudadDAO.getInstancia().getCiudades();
            return View();
        }

        [HttpPost]
        public ActionResult AddPrestadorDeServicios(PrestadorDeServicios PDS)
        {
            try
            {
                PDS.idEntidad = ((Usuario)Session["usuario"]).idEntidad;
                PDS.direccionPostal.validarDireccion();
                PrestadorDeServiciosDAO.getInstancia().add(PDS);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewBag.paises = PaisDAO.getInstancia().getPaises();
                ViewBag.provincias = ProvinciaDAO.getInstancia().getProvincias();
                ViewBag.ciudades = CiudadDAO.getInstancia().getCiudades();
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View();
            }
        }


        public ActionResult DetallePrestadorDeServicios(int id)
        {
            PrestadorDeServicios pres = PrestadorDeServiciosDAO.getInstancia().getPrestadorDeServicios(id);
            return View(pres);
        }


        public ActionResult DeletePrestadorDeServicios(int id)
        {
            try
            {
                PrestadorDeServiciosDAO.getInstancia().deletePrestador(id);
                return View("ListPrestadorDeServicios");
            }
            catch (Exception e)
            {
                MyLogger.log(e.Message);
                ViewBag.errorMsg = "Hubo un error al intentar eliminar el prestador de servicios";
                return View("ListPrestadorDeServicios");
            }
        }

        public ActionResult EditPrestadorDeServicios(int id)
        {
            PrestadorDeServicios pres = PrestadorDeServiciosDAO.getInstancia().getPrestadorDeServicios(id);
            ViewBag.paises = PaisDAO.getInstancia().getPaises();
            ViewBag.provincias = ProvinciaDAO.getInstancia().getProvincias();
            ViewBag.ciudades = CiudadDAO.getInstancia().getCiudades();
            return View(pres);
        }

        [HttpPost]
        public ActionResult EditPrestadorDeServicios(PrestadorDeServicios PDS)
        {
            try
            {
                PDS.direccionPostal.validarDireccion();
                PrestadorDeServiciosDAO.getInstancia().updatePrestadorDeServicios(PDS);
                return RedirectToAction("ListPrestadorDeServicios", "Compra");
            }
            catch (Exception e)
            {
                ViewBag.paises = PaisDAO.getInstancia().getPaises();
                ViewBag.provincias = ProvinciaDAO.getInstancia().getProvincias();
                ViewBag.ciudades = CiudadDAO.getInstancia().getCiudades();
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View();
            }
        }

        ///////////////////////////////////////////////
        ///             Medio de Pago               ///
        ///////////////////////////////////////////////
        public ActionResult AddMedioDePago()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMedioDePago(MedioDePago MDP)
        {
            try
            {
                MDP.idEntidad = ((Usuario)Session["usuario"]).idEntidad;
                MedioDePagoDAO.getInstancia().add(MDP);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                MyLogger.log(e.Message);
                return View();
            }
        }

        public ActionResult ListMedioDePago()
        {
            int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
            List<MedioDePago> pres = MedioDePagoDAO.getInstancia().getMediosDePago(idEntidad);
            return View(pres);
        }

        public ActionResult DeleteMedioDePago(int idMedioDePago)
        {
            try
            {
                MedioDePagoDAO.getInstancia().deleteMedioDePago(idMedioDePago);
                return View("ListMedioDePago");
            }
            catch (Exception e)
            {
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View("ListMedioDePago");
            }
        }

        ///////////////////////////////////////////////
        ///              Presupuesto                ///
        ///////////////////////////////////////////////


        public ActionResult ListPresupuestos()
        {
            List<Presupuesto> pres = PresupuestoDAO.getInstancia().getPresupuestos();
            return View(pres);
        }

        public ActionResult AddPresupuesto()
        {
            int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
            ViewBag.mediosDePago = MedioDePagoDAO.getInstancia().getMediosDePago(idEntidad);
            ViewBag.proveedores = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios(idEntidad);
            ViewBag.compras = CompraDAO.getInstancia().getCompras();
            return View();
        }

        [HttpPost]
        public ActionResult AddPresupuesto(JsonPresupuesto req)
        {
            try
            {
                req.presupuesto.idEntidad = ((Usuario)Session["usuario"]).idEntidad;
                if (req.setEgreso && req.presupuesto.idCompra != null)
                {
                    Compra comp = CompraDAO.getInstancia().getCompraConEgresoYDocumentos(req.presupuesto.idCompra.Value);
                    if(comp.egreso.docsComerciales.Exists(dc => dc.tipo_enlace == "Presupuesto"))
                    {
                        throw new Exception("La compra seleccionada ya tiene un presupuesto elegido para el egreso");
                    }
                    req.presupuesto.idEgreso = comp.idEgreso;
                }
                PresupuestoDAO.getInstancia().add(req.presupuesto);
                
                /*BsonDocument presupuesto = new BsonDocument {
                     { "montoTotal", req.presupuesto.montoTotal },
                     { "idPrestadorDeServicios", req.presupuesto.idPrestadorDeServicios } };

                Mongo.MongoDB.insertarDocumento("Egreso", "alta", req.presupuesto.ToBsonDocument());*/

                Mongo.MongoDB.insertarDocumento("Presupuesto", "alta", req.presupuesto.ToBsonDocument());

                return Json(Url.Action("Index", "Home"));
            }
            catch (Exception e)
            {
                int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
                ViewBag.mediosDePago = MedioDePagoDAO.getInstancia().getMediosDePago(idEntidad);
                ViewBag.proveedores = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios(idEntidad);
                ViewBag.compras = CompraDAO.getInstancia().getCompras();
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View();
            }
        }

        public ActionResult DeletePresupuesto(int idPresupuesto)
        {
            try
            {
                PresupuestoDAO.getInstancia().deletePresupuesto(idPresupuesto);
                return View("ListPresupuestos");
            }
            catch (Exception e)
            {
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View("ListPresupuestos");
            }
        }

        public ActionResult DetallePresupuesto(int id)
        {
            Presupuesto pres = PresupuestoDAO.getInstancia().getPresupuesto(id);
            return View(pres);
        }

        ///////////////////////////////////////////////
        ///              Egreso                     ///
        ///////////////////////////////////////////////
        /*
        public ActionResult AddEgreso()
        {
            ViewBag.docsComerciales = DocumentoComercialDAO.getInstancia().getDocumentosComerciales();
            ViewBag.mediosDePago = MedioDePagoDAO.getInstancia().getMediosDePago();
            ViewBag.proveedores = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios();
            return View();
        }

        [HttpPost]
        public ActionResult AddEgreso(JsonEgreso req)
        {
            
            try
            {
                int idEgreso = EgresoDAO.getInstancia().add(req.egreso).idEgreso;
                foreach (string nroId in req.docsComerciales)
                {
                    DocumentoComercialDAO.getInstancia().setEgresoId(idEgreso, nroId);
                }

                var dataJson = JsonConvert.SerializeObject(req);

                BsonDocument egreso = new BsonDocument {
                     { "idEgreso", req.egreso.idEgreso },
                     { "montoTotal", req.egreso.montoTotal },
                     { "fechaEgreso", req.egreso.fechaEgreso } };

                BsonDocument document = BsonDocument.Parse(dataJson);

                Mongo.MongoDB.insertarDocumento("Egreso", "alta", document);

                return Json(Url.Action("Index", "Home"));
            }
            catch (Exception e)
            {
                ViewBag.docsComerciales = DocumentoComercialDAO.getInstancia().getDocumentosComerciales();
                ViewBag.mediosDePago = MedioDePagoDAO.getInstancia().getMediosDePago();
                ViewBag.proveedores = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios();
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View();
            }
        }*/


        ///////////////////////////////////////////////
        ///              Compra                     ///
        ///////////////////////////////////////////////

        // GET: Compras
        public ActionResult ListCompras()
        {
            List<Compra> compras = CompraDAO.getInstancia().getComprasConEgreso();
            return View(compras);
        }

        public ActionResult DetalleCompra(int idCompra)
        {
            Compra pres = CompraDAO.getInstancia().getCompraConEgreso(idCompra);
            return View(pres);
        }

        /*
        public ActionResult EditCompra(int idCompra)
        {
            Compra pres = CompraDAO.getInstancia().getCompraConEgreso(idCompra);
            return View(pres);
        }*/

        public ActionResult AddCompra()
        {
            int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
            ViewBag.mediosDePago = MedioDePagoDAO.getInstancia().getMediosDePago(idEntidad);
            ViewBag.proveedores = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios(idEntidad);
            ViewBag.usuarios = UsuarioDAO.getInstancia().getUsuarios(idEntidad);
            ViewBag.categorias = CategoriaDAO.getInstancia().getCategorias(idEntidad);
            return View();
        }

        [HttpPost]
        public ActionResult AddCompra(JsonCompra req)
        {
            
            try
            {

                for (int i = 0; i < req.compra.egreso.detalle.Count(); i++)
                {
                    req.compra.egreso.detalle.ElementAt(i).categorias = req.compra.egreso.detalle.ElementAt(i).categorias.Select(c => CategoriaDAO.getInstancia().getCategoria(c.idCategoria)).ToList();
                }

                req.compra.idEntidad = ((Usuario)Session["usuario"]).idEntidad;
                if (req.revisores != null)
                {
                    req.compra.revisores = new List<Usuario>();
                    foreach (int idUsuario in req.revisores)
                    {
                        req.compra.revisores.Add(UsuarioDAO.getInstancia().getUsuario(idUsuario));
                    }
                }
                
                Compra compra = CompraDAO.getInstancia().add(req.compra);

                Mongo.MongoDB.insertarDocumento("Compra", "alta", req.compra.ToBsonDocument());

                /*BsonDocument compra1 = new BsonDocument {
                     { "descripcion", req.compra.descripcion },
                     { "cantMinimaPresupuestos", req.compra.cantMinimaPresupuestos },
                     { "idCompra", req.compra.idCompra } };

                var dataJson = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                string json = JsonConvert.SerializeObject(joe, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });*/

                //BsonDocument document = BsonDocument.Parse(dataJson);

                Mongo.MongoDB.insertarDocumento("Egreso", "alta", req.compra.egreso.ToBsonDocument());


                //Mongo.MongoDB.insertarDocumento("Compra", "alta", compra1 );

                //BsonDocument egreso = new BsonDocument {
                //     { "idEgreso", req.compra.egreso.idEgreso },
                //     { "montoTotal", req.compra.egreso.montoTotal },
                //     { "fechaEgreso", req.compra.egreso.fechaEgreso } };

                //Mongo.MongoDB.insertarDocumento("Egreso", "alta", egreso);

                return Json(Url.Action("Index", "Home"));
            }
            catch (Exception e)
            {
                int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
                ViewBag.mediosDePago = MedioDePagoDAO.getInstancia().getMediosDePago(idEntidad);
                ViewBag.proveedores = PrestadorDeServiciosDAO.getInstancia().getPrestadoresDeServicios(idEntidad);
                ViewBag.usuarios = UsuarioDAO.getInstancia().getUsuarios(idEntidad);
                ViewBag.categorias = CategoriaDAO.getInstancia().getCategorias(idEntidad);
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return View();
            }
        }

        // GET: Compra/Delete/5
        public ActionResult DeleteCompra(int idCompra)

        {
            try
            {
                CompraDAO.getInstancia().deleteCompra(idCompra);
                return RedirectToAction("ListCompras");
            }
            catch (Exception e)
            {
                MyLogger.log(e.Message);
                ViewBag.errorMsg = e.Message;
                return RedirectToAction("ListCompras");
                 
            }

        }


        ///////////////////////////////////////////////
        ///              Criterio                   ///
        ///////////////////////////////////////////////


        public ActionResult criterios()
        {
            int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
            ViewBag.criterios = CriterioDAO.getInstancia().getCriterios(idEntidad);
            return View();
        }

        [HttpPost]
        public ActionResult criterios(string criterio, int? idPadre, string categorias)
        {

            int idEntidad = ((Usuario)Session["usuario"]).idEntidad.Value;
            
            List<string> categoriasSeparadas = categorias.Split(',').ToList();

            List<Categoria> categoriasNuevas = new List<Categoria>();

            for (int i = 0; i < categoriasSeparadas.Count; i++)
            {
                Categoria categoriaNueva = new Categoria(categoriasSeparadas[i].Trim());
                categoriasNuevas.Add(categoriaNueva);
            }

            Criterio criterioNuevo = new Criterio(criterio, idEntidad, idPadre, categoriasNuevas);

            var criterio1 = CriterioDAO.getInstancia().add(criterioNuevo);
            

            return RedirectToAction("Index", "Home");
        }


        //    // GET: Compra/Details/5
        //    public ActionResult DetailCompra(int id)
        //    {
        //        ViewBag.compra = CompraDAO.getInstancia().getCompra(id);
        //        return View();
        //    }
        //    // POST: Compra/Create


        //    // GET: Compra/Edit/5
        //    public ActionResult Edit(int id)
        //    {
        //        return View();
        //    }

        //    // POST: Compra/Edit/5
        //    [HttpPost]
        //    public ActionResult Edit(int id, FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add update logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    // GET: Compra/Delete/5
        //    public ActionResult Delete(int id)
        //    {
        //        return View();
        //    }

        //    // POST: Compra/Delete/5
        //    [HttpPost]
        //    public ActionResult Delete(int id, FormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add delete logic here

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
    }
}
