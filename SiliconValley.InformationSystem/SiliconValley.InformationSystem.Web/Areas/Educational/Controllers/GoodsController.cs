using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class GoodsController : Controller
    {
        // GET: /Educational/Goods/DeleteGoods
        public static readonly GoodsManeger Goods_Entity = new GoodsManeger();
        public static readonly ShppingDetailManeger ShppingDetail_Entity = new ShppingDetailManeger();
        public static readonly StockInfoManeger Stockinfo_Entity = new StockInfoManeger();
        public ActionResult GoodsIndexView()
        {
            return View();
        }
        /// <summary>
        /// 获取table 数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult GetGoodsData(int limit,int page)
        {
            List<Goods> g_list=  Goods_Entity.GetList().Where(g => g.IsDel == false).ToList();
            var mydata = g_list.OrderByDescending(g => g.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var jsondata = new { code = 0,msg = "",count=g_list.Count,data=mydata };
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加或编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GoodsAddorEditView(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {

                //编辑页面
               string[] myidlist= id.Split(',');//如果长度大于1，说明该物品名称不能编辑
                if (myidlist.Length>1)
                {
                    ViewBag.you = "you";
                }
                int myid = Convert.ToInt32(myidlist[0]);
                Goods find_g= Goods_Entity.GetEntity(myid);
                return View(find_g);
            }
            else
            {
                //添加页面
                return View();
            }
        }
        /// <summary>
        /// 添加或编辑物品表方法
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GoodsAddorEditFunction(Goods g)
        {
            try
            {
                //名字对象
               var count= Goods_Entity.GetList().Where(gg => gg.GoodsName == g.GoodsName).FirstOrDefault();                
                //数据库存在同样对象
                if (count!=null)
                {
                    //编辑
                    if (g.Id > 0)
                    {
                        if (g.Id !=count.Id)
                        {
                            return Json("已有该名称!!!", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Goods_Entity.NewMethod(g);
                            return Json("ok", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("已有该名称!!!", JsonRequestBehavior.AllowGet);
                    }
                   
                }
                else
                {
                    if (g.Id > 0)
                    {
                        Goods_Entity.NewMethod(g);
                    }
                    else
                    {
                        //添加
                        g.IsDel = false;
                        Goods_Entity.Insert(g);
                    }
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                 
            }
            catch (Exception)
            {
                return Json("系统错误,请重试!!!", JsonRequestBehavior.AllowGet);
            }             
        }
        

        /// <summary>
        /// 检测该数据是否被其他数据使用
        /// </summary>
        /// <param name="id">物品Id</param>
        /// <returns></returns>
        public ActionResult IsYingyong(int id)
        {
            //查看采购详情里是否有这个物品
            int count1= ShppingDetail_Entity.GetList().Where(s => s.Goods_Id == id).ToList().Count;
            //查看仓库里面是否有这个物品
            int count2 = Stockinfo_Entity.GetList().Where(s => s.goods_Id == id).ToList().Count;

            if (count1<=0 && count2<=0)
            {
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            else 
            {
                return Json("no", JsonRequestBehavior.AllowGet);
            }
            
        }
        //删除物品
        public ActionResult DeleteGoods(int id)
        {
            try
            {
              Goods find_g=  Goods_Entity.GetEntity(id);
                Goods_Entity.Delete(find_g);
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
            }
        }
    }
}