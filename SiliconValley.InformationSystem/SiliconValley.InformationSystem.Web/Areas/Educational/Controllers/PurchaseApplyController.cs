using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using System.Text;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class PurchaseApplyController : Controller
    {
        public static readonly PurchaseTypeManeger PurchaseType_Entity = new PurchaseTypeManeger();
        public static readonly EmployeesInfoManage EmployeesInfo_Entity = new EmployeesInfoManage();
        public static readonly ShppingDetailManeger ShppingDetail_Entity = new ShppingDetailManeger();
        public static readonly GoodsManeger Goods_Entity = new GoodsManeger();
        public static readonly StockInfoManeger StockInfo_Entity = new StockInfoManeger();
        public static readonly PurchaseApplyManeger PurchaseApply_Entity = new PurchaseApplyManeger();
        // GET: /Educational/PurchaseApply/MyDeleteData
        public ActionResult PurchaseApplyIndexView()
        {
            return View();
        }

        //这是一个采购入库方法
        public ActionResult AddView()
        {
           List<SelectListItem> p_list= PurchaseType_Entity.GetList().Where(p => p.IsDelete == false).Select(p => new SelectListItem() { Text = p.TypeName, Value = p.Id.ToString() }).ToList();
            p_list.Add(new SelectListItem() { Text = "请选择", Value = "0", Selected = true });
            ViewBag.type = p_list;
            ViewBag.Shenqi = EmployeesInfo_Entity.GetEntity("201908150001").EmpName;
            //获取登录的人员的员工编号
            ViewBag.UserId = "201908150001";
            return View();
        }
        

        public ActionResult GetdetailsData(int limit,int page)
        {
           string emp= "201908150001";//操作人员工编号
            List<ShppingDetail> s_list= ShppingDetail_Entity.GetList().Where(s => s.PurchaseApply_Id == null && s.Temporary==true && s.EmpId== emp).ToList();
            var mydata = s_list.Skip((page-1)*limit).Take(limit).Select(s => new {
                Id=s.Id,
                Goods_Id = Goods_Entity.GetEntity(s.Goods_Id).GoodsName,
                GoodsCount=s.GoodsCount,
                Rmark=s.Rmark,
                Company=s.Company
            }).ToList();
            var jsondata = new {
                code=0,
                msg="",
                count= s_list.Count,
                data= mydata
            };
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        //添加采购详情数据页面
        public ActionResult AddPurchaseApplyDetailsData(int? id)
        {
            //获取所有库存的物品数据
            List<SelectListItem> g_list= Goods_Entity.GetList().Where(g=>g.IsDel==false).Select(g=>new SelectListItem() { Text=g.GoodsName,Value=g.Id.ToString()}).ToList();
            g_list.Add(new SelectListItem() {Text="--请选择--",Value="0",Selected=true });
            ViewBag.goods = g_list;
            if (id>0)
            {
                //编辑页面
                ShppingDetail find_s = ShppingDetail_Entity.GetEntity(id);
                return View(find_s);
            }
            else
            {
                //添加页面
                return View();
            }
            
        }

        //添加或编辑详情数据的方法
        public ActionResult AddorEditfunction(ShppingDetail p)
        {
            try
            {
                if (p.Id > 0)
                {
                    //编辑
                    ShppingDetail find_s= ShppingDetail_Entity.GetEntity(p.Id);
                    find_s.GoodsCount = p.GoodsCount;
                    find_s.Goods_Id = p.Goods_Id;
                    find_s.Rmark = p.Rmark;
                    ShppingDetail_Entity.Update(find_s);
                    return Json("oo", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //添加
                    p.IsDelete = false;
                    p.Temporary = true;//目前还只是临时数据，因为还没有真正的提交
                    p.EmpId = "201908150001";//操作人员工编号
                    ShppingDetail_Entity.Insert(p);
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch (Exception)
            {
                return Json("操作失败！！！", JsonRequestBehavior.AllowGet);
            }
            
        }

        //删除一个采购详情方法
        public ActionResult DeleteFunction(int id)
        {
            try
            {
                ShppingDetail find_s = ShppingDetail_Entity.GetEntity(id);
                ShppingDetail_Entity.Delete(find_s);
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("系统错误，请重试", JsonRequestBehavior.AllowGet);
            }
            
        }


       //添加采购数据
       public ActionResult AddPurchaseApplyFunction(PurchaseApply p)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                //首先查看数据是否有正在操作的数据
                 bool iswarth=  PurchaseApply_Entity.IsWartherData();
                if (iswarth)
                {
                    //有正在操作的数据
                    return Json("有人正在操作，请稍等！！！", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string empid = "201908150001";//操作人员工编号
                    List<ShppingDetail> s_list = ShppingDetail_Entity.GetList().Where(s => s.PurchaseApply_Id == null && s.EmpId== empid).ToList();//查看是否有
                    if (s_list.Count<=0)
                    {
                        return Json("请添加采购详情！！！", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //首先向采购表添加一条数据
                        p.IsDelete = false;
                        p.AddDate = DateTime.Now;
                        p.IsWhether = true;
                        bool Isok = PurchaseApply_Entity.AddData(p);
                        if (Isok)
                        {
                            int fistid = PurchaseApply_Entity.GetList().OrderByDescending(pp => pp.Id).First().Id;
                            //将采购详情绑定该采购Id

                            bool ss = ShppingDetail_Entity.MyUpdateData(fistid, s_list);
                            if (ss)
                            {
                                //成功(将是否操作数据属性修改为false)
                                PurchaseApply find_p = PurchaseApply_Entity.GetEntity(fistid);
                                find_p.IsWhether = false;
                                PurchaseApply_Entity.Update(find_p);
                                //修改库存数
                                sb.Append("ok");                               
                            }
                            else
                            {
                                //失败
                                sb.Append("系统错误，请重试!!!");                               
                            }
                        }
                        else
                        {
                            sb.Append("系统错误，请重试!!!");                            
                        }
                    }
                     
                }
                return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("系统错误，请重试!!!", JsonRequestBehavior.AllowGet);
            }
            
        }

        //放弃数据
       public ActionResult MyDeleteData()
        {
            //获取当前登录人员的员工编号
            string empId = "201908150001";
            //删除临时数据
           List<ShppingDetail> s_list= ShppingDetail_Entity.GetList().Where(s => s.EmpId == empId && s.Temporary == true).ToList();
           bool Is= ShppingDetail_Entity.DeleteList(s_list);
            if (Is)
            {
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("系统错误，请重试", JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}