using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCFinalExam.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Msg = TempData["msg"];
            return View();
        }
        [System.Web.Mvc.HttpPost]
        public ActionResult Login([FromBody]string mail, string pwd)
        {
            if (HttpContext.User.Identity.Name != "")
            {
                return RedirectToAction("Index", "Home");
            }
            using (var conn = new HttpClient())
            {
                var req = conn.GetAsync(@"https://localhost:44335/api/user/" + mail + "/" + pwd);
                req.Wait();
                var res = req.Result;
                if (res.IsSuccessStatusCode)
                {
                    var data = res.Content.ReadAsStringAsync();
                    data.Wait();
                    FormsAuthentication.SetAuthCookie(mail, false);
                    Session["iduser"] = data.Result.Split(',')[1];
                    Session["usertype"] = data.Result.Split(',')[0];
                    return Session["usertype"].Equals("0") ? RedirectToAction("Index", "Home") : RedirectToAction("Index", "SellerArea");
                }
            }
            ViewBag.Msg = "Login Failed.";
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}