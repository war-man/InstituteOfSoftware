using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    public class EvningSelfStudyManeger : BaseBusiness<EvningSelfStudy>
    {
        static readonly RedisCache redisCache = new RedisCache();
        private BaseDataEnumManeger BaseDataEnum_Entity;
        /// <summary>
        /// 从缓存中获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<EvningSelfStudy> EvningSelfStudyGetAll()
        {
            EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
            List<EvningSelfStudy> EvningSelfStudy_list = new List<EvningSelfStudy>();
            EvningSelfStudy_list = EvningSelfStudyManeger.redisCache.GetCache<List<EvningSelfStudy>>("EvningSelfStudyList");
            if (EvningSelfStudy_list == null || EvningSelfStudy_list.Count == 0)
            {
                EvningSelfStudy_list = this.GetIQueryable().ToList();
                Reconcile_Com.redisCache.SetCache("EvningSelfStudyList", EvningSelfStudy_list);
            }
            return EvningSelfStudy_list;
        }
        /// <summary>
        /// 添加单个数据
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public AjaxResult Add_Data(EvningSelfStudy e)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                int count = EvningSelfStudyGetAll().Where(e1 => e1.Classroom_id == e.Classroom_id && e1.ClassSchedule_id == e.ClassSchedule_id && e1.curd_name == e.curd_name && e1.Anpaidate == e.Anpaidate).ToList().Count;
                if (count <= 0)
                {
                    this.Insert(e);
                    EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                    a.Success = true;
                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 添加多个数据
        /// </summary>
        /// <param name="e_list"></param>
        /// <returns></returns>
        public AjaxResult Add_Data(List<EvningSelfStudy> e_list)
        {
            AjaxResult a = new AjaxResult();
            List<EvningSelfStudy> ore = new List<EvningSelfStudy>();//获取没有重复数据的集合
            try
            {
                int index = 0;
                foreach (EvningSelfStudy new_e in e_list)
                {
                    int count = EvningSelfStudyGetAll().Where(e1 => e1.Classroom_id == new_e.Classroom_id && e1.ClassSchedule_id == new_e.ClassSchedule_id && e1.curd_name == new_e.curd_name && e1.Anpaidate == new_e.Anpaidate).ToList().Count;
                    if (count <= 0)
                    {
                        ore.Add(new_e);
                        index++;
                    }                     
                }
                this.Insert(ore);
                EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                a.Success = true;
                if (index==0) {
                    a.Msg = "晚自习安排成功！！！，没有发现重复数据";
                }
                else
                {
                    a.Msg = "晚自习安排成功！！！，重复数据"+index+"条，已排除";
                }
              
            }
            catch (Exception ex)
            {
                a.Success = true;
                a.Msg = "添加数据有误，请重试！！";
            }
            return a;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public AjaxResult Delete_Data(int eid)
        {
            AjaxResult a = new AjaxResult();
            EvningSelfStudy find_e = this.GetEntity(eid);
            try
            {
                if (find_e != null)
                {
                    this.Delete(find_e);
                    EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                    a.Success = true;
                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 修改(只修改安排的日期跟上课时间段)
        /// </summary>
        /// <param name="new_e"></param>
        /// <returns></returns>
        public AjaxResult Update_Data(EvningSelfStudy new_e)
        {
            AjaxResult a = new AjaxResult();
            EvningSelfStudy old_e = this.GetEntity(new_e.id);
            try
            {
                if (old_e != null)
                {
                    old_e.Anpaidate = new_e.Anpaidate;
                    old_e.curd_name = new_e.curd_name;
                    this.Update(old_e);
                    redisCache.RemoveCache("EvningSelfStudyList");
                    a.Success = true;
                }
            }
            catch (Exception ex)
            {
                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        public AjaxResult Update_DataTwo(EvningSelfStudy new_e)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                if (new_e.emp_id == "0")
                {
                    new_e.emp_id = null;
                }
                this.Update(new_e);
                redisCache.RemoveCache("EvningSelfStudyList");
                a.Success = true;
            }
            catch (Exception ex)
            {

                a.Msg = ex.Message;
                a.Success = false;
            }
            return a;
        }
        /// <summary>
        /// 判断XX班级是否安排了晚自习
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="class_id">班级编号</param>
        /// <returns></returns>
        public bool IsAlreadAnpai(DateTime time, int class_id)
        {
            int count = EvningSelfStudyGetAll().Where(e => e.Anpaidate == time && e.ClassSchedule_id == class_id).Count();
            return count > 0 ? true : false;
        }
        /// <summary>
        /// 查询晚自习安排数据
        /// </summary>
        /// <param name="timename">上课时间段</param>
        /// <param name="time">日期</param>
        /// <param name="roomlist">教室集合</param>
        /// <returns></returns>
        public List<AnPaiData> getAppoint(string timename, DateTime time, List<Classroom> roomlist)
        {
            List<AnPaiData> list = new List<AnPaiData>();
            foreach (Classroom item in roomlist)
            {
                EvningSelfStudy fine = EvningSelfStudyGetAll().Where(e => e.curd_name == timename && e.Anpaidate == time && e.Classroom_id == item.Id).FirstOrDefault();
                AnPaiData a = new AnPaiData();
                if (fine != null)
                {
                    a.NeiRong = "晚自习";
                    a.ClassName = Reconcile_Com.ClassSchedule_Entity.GetEntity(fine.ClassSchedule_id).ClassNumber;
                    a.class_Id = fine.ClassSchedule_id;
                }
                list.Add(a);
            }
            return list;
        }
        /// <summary>
        /// 获取XX日期XX教室晚自习安排
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="classroom_id">教室</param>
        /// <returns></returns>
        public EvningSelfStudy GetNving(DateTime time, int class_id)
        {
            return EvningSelfStudyGetAll().Where(e => e.Anpaidate == time && e.ClassSchedule_id == class_id).FirstOrDefault();
        }
        /// <summary>
        /// 获取晚自习在XX教室上课的班级
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="classoom_id"></param>
        /// <returns></returns>
        public List<EvningSelfStudy> GetOnCurrClass(DateTime dateTime, int classoom_id)
        {
            return EvningSelfStudyGetAll().Where(e => e.Anpaidate == dateTime && e.Classroom_id == classoom_id).ToList();
        }
        /// <summary>
        /// 获取空教室
        /// </summary>
        /// <param name="dateTime">日期</param>
        /// <param name="old">false-达康维嘉校区，true--继善高科校区</param>
        /// <returns></returns>
        public ClassRoom_AddCourse GetEmptyClassroom(DateTime dateTime, bool old)
        {

            ClassRoom_AddCourse result = new ClassRoom_AddCourse();
            BaseDataEnum_Entity = new BaseDataEnumManeger();
            List<EvningSelfStudy> getlist = EvningSelfStudyGetAll().Where(e => e.Anpaidate == dateTime).ToList();
            int base_id = 0;
            if (old)
            {
                //获取继善高科校区教室
                base_id = BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
            }
            else
            {
                base_id = BaseDataEnum_Entity.GetSingData("达嘉维康校区", false).Id;
            }
            if (base_id != 0)
            {
                List<Classroom> list_classroom1 = Reconcile_Com.Classroom_Entity.GetAddreeClassRoom(base_id);
                foreach (var item in list_classroom1)
                {
                    //ClassRoom_AddCourse
                    List<EvningSelfStudy> list_ev = getlist.Where(e => e.Classroom_id == item.Id && e.Anpaidate == dateTime).ToList();
                    int count = list_ev.Count;
                    if (count == 0)
                    {
                        result.ClassRoomId = item.Id;
                        result.TimeName = "晚一";
                        break;
                    }
                    else if (count == 1)
                    {
                        result.ClassRoomId = item.Id;
                        result.TimeName = list_ev[0].curd_name == "晚一" ? "晚二" : "晚一";
                        break;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 日期往前挪或往后退(全体调课)
        /// </summary>
        /// <param name="s1ors3"></param>
        /// <param name="count"></param>
        /// <param name="starTime"></param>
        /// <returns></returns>
        public AjaxResult ALLDataADI(int count, List<EvningSelfStudy> e_list)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (EvningSelfStudy item in e_list)
                {
                    item.Anpaidate = item.Anpaidate.AddDays(count);
                    this.Update(item);
                }
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }
            return a;
        }


        /// <summary>
        /// 上课日期调换
        /// </summary>
        /// <param name="e"></param>
        /// <param name="d1"></param>
        /// <returns></returns>
        public AjaxResult ChangDate(List<EvningSelfStudy> e, DateTime d1)
        {
            AjaxResult a = new AjaxResult();
            try
            {
                foreach (EvningSelfStudy it in e)
                {
                    it.Anpaidate = d1;
                    this.Update(it);
                }
                EvningSelfStudyManeger.redisCache.RemoveCache("EvningSelfStudyList");
                a.Success = true;
            }
            catch (Exception ex)
            {
                a.Success = false;
                a.Msg = ex.Message;
            }

            return a;
        }

        /// <summary>
        /// 获取XX班级在XX日期段的晚自习安排
        /// </summary>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <param name="class_id"></param>
        /// <returns></returns>
        public List<EvningSelfStudy> GetConditionEvningData(DateTime starTime, DateTime endTime, int class_id)
        {
            return EvningSelfStudyGetAll().Where(e => e.Anpaidate >= starTime && e.Anpaidate <= e.Anpaidate && e.ClassSchedule_id == class_id).ToList();
        }

        /// <summary>
        /// 晚自习安排
        /// </summary>
        /// <param name="startime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public AjaxResult GetNewEvningData(DateTime startime, DateTime endtime, bool Isemptyclass, bool Doublerest)
        {
            AjaxResult a = new AjaxResult();
            List<EmptyClass> emotylist = new List<EmptyClass>();
            ReconcileManeger Reconcile_Entity = new ReconcileManeger();
            StringBuilder msg = new StringBuilder();
            List<EvningSelfStudy> ev_list = new List<EvningSelfStudy>();
            int curtypeid = Reconcile_Com.CourseType_Entity.FindSingeData("专业课", false).Id;//获取课程类型id
            int curtypeid2 = Reconcile_Com.CourseType_Entity.FindSingeData("语文课", false).Id; 
            int curtypeid3 = Reconcile_Com.CourseType_Entity.FindSingeData("数学课", false).Id; 
            int curtypeid4 = Reconcile_Com.CourseType_Entity.FindSingeData("英语课", false).Id; 
            List<Reconcile> Recon_all = Reconcile_Entity.AllReconcile().Where(r => r.AnPaiDate >= startime && r.AnPaiDate <= endtime && Reconcile_Com.GetNameGetCur(r.Curriculum_Id) != null).ToList();
            Recon_all = Recon_all.Where(r => Reconcile_Com.GetNameGetCur(r.Curriculum_Id).CourseType_Id == curtypeid || Reconcile_Com.GetNameGetCur(r.Curriculum_Id).CourseType_Id == curtypeid2 || Reconcile_Com.GetNameGetCur(r.Curriculum_Id).CourseType_Id == curtypeid3 || Reconcile_Com.GetNameGetCur(r.Curriculum_Id).CourseType_Id == curtypeid4).OrderBy(r => r.AnPaiDate).ToList();//获取这个时间段上专业课的排课数据
            List<ClassSchedule> classSchedule_all = Reconcile_Com.GetClass();//获取所有有效的班级

            int timenameindex = 0;
            string[] timename = new string[] { "晚一", "晚二" };

            TimeSpan t = endtime - startime;
            int days = t.Days;
            for (int n = days; n >= 0; n--)
            {

                bool isok = true;
                if (Doublerest)//双休
                {
                    int studyindex = IsSaturday(startime);
                    if (studyindex == 1 || studyindex == 2)
                    {
                        isok = false;//该日期不能安排晚自习
                    }
                }
                else //单休
                {
                    int studyindex = IsSaturday(startime);
                    if (studyindex == 2)
                    {
                        isok = false;
                    }
                }

                if (isok) //这个日期合理可以安排
                {
                    EmptyClass new_e = new EmptyClass();
                    new_e.date = startime;
                    List<string> classname = new List<string>();
                    for (int i = 0; i < classSchedule_all.Count; i++)//判断这个日期这个班级是否安排了专业课
                    {
                        Reconcile find_r = Recon_all.Where(r => r.ClassSchedule_Id == classSchedule_all[i].id && r.AnPaiDate == startime).FirstOrDefault();

                        //返回没有排课的班级
                        if (find_r == null)
                        {
                            classname.Add(classSchedule_all[i].ClassNumber);
                        }
                        else
                        {
                            if (find_r != null)//安排晚自习
                            {
                                Reconcile find_r2 = Recon_all.Where(r => r.ClassRoom_Id == find_r.ClassRoom_Id && r.ClassSchedule_Id != find_r.ClassSchedule_Id).FirstOrDefault();
                                EvningSelfStudy new_ev = new EvningSelfStudy();
                                new_ev.Anpaidate = startime;
                                new_ev.Classroom_id = Convert.ToInt32(find_r.ClassRoom_Id);
                                new_ev.ClassSchedule_id = classSchedule_all[i].id;
                                new_ev.curd_name = timename[timenameindex];
                                new_ev.emp_id = null;
                                new_ev.IsDelete = false;
                                new_ev.Newdate = DateTime.Now;
                                ev_list.Add(new_ev);
                                if (find_r2 != null)
                                {
                                    ClassSchedule find_class = classSchedule_all.Where(c => c.id == find_r2.ClassSchedule_Id).FirstOrDefault();
                                    if (find_class != null)
                                    {
                                        classSchedule_all.Remove(find_class);
                                        EvningSelfStudy new_ev2 = new EvningSelfStudy();
                                        new_ev2.Anpaidate = startime;
                                        new_ev2.Classroom_id = Convert.ToInt32(find_r2.ClassRoom_Id);
                                        new_ev2.ClassSchedule_id = find_r2.ClassSchedule_Id;
                                        new_ev2.curd_name = new_ev.curd_name == "晚一" ? "晚二" : "晚一";
                                        new_ev2.emp_id = null;
                                        new_ev2.IsDelete = false;
                                        new_ev2.Newdate = DateTime.Now;
                                        ev_list.Add(new_ev2);
                                    }

                                }
                            }                            
                        }
                    }
                    startime = startime.AddDays(1);
                    new_e.ClassName = classname;
                    if (classname.Count>0)
                    {
                        emotylist.Add(new_e);
                    }                    
                }
                else
                {
                    startime = startime.AddDays(1);
                }
                timenameindex = timenameindex == 0 ? 1 : 0;
            }
            
            if (emotylist.Count > 0)
            {
                a.Data = emotylist;
                a.Success = false;//有没安排专业课的班级
            }
            else
            {
                a.Success = true;//没有
                a.Data = ev_list;
            }
            return a;
        }

        /// <summary>
        /// 获取单休月份数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="XmlFile_url"></param>
        /// <returns></returns>
        public GetYear MyGetYear(string year, string XmlFile_url)
        {
            ReconcileManeger Reconcile_Entity = new ReconcileManeger();
            return Reconcile_Entity.MyGetYear(year, XmlFile_url);
        }

        /// <summary>
        /// 判断该日期是否是周六或周末（1--周六，2--周日）
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public int IsSaturday(DateTime date)
        {
            ReconcileManeger Reconcile_Entity = new ReconcileManeger();
            return Reconcile_Entity.IsSaturday(date);
        }

        /// <summary>
        /// 获取空教室
        /// </summary>
        /// <param name="timename">时间</param>
        /// <param name="time">日期</param>
        /// <param name="shcooladrees_id">校区ID</param>
        /// <returns></returns>
        public List<Classroom> GetEmptyClassrooms(string timename, DateTime time, int shcooladrees_id)
        {
            var classroomid = this.EvningSelfStudyGetAll().Where(e => e.Anpaidate == time && e.curd_name == timename).ToList().Select(e => e.Classroom_id).ToList();//获取被安排的教室id

            List<Classroom> list = Reconcile_Com.Classroom_Entity.GetAddreeClassRoom(shcooladrees_id);//获取校区的有效教室

            foreach (var id in classroomid)
            {
                int cid = Convert.ToInt32(id);
                Classroom find = list.Where(c => c.Id == cid).FirstOrDefault();
                if (find != null)
                {
                    list.Remove(find);
                }
            }

            return list;
        }
    }
}
