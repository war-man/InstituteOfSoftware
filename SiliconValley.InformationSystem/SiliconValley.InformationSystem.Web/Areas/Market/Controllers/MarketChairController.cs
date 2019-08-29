using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.MarketChair_Business;//获取市场讲座实体
using SiliconValley.InformationSystem.Business.EmployeesBusiness;//获取员工实体
using SiliconValley.InformationSystem.Business.Common;//获取日志实体

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class MarketChairController : Controller
    {
        #region 创建业务实体
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
        // GET: /Market/MarketChair/GetListMarketDataFunction
        public ActionResult MarketChairIndex()
        {
            return View();
        }
        /// <summary>
        /// 获取市场讲座的所有数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetListMarketDataFunction(int limit,int page)
        {
            try
            {
                List<MarketChair> MC_List = MarketChair_Entity.GetList();
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
    }
}