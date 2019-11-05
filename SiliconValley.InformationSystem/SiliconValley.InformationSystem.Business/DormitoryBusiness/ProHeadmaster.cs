﻿using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 班主任业务类
    /// </summary>
    public class ProHeadmaster : BaseBusiness<Headmaster>
    {
        private EmployeesInfoManage dbemp;
        /// <summary>
        /// 获取在职的班主任
        /// </summary>
        /// <returns></returns>
        public List<Headmaster> GetHeadmasters() {
          return  this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        /// <summary>
        /// 根据id获取班主任
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Headmaster GetHeadById(int? Id) {
           return this.GetHeadmasters().Where(a => a.ID == Id).FirstOrDefault();
        }

        /// <summary>
        /// 根据员工编号获取班主任
        /// </summary>
        /// <param name="Empinfoid"></param>
        /// <returns></returns>
        public Headmaster GetHeadByeEmpinfoid(string Empinfoid) {
            return this.GetHeadmasters().Where(a => a.informatiees_Id == Empinfoid).FirstOrDefault();
        }

        /// <summary>
        /// 根据老师id获取员工对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmployeesInfoByHeadID(int id) {
            dbemp = new EmployeesInfoManage();
            var obj0=this.GetHeadById(id);
          return  dbemp.GetEntity(obj0.informatiees_Id);
        }
    }
}