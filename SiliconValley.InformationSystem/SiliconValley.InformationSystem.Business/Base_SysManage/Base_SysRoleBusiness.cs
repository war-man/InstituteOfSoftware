using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace SiliconValley.InformationSystem.Business.Base_SysManage
{
   
    public class Base_SysRoleBusiness : BaseBusiness<Base_SysRole>
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Base_SysRole> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Base_SysRole GetTheData(string id)
        {
            return GetEntity(id);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(Base_SysRole newData)
        {
            Insert(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(Base_SysRole theData)
        {
            Update(theData);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            //删除角色
            Delete(ids);
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">权限值</param>
        public void SavePermission(string roleId,List<string> permissions)
        {
            Service.Delete_Sql<Base_PermissionRole>(x => x.RoleId == roleId);
            List<Base_PermissionRole> insertList = new List<Base_PermissionRole>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionRole
                {
                    Id=Guid.NewGuid().ToSequentialGuid(),
                    RoleId=roleId,
                    PermissionValue=newPermission
                });
            });

            Service.Insert(insertList);
            PermissionManage.ClearUserPermissionCache();
        }

        public AjaxResult createRole(string roleName, string businessName)
        {
            AjaxResult result = new AjaxResult();

            //首先验证 是否存在
           var existrole = this.GetList().Where(d => d.RoleName == roleName).FirstOrDefault();

            if (existrole != null)
            {
                //已存在
                result.ErrorCode = 444;
                result.Msg = "角色名称重复！";
                result.Data = null;

                return result;
            }

            //********************************************************************//

            Base_SysRole role = new Base_SysRole();
            role.BusinessName = businessName;
            role.Id = Guid.NewGuid().ToString();
            role.RoleId = Guid.NewGuid().ToString();
            role.RoleName = roleName;

            this.AddData(role);

            result.ErrorCode = 200;
            result.Msg = "成功！";
            result.Data = null;

            return result;

        }



        /// <summary>
        /// 获取角色所拥有的url权限
        /// </summary>
        /// <returns></returns>
        public List<PermissionModule> RolePermission(string roleId)
        {

           var result = PermissionManage.GetRolePermissionModules(roleId);

            return result;
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}