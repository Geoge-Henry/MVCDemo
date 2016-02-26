using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVCDemo.Models
{
    //该类用于获取用户请求的具体信息,便于前端显示
    public class ShowContents
    {
        //定义该结构体用于获取用户请求的具体信息
        public struct ContentStruct
        {
            public string URL { get; set; }                             //获取URL
            public string Method { get; set; }                          //获取传输方法GET、POST等
            public string HostIp { get; set; }                          //获取请求的主机IP地址
            public string ContentType { get; set; }                     //获取请求的内容类型
            public string Port { get; set; }                             //获取请求的端口号
            public int TotalBytes { get; set; }                         //获取请求内容的字节数（流量）
            public string RequestContents { get; set; }                 //获取请求内容
            public Dictionary<string, string> RequestHeader { get; set; }   //获取头部详细信息
        }

        //调用方法获取请求具体信息,并返回结果
        public ContentStruct GetRequestResult(HttpRequestBase Request)     
        {
            ContentStruct RequestContent = GetHttpContent(Request);
            return RequestContent;
        }

        //该方法获取http请求信息
        public ContentStruct GetHttpContent(HttpRequestBase Request)    
        {
            string Url = Request.Url.ToString();
            string Method = Request.HttpMethod.ToString();
            string HostIp = Request.UserHostAddress;
            string ContentType = Request.ContentType;
            string Port = Request.Url.Port.ToString();
            int TotalBytes = Request.TotalBytes;
            Stream RequestStream = Request.InputStream;
            StreamReader MyStream = new StreamReader(RequestStream, Encoding.UTF8);
            string RequestContents = MyStream.ReadToEnd();
            Dictionary<string, string> RequestHeader = new Dictionary<string, string>();
            foreach (string HeadersKey in Request.Headers.Keys)
            {
                RequestHeader.Add(HeadersKey, Request.Headers.Get(HeadersKey).ToString());
            }
            ContentStruct RequestContent = new ContentStruct();
            RequestContent.URL = Url;
            RequestContent.Method = Method;
            RequestContent.HostIp = HostIp;
            RequestContent.ContentType = ContentType;
            RequestContent.Port = Port;
            RequestContent.TotalBytes = TotalBytes;
            RequestContent.RequestHeader = RequestHeader;
            RequestContent.RequestContents = RequestContents;
            return RequestContent;
        }

    }
}