using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.Common;//获取日志实体

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
             
            try
            {
                List<StudentPutOnRecord> Get_List_studentPutOnRecord =s_Entity.GetList();//获取了数据库中所有数据备案信息;
                var JsonData = new {
                    code=0, //解析接口状态
                    msg="", //解析提示文本
                    count= Get_List_studentPutOnRecord.Count, //解析数据长度
                    data= Get_List_studentPutOnRecord //解析数据列表
                };
                return Json(JsonData,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //将错误填写到日志中
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据异常);
                return Json(Error("加载数据有误，请联系开发人员:唐敏--电话:13204961361"),JsonRequestBehavior.AllowGet);
            }                 
        }
    }
}