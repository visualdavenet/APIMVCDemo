﻿using APIMVCDemo.CosmosDB;
using APIMVCDemo.Data.BusinessLogic;
using APIMVCDemo.Models;
using Microsoft.Azure.Cosmos.Table;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public async Task<ActionResult> CustomerListCosmoDB()
        {
            List<CustomerEntity> cosmoCustomerList = await GetCustomersCosmo();
            return View(cosmoCustomerList);
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

        public ActionResult SignUp()
        {
            ViewBag.Message = "Customer Sign Up";

            return View();
        }

        public ActionResult CustomerList()
        {
            ViewBag.Message = "Customer List";

            var result = CustomerProcessor.LoadCustomers();
            List<Customer> customer = new List<Customer>();

            foreach (var row in result)
            {
                customer.Add(new Customer
                {
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Occupation = row.Occupation,
                    City = row.City,
                    State = row.State,
                    Email = row.Email,
                    ImageURL = row.ImageURL
                });
            }

            return View(customer);
        }

        public JsonResult GetCustomers()
        {
            ViewBag.Message = "Customer List";

            var result = CustomerProcessor.LoadCustomers();
            List<Customer> customer = new List<Customer>();

            foreach (var row in result)
            {
                customer.Add(new Customer
                {
                    FirstName = row.FirstName,
                    LastName = row.LastName,
                    Occupation = row.Occupation,
                    City = row.City,
                    State = row.State,
                    Email = row.Email,
                    ImageURL = row.ImageURL
                });
            }

            return Json(customer, JsonRequestBehavior.AllowGet);
        }

        public async Task<List<CustomerEntity>> GetCustomersCosmo() 
        {
            CloudTable table = await Common.CreateTableAsync("Customers");
            List<CustomerEntity> entities = table.ExecuteQuery(new TableQuery<CustomerEntity>()).ToList();
            return entities;
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Customer customer)
        {
            if (ModelState.IsValid)
            {
                int recordsCreated = CustomerProcessor.CreateCustomer(
                    customer.FirstName,
                    customer.LastName,
                    customer.Occupation,
                    customer.City,
                    customer.State,
                    customer.Email,
                    customer.ImageURL);

                return RedirectToAction("CustomerList");
            }

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