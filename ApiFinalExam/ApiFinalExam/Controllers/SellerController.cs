using ApiFinalExam.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiFinalExam.Controllers
{
    public class SellerController : ApiController
    {
        [HttpGet]
        [Route("api/seller/getorders")]
        public List<ModelOrder> GetOrders()
        {
            using (var db = new LibraryEntities1())
            {
                List<Order> res = new List<Order>();
                res = db.Order.OrderBy(p => p.Id_Person).ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Order, ModelOrder>();
                    cfg.CreateMap<Book, ModelBook>();
                    cfg.CreateMap<Person, ModelPerson>();
                });
                var mapper = new Mapper(config);
                return mapper.Map<List<Order>, List<ModelOrder>>(res); ;
            }
        }
        [HttpGet]
        [Route("api/seller/getrequests")]
        public List<ModelRequest> GetRequests()
        {
            using (var db = new LibraryEntities1())
            {
                List<Request> res = new List<Request>();
                res = db.Request.OrderBy(p => p.Id_Person).ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Request, ModelRequest>();
                    cfg.CreateMap<Book, ModelBook>();
                    cfg.CreateMap<Person, ModelPerson>();
                });
                var mapper = new Mapper(config);
                return mapper.Map<List<Request>, List<ModelRequest>>(res); ;
            }
        }
        [HttpGet]
        [Route("api/seller/takecare/{type}/{id}/{iduser}/{sellerid}")]
        public HttpResponseMessage TakeCareOf(string type, int id, int iduser, int sellerid)
        {
            using (var db = new LibraryEntities1())
            {
                if (type.Equals("orders"))
                {
                    var res = db.Order.Where(p => p.Id_Book == id && p.Id_Person == iduser && p.Id_Seller == null).FirstOrDefault();
                    if (res != null)
                    {
                        res.InCharge = 1;
                        res.Id_Seller = sellerid;
                        db.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.OK);

                    }
                }
                else if (type.Equals("request"))
                {
                    var res = db.Request.Where(p => p.Id_Book == id && p.Id_Person == iduser && p.Id_Seller == null).FirstOrDefault();
                    if (res != null)
                    {
                        res.InCharge = 1;
                        res.Id_Seller = sellerid;
                        db.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        [Route("api/seller/leavecare/{type}/{id}/{iduser}/{sellerid}")]
        public HttpResponseMessage LeaveCareOf(string type, int id, int iduser, int sellerid)
        {
            using (var db = new LibraryEntities1())
            {
                if (type.Equals("orders"))
                {
                    var res = db.Order.Where(p => p.Id_Book == id && p.Id_Person == iduser && p.Id_Seller != null).FirstOrDefault();
                    if (res != null)
                    {
                        res.InCharge = 0;
                        res.Id_Seller = null;
                        db.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.OK);

                    }
                }
                else if (type.Equals("request"))
                {
                    var res = db.Request.Where(p => p.Id_Book == id && p.Id_Person == iduser && p.Id_Seller != null).FirstOrDefault();
                    if (res != null)
                    {
                        res.InCharge = 0;
                        res.Id_Seller = null;
                        db.SaveChanges();
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }


}

