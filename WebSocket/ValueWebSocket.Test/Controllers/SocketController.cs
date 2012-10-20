using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ValueWebSocket.Infrastructure;
using ValueWebSocket.Test.Basic;
using ValueWebSocket.Session;

namespace ValueWebSocket.Test.Controllers
{
    public class SocketController : Controller
    {
        //
        // GET: /Socket/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateUser()
        {
            ViewData["users"] = getUserList();
            return PartialView();
        }

        private List<SelectListItem> getUserList()
        {
            var session = SessionManager.Sessions;
            List<SelectListItem> userlist = new List<SelectListItem>();
            userlist.Add(new SelectListItem
            {
                Value = "All",
                Text = "All"
            });

            foreach (var item in session)
            {
                userlist.Add(new SelectListItem
                {
                    Text = item.Cookies["name"],
                    Value = item.Cookies["name"]
                });
            }
            return userlist;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collect)
        {
            String userName = collect["userName"];
            Response.SetCookie(new HttpCookie("name", userName));
            return RedirectToAction("Index");
        }
    }
}
