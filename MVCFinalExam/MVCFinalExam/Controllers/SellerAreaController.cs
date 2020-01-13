using MVCFinalExam.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVCFinalExam.Controllers
{
    public class SellerAreaController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }
        public PartialViewResult OrdersList()
        {
            List<ModelOrder> ls = new List<ModelOrder>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/seller/getorders");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelOrder>>(data.Result);
                }
            }
            return PartialView(ls);
        }
        public PartialViewResult RequestList()
        {
            List<ModelRequest> ls = new List<ModelRequest>();
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/seller/getrequests");
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    ls = JsonConvert.DeserializeObject<List<ModelRequest>>(data.Result);
                }
            }
            return PartialView(ls);
        }
        public ActionResult TakeCare(string type, int id, int iduser)
        {
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/seller/takecare/" + type + "/" +id + "/" + iduser + "/" + Session["iduser"]);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                }
            }
            
            return RedirectToAction("Index", "SellerArea");
        }
        public ActionResult LeaveCare(string type, int id, int iduser)
        {
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync("https://localhost:44335/api/seller/leavecare/" + type + "/" + id + "/" + iduser + "/" + Session["iduser"]);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                }
            }
            return RedirectToAction("Index", "SellerArea");

        }
    }
}