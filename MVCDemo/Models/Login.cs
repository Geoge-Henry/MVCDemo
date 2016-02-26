using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MVCDemo.Models
{
    public class Login
    {
        [Key]
        public int id { get; set; }

        [MaxLength(10)]
        public string username { get; set; }
        //默认string映射到mysql里是longtext类型的，加长度之后就变成varchar了

        //[MinLength(6)]
        //[MaxLength(10)]
        [StringLength(10, MinimumLength = 6)]
        public string password { get; set; }

        //该字段用于验证请求报文是否来自防火墙
        public string checknum { get; set; }

    }

    public interface Userinfo
    {
        //int insert(login login);
        string LoginCheck(Login login);
        //int update(login login);
        //Boolean delete(login login);
    }
    

    //该类用于处理用户信息验证和重定向转发用户请求报文到防火墙等功能
    public class Operation : Userinfo
    {
        //连接数据库
        private static String mysqlcon = ConfigurationManager.ConnectionStrings["MVCDemo"].ConnectionString;
        private MySqlConnection conn;
        private MySqlCommand comm;

        public string LoginCheck(Login login)
        {
            //判断请求信息是否来自防火墙,若不是,则返回重定向
            if (string.IsNullOrEmpty(login.checknum))
                return "重定向";

            try
            {
                conn = new MySqlConnection(mysqlcon);
                if (conn.State == System.Data.ConnectionState.Open)
                    return "数据库错误！";
                conn.Open();
                comm = new MySqlCommand();
                comm.CommandText = "select * from logincheck where username=" + "\"" + login.username + "\"" +
                    "and password=" + "\"" + login.password + "\"";
                comm.Connection = conn;
                comm.ExecuteNonQuery();
                MySqlDataReader SelectResult = comm.ExecuteReader();
                if (!SelectResult.Read())
                    return "账号或密码错误！请重试！";
                else
                        return "登录成功！";
            }
            catch
            {
                return "数据库错误！";
            }
            finally
            {
                comm.Dispose();
                conn.Close();
            }
        }

        //重定向发送请求报文到防火墙
        public string Redirect(HttpRequestBase Request)
        {
            Stream RequestStream = Request.InputStream;
            StreamReader MyStream = new StreamReader(RequestStream, Encoding.UTF8);
            string MyRequestString = MyStream.ReadToEnd();
            string postData = MyRequestString;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            string url = "http://mymvclogindemo.top/Home";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webRequest.Method = "POST";
            webRequest.ContentType =Request.ContentType;
            webRequest.ContentLength = byteArray.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            HttpWebResponse ResponseList = (HttpWebResponse)webRequest.GetResponse();
            StreamReader Message = new StreamReader(ResponseList.GetResponseStream(), Encoding.UTF8);
            string ResponseResult = Message.ReadToEnd();
            return ResponseResult;
        }


    }

}