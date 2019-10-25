using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using System.Transactions;
    public class GoodsProvideController : Controller
    {        
            public static readonly GoodsProvideManeger GoodsProvide_Entity = new GoodsProvideManeger();
            public static readonly StockInfoManeger StockInfo_Entity = new StockInfoManeger();

        // GET: /Educational/GoodsProvide/CountTure
        public ActionResult GoodsProvideIndexView()
        {
            List<SelectListItem> list_Sele1 = StockInfo_Entity.Get_All_StockGoods("").Select(s => new SelectListItem() { Text = s.GoodsName, Value = s.Id.ToString() }).ToList();
            list_Sele1.Add(new SelectListItem() { Text="--请选择--",Value="0",Selected=true});
            ViewBag.goods = list_Sele1;
            List<SelectListItem> list_Sele2 = GoodsProvideManeger.EmployeesInfo_Entity.GetList().Select(e => new SelectListItem() { Text = e.EmpName, Value = e.EmployeeId }).ToList();
            list_Sele2.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.emplod = list_Sele2;
            return View();
        }

        public ActionResult GetTableData(int limit,int page)
        {
            List<GoodsProvide> all_list = GoodsProvide_Entity.GetList();
            string shenheVluae= Request.QueryString["shenhe"];//1---未审核,0---已审核

            string mygoodsname = Request.QueryString["mygoodsname"];
            string shenqingMan = Request.QueryString["shenqingMan"];
            string stardate = Request.QueryString["stardate"];
            string enddate = Request.QueryString["enddate"];
            if (shenheVluae=="1")
            {
                //加载未审核的数据
                all_list = all_list.Where(a => a.IsDel == false).OrderByDescending(g => g.UseDate).ToList();//根据申请日期排序
                 
            }
            else if(shenheVluae == "0")
            {
                //加载已审核的数据
                all_list = all_list.Where(a => a.IsDel == true).OrderByDescending(g => g.ShenheDate).ToList();//根据审核日期排序
            }
            if (!string.IsNullOrEmpty(mygoodsname) && mygoodsname!="0")
            {
                int goods = Convert.ToInt32(mygoodsname);
                all_list = all_list.Where(g => g.StockInfo_Id == goods).ToList();
            }
            if (!string.IsNullOrEmpty(shenqingMan) && shenqingMan!="0")
            {
                all_list = all_list.Where(g => g.EmployeesInfo_Id == shenqingMan).ToList();
            }
            if (!string.IsNullOrEmpty(stardate))
            {
                DateTime d1 = Convert.ToDateTime(stardate);
                all_list = all_list.Where(g => g.UseDate >= d1).ToList();
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                DateTime d2 = Convert.ToDateTime(enddate);
                all_list = all_list.Where(g => g.UseDate <= d2).ToList();
            }
            var mydata = all_list.Skip((page - 1) * limit).Take(limit).Select(g => new
            {
                Id=g.Id,
                StockInfo_Id=g.StockInfo_Id,
                EmployeesInfo_Id=g.EmployeesInfo_Id,
                UseCount=g.UseCount,
                UseDate = g.UseDate,
                ReturnDate=g.ReturnDate,
                Rmark=g.Rmark,
                EmployeesInfoName=GoodsProvideManeger.EmployeesInfo_Entity.FindEmpData(g.EmployeesInfo_Id,true).EmpName,
                GoodsName =GoodsProvide_Entity.GetGoodsName(g.StockInfo_Id).GoodsName,
                IsDel=g.IsDel,
                ShenheDate=g.ShenheDate,
                IsReturn= GoodsProvide_Entity.GetGoodsName(g.StockInfo_Id).IsReturn
            }).ToList();
            var jsondata = new { code=0,count=all_list.Count,data=mydata};
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddorEditFunction(GoodsProvide new_g)
        {
            try
            {                 
                    if (new_g.Id > 0)
                    {
                        GoodsProvide find_g = GoodsProvide_Entity.GetEntity(new_g.Id);
                        if (find_g != null)
                        {
                            find_g.UseCount = new_g.UseCount;
                            find_g.StockInfo_Id = StockInfo_Entity.GetGoods_Stock(new_g.StockInfo_Id).Id;
                            find_g.Rmark = new_g.Rmark;
                            GoodsProvide_Entity.Update(find_g);
                        }
                        else
                        {
                            return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //添加
                        new_g.EmployeesInfo_Id = "201908160008";//需要判断登陆的人
                        new_g.IsDel = false;
                        new_g.UseDate = DateTime.Now;
                        GoodsProvide_Entity.Insert(new_g);
                    }
                           
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
             
            
        }

        public ActionResult AddorEditView(int? id)
        {
            ViewBag.goods = StockInfo_Entity.Get_All_StockGoods("").Select(s => new SelectListItem() { Text = s.GoodsName, Value = s.Id.ToString() }).ToList();
            if (id>0)
            {                 
                    //编辑页面
                    GoodsProvide find_goodsprovide = GoodsProvide_Entity.GetEntity(id);                    
                    ViewBag.Man = GoodsProvideManeger.EmployeesInfo_Entity.FindEmpData(find_goodsprovide.EmployeesInfo_Id, true).EmpName;
                    ViewBag.Date = find_goodsprovide.UseDate;
                    return View(find_goodsprovide);                 
            }
              //添加页面
              return View();            
        }
        [HttpPost]
        public ActionResult ShenheFunction()
        {
            int id =Convert.ToInt32( Request.Form["Id"]);
            int count = Convert.ToInt32(Request.Form["count"]);
            GoodsProvide find_goodsprovide = GoodsProvide_Entity.GetEntity(id);
            if (find_goodsprovide!=null)
            {
                find_goodsprovide.IsDel = true;
                find_goodsprovide.ShenheDate = DateTime.Now;
                GoodsProvide_Entity.Update(find_goodsprovide);
                //调用方法去库存修改数据
                bool _tre= StockInfo_Entity.UpdateCount(find_goodsprovide.StockInfo_Id, count,false);
                if (_tre)
                {
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    find_goodsprovide.IsDel = false;
                    find_goodsprovide.ShenheDate = null;
                    GoodsProvide_Entity.Update(find_goodsprovide);
                    //获取该物品的库存数
                    StockInfo find_s= GoodsProvideManeger.StockInfo_Entity.GetEntity(find_goodsprovide.StockInfo_Id);
                    if (find_s != null)
                    {                        
                      return Json("该物品的库存数量为"+ find_s.stockcount + "！！！", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("该物品的库存数量不够！！！", JsonRequestBehavior.AllowGet);
                    }
                     
                }                
            }
            else
            {
                return Json("系统错误,请重试！！！", JsonRequestBehavior.AllowGet);
            }
             
        }
        /// <summary>
        /// 用于修改归还时间的页面
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <returns></returns>
        public  ActionResult UpdateReturnDate(int? id)
        {
            ViewBag.goods = StockInfo_Entity.Get_All_StockGoods("").Select(s => new SelectListItem() { Text = s.GoodsName, Value = s.Id.ToString() }).ToList();
            GoodsProvide find_goodsprovide = GoodsProvide_Entity.GetEntity(id);
            ViewBag.Man = GoodsProvideManeger.EmployeesInfo_Entity.FindEmpData(find_goodsprovide.EmployeesInfo_Id, true).EmpName;
            ViewBag.Date = find_goodsprovide.UseDate;
            ViewBag.goods = GoodsProvide_Entity.GetGoodsName(find_goodsprovide.StockInfo_Id).GoodsName;
            ViewBag.usecount = find_goodsprovide.UseCount;
            return View(find_goodsprovide);
        }
        /// <summary>
        /// 用于修改归还时间的方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateReturnFunction()
        {
            try
            {
                int Id = Convert.ToInt32(Request.Form["Id"]);//获取要修改的申请编号

                DateTime returndate = Convert.ToDateTime(Request.Form["ReturnDate"]);//获取归还的时间

                GoodsProvide find_g = GoodsProvide_Entity.GetEntity(Id);//找到要修改的申请物品数据
                if (find_g.ReturnDate == null)
                {
                    //修改库存数
                    bool Is = StockInfo_Entity.UpdateCount(find_g.StockInfo_Id, find_g.UseCount, true);

                    if (Is == false)
                    {
                        return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
                    }
                }

                string Reak = Request.Form["Reark"];//获取说明

                find_g.ReturnDate = returndate;

                if (!string.IsNullOrEmpty(Reak))
                {
                    find_g.Rmark = Reak;
                }

                GoodsProvide_Entity.Update(find_g);

                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception )
            {

                return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public ActionResult CountTure()
        {
            int id = Convert.ToInt32(Request.Form["Id"]);
            int count = Convert.ToInt32(Request.Form["count"]);

           StockInfo find_s= StockInfo_Entity.GetEntity(id);

           if(find_s.stockcount>= count)
            {
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("物品库存数量不够！！！", JsonRequestBehavior.AllowGet);
            }

        }
    }
}