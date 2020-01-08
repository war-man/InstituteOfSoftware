using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using Base_SysManage;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using EmployeesBusiness;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using System.Linq.Dynamic;
    using SiliconValley.InformationSystem.Business.CourseSchedulingSysBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Entity.Entity;
    using System.Xml;

    public class TeacherBusiness : BaseBusiness<Teacher>
    {

        //员工 业务上下文
        private readonly EmployeesInfoManage db_emp;



        //阶段 业务上下文

        private readonly GrandBusiness db_grand;

        //专业上下文

        private readonly SpecialtyBusiness db_specialty;

        /// <summary>
        /// 调课业务实例
        /// </summary>
        private readonly BaseBusiness<ConvertCourse> db_convertCourse;

        private readonly BaseBusiness<SubstituteTeachCourse> db_substituteTeacherCourse;
        public TeacherBusiness()
        {
            db_grand = new GrandBusiness();
            db_emp = new EmployeesInfoManage();
            db_specialty = new SpecialtyBusiness();
            db_convertCourse = new BaseBusiness<ConvertCourse>();
            db_substituteTeacherCourse = new BaseBusiness<SubstituteTeachCourse>();

        }

        public EmpDetailView ConvertToEmpDetailView(EmployeesInfo employeesInfo)
        {
            EmpDetailView view = new EmpDetailView();

            view.Address = employeesInfo.Address;
            view.Age = employeesInfo.Age;
            view.BCNum = employeesInfo.BCNum;
            view.Birthdate = employeesInfo.Birthdate;
            view.Birthday = employeesInfo.Birthday;
            view.ContractEndTime = employeesInfo.ContractEndTime;
            view.ContractStartTime = employeesInfo.ContractStartTime;
            view.DDAppId = db_emp.GetDeptByEmpid(employeesInfo.EmployeeId);
            view.DomicileAddress = employeesInfo.DomicileAddress;
            view.Education = employeesInfo.Education;
            view.EmployeeId = employeesInfo.EmployeeId;
            view.EmpName = employeesInfo.EmpName;
            view.EntryTime = employeesInfo.EntryTime;
            view.IdCardIndate = employeesInfo.IdCardIndate;
            view.IdCardNum = employeesInfo.IdCardNum;

            //获取图片路径

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(System.Web.HttpContext.Current.Server.MapPath("/Areas/Teaching/config/teacherConfig.xml"));

            var xmlRoot = xmlDocument.DocumentElement;

            var Avatar = (XmlElement)xmlRoot.GetElementsByTagName("Avatar")[0];

            //头像路径 
            var avatarUrl = Avatar.Attributes["url"].Value;
            if (employeesInfo.Image == null || employeesInfo.Image == "")
            {
                //默认头像
                var defaultImg = Avatar.GetElementsByTagName("default")[0];
                view.Image = avatarUrl + defaultImg.Attributes["img"].Value;
            }
            else
            {
                view.Image = avatarUrl + employeesInfo.Image;
            }

            
            view.IsDel = employeesInfo.IsDel;
            view.MaritalStatus = employeesInfo.MaritalStatus;
            view.Material = employeesInfo.Material;
            view.Nation = employeesInfo.Nation;
            view.Phone = employeesInfo.Phone;
            view.PoliticsStatus = employeesInfo.PoliticsStatus;
            view.PositionId = db_emp.GetPositionByEmpid(employeesInfo.EmployeeId);
            view.PositiveDate = employeesInfo.PositiveDate;
            view.ProbationSalary = employeesInfo.ProbationSalary;
            view.Remark = employeesInfo.Remark;
            view.Salary = employeesInfo.Salary;
            view.Sex = employeesInfo.Sex;
            view.SSStartMonth = employeesInfo.SSStartMonth;
            view.UrgentPhone = employeesInfo.UrgentPhone;
            view.WorkExperience = employeesInfo.WorkExperience;

            return view;
        }


        public bool EmpIsDel(Teacher teacher)
        {
            return (bool)db_emp.GetList().Where(d => d.EmployeeId == teacher.EmployeeId).FirstOrDefault().IsDel;
        }



        /// <summary>
        /// 根据ID获取教员
        /// </summary>
        /// <param name="">教员ID</param>
        /// <returns>教员实体</returns>
        public Teacher GetTeacherByID(int? id)
        {
            Teacher teacher = this.GetList().Where(t => t.TeacherID == id).FirstOrDefault();


            if (EmpIsDel(teacher))
            {
                return null;
            }

            return teacher;
        }


        /// <summary>
        /// 获取所有教员
        /// </summary>
        /// <returns></returns>
        public List<Teacher> GetTeachers(bool isContains_Jiaowu = false)
        {

            List<Teacher> resultlist = new List<Teacher>();

            //从缓存中获取
            RedisCache redisCache = new RedisCache();

            resultlist = redisCache.GetCache<List<Teacher>>("TeacherList");

            if (resultlist == null || resultlist.Count == 0)
            {

                resultlist = new List<Teacher>();


                var temp = this.GetList().Where(d => d.IsDel == false).ToList();

                foreach (var item in temp)
                {
                    if (!EmpIsDel(item))
                    {
                        resultlist.Add(item);
                    }
                }


                redisCache.SetCache("TeacherList", resultlist);
            }

            if (isContains_Jiaowu)
            {
                return resultlist.OrderByDescending(d => d.TeacherID).ToList();
            }
           

            //排除掉教务

            List<Teacher> returnlist = new List<Teacher>();

            foreach (var item in resultlist)
            {
               var emp = this.GetEmpByEmpNo(item.EmployeeId);

                var empview = this.ConvertToEmpDetailView(emp);

                if (empview != null)
                {
                    if (!empview.PositionId.PositionName.Contains("教务"))
                    {
                        returnlist.Add(item);
                    }
                }

            }

            return returnlist.OrderByDescending(d => d.TeacherID).ToList();


        }
        /// <summary>
        /// 获取所有教员
        /// </summary>
        /// <returns></returns>
        public List<Teacher> GetTeachers1()
        {

            List<Teacher> resultlist = new List<Teacher>();

            var temp = this.GetList().Where(d => d.IsDel == false).ToList();

            foreach (var item in temp)
            {
                if (!EmpIsDel(item))
                {
                    resultlist.Add(item);
                }
            }

            //排除掉教务

            List<Teacher> returnlist = new List<Teacher>();

            foreach (var item in resultlist)
            {
                var emp = this.GetEmpByEmpNo(item.EmployeeId);

                var empview = this.ConvertToEmpDetailView(emp);

                if (empview != null)
                {
                    if (!empview.PositionId.PositionName.Contains("教务"))
                    {
                        returnlist.Add(item);
                    }
                }

            }


            return returnlist.OrderByDescending(d => d.TeacherID).ToList();

        }

        /// <summary>
        /// / 更具员工编号获取员工
        /// </summary>
        /// <param name="EmpNo">员工编号</param>
        /// <returns>员工实体</returns>
        public EmployeesInfo GetEmpByEmpNo(string EmpNo)
        {


            return db_emp.GetList().Where(d => d.EmployeeId == EmpNo && d.IsDel == false).FirstOrDefault();
        }


        /// <summary>
        /// 根据教员ID 获取教员阶段
        /// </summary>
        /// <param name="id">j教员ID</param>
        public List<Grand> GetGrandByTeacherID(int id)
        {
            List<Grand> resultList = new List<Grand>();

            BaseBusiness<TecharOnstageBearing> business = new BaseBusiness<TecharOnstageBearing>();

            var bus = business.GetList().Where(t => t.TeacherID == id).ToList();

            foreach (var item in bus)
            {
                var tempobj = db_grand.GetList().Where(t => t.Id == item.Stage && t.IsDelete == false).FirstOrDefault();

                    if (tempobj != null)
                    {
                        if (!Grand.IsInList(resultList, tempobj))
                        {
                            resultList.Add(tempobj);
                        }
                        

                       
                    }
            }

            return resultList;
        }

        /// <summary>
        /// 根据教员ID 获取教员的技术专业
        /// </summary>
        /// <param name="ID">教员ID</param>
        /// <returns>返回专业集合</returns>
        public List<Specialty> GetMajorByTeacherID(int ID)
        {
            List<Specialty> returnList = new List<Specialty>();


            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var majors = baseBusiness.GetList().Where(t => t.TeacherID == ID).ToList();

            foreach (var item in majors)
            {
                if (item.Major != null)
                {

                    var dd = db_specialty.GetList().Where(t => t.Id == item.Major && t.IsDelete == false).FirstOrDefault();

                    if (!db_specialty.IsInList(returnList, dd))
                    {
                        returnList.Add(dd);
                    }
                }

            }
            return returnList;

        }

        public bool ContainDic(Dictionary<Specialty, List<Grand>> source, Specialty key)
        {

            foreach (var item in source.Keys)
            {
                if (item.Id == key.Id)
                {

                    return true;
                }
            }

            return false;

        }

        public TeacherDetailView GetTeacherView(int id)
        {

            BaseBusiness<Position> curent_posi = new BaseBusiness<Position>();

            BaseBusiness<Department> curent_Dep = new BaseBusiness<Department>();

            TeacherDetailView teacherResult = new TeacherDetailView();

            //获取教员信息
            Teacher t = this.GetTeacherByID(id);

            //获取教员基本信息
            EmployeesInfo emp = this.GetEmpByEmpNo(t.EmployeeId);
            teacherResult.emp = emp;

            var post = curent_posi.GetList().Where(d => d.Pid == emp.PositionId && d.IsDel == false).FirstOrDefault();


            teacherResult.TeacherID = t.TeacherID;

            teacherResult.Position = post;

            var dep = curent_Dep.GetList().Where(d => d.DeptId == post.DeptId && d.IsDel == false).FirstOrDefault();


            teacherResult.Department = dep;


            //获取教员阶段信息
            teacherResult.Grands = this.GetGrandByTeacherID(t.TeacherID);

            //获取教员专业信息
            teacherResult.Major = this.GetMajorByTeacherID(t.TeacherID);

            //获取技术信息
            teacherResult.AttendClassStyle = t.AttendClassStyle;
            teacherResult.ProjectExperience = t.ProjectExperience;
            teacherResult.TeachingExperience = t.TeachingExperience;
            teacherResult.WorkExperience = t.WorkExperience;

            return teacherResult;

        }



        /// <summary>
        /// 获取教员的专业没有阶段
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="marjorId"></param>
        /// <returns></returns>
        public List<Grand> GetNoGrand(int teacherId, int majorId)
        {


            List<Grand> resultList = new List<Grand>();

            Specialty Major = db_specialty.GetSpecialtyByID(majorId);
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var haveGrands = baseBusiness.GetList().Where(t => t.TeacherID == teacherId && t.Major == majorId).ToList().OrderBy(t => t.Stage).ToList();

            //获取全部阶段 
            List<Grand> grands = db_grand.GetList().Where(d => d.IsDelete == false).OrderBy(t => t.Id).ToList();

            foreach (var item in grands)//1 2 3
            {
                if (!contains(item.Id, haveGrands))
                {
                    resultList.Add(item);

                }
            }

            //var ss = resultList.Distinct() as List<Grand>;

            return resultList;
        }

        public bool contains(int id, List<TecharOnstageBearing> grands)
        {
            return grands.Where(e => e.Stage == id).ToList().Count > 0;

        }

        public List<Grand> GetHaveGrand(int teacherid, int majorid)
        {

            List<Grand> grands = new List<Grand>();

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var lsit = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == majorid).ToList();

            var grandlist = db_grand.GetList().Where(d => d.IsDelete == false);

            foreach (var item in grandlist)
            {
                foreach (var item1 in lsit)
                {
                    if (item.Id == item1.Stage)
                    {
                        grands.Add(item);
                    }
                }
            }

            return grands;

        }


        public List<EmployeesInfo> employeesInfos()
        {

            BaseBusiness<Department> DepbaseBusiness = new BaseBusiness<Department>();

            var deplist = DepbaseBusiness.GetList().Where(d => d.IsDel == false).ToList();

            var emps = db_emp.GetList().Where(d => d.IsDel == false).OrderBy(d => d.PositionId).ToList();

            BaseBusiness<Position> PobaseBusiness = new BaseBusiness<Position>();

            var ind = PobaseBusiness.GetList().Where(d => d.IsDel == false).Where(d => d.DeptId == 2).ToList().OrderBy(d => d.Pid).ToList();


            List<EmployeesInfo> result = new List<EmployeesInfo>();

            foreach (var item in emps)
            {
                foreach (var item1 in ind)
                {
                    if (item.PositionId == item1.Pid)
                    {
                        result.Add(item);
                    }
                }
            }
            var list = new List<EmployeesInfo>();

            foreach (var item in result)
            {
                var ss = this.GetTeachers().Where(d => d.EmployeeId == item.EmployeeId).ToList();

                if (ss.Count <= 0 || ss == null)
                {
                    list.Add(item);
                }

            }

            return list;
        }


        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>

        public List<Teacher> BrushSelectionTeacher(string Name, string Phone)
        {
            return null;
        }



        /// <summary>
        /// 根据岗位获取教员
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>

        public List<Teacher> BrushSelectionTeacher(int positionId)
        {

            List<Teacher> resultlist = new List<Teacher>();

            var emps = db_emp.GetList().Where(d => d.PositionId == positionId && d.IsDel == false).ToList();

            var teachers = this.GetTeachers();

            foreach (var e in emps)
            {

                foreach (var t in teachers)
                {
                    if (t.EmployeeId == e.EmployeeId)
                    {
                        resultlist.Add(t);

                    }
                }

            }

            return resultlist;
        }


        /// <summary>
        /// 获取阶段教员
        /// </summary>
        /// <param name="grandid"></param>
        /// <returns></returns>
        public List<Teacher> BrushSelectionByGrand(int grandid)
        {
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var ss = baseBusiness.GetList().Where(d => d.Stage == grandid).ToList();

            List<TecharOnstageBearing> onstageBearings = new List<TecharOnstageBearing>();


            foreach (var item in ss)
            {
                if (!TecharOnstageBearing.ISContainTeacher(onstageBearings, item.TeacherID))
                {
                    onstageBearings.Add(item);
                }
            }

            List<Teacher> resultlist = new List<Teacher>();

            foreach (var item in onstageBearings)
            {
                resultlist.Add(this.GetTeachers().Where(d => d.TeacherID == item.TeacherID).FirstOrDefault());
            }

            return resultlist;
        }

        /// <summary>
        ///获取教员
        /// </summary>
        /// <param name="majorid">阶段</param>
        /// <returns>老师集合</returns>

        public List<Teacher> BrushSelectionByMajor(int majorid)
        {

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var ss = baseBusiness.GetList().Where(d => d.Major == majorid).ToList();

            List<TecharOnstageBearing> onstageBearings = new List<TecharOnstageBearing>();


            foreach (var item in ss)
            {
                if (!TecharOnstageBearing.ISContainTeacher(onstageBearings, item.TeacherID))
                {
                    onstageBearings.Add(item);
                }
            }

            List<Teacher> resultlist = new List<Teacher>();

            foreach (var item in onstageBearings)
            {
                resultlist.Add(this.GetTeachers().Where(d => d.TeacherID == item.TeacherID && d.IsDel == false).FirstOrDefault());
            }

            return resultlist;
        }


        public List<Teacher> getTeacherByMajorAndGrand(int majorid, int grandid)
        {

            List<Teacher> resultlist = new List<Teacher>();

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var ss = baseBusiness.GetList().Where(d => d.Major == majorid && grandid == d.Stage).ToList();


            foreach (var item in ss)
            {
                resultlist.Add(this.GetTeachers().Where(d => d.TeacherID == item.TeacherID && d.IsDel == false).FirstOrDefault());
            }

            return resultlist;
        }

        /// <summary>
        /// 获取教员擅长的技术
        /// </summary>
        /// <param name="teacherid">教员id</param>
        /// <param name="majorid">专业id</param>
        /// <returns></returns>
        public List<Curriculum> GetTeacherGoodCurriculum(int teacherid, int majorid)
        {

            List<Curriculum> resultlist = new List<Curriculum>();

            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();
            BaseBusiness<Curriculum> Curriculum_db = new BaseBusiness<Curriculum>();
            var temp = goodskill_db.GetList().Where(d => d.TearchID == teacherid).ToList();


            foreach (var item in temp)
            {
                var curr = Curriculum_db.GetList().Where(d => d.IsDelete == false && d.CurriculumID == item.Curriculum).FirstOrDefault();

                if (curr.MajorID == majorid)
                {

                    resultlist.Add(curr);
                }

            }

            return resultlist;
        }


        public List<Curriculum> GetCurriculaOnTeacherNoHave(int teacherid, int majorid)
        {

            var resultlist = new List<Curriculum>();

            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();

            BaseBusiness<Curriculum> curr_db = new BaseBusiness<Curriculum>();

            var temp1 = goodskill_db.GetList().Where(d => d.TearchID == teacherid).ToList().OrderBy(d => d.Curriculum).ToList();

            var temp2 = db_specialty.GetList().Where(d => d.IsDelete == false && d.Id == majorid).FirstOrDefault();

            var temp3 = curr_db.GetList().Where(d => d.IsDelete == false && d.MajorID == majorid).ToList();


            var all = new List<Curriculum>();

            foreach (var item in temp1)
            {
                var cur = temp3.Where(d => d.CurriculumID == item.Curriculum).FirstOrDefault();

                if ( cur !=null && cur.MajorID == majorid)
                {

                    all.Add(cur);

                }
            }
            CurriculumBusiness curriculumBusiness = new CurriculumBusiness();

            //这个专业的技能

            all = all.OrderBy(d => d.CurriculumID).ToList();


            foreach (var item in temp3)
            {

                if (!curriculumBusiness.IsHave(all, item.CurriculumID))
                {
                    resultlist.Add(item);

                }

            }

            return resultlist;
        }



        /// <summary>
        /// 获取教员不擅长的公共课
        /// </summary>
        /// <returns></returns>
        public List<Curriculum> GetpublicCurriculaOnTeacherNoHave(int teacherId)
        {
            List<Curriculum> result = new List<Curriculum>();

           CourseSyllabusBusiness.CourseBusiness courseBusiness = new CourseSyllabusBusiness.CourseBusiness();

            var alllist = courseBusiness.GetCurriculas().Where(d => d.MajorID == null).ToList(); //所有公共课

            var list1 = GetPublickCurriculaOnTeacher(teacherId); //教员可以上的公共课
            //排除掉可以上的公共课


            if (list1.Count == 0)
            {
                result = alllist;
            }
            else
            {
                foreach (var item in alllist)
                {
                    var tempobj = list1.Where(d => d.CurriculumID == item.CurriculumID).FirstOrDefault();

                    if (tempobj == null)
                    {
                        result.Add(tempobj);
                    }
                }
            }
            

            return result;

        }

        /// <summary>
        /// 获取教员可以上的公共课
        /// </summary>
        /// <returns></returns>
        public List<Curriculum> GetPublickCurriculaOnTeacher(int teacher)
        {
            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();
            CourseSyllabusBusiness.CourseBusiness courseBusiness = new CourseSyllabusBusiness.CourseBusiness();
            var templist = goodskill_db.GetIQueryable().ToList().Where(d => d.TearchID == teacher).ToList();

            List<Curriculum> result = new List<Curriculum>();

            foreach (var item in templist)
            {
               var tempobj = courseBusiness.GetCurriculas().ToList().Where(d => d.CurriculumID == item.Curriculum).FirstOrDefault();

                if (tempobj.MajorID == null)
                {
                    result.Add(tempobj);
                }
            }

            return result;
        }

        /// <summary>
        /// 给教员添加新的技能
        /// </summary>
        /// <param name="teacherid"></param>
        /// <param name="currid"></param>
        public void SetNewSkillToTeacher(int teacherid, string[] currid)
        {

            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();


            foreach (var item in currid)
            {
                if (goodskill_db.GetList().Where(d => d.TearchID == teacherid && d.Curriculum == int.Parse(item)).FirstOrDefault() == null)
                {

                    GoodSkill goodSkill = new GoodSkill();
                    goodSkill.Curriculum = int.Parse(item);
                    goodSkill.TearchID = teacherid;

                    goodskill_db.Insert(goodSkill);

                }
            }

        }
        /// <summary>
        /// 给教员分配阶段
        /// </summary>
        /// <param name="ids">阶段</param>
        /// <param name="majorid">专业id</param>
        /// <param name="teacherid">教员id</param>
        /// <returns></returns>
        public void SetGrandToTeacherOnMajor(string[] grands, int majorid, int teacherid)
        {
            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            if (this.GetTeacherByID(teacherid) != null)
            {

                foreach (var item in grands)
                {

                    var tempobj = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Stage == int.Parse(item) && majorid == d.Major).FirstOrDefault();

                    var tempobj1 = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Stage == null && majorid == d.Major).FirstOrDefault();

                    if (tempobj == null  && tempobj1==null)
                    {

                        TecharOnstageBearing techarOnstageBearing = new TecharOnstageBearing();

                        techarOnstageBearing.TeacherID = teacherid;

                        techarOnstageBearing.Stage = int.Parse(item);

                        techarOnstageBearing.Major = majorid;


                        baseBusiness.Insert(techarOnstageBearing);

                    }

                    if (tempobj1 != null)
                    {

                        tempobj1.Stage = int.Parse(item);
                        baseBusiness.Update(tempobj1);
                    }



                }


            }


        }


        /// <summary>
        /// 获取教员没有的专业
        /// </summary>
        /// <param name="teacherid"></param>
        /// <returns></returns>

        public List<Specialty> GetNoHaveMajorOnTeacher(int teacherid)
        {
            List<Specialty> resultlist = new List<Specialty>();

            if (this.GetTeacherByID(teacherid) == null)
            {
                return resultlist;
            }


            List<Specialty> havemajors = this.GetMajorByTeacherID(teacherid).OrderBy(d => d.Id).ToList();

            List<Specialty> templist = db_specialty.GetList().OrderBy(d => d.Id).ToList();

            foreach (var item in templist)
            {
                if (!Specialty.IsContain(havemajors, item))
                {
                    resultlist.Add(item);
                }
            }

            return resultlist;
        }


        /// <summary>
        /// 给教员添加专业
        /// </summary>
        public void SetMajorToTeacher(string[] ids, int teacherid)
        {

            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            if (this.GetTeacherByID(teacherid) != null)
            {

                foreach (var item in ids)
                {
                    var tempobj1 = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == int.Parse(item) && d.Stage == null).FirstOrDefault();

                    if (tempobj1 == null)
                    {

                        //添加
                        TecharOnstageBearing techarOnstageBearing = new TecharOnstageBearing();
                        techarOnstageBearing.TeacherID = teacherid;
                        techarOnstageBearing.Major = int.Parse(item);

                        baseBusiness.Insert(techarOnstageBearing);

                    }

                }
            }

        }
        /// <summary>
        /// 移除教员在专业上对应的阶段
        /// </summary>
        /// <param name="grandid">阶段ID</param>
        /// <param name="teacherid">教员ID</param>
        /// <param name="majorid">专业ID</param>
        /// <returns></returns>
        public void RemoveGrandOnTeacherMajor(int grandid, int teacherid, int majorid)
        {

            if (this.GetTeacherByID(teacherid) == null)
                return;


            BaseBusiness<TecharOnstageBearing> baseBusiness = new BaseBusiness<TecharOnstageBearing>();

            var temp = baseBusiness.GetList();
            var temp2list = baseBusiness.GetList().Where(d => d.TeacherID == teacherid && d.Major == majorid).ToList();
            foreach (var item in temp)
            {

                
                    if (item.TeacherID == teacherid && item.Major == majorid && item.Stage == grandid)
                    {
                        if (temp2list.Count > 1)
                        {

                            baseBusiness.Delete(item);
                             break;   
                        }
                        else
                        {
                            item.Stage = null;

                            baseBusiness.Update(item);

                            break;
                        }

                   
                    }
                


               
            }
        }


        /// <summary>
        /// 移除教员擅长的课程
        /// </summary>
        /// <param name="teacherid">教员ID</param>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public void removeTeacherGoodSkill(int teacherid, int courseid)
        {
            BaseBusiness<GoodSkill> goodskill_db = new BaseBusiness<GoodSkill>();

            if (this.GetTeacherByID(teacherid) == null)
                return;

           var tempobj= goodskill_db.GetList().Where(d => d.TearchID == teacherid && d.Curriculum == courseid).FirstOrDefault();

            if (tempobj == null)
                return;

            goodskill_db.Delete(tempobj);

        }


        /// <summary>
        /// 添加教员
        /// </summary>
        /// <returns></returns>
        
        public bool AddTeacher(Teacher teacher)
        {

                teacher.IsDel = false;

                teacher.MinimumCourseHours = 0;

            bool result = true;

            try
            {
                this.Insert(teacher);


                //更新缓存
                RedisCache redisCache = new RedisCache();

                redisCache.RemoveCache("TeacherList");

            }
            catch (Exception)
            {

                result = false;
            }

            return result;

        }


        /// <summary>
        ///教员离职
        /// </summary>
        /// <param name="empid">员工编号</param>
        /// <returns></returns>
        public bool dimission(string empid)
        {

            bool result = true;

            try
            {

               var obj = this . GetTeachers(true).Where(db_emp => db_emp.EmployeeId == empid).FirstOrDefault();

                obj.IsDel = true;

                this.Update(obj);


            }
            catch (Exception)
            {

                result = false;
            }

            return result;

        }

        /// <summary>
        /// 调课
        /// </summary>
        public void AdjustmentCourse(ConvertCourse convertCourse)
        {

            //1`填写表单  
                db_convertCourse.Insert(convertCourse);
               
        }

        /// <summary>
        /// 提交代课表单
        /// </summary>
        /// <param name="convertCourse"></param>
        public void SubstituteTeachCourse(SubstituteTeachCourse convertCourse)
        {

            //1`填写表单  
            db_substituteTeacherCourse.Insert(convertCourse);

        }


        /// <summary>
        /// 获取专业老师
        /// </summary>
        /// <returns></returns>
        public List<Teacher> GetTeacherByMajor(int majorId)
        {
            List<Teacher> teachers = new List<Teacher>();

            TecharOnstageBearingBusiness techarOnstageBearingBusiness = new TecharOnstageBearingBusiness();

           var templist = techarOnstageBearingBusiness.AllTeacherOnstageBearing().Where(d => d.Major == majorId).ToList();


            foreach (var item in templist)
            {
               var teacher = this.GetTeacherByID(item.TeacherID);

                if (teacher != null)
                    teachers.Add(teacher);
            }

            return teachers;
        }



        /// <summary>
        /// 获取阶段教员
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetMyGrandTeacher(Base_UserModel user)
        {
            //获取账号所有的角色

            var userRoles = user.RoleIdList;

            //当前登录人的部门下的人  (人员可能重复)
            List<Teacher> emplist = new List<Teacher>();

            //循环获取每个角色的权限

            foreach (var role in userRoles)
            {
                // 权限id 权限名称 ,可查看的部门 


                //var permissions = PermissionManage.GetRolePermissionModules(role);  //获取角色所拥有的的权限
                BaseBusiness<OtherRoleMapPermissionValue> db_permissrole = new BaseBusiness<OtherRoleMapPermissionValue>();

                var permissions = db_permissrole.GetIQueryable().Where(d => d.RoleId == role).ToList();

                foreach (var permission in permissions)
                {
                    //根据权限到 配置文件中去匹配
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(System.Web.HttpContext.Current.Server.MapPath("/Areas/Teaching/config/empmanageConfig.xml"));

                    var xmlRoot = xmlDocument.DocumentElement;

                    var permissionConfig = (XmlElement)xmlRoot.GetElementsByTagName("grandteacherPermissions")[0];

                    //获取配置文件中的权限
                    XmlNodeList permissNmaes = permissionConfig.ChildNodes;

                    foreach (XmlElement item in permissNmaes)
                    {
                        if (item.Attributes["permissionid"].Value == permission.PermissionValue)
                        {
                            //获取部门
                            var grandStr = item.Attributes["grand"].Value.Split(',');
                            List<string> deplist = grandStr.ToList();
                            if (grandStr[grandStr.Length - 1] == "")
                            {
                                deplist.RemoveAt(grandStr.Length - 1);
                            }
                            //阶段教员

                            foreach (var depItem in deplist)
                            {
                                emplist.AddRange( BrushSelectionByGrand(int.Parse(depItem)));
                            }
                        }
                    }
                }

            }

            List<EmployeesInfo> resultlist = new List<EmployeesInfo>();

            foreach (var item in emplist)
            {
                var temp = db_emp.GetInfoByEmpID(item.EmployeeId);

                if (temp != null)
                {
                    resultlist.Add(temp);
                }
            }



            return resultlist;



        }



    }
}
