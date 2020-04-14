using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    public class TeacherNightManeger : BaseBusiness<TeacherNight>
    {
        RedisCache Redis = new RedisCache();
        TeacherClassBusiness TeacherClass_Entity;//任课老师所教班级业务类
        TeacherBusiness Teacher_Entity;
        BeOnDutyManeger BeOnDuty_Entity;
        EvningSelfStudyManeger EvningSelfStudent_Entity = new EvningSelfStudyManeger();

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<TeacherNight> GetAllTeacherNight()
        {
            Redis = new RedisCache();
            Redis.RemoveCache("TeacherNight");
            List<TeacherNight> teacherNights = new List<TeacherNight>();
            teacherNights = Redis.GetCache<List<TeacherNight>>("TeacherNight");
            
            if (teacherNights == null || teacherNights.Count <= 0)
            {
                teacherNights = this.GetIQueryable().ToList();
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
        public int GetTimeTeacherNight(DateTime time, string emp, string timename)
        {
            return GetAllTeacherNight().Where(t => t.OrwatchDate == time && t.Tearcher_Id == emp && t.timename == timename).ToList().Count;
        }
        /// <summary>
        /// 判断在这期间这个班级是否安排了晚自习老师值班
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public int GetTimeClassNight(DateTime starTime, DateTime endTime, int class_id)
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
                BeOnDuty_Entity = new BeOnDutyManeger();
                new_t.BeOnDuty_Id = BeOnDuty_Entity.GetSingleBeOnButy("晚自习", false).Id;
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
        public AjaxResult Add_data(List<TeacherNight> new_t)
        {
            int data_override = 0;

            AjaxResult a = new AjaxResult();
            try
            {
                BeOnDuty_Entity = new BeOnDutyManeger();
                int be_id = BeOnDuty_Entity.GetSingleBeOnButy("晚自习", false).Id;
                foreach (TeacherNight new_data in new_t)
                {
                    new_data.BeOnDuty_Id = be_id;
                    //判断是否有重复数据
                    int cout = this.GetList().Where(all => all.ClassSchedule_Id == new_data.ClassSchedule_Id && all.OrwatchDate == new_data.OrwatchDate).ToList().Count;
                    if (cout > 0)
                    {
                        data_override++;
                    }
                    else
                    {
                        this.Insert(new_data);
                    }

                }
                a.Success = true;
                if (data_override != 0)
                {
                    a.Msg = "有重复数据" + data_override + "条，系统自动处理完毕！";
                }
                else
                {
                    a.Msg = "无重复数据，成功安排！";
                }
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
        /// 系统自动安排晚自习值班
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="s1ors3"></param>
        /// <returns></returns>
        public AjaxResult AnpaiNight(DateTime starTime, DateTime endTime)
        {
            AjaxResult a = new AjaxResult();

            List<ClassSchedule> Class_All = Reconcile_Com.GetClass();//获取所有有效班级

            EvningSelfStudent_Entity = new EvningSelfStudyManeger();
            List<EvningSelfStudy> evning_list = EvningSelfStudent_Entity.GetList().Where(e => e.Anpaidate >= starTime && e.Anpaidate <= endTime).ToList(); //获取在这段期间要上晚自习的班级

            List<EvningSelfStudy> evningSelves_have = new List<EvningSelfStudy>();//获取这个时间段中要上晚自习的班级


            Random random = new Random();  //随机抽取
            foreach (ClassSchedule e in Class_All)
            {
                List<EvningSelfStudy> E_list_findclass = evning_list.Where(ev => ev.ClassSchedule_id == e.id).ToList();
                if (E_list_findclass.Count != 0 && E_list_findclass.Count == 1)
                {
                    evningSelves_have.AddRange(E_list_findclass);
                }
                else
                {
                    int num = random.Next(0, E_list_findclass.Count);
                    evningSelves_have.Add(E_list_findclass[num]);
                }
            }

            TeacherClass_Entity = new TeacherClassBusiness();  //获取任课老师
            List<ClassTeacher> class_teacher_list = TeacherClass_Entity.GetList();

            List<TeacherNight> list_new = new List<TeacherNight>();   //系统安排的值班数据
            Teacher_Entity = new TeacherBusiness();
            foreach (EvningSelfStudy ev in evningSelves_have)
            {
                //找到上这个班级的任课老师
                ClassTeacher find_class_teacher = class_teacher_list.Where(ct => ct.ClassNumber == ev.ClassSchedule_id && ct.IsDel == false).FirstOrDefault();
                if (find_class_teacher != null)
                {
                    //系统安排值班
                    TeacherNight new_teachernight_data = new TeacherNight();
                    new_teachernight_data.ClassRoom_id = ev.Classroom_id;
                    new_teachernight_data.ClassSchedule_Id = ev.ClassSchedule_id;
                    new_teachernight_data.IsDelete = false;
                    new_teachernight_data.OrwatchDate = ev.Anpaidate;
                    new_teachernight_data.Tearcher_Id = Teacher_Entity.GetTeacherByID(find_class_teacher.TeacherID).EmployeeId;
                    new_teachernight_data.timename = ev.curd_name;
                    new_teachernight_data.AttendDate = DateTime.Now;
                    list_new.Add(new_teachernight_data);
                }
            }

            List<TeacherNight> all = this.GetAllTeacherNight();    //判断在这期间是否已安排晚自习值班
            for (int i = 0; i < list_new.Count; i++)
            {
                int count = this.GetTimeClassNight(starTime, endTime,Convert.ToInt32( list_new[i].ClassSchedule_Id));
                if (count > 0)
                {
                    //删除
                    list_new.Remove(list_new[i]);
                }
            }

            a = this.Add_data(list_new); //添加数据
            return a;
        }

        /// <summary>
        /// 删除值班数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AjaxResult My_Delete(int Id)
        {
            //找到数据
            TeacherNight find_data_del = this.GetAllTeacherNight().Where(al => al.Id == Id).FirstOrDefault();
            //删除
            AjaxResult a = new AjaxResult();
            try
            {
                this.Delete(find_data_del);
                a.Success = true;
                a.Msg = "删除成功！";
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
        /// 获取空教室
        /// </summary>
        /// <param name="timename">上课时间</param>
        /// <param name="time">安排日期</param>
        /// <param name="schooladdress_id">所属校区</param>
        /// <returns></returns>
        public List<Classroom> GetEmptyClassroom(string timename, DateTime time, int schooladdress_id)
        {
            ClassroomManeger classroom_Entity = new ClassroomManeger();
            List<Classroom> roomlist = classroom_Entity.GetAddreeClassRoom(schooladdress_id);//获取某个校区的所有有效教室

            List<EvningSelfStudy> e_list = EvningSelfStudent_Entity.EvningSelfStudyGetAll().Where(e => e.Anpaidate == time).ToList(); //获取符合日期的晚自习安排

            foreach (EvningSelfStudy item in e_list)
            {
                Classroom find = roomlist.Where(c => c.Id == item.Classroom_id).FirstOrDefault();//获取没有安排的空教室
                if (find != null)
                {
                    roomlist.Remove(find);
                }
            }

            return roomlist;
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="new_t"></param>
        /// <returns></returns>
        public AjaxResult Edit_Data(TeacherNight new_t)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                this.Update(new_t);
                a.Success = true;
                a.Msg = "编辑成功！！";
                Redis.RemoveCache("TeacherNight");
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = "数据编辑有误，请刷新重试！！";
            }
            return a;
        }

        /// <summary>
        /// 更改日期
        /// </summary>
        /// <returns></returns>
        public AjaxResult Update_Date(bool ischangdate, List<TeacherNight> updata_data, int count, DateTime newdate)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                if (ischangdate)//调课
                {
                    foreach (TeacherNight t in updata_data)
                    {
                        t.OrwatchDate = t.OrwatchDate.AddDays(count);
                        this.Update(t);
                    }
                }
                else //日期调换
                {
                    foreach (TeacherNight t in updata_data)
                    {
                        t.OrwatchDate = newdate;
                        this.Update(t);
                    }
                }
                a.Success = true;
                a.Msg = "操作成功！！！";
                Redis.RemoveCache("TeacherNight");
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = "数据错误，请重试！！！";
                throw;
            }

            return a;
        }

        

        public AjaxResult Add_masterdata(List<TeacherNight> list)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (TeacherNight i in list)
                {
                    int overridecount = this.GetIQueryable().Where(t => t.Tearcher_Id == i.Tearcher_Id && t.OrwatchDate == i.OrwatchDate).ToList().Count;
                    if (overridecount<=0)
                    {
                        this.Insert(i);
                    }
                }
                a.Success = true;
                Redis.RemoveCache("TeacherNight");
                a.Msg = "安排成功！！！";
            }
            catch (Exception ex)
            {
                a.Msg = "数据异常！！,请刷新重试";
                a.Success = false;
            }

            return a;
        }

        
    }
}
