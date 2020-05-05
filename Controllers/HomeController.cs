using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace APIMVCDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult API()
        {
            return View();
        }
        
        public ActionResult RSS()
        {
            List<RssNews> newsList = ReadNews("https://www.espn.com/espn/rss/news");
            return View(newsList);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static List<RssNews> ReadNews(string url)
        {
            var webResponse = WebRequest.Create(url).GetResponse();
            if (webResponse == null)
                return null;
            var ds = new DataSet();
            ds.ReadXml(webResponse.GetResponseStream());

            var news = (from row in ds.Tables["item"].AsEnumerable()
                        select new RssNews
                        {
                            Title = row.Field<string>("title"),
                            Image_URL = row.Field<string>("image"),
                            Description = row.Field<string>("description"),
                            Link = row.Field<string>("link")
                        }).ToList();
            return news;
        }
    }

    public class RssNews
    {
        public string Title { get; set; }
        public string Image_URL { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}