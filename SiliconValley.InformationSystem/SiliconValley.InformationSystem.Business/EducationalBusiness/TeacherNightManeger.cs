using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
     public class TeacherNightManeger:BaseBusiness<TeacherNight>
      {
        RedisCache Redis;
        TeacherClassBusiness TeacherClass_Entity;
        TeacherBusiness Teacher_Entity;
        EvningSelfStudyManeger EvningselfStudy_Entity;
        BeOnDutyManeger BeOnDuty_Entity;
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<TeacherNight> GetAllTeacherNight()
        {
            Redis = new RedisCache();
            List<TeacherNight> teacherNights = new List<TeacherNight>();
            teacherNights = Redis.GetCache<List<TeacherNight>>("TeacherNight");
            if (teacherNights==null || teacherNights.Count<=0)
            {
                teacherNights = this.GetList();
                Redis.SetCache("TeacherNight", teacherNights);
            }
            return teacherNights;
        }

        /// <summary>
        /// 判断上课老师是否冲突(大于0--有冲突，小于或等于0--没有冲突)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="emp"></param>
        /// <param name="timename"></param>
        /// <returns></returns>
        public int GetTimeTeacherNight(DateTime time,string emp,string timename)
        {
           return GetAllTeacherNight().Where(t => t.OrwatchDate== time && t.Tearcher_Id==emp && t.timename==timename).ToList().Count;
        }
        /// <summary>
        /// 判断在这期间这个班级是否安排了晚自习老师值班
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public int GetTimeClassNight(DateTime starTime,DateTime endTime,int class_id)
        {
           return GetAllTeacherNight().Where(t => t.OrwatchDate >= starTime && t.OrwatchDate <= endTime && t.ClassSchedule_Id == class_id).ToList().Count;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="new_t"></param>
        /// <returns></returns>
        public AjaxResult Add_data(TeacherNight new_t)
        {             
            AjaxResult a = new AjaxResult();
            try
            {
                new_t.BeOnDuty_Id=BeOnDuty_Entity.GetSingleBeOnButy("晚自习", false).Id;
                this.Insert(new_t);                
                a.Success = true;
                Redis.RemoveCache("TeacherNight");
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }

            return a;                 
        }
        /// <summary>
        /// 安排晚自习值班
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="s1ors3"></param>
        /// <returns></returns>
        public AjaxResult AnpaiNight(DateTime starTime,DateTime endTime,bool s1ors3)
        {
            TeacherClass_Entity = new TeacherClassBusiness();
            EvningselfStudy_Entity = new EvningSelfStudyManeger();
            Teacher_Entity = new TeacherBusiness();
            BeOnDuty_Entity = new BeOnDutyManeger();
            AjaxResult a = new AjaxResult();
            int count_SUM = 0;
            //获取班级
            List<ClassSchedule> classes= Reconcile_Com.GetClass(s1ors3);
            //获取班级的晚自习情况
            foreach (ClassSchedule c1 in classes)
            {
                TeacherNight new_t = new TeacherNight();
                new_t.ClassSchedule_Id = c1.id;              
                new_t.IsDelete = false;

                //获取班级的任课老师
                EmployeesInfo find_e= TeacherClass_Entity.ClassTeacher(c1.id.ToString());
                if (find_e!=null)
                {
                    new_t.Tearcher_Id = find_e.EmployeeId;
                }
                //获取班级晚自习安排信息
                List<EvningSelfStudy> find_evning= EvningselfStudy_Entity.GetConditionEvningData(starTime, endTime, c1.id);
                if (find_evning.Count>0)
                {
                    new_t.timename = find_evning[0].curd_name;
                    new_t.ClassRoom_id = find_evning[0].Classroom_id;
                    new_t.OrwatchDate = find_evning[0].Anpaidate;                    
                }
                int count= GetTimeTeacherNight(new_t.OrwatchDate, new_t.Tearcher_Id, new_t.timename);
                if (count > 0)
                {
                    int index = 1;
                    new_t.OrwatchDate = find_evning[index].Anpaidate;
                    int count3 =0;
                    do {
                        count3 = GetTimeTeacherNight(new_t.OrwatchDate, new_t.Tearcher_Id, new_t.timename);
                        new_t.timename = find_evning[index].curd_name;
                        new_t.ClassRoom_id = find_evning[index].Classroom_id;
                        if (index < find_evning.Count)
                        {
                            index++;
                        }                       
                    }
                    while (count3<=0);
                    //判断这个班级是否安排了老师
                    int count2 = GetTimeClassNight(starTime, endTime, new_t.ClassSchedule_Id);
                    if (count2 <= 0)
                    {
                        //添加
                       AjaxResult s= Add_data(new_t);
                        if (s.Success==true)
                        {
                            count_SUM++;
                        }
                    }
                }
                else
                {
                    //判断这个班级是否安排了老师
                    int count2=  GetTimeClassNight(starTime, endTime, new_t.ClassSchedule_Id);
                    if (count2<=0)
                    {
                        //添加
                       AjaxResult s= Add_data(new_t);
                        if (s.Success == true)
                        {
                            count_SUM++;
                        }
                    }
                }

                if (count_SUM== (classes.Count-1))
                {
                    a.Success = true;
                }
                else
                {
                    a.Success = false;
                }
            }
            return a;
        }
    }
}
