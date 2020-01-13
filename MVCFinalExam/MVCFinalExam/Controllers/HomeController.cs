using MVCFinalExam.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVCFinalExam.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            ViewBag.Msg = TempData["msg"];
            List<ModelBook> ls = new List<ModelBook>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/books/getavailablebooks");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelBook>>(data.Result);
                }
            }
            return View(ls);
        }

        [HttpPost]
        public ActionResult PostOrder(string dictionary)
        {
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            using (var conn = new HttpClient())
            {
                var content = new StringContent(dictionary, Encoding.UTF8, "application/json");
                var req = conn.PostAsync("https://localhost:44335/api/books/orderbooks", content);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    TempData["msg"] = "Order Posted, please check your order page for informations.";
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult DeleteOrder(int book_id, int user)
        {
            if (user != int.Parse(Session["iduser"].ToString())) return RedirectToAction("YourOrders");
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            using (var conn = new HttpClient())
            {
                var req = conn.DeleteAsync("https://localhost:44335/api/books/deleteorder/" + user + "/" + book_id);
                req.Wait();
                var res = req.Result;
                TempData["msg"] = res.IsSuccessStatusCode ? "Successful Delete" : "I'm Sorry";
            }
            return RedirectToAction("YourOrders");

        }

        public ActionResult OutOfStockRequest()
        {
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            ViewBag.Msg = TempData["msg"];
            List<ModelBook> ls = new List<ModelBook>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/books/getoutofstockbooks");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelBook>>(data.Result);
                }
            }
            return View(ls);
        }

        [HttpPost]
        public ActionResult PostRequest(string dictionary)
        {
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            using (var conn = new HttpClient())
            {
                var content = new StringContent(dictionary, Encoding.UTF8, "application/json");
                var req = conn.PostAsync("https://localhost:44335/api/books/requestbooks", content);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    TempData["msg"] = "Request Posted, please check your request page for informations.";
                }
            }
            return RedirectToAction("OutOfStockRequest");
        }
        [HttpGet]
        public ActionResult DeleteRequest(int book_id, int user)
        {
            if (user != int.Parse(Session["iduser"].ToString())) return RedirectToAction("YourOrders");

            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            using (var conn = new HttpClient())
            {
                var req = conn.DeleteAsync("https://localhost:44335/api/books/deleterequest/" + user + "/" + book_id);
                req.Wait();
                var res = req.Result;
                TempData["msg"] = res.IsSuccessStatusCode ? "Successfull Delete" : "I'm Sorry";
            }
            return RedirectToAction("YourRequests");

        }
        [HttpPost]
        public PartialViewResult VisualizeMessage(string act)
        {
            if (Session["usertype"].Equals("1")) return PartialView();
            Dictionary<string, string> dictionary = CreateValuesDictionary();
            using (var conn = new HttpClient())
            {
                var dic = JsonConvert.SerializeObject(dictionary);
                var content = new StringContent(dic, Encoding.UTF8, "application/json");
                var req = conn.PostAsync("https://localhost:44335/api/books/total", content);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ViewBag.Price = "Il prezzo è: " + data.Result;
                    ViewBag.Dic = dic;
                    ViewBag.CallAction = act;
                }
            }
            return PartialView();
        }
        public ActionResult YourOrders()
        {
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            ViewBag.Msg = TempData["msg"];
            List<ModelOrder> ls = new List<ModelOrder>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/books/userorders/" + Session["iduser"]);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelOrder>>(data.Result);
                }
            }
            return View(ls);
        }
        public ActionResult YourRequests()
        {
            if (Session["usertype"].Equals("1")) return RedirectToAction("Index", "SellerArea");
            ViewBag.Msg = TempData["msg"];
            List<ModelRequest> ls = new List<ModelRequest>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/books/userrequests/" + Session["iduser"]);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelRequest>>(data.Result);
                }
            }
            return View(ls);
        }

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public PartialViewResult SearchResult()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            var x = Request.Form.AllKeys;
            foreach (var el in x)
            {
                res.Add(el, Request.Form[el]);
            }
            List<ModelBook> ls = new List<ModelBook>();
            using (var conn = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(res), Encoding.UTF8, "application/json");
                var send = conn.PostAsync("https://localhost:44335/api/books/search/", content);
                send.Wait();
                if (send.Result.IsSuccessStatusCode)
                {
                    var data = send.Result.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelBook>>(data.Result);
                }
            }
            return PartialView(ls);
        }
        public Dictionary<string, string> CreateValuesDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            var r = Request.Form.AllKeys;
            foreach (var el in r)
            {
                string[] val = Request.Form[el].Split(',');
                if (val.ElementAtOrDefault(2) != null && val[0] != "")
                {
                    dictionary.Add(el + "," + Session["iduser"], val[0]);
                }
            }
            return dictionary;
        }
    }
}