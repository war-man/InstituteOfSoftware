using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.BaseSysManage.Controllers
{
    using System.Xml;
    public class Base_PermissionController : Controller
    {
        // GET: BaseSysManage/Base_Permission


        /// <summary>
        /// 菜单权限页面
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuPermissionIndex()
        {
            return View();
        }


        /// <summary>
        /// 菜单权限数据
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuPermissionData()
        {
            AjaxResult result = new AjaxResult();

            try
            {
             

                var allpermisslist = PermissionManage.GetAllPermissionModules();

                List<Common.layuitree> treelist = new List<Common.layuitree>();

                foreach (var item in allpermisslist)
                {
                    //加载第一层
                    Common.layuitree firsttree = new Common.layuitree();

                    firsttree.field = item.Value;
                    firsttree.title = item.Name;
                    firsttree.id = item.Value;
                    //加载第二ceng
                    foreach (var item1 in item.Items)
                    {
                        Common.layuitree secondtree = new Common.layuitree();

                        secondtree.field = item.Value+"."+
item1.Value;
                        secondtree.title = item1.Name;
                        secondtree.id = item.Value + item1.Value;

                        secondtree.@checked = false;

                        firsttree.children.Add(secondtree);
                    }

                    treelist.Add(firsttree);
                }


                result.ErrorCode = 200;
                result.Msg = "";
                result.Data = treelist;

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "";
                result.Data = null;
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取权限的url
        /// </summary>
        /// <returns></returns>
        public ActionResult PermissionUrl(string permission)
        {
            AjaxResult result = new AjaxResult();
            
            try
            {
                var menu = new Menu();

                //首先获取所有权限菜单
                var allurl = SystemMenuManage.GetAllSysMenu();



                List<Menu> menulist = new List<Menu>();
                foreach (var item in allurl)
                {
                    List<Menu> tst = new List<Menu>();
                    //获取最后一层
                    var lastmenu = SystemMenuManage.lastMenu(item, tst);

                    menulist.AddRange(lastmenu);
                }

                foreach (var item in menulist)
                {
                    if (item.Permission!=null &&item.Permission == permission)
                    {
                        menu = item;
                        break;
                    }
                }

                //获取父节点

                var ss =  SystemMenuManage.ParentMenu(menu, allurl);

                var obj = new {
                    node = menu,
                    parentNode = ss
                };

                result.ErrorCode = 200;
                result.Msg = "";
                result.Data = obj;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "";
                result.Data = "";
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }


        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="permission">菜单权限 以 逗号 分割</param>
        /// <param name="menusNames">菜单级 以 逗号分割</param>
        /// <returns></returns>
        public ActionResult editMenu(string permission, string url)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                var paermissiontemp_array = permission.Split(',').ToList();
                paermissiontemp_array.RemoveAt(paermissiontemp_array.Count - 1);

                //加载 url xml
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Server.MapPath(SystemMenuManage._configFile));

                var root = xmlDocument.DocumentElement;

                //获取所有一级菜单

                var firstMenus = root.GetElementsByTagName("FirstMenu");

                XmlElement l = null;

                foreach (XmlElement item in firstMenus)
                {
                    if (item.Attributes["Permission"] == null)
                    {
                        foreach (XmlElement item1 in item.ChildNodes)
                        {
                            if (item.Attributes["Permission"] == null)
                            {

                                foreach (XmlElement item2 in item1.ChildNodes)
                                {
                                    if (item2.Attributes["Permission"].Value == permission)
                                    {


                                        l = item;
                                        item2.Attributes["Url"].Value = url;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (item1.Attributes["Permission"].Value == permission)
                                {
                                    l = item1;
                                    item1.Attributes["Url"].Value = url;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (item.Attributes["Permission"].Value == permission)
                        {
                            l = item;
                            item.Attributes["Url"].Value = url;
                            break;
                        }
                    }

                }

                xmlDocument.Save(Server.MapPath(SystemMenuManage._configFile));

                result.ErrorCode = 200;
                result.Msg = "";
                result.Data = null;

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = "";
                result.Data = null;
            }


            return Json(result, JsonRequestBehavior.AllowGet);


        }

        public XmlElement test(XmlElement elmet,string permission)
        {
            if (elmet.Attributes["Permission"] == null)
            {
                foreach (XmlElement item in elmet)
                {


                }
            }
            else
            {
                if (elmet.Attributes["Permission"].Value == permission)
                {
                    return elmet;
                }
            }


            return null;
        }
    }
}