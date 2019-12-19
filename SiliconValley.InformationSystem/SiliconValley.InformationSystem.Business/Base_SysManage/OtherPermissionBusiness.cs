using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Base_SysManage
{
    using SiliconValley.InformationSystem.Entity.Entity;
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 其他权限
    /// 
    /// 
    /// 
    /// </summary>
    /// 
    public class OtherPermissionBusiness : BaseBusiness<OthorPermission>
    {
        public readonly BaseBusiness<OtherRoleMapPermissionValue> db_role_otherPermission = new BaseBusiness<OtherRoleMapPermissionValue>();

        public OtherPermissionBusiness()
        {
            db_role_otherPermission = new BaseBusiness<OtherRoleMapPermissionValue>();
        }



        /// <summary>
        /// 获取角色的其他权限
        /// </summary>
        /// <returns></returns>
        public List<OthorPermission> GetPermissionByRole(string role)
        {
            var role_permissionlist = db_role_otherPermission.GetList().Where(d=>d.RoleId == role).ToList();

            List<OthorPermission> resultlist = new List<OthorPermission>();

            foreach (var item in role_permissionlist)
            {
                var temp = this.GetList().Where(d => d.PermissionValue == item.PermissionValue).FirstOrDefault();

                if (!IsContains(resultlist, temp))
                {
                    resultlist.Add(temp);
                }
            }


            return resultlist;
        }

        public bool IsContains(List<OthorPermission> sources, OthorPermission permission)
        {
            return sources.Where(d => d.PermissionValue == permission.PermissionValue).FirstOrDefault() != null;
        }


        /// <summary>
        /// 获取所有其他权限
        /// </summary>
        /// <returns></returns>
        public List<OthorPermission> AllOtherPermissions()
        {
            return this.GetList();
        }


        /// <summary>
        /// 删除角色其他权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="permissions"></param>
        public void removeRolePermissions(string role, List<string> permission_arry)
        {
          

            //var rolepermisslist =  db_role_otherPermission.GetList().Where(d => d.RoleId == role).ToList();

            foreach (var item in permission_arry)
            {
               var temp = db_role_otherPermission.GetList().Where(d => d.RoleId == role && d.PermissionValue == item).FirstOrDefault();

                if (temp != null)
                {
                    db_role_otherPermission.Delete(temp);
                }
                
            }
        }


        /// <summary>
        /// 给角色添加其他权限
        /// </summary>
        public void addRolePermissions(string role, List<string> permission_arry)
        {
          

            foreach (var item in permission_arry)
            {
                var temp = db_role_otherPermission.GetList().Where(d => d.RoleId == role && d.PermissionValue == item).FirstOrDefault();

                if (temp == null)
                {
                    OtherRoleMapPermissionValue otherRoleMapPermissionValue = new OtherRoleMapPermissionValue();

                    otherRoleMapPermissionValue.PermissionValue = item;
                    otherRoleMapPermissionValue.RoleId = role;

                    db_role_otherPermission.Insert(otherRoleMapPermissionValue);

                }

            }
        }
    }
}
