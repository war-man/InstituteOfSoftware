using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: Teaching/StudentDataKeep

        //创建一个用于操作数据的实体
        StudentDataKeepAndRecordBusiness s_Entity = new StudentDataKeepAndRecordBusiness();

        //这是一个数据备案的主页面

        public ActionResult StudentDataKeepIndex()
        {
            return View();
        }

        //往数据库中获取数据备案的信息
        public ActionResult GetStudentPutOnRecordData()
        {
            List<StudentPutOnRecord> Get_List_studentPutOnRecord =s_Entity.GetList();//获取了数据库中所有数据备案信息;

            return null;
        }
    }
}