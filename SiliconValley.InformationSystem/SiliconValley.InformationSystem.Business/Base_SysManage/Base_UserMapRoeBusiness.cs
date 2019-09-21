using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Base_SysManage
{
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using SiliconValley.InformationSystem.Business;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;

    public class Base_UserMapRoeBusinessL:BaseBusiness<Base_UserRoleMap>
    {


        Base_SysRoleBusiness db_role = new Base_SysRoleBusiness();

        /// <summary>
        /// 获取当前用户的所有角色
        /// </summary>
        /// <returns></returns>
        public List<Base_SysRole> CurrentUserRoles()
        {

            List<Base_SysRole> resultlist = new List<Base_SysRole>();

            //获取当前用户
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

           var templist =  this.GetList().Where(d => d.UserId == user.UserId).ToList();


          

            foreach (var item in templist)
            {
              var obj =  db_role.GetList().Where(d => d.RoleId == item.RoleId).FirstOrDefault();

                resultlist.Add(obj);
            }

            return resultlist;

        }


        /// <summary>
        /// 获取配置文件里面的角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public Base_SysRole GetSysRoleAtConfig(string id)
        {

            

            string filePath = HttpContext.Current.Server.MapPath("~/Config/UserMapRole.config");
            XElement xe = XElement.Load(filePath);

            //角色集合
            var roles = xe.Elements("Role");

            foreach (var item in roles)
            {
                //获取id属性 和参数id进行比较

               XAttribute attributeid = item.Attribute("id");

                if (attributeid.Value == id)
                {
                    Base_SysRole ResultRole = new Base_SysRole();

                    ResultRole.Id = attributeid.Value;
                    ResultRole.RoleId = item.Attribute("roleid").Value;
                    ResultRole.RoleName = item.Attribute("rolename").Value;

                    return ResultRole;
                }


            }

            return null;

        }

    }
}
