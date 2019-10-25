using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Entity.Entity;

    public class StockInfoController : Controller
    {
            public static readonly StockInfoManeger StockInfo_Entity = new StockInfoManeger();
        
        // GET: /Educational/StockInfo/AddorEdit
        public ActionResult StockInfoIndexView()
        {
            return View();
        }

        /// <summary>
        /// 获取物品类型的树形数据
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadTree_GoodsTypeData()
        {
            List<TreeClass> list_Tree= StockInfo_Entity.GetTreeData(0);      
            return Json(list_Tree, JsonRequestBehavior.AllowGet);
        }
        /// 获取库存数据
        public ActionResult GetStockInfo_data(int limit,int page)
        {
            List<StockInfo> stockinfo_list = StockInfo_Entity.Get_All_Data();
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id) && id!="0")
            {
                int Id = Convert.ToInt32(id);
                List<GoodsType> g_list= StockInfo_Entity.Get_child(Id);
                List<StockInfo> s = new List<StockInfo>();
                foreach (var item2 in g_list)
                {
                    foreach (var item3 in stockinfo_list)
                    {
                        if (item3.GoodsType_Id== item2.Id)
                        {
                            s.Add(item3);
                        }
                    }
                }
                stockinfo_list = s;
            }
            string goodsname = Request.QueryString["GoodsName"];
            if (!string.IsNullOrEmpty(goodsname))
            {
                List<Goods> some_goodsname= StockInfo_Entity.Get_SomeGoodsData(goodsname);
                List<StockInfo> s2 = new List<StockInfo>();
                foreach (var item4 in some_goodsname)
                {
                    foreach (var item5 in stockinfo_list)
                    {
                        if (item5.goods_Id==item4.Id)
                        {
                            s2.Add(item5);
                        }
                    }
                }

                stockinfo_list = s2;
            }
            var jsondata = stockinfo_list.Skip((page - 1) * limit).Take(limit).OrderByDescending(s => s.Id).Select(s => new {
                goods_Id = s.goods_Id,
                GoodsType_Id = s.GoodsType_Id,
                stockcount = s.stockcount,
                Address = s.Address,
                Rmark = s.Rmark,
                IsDel = s.IsDel,
                GoodsTypeName = StockInfoManeger.GoodsType_Entity.GetSingleGoodsType(s.GoodsType_Id.ToString(), true)==null?"无": StockInfoManeger.GoodsType_Entity.GetSingleGoodsType(s.GoodsType_Id.ToString(), true).GoodsTypeName,
                GoodsName = StockInfoManeger.Goods_Entity.GetSingleGoods(s.goods_Id.ToString(), true)==null?"无": StockInfoManeger.Goods_Entity.GetSingleGoods(s.goods_Id.ToString(), true).GoodsName
            });

            var mydata = new { code=0,mas="",count= stockinfo_list .Count,data=jsondata};

            return Json(mydata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddorEdit()
        {
            try
            {
                int farther_id = Convert.ToInt32(Request.Form["Id"]);
                string name = Request.Form["Name"];
                bool Is = Convert.ToBoolean(Request.Form["Tiaojian"]);
                if (Is)
                {
                   GoodsType ff= StockInfoManeger.GoodsType_Entity.GetSingleGoodsType("未命名", false);
                    if (ff==null)
                    {
                        //添加
                        GoodsType new_g = new GoodsType() { GoodsTypeName = "未命名", FarTher_Id = farther_id, AddTime = DateTime.Now, IsDelete = false };
                        StockInfoManeger.GoodsType_Entity.Insert(new_g);
                    }
                    else
                    {
                        return Json("有未命名的数据,请填充！！！", JsonRequestBehavior.AllowGet);
                    }
                     
                }
                else
                {
                    //编辑
                    GoodsType find_goodstype = StockInfoManeger.GoodsType_Entity.GetSingleGoodsType(farther_id.ToString(), true);
                    find_goodstype.GoodsTypeName = name;
                    StockInfoManeger.GoodsType_Entity.Update(find_goodstype);
                }
                return Json("ok", JsonRequestBehavior.AllowGet);
            }
            catch (Exception )
            {
                return Json("系统错误，请重试！！！",JsonRequestBehavior.AllowGet);
            }
             
        }
    }
}