using ApiFinalExam.Models;
using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ApiFinalExam.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/books/getavailablebooks")]
        public List<ModelBook> GetAvailableBooks()
        {
            List<ModelBook> avbooks = new List<ModelBook>();
            using (var db = new LibraryEntities1())
            {
                var res = db.Book.Where(x => x.Quantity > 0).ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Book, ModelBook>();
                });
                var mapper = new Mapper(config);
                avbooks = mapper.Map<List<Book>, List<ModelBook>>(res);
            }
            return avbooks;
        }
        [Route("api/books/getoutofstockbooks")]
        public List<ModelBook> GetOutOfStockBooks()
        {
            List<ModelBook> avbooks = new List<ModelBook>();
            using (var db = new LibraryEntities1())
            {
                var res = db.Book.Where(x => x.Quantity == 0).ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Book, ModelBook>();
                });
                var mapper = new Mapper(config);
                avbooks = mapper.Map<List<Book>, List<ModelBook>>(res);
            }
            return avbooks;
        }
        [HttpGet]
        [Route("api/books/userorders/{iduser}")]
        public List<ModelOrder> GetOrdersByIdUser(int iduser)
        {
            using (var db = new LibraryEntities1())
            {
                List<Order> res = new List<Order>();
                res = db.Order.Where(p => p.Id_Person == iduser).ToList();
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
        [Route("api/books/userrequests/{iduser}")]
        public List<ModelRequest> GetRequestsByIdUser(int iduser)
        {
            using (var db = new LibraryEntities1())
            {
                List<Request> res = new List<Request>();
                res = db.Request.Where(p => p.Id_Person == iduser).ToList();
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
        [HttpPost]
        [Route("api/books/orderbooks")]
        public string PlaceOrder([FromBody]IDictionary<string, string> el)
        {
            using (var db = new LibraryEntities1())
            {
                foreach (var x in el)
                {
                    string[] data = x.Key.Split(',');
                    int isbn = int.Parse(data[0]);
                    int user = int.Parse(data[1]);
                    int value = int.Parse(x.Value);
                    Order o = db.Order.Where(p => p.Id_Book == isbn && p.Id_Person == user).FirstOrDefault();
                    Book b = db.Book.Where(p => p.ISBN == isbn).FirstOrDefault();
                    if (o == null)
                    {
                        if (value > 0
                            && b != null && b.Quantity>=value)
                        {
                            db.Order.Add(new Order() { Id_Person = user, Id_Book = isbn, Quantity = value });
                            b.Quantity -= value;
                        }
                    }
                    else if (value > 0 && b!= null && o.Quantity+b.Quantity >=value)
                    {
                        int valUpd = o.Quantity+b.Quantity == value ? b.Quantity : value - o.Quantity;
                        o.Quantity = value;
                        b.Quantity -= valUpd;
                    }
                }
                db.SaveChanges();
                return "ok";
            }
        }
        [HttpPost]
        [Route("api/books/requestbooks")]
        public string PlaceOutOfStockRequest([FromBody]IDictionary<string, string> el)
        {
            using (var db = new LibraryEntities1())
            {
                foreach (var x in el)
                {
                    string[] data = x.Key.Split(',');
                    int isbn = int.Parse(data[0]);
                    int user = int.Parse(data[1]);
                    int value = int.Parse(x.Value);
                    if (value > 0)
                    {
                        Request r = db.Request.Where(p => p.Id_Book == isbn && p.Id_Person == user).FirstOrDefault();
                        if (r == null)
                        {
                            db.Request.Add(new Request() { Id_Person = user, Id_Book = isbn, Quantity = value });
                        }
                        else
                        {
                            r.Quantity = value;
                        }
                    }
                }
                db.SaveChanges();
                return "ok";
            }
        }
        [HttpPost]
        [Route("api/books/total")]
        public string TotalOrderPrice([FromBody]IDictionary<string, string> el)
        {
            using (var db = new LibraryEntities1())
            {
                decimal price = 0;
                foreach (var x in el)
                {

                    string[] data = x.Key.Split(',');
                    int isbn = int.Parse(data[0]);
                    int user = int.Parse(data[1]);
                    int value = int.Parse(x.Value);
                    if (value > 0)
                    {
                        Book obj = db.Book.Where(p => p.ISBN == isbn).FirstOrDefault();
                        price += (obj.Price * value);
                    }
                }
                return string.Format("{0:C}", price);
            }
        }
        [HttpDelete]
        [Route("api/books/deleteorder/{user}/{book_id}")]
        public HttpResponseMessage DeleteOrder (int book_id, int user)
        {
            using (var db = new LibraryEntities1())
            {
                var res = db.Order.Where(p => p.Id_Book == book_id && p.Id_Person == user).FirstOrDefault();
                if (res!= null)
                {
                    db.Order.Remove(res);
                    db.Book.Where(p => p.ISBN == book_id).FirstOrDefault().Quantity += res.Quantity;
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                } else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
            }
        }
        [HttpDelete]
        [Route("api/books/deleterequest/{user}/{book_id}")]
        public HttpResponseMessage DeleteRequest(int book_id, int user)
        {
            using (var db = new LibraryEntities1())
            {
                var res = db.Request.Where(p => p.Id_Book == book_id && p.Id_Person == user).FirstOrDefault();
                if (res != null)
                {
                    db.Request.Remove(res);
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }
            }
        }
        [HttpPost]
        [Route("api/books/search")]
        public List<ModelBook> SearchBooks(Dictionary<string, string> dictionary)
        {
            List<ModelBook> ls = new List<ModelBook>();
            using (var db = new LibraryEntities1())
            {
                var res = db.Book;
                IQueryable<Book> el = null;
                if (!dictionary["ISBN"].Equals("")) {
                    int id = int.Parse(dictionary["ISBN"]);
                    el = res.Where(p => p.ISBN == id); }
                if (!dictionary["Title"].Equals("")) {
                    string title = dictionary["Title"];
                    el = el == null ? res.Where(p => p.Title.Equals(title)) : el.Where(p => p.Title.Equals(title)); }
                if (!dictionary["Author"].Equals("")) {
                    string author = dictionary["Author"];
                    el = el == null ? res.Where(p => p.Author.Equals(author)) : el.Where(p => p.Author.Equals(author)); }
                if (!dictionary["MinPrice"].Equals("")) {
                    decimal min = decimal.Parse(dictionary["MinPrice"]);
                    el = el == null ? res.Where(p => p.Price >= min) : el.Where(p => p.Price >= min); }
                if (!dictionary["MaxPrice"].Equals("")) {
                    decimal max = decimal.Parse(dictionary["MaxPrice"]);
                    el = el == null ? res.Where(p => p.Price <= max) : el.Where(p => p.Price <= max); }
                var prova = el == null ? res.ToList() : el.ToList();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Book, ModelBook>();
                });
                var map = new Mapper(config);
                
                return map.Map<List<Book>, List<ModelBook>>(prova);
            }
        }

        [HttpGet]
        [Route("api/user/{mail}/{password}")]
        public HttpResponseMessage Login(string mail, string password)
        {
            using (var db = new LibraryEntities1())
            {
                string hash = HashPassword(password);
                Person res = db.Person.Where(x => x.Mail.Equals(mail) && x.Pwd.Equals(hash)).FirstOrDefault();
                if (res != null)
                {
                    var x = new HttpResponseMessage();
                    x.StatusCode = HttpStatusCode.OK;
                    x.Content = new StringContent(res.IsSeller.ToString() + "," + res.Id.ToString(), Encoding.UTF8, "application/json");
                    return x;
                }
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        protected string HashPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// Hidden Method to Add user to Database with hashed password.
        /// The password for all users in the database is:
        /// ciao
        /// </summary>
        /*[HttpGet]
        [Route("api/user/{pwd}")]
        public string InsideRegister(string pwd)
        {
            using (var db = new LibraryEntities1())
            {
                string hash = HashPassword(pwd);
                db.Person.Add(new Person()
                {
                    IsSeller = 1,
                    Mail = "seller2@gmail.it",
                    Pwd = hash,
                    Address = "Civitanova 200",
                    City = "Roma",
                    CAP = "20002",
                    Name = "Seller2",
                    Surname = "Seller2"
                });
                db.SaveChanges();
                return "ok";
            }
        }*/
    }
}
