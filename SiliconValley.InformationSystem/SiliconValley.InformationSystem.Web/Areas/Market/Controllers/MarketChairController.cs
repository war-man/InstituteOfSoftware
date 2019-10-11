using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.MarketChair_Business;//获取市场讲座实体
using SiliconValley.InformationSystem.Business.EmployeesBusiness;//获取员工实体
using SiliconValley.InformationSystem.Business.Common;//获取日志实体
using SiliconValley.InformationSystem.Business.Base_SysManage;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class MarketChairController : Controller
    {
        #region 创建业务实体
        //获取当前上传的操作人
        string UserName = Base_UserBusiness.GetCurrentUser().UserName;
        MarketChairManeger MarketChair_Entity = new MarketChairManeger();
        EmployeesInfoManage Employes_Entity = new EmployeesInfoManage();
        #endregion
        #region 获取外键值
        /// <summary>
        /// 根据id获取员工表的员工名称
        /// </summary>
        /// <param name="id">员工id</param>
        /// <returns></returns>
        public string GetEmployesName(string id)
        {
            EmployeesInfo finde= Employes_Entity.GetEntity(id);
            if (finde != null)
            {
                return finde.EmpName;
            }
            else
            {
                return "未找到";
            }
        }
        #endregion
        /// <summary>
        /// 删除没有意义的数据
        /// </summary>
        public void RmoveData()
        {
            List<MarketChair> Mc = MarketChair_Entity.GetList().Where(m => m.ChairName == null && m.ManCount == null && m.TerCharName == null).ToList();
            if (Mc.Count > 0)
            {
                foreach (MarketChair m in Mc)
                {
                    MarketChair_Entity.Delete(m);
                }
            }
        }
        // GET: /Market/MarketChair/MarketChairIndex
        public ActionResult MarketChairIndex()
        {
            return View();
        }
        /// <summary>
        /// 获取市场讲座的所有数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListMarketDataFunction(int limit,int page,string JangzMan,string StarTime,string EndTime,string MCount)
        {
            try
            {
                RmoveData();
                List<MarketChair> MC_List = MarketChair_Entity.GetList();
                if (!string.IsNullOrEmpty(JangzMan))
                {
                    MC_List= MC_List.Where(m => m.ChairName.Contains(JangzMan)).ToList();
                }
                if (!string.IsNullOrEmpty(StarTime))
                {
                    MC_List= MC_List.Where(m => m.ChairTime >= Convert.ToDateTime(StarTime)).ToList();
                }
                if (!string.IsNullOrEmpty(EndTime))
                {
                    MC_List= MC_List.Where(m => m.ChairTime <= Convert.ToDateTime(EndTime)).ToList();
                }
                if (!string.IsNullOrEmpty(MCount))
                {
                    MC_List= MC_List.Where(m => m.ManCount== Convert.ToInt32(MCount)).ToList();
                }
               
                var ChangeJson = MC_List.Select(m => new {
                    Id = m.Id,
                    TerCharName = m.TerCharName,
                    ChairName = m.ChairName,
                    ManCount = m.ManCount,
                    ChairAddress = m.ChairAddress,
                    ChairTime = m.ChairTime,
                    Employees_Id = m.Employees_Id,
                    Rmark = m.Rmark,
                    IsDelete = m.IsDelete,
                    EmpName = GetEmployesName(m.Employees_Id)
                }).ToList().OrderByDescending(m => m.Id).Skip((page - 1) * limit).Take(limit);

                var JsonData = new
                {
                    code = 0, //解析接口状态,
                    msg = "", //解析提示文本,
                    count = MC_List.Count, //解析数据长度
                    data = ChangeJson //解析数据列表
                };
                return Json(JsonData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //将错误填写到日志中     
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json("加载数据有误", JsonRequestBehavior.AllowGet);
            }
           
        }
        /// <summary>
        /// 添加一行文本框让用户填写数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AddMarketDataFunction()
        {
            MarketChair findm = MarketChair_Entity.GetList().Where(m => m.ChairName == null || m.TerCharName == null || m.ChairAddress==null || m.ManCount<=0).FirstOrDefault();
            if (findm==null)
            {
                try
                {
                    DateTime d1 = DateTime.Now;
                    MarketChair new_M = new MarketChair() { ChairTime = d1, Employees_Id = "201908150001", IsDelete = false, TerCharName = "", ManCount = 0, ChairAddress = "" };
                    MarketChair_Entity.Insert(new_M);
                    BusHelper.WriteSysLog("操作人:" + UserName + "触发了添加按钮" , Entity.Base_SysManage.EnumType.LogType.添加数据);
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                    return Json("数据有误，请重试!", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {                
                return Json("有信息未补充完，请先填写！！！", JsonRequestBehavior.AllowGet);               
            }                        
        }
        [HttpPost]
        public ActionResult EditMarketDataFunction()
        {
            try
            {
                int Id = Convert.ToInt32(Request.Form["Id"]);
                string Value = Request.Form["Values"];
                string KeyName = Request.Form["KeyName"];
                MarketChair FINDMC = MarketChair_Entity.GetEntity(Id);
                switch (KeyName)
                {
                    case "ChairName":
                        FINDMC.ChairName = Value;
                        break;
                    case "TerCharName":
                        FINDMC.TerCharName = Value;
                        break;
                    case "ChairAddress":
                        FINDMC.ChairAddress = Value;
                        break;
                    case "ChairTime":
                        FINDMC.ChairTime = Convert.ToDateTime(Value);
                        break;
                    case "Rmark":
                        FINDMC.Rmark = Value;
                        break;
                    case "ManCount":
                        FINDMC.ManCount =Convert.ToInt32( Value);
                        break;
                    case "IsDelete":
                        FINDMC.IsDelete = Value == "false" ? true : false;
                        break;
                }
                MarketChair_Entity.Update(FINDMC);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("操作人:" + UserName + "操作时出现:" + ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                return Json("数据有误，请重试!", JsonRequestBehavior.AllowGet);
            }

            return Json("ok", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 这是一个线性图
        /// </summary>
        /// <returns></returns>
        public ActionResult ThisIsImageDataView()
        {
            return View();
        }
        ///Market/MarketChair/GetImageDataFunction
        public ActionResult GetImageDataFunction()
        {
            //获取所有年份
            object Year_list = MarketChair_Entity.GetList().Select(a =>Convert.ToDateTime(a.ChairTime).Year).ToList().Distinct().ToList();
        //    object Year_list = MarketChair_Entity.GetList().Select(a => a.ChairTime.ToString().Substring(0, 4)).ToList().Distinct().ToList();
            var ss = Year_list;
            return null;
        }
        /// <summary>
        /// 这是一个柱状图
        /// </summary>
        /// <returns></returns>
        public ActionResult ThisIsZhuZhuangImageView()
        {
            return View();
        }

   }
}