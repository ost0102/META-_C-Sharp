using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using ELVIS_META_COMMON.Controller;
using ELVIS_META_COMMON.YJIT_Utils;
using System.Net;
using System.Text;
using System.IO;

namespace ELVIS_META_WEB.Controllers
{
    public class ImportController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sea()
        {
            return View();
        }
        public ActionResult Air()
        {
            return View();
        }
    }
}