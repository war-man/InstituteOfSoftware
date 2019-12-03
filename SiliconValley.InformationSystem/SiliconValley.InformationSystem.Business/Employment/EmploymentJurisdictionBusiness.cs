using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业权限
    /// </summary>
    public class EmploymentJurisdictionBusiness
    {
        private BaseBusiness<Base_UserRoleMap> dbuserrole;
        private BaseBusiness<OtherRoleMapPermissionValue> dbotherrole;
        private BaseBusiness<Base_SysRole> dbrole;

        /// <summary>
        /// 有没有查看全部数据的权限
        /// </summary>
        /// <returns></returns>
        public bool isstaffJurisdiction(Base_UserModel user)
        {
            var result = false;
           var query=  this.GetAllPermissionByRole(user);
            var xmlquery = this.Jurisdiction(RoomTypeEnum.Jurisdiction.directorJurisdiction);
            foreach (var item in query)
            {
                if (item.PermissionValue == xmlquery)
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 权限值
        /// </summary>
        /// <returns></returns>
        public string Jurisdiction(RoomTypeEnum.Jurisdiction jurisdiction) {

            string name = string.Empty;
            string param0 =jurisdiction.ToString();
            XElement xe = XElement.Load(HttpContext.Current.Server.MapPath("~/Config/Obempstaff.config"));
            IEnumerable<XElement> elements = from ele in xe.Elements(param0)
                                             select ele;
            foreach (var ele in elements)
            {
                name = ele.Attribute("val").Value;
            }
            return name;

        }
        

        /// <summary>
        /// 角色对应的权限值
        /// </summary>
        /// <returns></returns>
        public List<OtherRoleMapPermissionValue> GetAllPermissionByRole(Base_UserModel user)
        {
            List < Base_UserRoleMap > query= this.GetAllUserRoleMapByuser(user);
            dbotherrole = new BaseBusiness<OtherRoleMapPermissionValue>();
            List<OtherRoleMapPermissionValue> result = new List<OtherRoleMapPermissionValue>();
            foreach (var item in query)
            {
                result.AddRange(dbotherrole.GetIQueryable().Where(a => a.RoleId == item.RoleId));
            }
            return result;
        }

        /// <summary>
        ///根据用户返回所有的角色用户中间对象数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Base_UserRoleMap> GetAllUserRoleMapByuser(Base_UserModel user)
        {
            dbuserrole = new BaseBusiness<Base_UserRoleMap>();
            return dbuserrole.GetIQueryable().Where(a => a.UserId == user.UserId).ToList();
        }

        /// <summary>
        /// 根据用户返回所有的角色对象
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Base_SysRole> GetAllRoleByuser(Base_UserModel user)
        {
            dbrole = new BaseBusiness<Base_SysRole>();
            List < Base_UserRoleMap > query= this.GetAllUserRoleMapByuser(user);
            List<Base_SysRole> result = new List<Base_SysRole>();
            foreach (var item in query)
            {
                result.Add(dbrole.GetIQueryable().Where(a => a.RoleId == item.RoleId).FirstOrDefault());
            }
            return result;
        }
    } 

}
