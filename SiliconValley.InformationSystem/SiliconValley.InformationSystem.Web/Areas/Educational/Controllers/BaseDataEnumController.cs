using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;

    public class BaseDataEnumController : Controller
    {
        // GET: /Educational/BaseDataEnum/AddorEditView
        public ActionResult BaseDataEnumIndexView()
        {
            return View();
        }
        /// <summary>
        /// 创建业务对象
        /// </summary>
        public static class MyEntity {
            public  static readonly BaseDataEnumManeger BaseDataEnum_Entity = new BaseDataEnumManeger();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="limit">条数</param>
        /// <param name="page">页数</param>
        /// <returns></returns>
        public ActionResult GetBaseDataEnumData(int limit,int page)
        {            
            try
            {
                List<BaseDataEnum> basedata_list = MyEntity.BaseDataEnum_Entity.GetList();
                string name = Request.QueryString["Name"];
                if (!string.IsNullOrEmpty(name))
                {
                    basedata_list = basedata_list.Where(b => b.Name.Contains(name)).ToList();
                }

                var mydata = basedata_list.Skip((page - 1) * limit).Take(limit).OrderByDescending(b => b.Id).Select(b => new {
                    Id = b.Id,
                    Name = b.Name,
                    Rmark = b.Rmark,
                    IsDelete = b.IsDelete,
                    fatherId=b.fatherId,                    
                    FarName = MyEntity.BaseDataEnum_Entity.GetSingData(b.fatherId.ToString(),true)==null?"无" : MyEntity.BaseDataEnum_Entity.GetSingData(b.fatherId.ToString(), true).Name
                }).ToList();
                var jsondata = new
                {
                    code = 0,
                    msg = "",
                    count = basedata_list.Count,
                    data = mydata,
                };
                return Json(jsondata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public ActionResult AddorEditFunction(BaseDataEnum basedata)
        {
            BaseDataEnum find_basedataenum= MyEntity.BaseDataEnum_Entity.GetSingData(basedata.Name, false);
             
                if (basedata.Id > 0)
                {
                    //编辑业务
                   BaseDataEnum find_base= MyEntity.BaseDataEnum_Entity.GetSingData(basedata.Id.ToString(), true);
                    find_base.Name = basedata.Name;
                    find_base.Rmark = basedata.Rmark;
                    find_base.fatherId = basedata.fatherId;
                    MyEntity.BaseDataEnum_Entity.Update(find_base);
                }
                else
                {
                   if (find_basedataenum!=null)
                   {
                     return Json("名称不能重复！！！", JsonRequestBehavior.AllowGet);
                   }
                    //添加业务
                    basedata.IsDelete = false;
                    MyEntity.BaseDataEnum_Entity.Insert(basedata);
                }
                return Json("ok", JsonRequestBehavior.AllowGet);
             
                         
        }

        /// <summary>
        /// 添加或编辑
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult AddorEditView(int? Id)
        {
            //获取所有父级节点
           
            List<SelectListItem> s1= MyEntity.BaseDataEnum_Entity.GetFartherData().Select(b=>new SelectListItem() { Text=b.Name,Value=b.Id.ToString()}).ToList().ToList() ;
            s1.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });             
            ViewBag.farther = s1;
            if (Id>0)
            {
                //编辑页面
               BaseDataEnum find_data= MyEntity.BaseDataEnum_Entity.GetSingData(Id.ToString(), true);
                for (int i = 0; i < s1.Count; i++)
                {
                    if (s1[i].Value == find_data.Id.ToString())
                    {
                        s1.Remove(s1[i]);
                    }
                }
                ViewBag.farther = s1;
                return View(find_data);
            }
            else
            {
                //添加页面
                return View();
            }
            
        }

        public ActionResult DeleteView(int? Id)
        {
            try
            {
                bool isdelete = Request.Form["value"].ToString() == "false" ? true : false;
                BaseDataEnum find_enum = MyEntity.BaseDataEnum_Entity.GetSingData(Id.ToString(), true);
                find_enum.IsDelete = isdelete;
                MyEntity.BaseDataEnum_Entity.Update(find_enum);
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }           
        }
    }
}