﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class ConvertCourseController : Controller
    {
        // GET: Educational/ConvertCourse
        public ActionResult ConvertCourseIndexView()
        {
            return View();
        }

        public ActionResult ConvertTableData(int limit,int page)
        {
            return View();
        }
    }
}