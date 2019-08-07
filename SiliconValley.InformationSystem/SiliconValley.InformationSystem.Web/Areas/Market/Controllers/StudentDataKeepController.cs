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
using SiliconValley.InformationSystem.Business.StuSatae_Maneger;//获取学生状态实体
using SiliconValley.InformationSystem.Business.StuInfomationType_Maneger;//获取学生信息来源实体
namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: Market/StudentDataKeep

        //创建一个用于操作数据的备案实体
        StudentDataKeepAndRecordBusiness s_Entity = new StudentDataKeepAndRecordBusiness();
        //创建一个用于查询数据的上门学生状态实体
        StuStateManeger Stustate_Entity = new StuStateManeger();
        //创建一个用于查询数据的上门学生信息来源实体
        StuInfomationTypeManeger StuInfomationType_Entity = new StuInfomationTypeManeger();
        //这是一个数据备案的主页面

        public ActionResult StudentDataKeepIndex()
        {
            return View();
        }

        //往数据库中获取数据备案的信息
        public ActionResult GetStudentPutOnRecordData(int limit,int page)
        { 
             
            try
            {
                int SunLimit = s_Entity.GetList().Count;//总行数
                int SunPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(SunLimit / limit)));//总页数
                IQueryable<StudentPutOnRecord> stu_IQueryable = s_Entity.GetIQueryable();              
                List<StudentPutOnRecord> PageData= s_Entity.GetPagination<StudentPutOnRecord>(stu_IQueryable, page,limit,"Id","desc",ref SunLimit, ref SunPage); //分页
                var Get_List_studentPutOnRecord = PageData.Select(s => new {
                    Id = s.Id,
                    StuName = s.StuName,
                    StuSex = s.StuSex,
                    StuBirthy = s.StuBirthy,
                    StuPhone = s.StuPhone,
                    StuSchoolName = s.StuSchoolName,
                    StuEducational = s.StuEducational,
                    StuAddress = s.StuAddress,
                    StuWeiXin = s.StuWeiXin,
                    StuQQ = s.StuQQ,
                    StuInfomationType_Id = GetStuInfomationTypeValue(s.StuInfomationType_Id),
                    StuStatus_Id = GetStuStatuValue(s.StuStatus_Id),
                    StuIsGoto =s.StuIsGoto,
                    StuVisit =s.StuVisit,
                    EmployeesInfo_Id =1,
                    StuDateTime =s.StuDateTime,
                    StuEntering =s.StuEntering,
                    Reak =s.Reak

                });//获取了数据库中所有数据备案信息;                                                                                          
                
                var JsonData = new {     
                    code=0, //解析接口状态,
                    msg="", //解析提示文本,
                    count= SunLimit, //解析数据长度
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
                                         
        //获取外键的值                   
        #region
        public string GetStuStatuValue(int? id)
        {
            List<StuStatus> Get_List_stustate = Stustate_Entity.GetList();//获取上门学生状态所有数据
            return Get_List_stustate.Where(s => s.Id == id).FirstOrDefault().StatusName;
        }

        public string GetStuInfomationTypeValue(int? id)
        {
            List<StuInfomationType> Get_List_stuInfomationtype = StuInfomationType_Entity.GetList();
            return Get_List_stuInfomationtype.Where(s => s.Id == id).FirstOrDefault().Name;
        }

        #endregion
    }
}