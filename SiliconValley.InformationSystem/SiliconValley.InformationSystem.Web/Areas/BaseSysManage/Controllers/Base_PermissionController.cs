using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.BaseSysManage.Controllers
{
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using System.Xml;
    [CheckLogin]
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

        public ActionResult MenusIndex()
        {
            return View();
        }

        /// <summary>
        /// 菜单数据
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuData()
        {
            AjaxResult result = new AjaxResult(); ;

            try
            {
               var list = SystemMenuManage.GetAllSysMenu();

                result.ErrorCode = 200;
                result.Msg = "";
                result.Data = list;
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
        /// 添加菜单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult addMenu()
        {
            return View();
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult addMenu(string MenuName, string ParentMenu, string url, string permiss,string Icon)
        {
            
            //加载 url xml
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Server.MapPath(SystemMenuManage._configFile));

            var root = xmlDocument.DocumentElement;

            if (ParentMenu == "")
            {
                //一级菜单
                var elem = xmlDocument.CreateElement("FirstMenu");
                var attrName = xmlDocument.CreateAttribute("Name");
               var Icons= xmlDocument.CreateAttribute("Icon");
                attrName.Value = MenuName;
                Icons.Value = Icon;
                elem.SetAttributeNode(attrName);
                elem.SetAttributeNode(Icons);
                if (url == "")
                {
                    //表示不是最后一级
                    root.AppendChild(elem);
                }
                else
                {
                    //表示最后一级  添加url permiss
                    var urlattr = xmlDocument.CreateAttribute("Url");
                    var perattr = xmlDocument.CreateAttribute("Permission");

                    elem.SetAttributeNode(attrName); elem.SetAttributeNode(perattr);
                    root.AppendChild(elem);
                }


            }
            else
            {
                //需要找到负极
                //添加二字


                var firstMenus = root.GetElementsByTagName("FirstMenu");

              

                foreach (XmlElement item in firstMenus)
                {
                    if (item.Attributes["Name"].Value != ParentMenu)
                    {
                        foreach (XmlElement item1 in item.ChildNodes)
                        {
                            if (item1.Attributes["Name"].Value != ParentMenu)
                            {

                                foreach (XmlElement item2 in item1.ChildNodes)
                                {
                                    if (item2.Attributes["Name"].Value != ParentMenu)
                                    {

                                    }
                                    else
                                    {
                                       
                                    }
                                }
                            }
                            else
                            {

                                //添加二字
                                var elem = xmlDocument.CreateElement("ThirdMenu");
                                var attrName = xmlDocument.CreateAttribute("Name");
                                attrName.Value = MenuName;
                                elem.SetAttributeNode(attrName);
                                if (url == "")
                                {
                                    //表示不是最后一级
                                    item1.AppendChild(elem);
                                }
                                else
                                {
                                    //表示最后一级  添加url permiss
                                    var urlattr = xmlDocument.CreateAttribute("Url");
                                    urlattr.Value = url;

                                    var perattr = xmlDocument.CreateAttribute("Permission");
                                    perattr.Value = permiss;

                                    elem.SetAttributeNode(urlattr); elem.SetAttributeNode(perattr);
                                    item1.AppendChild(elem);
                                }
                                xmlDocument.Save(Server.MapPath(SystemMenuManage._configFile));

                                return Json("1", JsonRequestBehavior.AllowGet);
                            }

                        }
                    }
                    else {
                        //添加二字
                        var elem = xmlDocument.CreateElement("SecondMenu");
                        var attrName = xmlDocument.CreateAttribute("Name");
                        attrName.Value = MenuName;
                        elem.SetAttributeNode(attrName);
                        if (url == "")
                        {
                            //表示不是最后一级
                            item.AppendChild(elem);
                        }
                        else
                        {
                            //表示最后一级  添加url permiss
                            var urlattr = xmlDocument.CreateAttribute("Url");
                            var perattr = xmlDocument.CreateAttribute("Permission");
                            urlattr.Value = url;
                            perattr.Value = permiss;
                            elem.SetAttributeNode(urlattr); elem.SetAttributeNode(perattr);
                            item.AppendChild(elem);
                        }

                        xmlDocument.Save(Server.MapPath(SystemMenuManage._configFile));

                        return Json("1", JsonRequestBehavior.AllowGet);
                    }
                }

             

            }
            xmlDocument.Save(Server.MapPath(SystemMenuManage._configFile));

            return Json("1", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 权限管理
        /// </summary>
        /// <returns></returns>

        public ActionResult permissManage()
        {
            return View();
        }


        /// <summary>
        /// 添加新的权限
        /// </summary>
        /// <returns></returns>
        public ActionResult addFirstPermiss(string name1, string name2, string code1, string code2)
        {

            AjaxResult result = new AjaxResult();

            try
            {

                //加载 url xml
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Server.MapPath("/Config/Permission.config"));

                var root = xmlDocument.DocumentElement;

                //创建一级菜单
                var first = xmlDocument.CreateElement("module");
                var attr_name = xmlDocument.CreateAttribute("name");
                attr_name.Value = name1;
                var attr_value = xmlDocument.CreateAttribute("value");
                attr_value.Value = code1;
                first.SetAttributeNode(attr_name);
                first.SetAttributeNode(attr_value);

                //二级
                var second = xmlDocument.CreateElement("permission");
                var attr_name2 = xmlDocument.CreateAttribute("name");
                attr_name2.Value = name2;
                var attr_value2 = xmlDocument.CreateAttribute("value");
                attr_value2.Value = code2;
                second.SetAttributeNode(attr_name2);
                second.SetAttributeNode(attr_value2);

                first.AppendChild(second);

                root.AppendChild(first);

                //添加到数据库

                BaseBusiness<Base_PermissionAppId> dbp = new BaseBusiness<Base_PermissionAppId>();

                Base_PermissionAppId base_PermissionAppId = new Base_PermissionAppId();

                base_PermissionAppId.AppId = "AppTest";
                base_PermissionAppId.Id = Guid.NewGuid().ToString();
                base_PermissionAppId.PermissionValue = code1 + "." + code2;

                dbp.Insert(base_PermissionAppId);

                xmlDocument.Save(Server.MapPath("/Config/Permission.config"));

                result.ErrorCode = 200;
                result.Msg = "";
                result.Data = null;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 200;
                result.Msg = "";
                result.Data = null;
            }


            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 添加新的权限
        /// </summary>
        /// <returns></returns>
        public ActionResult addsencdPermiss(string name1, string code1, string parentCode)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                //加载 url xml
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Server.MapPath("/Config/Permission.config"));

                var root = xmlDocument.DocumentElement;

                //获取一级菜单
                var firstModu = root.GetElementsByTagName("module");

                foreach (XmlElement item in firstModu)
                {
                    var value = item.Attributes["value"].Value;
                    if (value == parentCode)
                    {
                        //添加子节点

                        var second = xmlDocument.CreateElement("permission");
                        var attr_name2 = xmlDocument.CreateAttribute("name");
                        attr_name2.Value = name1;
                        var attr_value2 = xmlDocument.CreateAttribute("value");
                        attr_value2.Value = code1;
                        second.SetAttributeNode(attr_name2);
                        second.SetAttributeNode(attr_value2);

                        item.AppendChild(second);

                        root.AppendChild(item);

                        //添加到数据库

                        BaseBusiness<Base_PermissionAppId> dbp = new BaseBusiness<Base_PermissionAppId>();

                        Base_PermissionAppId base_PermissionAppId = new Base_PermissionAppId();

                        base_PermissionAppId.AppId = "AppTest";
                        base_PermissionAppId.Id = Guid.NewGuid().ToString();
                        base_PermissionAppId.PermissionValue = parentCode + "." + code1;

                        dbp.Insert(base_PermissionAppId);
                        xmlDocument.Save(Server.MapPath("/Config/Permission.config"));

                        break;
                    }
                }

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






            return Json(result, JsonRequestBehavior.AllowGet); ;

        }



    }
}