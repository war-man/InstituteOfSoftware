using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.CapitalView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Capital.Controllers
{
    [CheckLogin]
    public class CapitalController : Controller
    {
        private EmploymentStaffBusiness dbempstaff;
        private StudentDataKeepAndRecordBusiness dbbeian;
        private PrefundingBusiness dbprefunding;
        private PerInfoBusiness dbperinfo;
        private EmployeesInfoManage dbempinfo;
        private DepartmentManage departmentManage;
        private EmployeesInfoManage dbemployeesInfoManage;
        private DebitBusiness dbdebit;

        // GET: Capital/Capital
        public ActionResult PublicBorrowList()
        {
            return View();
        }

        public ActionResult PublicBorrowListData(int page, int limit,string param0)
        {
             dbemployeesInfoManage = new EmployeesInfoManage();
             dbdebit = new DebitBusiness();
            var data= dbdebit.GetAll();
            if (!string.IsNullOrEmpty(param0))
            {
                List< EmployeesInfo> emplist = dbemployeesInfoManage.GetIQueryable().Where(a => a.EmpName.Contains(param0)).ToList();
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < emplist.Count; j++)
                    {
                        if (data[i].EmpNumber != emplist[j].EmployeeId)
                        {
                            if (j== emplist.Count-1)
                            {
                                data.RemoveAt(i);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            List<PublicBorrowListView> result = new List<PublicBorrowListView>();
            dbempstaff = new EmploymentStaffBusiness();
            foreach (var item in data)
            {
                PublicBorrowListView publicBorrowListView = new PublicBorrowListView();
                publicBorrowListView.date = item.date;
                publicBorrowListView.DebitMoney = item.DebitMoney;
                publicBorrowListView.Debitwhy = item.Debitwhy;
                publicBorrowListView.ID = item.ID;

                //拿员工对象
                EmployeesInfo employeesInfo = dbempstaff.GetEmployeesInfoByID(item.EmpNumber);

                //拿岗位对象 
                var PositionInfo = dbempstaff.GetPositionByID(employeesInfo.PositionId);

                //拿部门对象
                var DepInfo = dbempstaff.GetDepartmentByID(PositionInfo.DeptId);

                publicBorrowListView.EmpName = employeesInfo.EmpName;
                publicBorrowListView.DeptName = DepInfo.DeptName;
                publicBorrowListView.PositionName = PositionInfo.PositionName;
                publicBorrowListView.Phone = employeesInfo.Phone;
                result.Add(publicBorrowListView);
            }
            var bnewdata = result.OrderBy(a=>a.date).Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = bnewdata.Count(),
                data = bnewdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DoPrefundingList() {
            return View();
        }

        /// <summary>
        /// 借资/预资页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BorrowMoney()
        {
            //现在采用登陆员工为渠道的员工 
            //存储到session里面的id找到用户对象，用户对象就会有一个属性为员工编号。

            //现在给一个固定数据id为039e3e3891eea-1e885eb2-75a3-4f82-bb9a-570d717f2af4 员工编号为201908160008 姓名：徐宝平 部门：渠道 岗位：主任

            dbempstaff = new EmploymentStaffBusiness();

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            //拿员工对象
            EmployeesInfo employeesInfo = dbempstaff.GetEmployeesInfoByID(user.EmpNumber);
            //EmployeesInfo employeesInfo = dbempstaff.GetEmployeesInfoByID("201908160008");

            //拿岗位对象 
            var PositionInfo = dbempstaff.GetPositionByID(employeesInfo.PositionId);

            //拿部门对象
            var DepInfo = dbempstaff.GetDepartmentByID(PositionInfo.DeptId);

            ViewBag.PositionInfo = PositionInfo;
            ViewBag.DepInfo = DepInfo;
            ViewBag.employeesInfo = employeesInfo;

            dbbeian = new StudentDataKeepAndRecordBusiness();
            //获取的是他这个备案而且是已经报名的学生集合
            List<StudentPutOnRecord> mystudentlist = dbbeian.GetrReport(user.EmpNumber);
            //List<StudentPutOnRecord> mystudentlist = dbbeian.GetrReport("201908160008");
            //获取这个员工的预资所有的预资单
            dbprefunding = new PrefundingBusiness();
            dbperinfo = new PerInfoBusiness();
            var mydata = dbprefunding.GetAll();
            List<PerInfo> mrdperInfos = new List<PerInfo>();
            //根据这个员工的预资单获取他用过的备案编号
            foreach (var item in mydata)
            {
                List<PerInfo> resultdata = dbperinfo.GetPrefundingByID(item.ID);
                mrdperInfos.AddRange(resultdata);
            }
            //排除用过的备案编号
            for (int i = mystudentlist.Count - 1; i >= 0; i--)
            {
                foreach (var item in mrdperInfos)
                {
                    if (mystudentlist[i].Id == item.BeianID)
                    {
                        mystudentlist.Remove(mystudentlist[i]);
                    }
                }
            }
            //存放的是这个员工备案数据已经报名的学生但未用这个学生借资过的。
            ViewBag.Student = Newtonsoft.Json.JsonConvert.SerializeObject(mystudentlist);

            return View();
        }
        /// <summary>
        /// 普通借资
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult PublicBorrow(string EmployeeId, float DebitMoney, string BorrowmoneyReason)
        {
            dbempinfo = new EmployeesInfoManage();
            AjaxResult ajaxResult = new AjaxResult();
            Debit debit = new Debit();
            debit.date = DateTime.Now;
            debit.DebitMoney = DebitMoney;
            debit.Debitwhy = BorrowmoneyReason;
            debit.EmpNumber = EmployeeId;
            debit.IsDel = false;
            debit.Remark = string.Empty;
            try
            {
                dbempinfo.Borrowmoney(debit);
                BusHelper.WriteSysLog("当员工普通借资的时候，位于Market区域ChannelStaffController控制器中PublicBorrow方法，添加成功。", EnumType.LogType.添加数据);
                ajaxResult.Data = "";
                ajaxResult.ErrorCode = 200;
                ajaxResult.Success = true;
                ajaxResult.Msg = "借资成功";
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("当员工普通借资的时候，位于Market区域ChannelStaffController控制器中PublicBorrow方法，添加失败。", EnumType.LogType.添加数据);

            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预资表添加
        /// </summary>
        /// <returns></returns>
        public ActionResult DoPrefunding(string EmployeeId, float DebitMoney, string strng1)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbprefunding = new PrefundingBusiness();
                dbperinfo = new PerInfoBusiness();
                Prefunding myPrefunding = new Prefunding();
                myPrefunding.EmpNumber = EmployeeId;
                myPrefunding.IsDel = false;
                myPrefunding.PerMoney = DebitMoney;

                var datetime = DateTime.Now;
                var baiozhi = datetime.ToFileTimeUtc().ToString();
                myPrefunding.PreDate = datetime;
                myPrefunding.Remark = baiozhi;
                dbprefunding.Insert(myPrefunding);
                BusHelper.WriteSysLog("当员工预资的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加成功。", EnumType.LogType.添加数据);
                ajaxResult = dbprefunding.Success("添加成功");
                dbprefunding = new PrefundingBusiness();
                var mydata = dbprefunding.GetAll();
                var dudu = mydata.Where(a => a.Remark == baiozhi).FirstOrDefault();
                List<string> studentnumber = strng1.Split(',').ToList();
                foreach (var item in studentnumber)
                {
                    PerInfo perInfo = new PerInfo();
                    perInfo.IsDel = false;
                    perInfo.PreID = dudu.ID;
                    perInfo.Remark = string.Empty;
                    perInfo.BeianID = int.Parse(item);
                    try
                    {
                        dbperinfo.Insert(perInfo);
                        BusHelper.WriteSysLog("当员工预资详细表的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加成功。", EnumType.LogType.添加数据);
                        ajaxResult = dbperinfo.Success("添加成功");
                    }
                    catch (Exception)
                    {

                        BusHelper.WriteSysLog("当员工预资详细表的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加失败。", EnumType.LogType.添加数据);
                        ajaxResult = dbperinfo.Error("添加失败");
                    }
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("当员工预资的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加失败。", EnumType.LogType.添加数据);
                ajaxResult = dbprefunding.Error("添加失败");
            }

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}