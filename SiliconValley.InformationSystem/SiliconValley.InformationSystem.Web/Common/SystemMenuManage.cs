using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Web
{
    /// <summary>
    /// 系统菜单管理
    /// </summary>
    public static class SystemMenuManage
    {
        #region 构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static SystemMenuManage()
        {
            InitAllMenu();
        }

        #endregion

        #region 私有成员

        public static string _configFile { get; } = "~/Config/SystemMenu.config";
        private static List<Menu> _allMenu { get; set; }
        private static void InitAllMenu()
        {
            Action<Menu, XElement> SetMenuProperty = (menu, element) =>
            {
                List<string> exceptProperties = new List<string> { "Id", "IsShow" };
                menu.GetType().GetProperties().Where(x => !exceptProperties.Contains(x.Name)).ForEach(aProperty =>
                {
                    aProperty.SetValue(menu, element.Attribute(aProperty.Name)?.Value);
                });
            };

            string filePath = HttpContext.Current.Server.MapPath(_configFile);
            XElement xe = XElement.Load(filePath);
            List<Menu> menus = new List<Menu>();
            xe.Elements("FirstMenu")?.ForEach(aElement1 =>
            {
                Menu newMenu1 = new Menu();
                menus.Add(newMenu1);
                SetMenuProperty(newMenu1, aElement1);
                newMenu1.SubMenus = new List<Menu>();
                aElement1.Elements("SecondMenu")?.ForEach(aElement2 =>
                {
                    Menu newMenu2 = new Menu();
                    newMenu1.SubMenus.Add(newMenu2);
                    SetMenuProperty(newMenu2, aElement2);
                    newMenu2.SubMenus = new List<Menu>();

                    aElement2.Elements("ThirdMenu")?.ForEach(aElement3 =>
                    {
                        Menu newMenu3 = new Menu();
                        newMenu2.SubMenus.Add(newMenu3);
                        SetMenuProperty(newMenu3, aElement3);
                        if (!newMenu3.Url.IsNullOrEmpty())
                        {
                            newMenu3.Url = GetUrl(newMenu3.Url);
                        }
                    });
                });
            });

            if (GlobalSwitch.RunModel == RunModel.LocalTest)
            {
                Menu newMenu1 = new Menu
                {
                    Name = "开发",
                    Icon = "icon_menu_prod",
                    SubMenus = new List<Menu>()
                };
                menus.Add(newMenu1);
                Menu newMenu1_1 = new Menu
                {
                    Name = "快速开发",
                    SubMenus = new List<Menu>()
                };
                newMenu1.SubMenus.Add(newMenu1_1);
                Menu newMenu1_1_1 = new Menu
                {
                    Name = "代码生成",
                    Url = GetUrl("~/Base_SysManage/RapidDevelopment/Index")
                };
                newMenu1_1.SubMenus.Add(newMenu1_1_1);

                Menu newMenu1_1_2 = new Menu
                {
                    Name = "数据库连接管理",
                    Url = GetUrl("~/Base_SysManage/Base_DatabaseLink/Index")
                };
                newMenu1_1.SubMenus.Add(newMenu1_1_2);

                Menu newMenu1_1_3 = new Menu
                {
                    Name = "UEditor Demo",
                    Url = GetUrl("~/Demo/UMEditor")
                };
                newMenu1_1.SubMenus.Add(newMenu1_1_3);

                Menu newMenu1_1_4 = new Menu
                {
                    Name = "文件上传Demo",
                    Url = GetUrl("~/Demo/UploadFileIndex")
                };
                newMenu1_1.SubMenus.Add(newMenu1_1_4);
            }

            _allMenu = menus;
        }
        private static void SetSubMenuShow(List<Menu> menus, List<string> userPermissionValues, int level)
        {
            if (level >= 4)
                return;
            menus?.ForEach(aMenu =>
            {
                if (!aMenu.Permission.IsNullOrEmpty() && !userPermissionValues.Contains(aMenu.Permission))
                {
                    aMenu.IsShow = false;
                    return;
                }
                else
                {
                    SetSubMenuShow(aMenu.SubMenus, userPermissionValues, level + 1);
                }

                if ((!aMenu?.SubMenus?.Any(x => x.IsShow)) ?? false)
                    aMenu.IsShow = false;
            });
        }


        /// <summary>
        /// 获取最后一层菜单
        /// </summary>
        /// <returns></returns>
        public static List<Menu> lastMenu(Menu menu, List<Menu> remenus)
        {
            Menu menus = new Menu();

            if (menu.SubMenus != null)
            {
                foreach (var item in menu.SubMenus)
                {
                    if (item.Permission != null)
                    {
                        remenus.Add(item);

                        
                    }
                    lastMenu(item, remenus);
                }
            }
            

            return remenus;
        }

        public static Menu ParentMenu(Menu menu, List<Menu> remenus)
        {
            Menu menus = new Menu();

            foreach (var item in remenus)
            {
                List<Menu> temp = new List<Menu>();
               var templist = lastMenu(item, temp);

                foreach (var item1 in templist)
                {
                    if (item1.Permission == menu.Permission)
                    {
                        menus = item;
                        break;
                    }
                }
            }

            return menus;

            
        }

       


      

        private static string GetUrl(string virtualUrl)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.Content(virtualUrl);
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 获取系统所有菜单
        /// </summary>
        /// <returns></returns>
        public static List<Menu> GetAllSysMenu()
        {
            return _allMenu.DeepClone();
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <returns></returns>
        public static List<Menu> GetOperatorMenu()
        {
            List<Menu> resList = GetAllSysMenu();

            if (Operator.IsAdmin())
                return resList;

            var userPermissions = PermissionManage.GetUserPermissionValues(Operator.UserId);
            SetSubMenuShow(resList, userPermissions, 1);

            return resList;
        }
        /// <summary>
        /// 获取登陆人信息
        /// </summary>
        /// <returns></returns>
        public static EmployeesInfo UserClass()
        {
            //员工表
            EmployeesInfoManage employeesInfoManage = new EmployeesInfoManage();
            //session["UserId"] = user.UserId;
            Base_UserBusiness base_UserBusiness = new Base_UserBusiness();
           var empid=  base_UserBusiness.GetList().Where(a => a.UserId == Operator.UserId).FirstOrDefault().EmpNumber;
            return employeesInfoManage.GetEntity(empid);
            
        }

        #endregion
    }

    #region 数据模型

    public class Menu
    {
        #region 属性

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Permission { get; set; }
        public bool IsShow { get; set; } = true;
        public List<Menu> SubMenus { get; set; }

        #endregion

        #region 前端映射



        #endregion
    }

    #endregion
}