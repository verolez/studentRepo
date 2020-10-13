using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using BO;

namespace Names.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetNames()
        {
            return Json(StudentLogic.GetNames(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertStudentName(NameObject name)
        {
            return Json(StudentLogic.InsertStudentName(name).Split(','));
        }

        public JsonResult UpdateStudent(NameObject name)
        {
            return Json(StudentLogic.UpdateStudentName(name).Split(','));
        }

        public JsonResult RemoveStudent(int ID)
        {
            return Json(StudentLogic.RemoveStudent(ID).Split(','));
        }
    }
}