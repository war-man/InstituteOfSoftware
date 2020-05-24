using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.ViewEntity;

namespace SiliconValley.InformationSystem.Business.EmployeesBusiness
{
    /// <summary>
    ///  用于数据备案一系列的操作类
    /// </summary>
   public static class EmployandCounTeacherCoom
    {
        /// <summary>
        /// 获取所有的咨询师，然后转化为员工集合
        /// </summary>
        /// <param name="all">false---返回在职咨询师，true--返回全部咨询师</param>
        /// <returns></returns>
        public static List<Emp_consult> getallCountTeacher(bool all)
        {
            ConsultTeacherManeger consult = new ConsultTeacherManeger();
            EmployeesInfoManage employeesInfo = new EmployeesInfoManage();
            List<Emp_consult> list = new List<Emp_consult>();//在职咨询师
            List<Emp_consult> list2 = new List<Emp_consult>();//全部咨询师
            consult.GetList().ForEach(c =>
            {
                Emp_consult e = new Emp_consult();
                e.consultercherid = c.Id;
                e.empname = employeesInfo.GetEntity(c.Employees_Id).EmpName;
                if (all) 
                {                     
                    list2.Add(e);                     
                }
                else 
                {
                    if (c.IsDelete == false)
                    {
                         list.Add(e);
                    }
                   
                }
                
            });

            return all==false? list:list2;


        }

        /// <summary>
        /// 添加分量数据
        /// </summary>
        /// <returns></returns>
        public static bool AddConsult(Consult consult)
        {
            bool s = false;
            ConsultManeger cont = new ConsultManeger();
            try
            {
                cont.Insert(consult);
                s = true;
            }
            catch (Exception)
            {

                s = false;
            }
            return s;
        }

         
        /// <summary>
        //根据咨询师编号获取员工名称
        /// </summary>
        /// <param name="c_id"></param>
        /// <returns></returns>
        public static string ConsultName(int c_id)
        {
            ConsultTeacherManeger consult = new ConsultTeacherManeger();
            ConsultTeacher findata= consult.GetEntity(c_id);
            EmployeesInfoManage business = new EmployeesInfoManage();
            return business.GetEntity(findata.Employees_Id).EmpName;
        }
 
 
    }
}
