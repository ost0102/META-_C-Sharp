using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Newtonsoft.Json;
using META_DATA_API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace META_DATA_API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            
            if (_DataHelper.CheckDataBaseConnecting())
            {
                ViewBag.Title = "DB Connection Success";
            }
            else
            {
                ViewBag.Title = "DB Connection Failed";
            }

            return View();
        }
    }
}
