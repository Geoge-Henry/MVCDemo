using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDemo.Models;

namespace MVCDemo.Controllers
{
    public class LoginController : Controller
    {
        //接收防火墙GET请求,进入登录页面
        [HttpGet]
        public ActionResult Index()
        {
            HttpContext.Session.Clear();                //情况服务器session

            //if (Request.Url.ToString().Contains("?message="))
            //{
            //    ViewData["errormessage"] = Request.Url.ToString().Substring(Request.Url.ToString().IndexOf("?")+9);
            //    return Content("<script>window.location.href='http://mymvclogindemo.top';</script>");
            //}
            return View();
        }

    }
}
