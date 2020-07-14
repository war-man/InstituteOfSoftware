using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data;
using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data.MyViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.MyEntity;
 

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;

    /// <summary>
    /// 迟到记录表
    /// </summary>
    public class LaterecordManeger:BaseBusiness<Laterecord>
    {

        /// <summary>
        /// 获取所有迟到的视图数据
        /// </summary>
        /// <returns></returns>
        public List<LaterecordView> GetallView()
        {
            return this.GetListBySql<LaterecordView>(" select * from LaterecordView");            
        }

        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AjaxResult Add_data(Laterecord data)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                //判断是否重复了
                int count= this.GetListBySql<LaterecordView>("select * from LaterecordView where Class_Id=" + data.Class_Id + " and Createdate='" + data.Createdate + "'").Count;
                if (count>0)
                {
                   
                    a.Success = false;
                    a.Msg = "班级一天只能登记一次！！！";
                }
                else
                {
                    this.Insert(data);
                    a.Success = true;
                    a.Msg = "操作成功！！！";
                }
                 
            }
            catch (Exception )
            {
                a.Success = false;
                a.Msg = "系统异常，请重试！！！";
            }

            return a;
        }

        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public AjaxResult update_data(Laterecord data)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Update(data);
                a.Success = true;
                a.Msg = "操作成功！！";
            }
            catch (Exception )
            {
                a.Success = false;
                a.Msg = "系统操作异常，请重试！！！";
            }

            return a;
        }

        /// <summary>
        /// 根据Id获取视图数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LaterecordView FindSingData(int id)
        {
           List<LaterecordView> findata= this.GetListBySql<LaterecordView>("select * from LaterecordView where Id=" + id);

            return findata.Count > 0 ? findata[0] : null;
        }
        /// <summary>
        /// 是否展示所有数据
        /// </summary>
        /// <returns>true--可以展示所有数据,false--不可以</returns>
        public bool IsShowAll(string emp)
        {
            bool s = false;
            EmployeesInfoManage emp_Entity = new EmployeesInfoManage();
            Department findata_d= emp_Entity.GetDeptByEmpid(emp);
            if (!findata_d.DeptName.Contains("教学部"))
            {
                s = true;
            }
            return s;
        }
    }


    /// <summary>
    /// 迟到详情记录表
    /// </summary>
    public class Laterecord_StudentAttendanceManeger : BaseBusiness<Laterecord_StudentAttendance>
    {
        LaterecordManeger L_entity = new LaterecordManeger();

        /// <summary>
        /// 添加迟到详情数据
        /// </summary>
        /// <param name="class_id">班级编号</param>
        /// <param name="studentlist">迟到学生</param>
        /// <param name="date">迟到日期</param>
        /// <returns></returns>
        public AjaxResult Add_data(int class_id,List<StudentAttendance> studentlist,DateTime date)
        {
            AjaxResult a = new AjaxResult();
            List<Laterecord_StudentAttendance> L_list = new List<Laterecord_StudentAttendance>();
            List<Laterecord> list= L_entity.GetListBySql<Laterecord>("select * from Laterecord where Class_Id" + class_id + " Createdate='" + date + "'");
            if (list.Count>0)
            {
                foreach (StudentAttendance it in studentlist)
                {
                    Laterecord_StudentAttendance laterecord = new Laterecord_StudentAttendance();
                    laterecord.Laterecord_Id = list[0].Id;

                    List<StudentInformation> studata= this.GetListBySql<StudentInformation>("select * from  StudentInformation where StudentNumber='" + it.StudentID + "'");//根据学号获取学生姓名

                    laterecord.StudentName = studata[0].Name;

                    L_list.Add(laterecord);
                }

                try
                {
                    this.Insert(L_list);
                    a = new AjaxResult() { Success = false, Msg = "操作成功！！！" };
                }
                catch (Exception  )
                {
                    a = new AjaxResult() { Success = false, Msg = "系统异常" };
                }

            }
            else
            {
                  a = new AjaxResult() { Success = false, Msg = "没有迟到记录" };
                
            }
            return a;
        }

        /// <summary>
        /// 根据迟到登记表获取迟到学生详情
        /// </summary>
        /// <param name="laterrecord_id"></param>
        /// <returns></returns>
        public List<Laterecord_StudentAttendance> GetRecord(int laterrecord_id)
        {
           return this.GetListBySql<Laterecord_StudentAttendance>("select * from  Laterecord_StudentAttendance where Laterecord_Id=" + laterrecord_id);
        }

    }
}
