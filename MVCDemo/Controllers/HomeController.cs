using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDemo.Models;
using System.Net;

namespace MVCDemo.Controllers
{
    public class HomeController : Controller
    {
        //处理从防火墙转发来的POST请求,用于验证用户登录
        [HttpPost]
        public ActionResult Index(Login login)
        {
            HttpContext.Session.Clear();                //情况服务器session

            //实例化Operation类,调用方法验证用户登录
            Operation operation = new Operation();      
            string LoginResult = string.Empty;
            LoginResult = operation.LoginCheck(login);

            if (LoginResult == "重定向")               
            {
                string RedirectResult=operation.Redirect(Request);          //如果用户不是从防火墙发过来的信息,则先转由防火墙处理
                return Content(RedirectResult,"text/html");
            }
            if (LoginResult != "登录成功！")                                //如果登录失败,则返回失败报告
                return Content("<script >window.location.href='http://mymvclogindemo.top';</script >", "text/html");

            //实例化ShowContents的类,用于成功登录后显示用户的登录信息和身份
            ShowContents showcontents = new ShowContents();
            ShowContents.ContentStruct contentstruct = showcontents.GetHttpContent(Request);
            HttpContext.Session["HostIp"] = contentstruct.HostIp;
            HttpContext.Session["Port"] = contentstruct.Port;
            HttpContext.Session["Method"] = contentstruct.Method;
            HttpContext.Session["Url"] = contentstruct.URL;
            HttpContext.Session["ContentType"] = contentstruct.ContentType;
            HttpContext.Session["TotalBytes"] = contentstruct.TotalBytes;
            HttpContext.Session["RequestContents"] = contentstruct.RequestContents.Substring(0, contentstruct.RequestContents.IndexOf("&checknum="));
            HttpContext.Session["RequestHeader"] = contentstruct.RequestHeader;
            HttpContext.Response.Write("请求的IP地址： " + HttpContext.Session["HostIp"].ToString() + "</br>");
            HttpContext.Response.Write("请求的端口： " + HttpContext.Session["Port"].ToString() + "</br>");
            HttpContext.Response.Write("请求的方法： " + HttpContext.Session["Method"].ToString() + "</br>");
            HttpContext.Response.Write("请求的URL： " + HttpContext.Session["Url"].ToString() + "</br>");
            HttpContext.Response.Write("请求的内容类型： " + HttpContext.Session["ContentType"].ToString() + "</br>");
            HttpContext.Response.Write("请求内容的字节数： " + HttpContext.Session["TotalBytes"].ToString() + "</br>");
            HttpContext.Response.Write("请求的内容： " + HttpContext.Session["RequestContents"].ToString() + "</br>");
            Dictionary<string, string> Header = (Dictionary<string, string>)HttpContext.Session["RequestHeader"];
            foreach (string HeaderKey in Header.Keys)
                HttpContext.Response.Write(HeaderKey + ": " + Header[HeaderKey] + "</br>");
            return View();
        }

    }
}
