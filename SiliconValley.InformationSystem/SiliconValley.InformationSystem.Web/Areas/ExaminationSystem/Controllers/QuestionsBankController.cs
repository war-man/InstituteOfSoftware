using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.ExaminationSystem.Controllers
{
   
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.ExaminationSystemBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView;
    using SiliconValley.InformationSystem.Util;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    /// <summary>
    /// 考试题库控制器
    /// </summary>
    /// 
    [CheckLogin]
    public class QuestionsBankController : Controller
    {

        private readonly ChoiceQuestionBusiness db_choiceQuestion;

        private readonly QuestionLevelBusiness db_questionLevel;

        private readonly TeacherBusiness db_teacher;

        private readonly AnswerQuestionBusiness db_answerQuestion;

        private readonly ComputerTestQuestionsBusiness db_computerTestQuestion;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QuestionsBankController()
        {

            //业务类初始化
            db_choiceQuestion = new ChoiceQuestionBusiness();
            db_questionLevel = new QuestionLevelBusiness();
            db_teacher = new TeacherBusiness();
            db_answerQuestion = new AnswerQuestionBusiness();
            db_computerTestQuestion = new ComputerTestQuestionsBusiness();

        }


        // GET: ExaminationSystem/QuestionsBank
        public ActionResult QuestionsBankIndex()
        {
         
            return View();
        }


        /// <summary>
        /// 选择题题库页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChoiceQuestionIndex()
        {
            SpecialtyBusiness specialtyBusiness = new SpecialtyBusiness();
            ViewBag.Major = specialtyBusiness.GetSpecialties();
            return View();

        }

        /// <summary>
        /// 获取选择题数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ChoiceQuestionTableData(int limit, int page)
        {

            List<MultipleChoiceQuestion> multipleChoicelist = db_choiceQuestion.AllChoiceQuestionData();

            List<MultipleChoiceQuestion> skiplist = multipleChoicelist.Skip((page - 1) * limit).Take(limit).ToList();

            //转换对象

            List<ChoiceQuestionTableView> resultlist = new List<ChoiceQuestionTableView>();

            foreach (var item in skiplist)
            {
                var tempobj = db_choiceQuestion.ConvertToChoiceQuestionTableView(item);

                resultlist.Add(tempobj);
            }

            var obj = new {

                code = 0,
                msg = "",
                count = multipleChoicelist.Count,
                data = resultlist


            };

            return Json(obj, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// 命题视图
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Proposition()
        {
            SpecialtyBusiness specialtyBusiness = new SpecialtyBusiness();

            //提供难度级别数据

            ViewBag.QuestionLevel = db_questionLevel.AllQuestionLevel();

            //提供专业数据
            ViewBag.Major = specialtyBusiness.GetSpecialties();

            return View();
        }
                        
        /// <summary>
        /// 获取课程
        /// </summary>
        /// <param name="majorid">专业</param>
        /// <returns></returns>
        public ActionResult CourseData(int majorid)
        {
            CourseBusiness courseBusiness = new CourseBusiness();

           List<Curriculum> curricullist = courseBusiness.GetCurriculaByMajor(majorid);

            return Json(curricullist,JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 添加选择题题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProposition(MultipleChoiceQuestion multipleChoice)
        {
            AjaxResult result = new AjaxResult();

            //获取当前登录的老师

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();


            multipleChoice.CreateTime = DateTime.Now;
            multipleChoice.IsUsing = true;
            multipleChoice.Proposition = teacher.TeacherID;


            try
            {
                db_choiceQuestion.Insert(multipleChoice);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = ex.Message;
                result.Data = null;
            }

            return Json(result, JsonRequestBehavior.AllowGet); ;
        }

        /// <summary>
        /// 修改选择题
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailChoiceQuestion(int id)
        {
            var ChoiceQuestion = db_choiceQuestion.AllChoiceQuestionData().Where(d => d.Id == id).FirstOrDefault();

            var obj  = db_choiceQuestion.ConvertToChoiceQuestionTableView(ChoiceQuestion);

            return View(obj);
        }


        /// <summary>
        /// 禁用或启用选择题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChoiceQuestionIsUsing(string  ids)
        {

            AjaxResult result = new AjaxResult();

            string [] ary1 = ids.Split(',');

            var list = ary1.ToList();

            list.RemoveAt(ary1.Length - 1);
            try
            {
                db_choiceQuestion.DisableOrEnable(list.ToArray());

                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }


        /// <summary>
        /// 删除选择题目
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DeleteChoiceQuestion(int id)
        {

           var result = db_choiceQuestion.RemoveChoiceQuestion(id);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查询选择题目
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult Search(string title, int course,int limit, int page)
        {
            List<MultipleChoiceQuestion> resultlist = new List<MultipleChoiceQuestion>();

            if (course == 0 && title=="")
            {
                resultlist = db_choiceQuestion.AllChoiceQuestionData().ToList();
            }

            if (course != 0 && title =="")
            {
                resultlist = db_choiceQuestion.AllChoiceQuestionData().Where(d => d.Course == course ).ToList();
            }

            if (title != "" && course != 0)
            {
                resultlist = db_choiceQuestion.AllChoiceQuestionData().Where(d => d.Course == course && d.Title.Contains(title)).ToList();
            }
            if (title != "" && course == 0)
            {
                resultlist = db_choiceQuestion.AllChoiceQuestionData().Where(d=>d.Title.Contains(title)).ToList();
            }

            


            //转换对象

            List<ChoiceQuestionTableView> returnlist = new List<ChoiceQuestionTableView>();

            foreach (var item in resultlist)
            {
                var tempobj = db_choiceQuestion.ConvertToChoiceQuestionTableView(item);

                returnlist.Add(tempobj);
            }


            var obj = new {

                code = 0,
                msg = "",
                count = returnlist.Count,
                data = returnlist.Skip((page - 1) * limit).Take(limit)
            };

            return Json(obj, JsonRequestBehavior.AllowGet);

            

        }



        //////////////////////////////////////////////////////////////////////////////解答题//////////////////////////////////////////////////////////////////



        /// <summary>
        /// 解答题题库页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearlyQuestionIndex()
        {
            SpecialtyBusiness specialtyBusiness = new SpecialtyBusiness();
            ViewBag.Major = specialtyBusiness.GetSpecialties();


            return View();
        }


        /// <summary>
        /// 解答题表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AnswerQuestionTableData(int limit, int page)
        {
            List<AnswerQuestionBank> list = db_answerQuestion.AllAnswerQuestion();

            List<AnswerQuestionBank> skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

            List<AnswerQuestionView> resultlist = new List<AnswerQuestionView>();

            foreach (var item in skiplist)
            {
               var tempobj = db_answerQuestion.ConvertToAnswerQuestionView(item);

                resultlist.Add(tempobj);
            }

            var obj = new {
                code = 0,
                msg = "",
                count = resultlist.Count,
                data=resultlist

            };

            return Json(obj, JsonRequestBehavior.AllowGet);



        }


        /// <summary>
        /// 命题-解答题页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PropositionAnswerQuestion()
        {
            SpecialtyBusiness specialtyBusiness = new SpecialtyBusiness();

            //提供难度级别数据

            ViewBag.QuestionLevel = db_questionLevel.AllQuestionLevel();

            //提供专业数据
            ViewBag.Major = specialtyBusiness.GetSpecialties();
            return View();
        }


        /// <summary>
        /// 命题-解答题
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PropositionAnswerQuestion(AnswerQuestionBank answerQuestionBank)
        {
            AjaxResult result = new AjaxResult();

            answerQuestionBank.IsUsing = true;

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();


            answerQuestionBank.Proposition = teacher.TeacherID;
            answerQuestionBank.PropositionDate = DateTime.Now;


            try
            {
                db_answerQuestion.Insert(answerQuestionBank);

                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);



        }


        /// <summary>
        /// 启用或者禁用选择题题目
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult AnswerQuestionIsUsing(string ids)
        {
            AjaxResult result = new AjaxResult();

            string[] ary1 = ids.Split(',');

            var list = ary1.ToList();

            list.RemoveAt(ary1.Length - 1);

            try
            {
                db_answerQuestion.DisableOrEnable(list);

                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);


        }


        /// <summary>
        /// 选择题题目详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailAnswerQuestion(int id)
        {


           var tempobj = db_answerQuestion.AllAnswerQuestion().Where(d => d.ID == id).FirstOrDefault();

            var obj = db_answerQuestion.ConvertToAnswerQuestionView(tempobj);


            return View(obj);


        }


        /// <summary>
        /// 删除解答题题目
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult DeleteAnswerQuestion(int id)
        {
           var result = db_answerQuestion.Remove(id);

            return Json(result);
        }



        /// <summary>
        /// 查询解答题题目
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchAnswerQuestion(string Title, int course, int page, int limit)
        {

           var list = db_answerQuestion.Search(Title, course);

            var skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

            //转换对象
            List<AnswerQuestionView> resultlist = new List<AnswerQuestionView>();

            foreach (var item in skiplist)
            {

               var tempobj = db_answerQuestion.ConvertToAnswerQuestionView(item);
                resultlist.Add(tempobj);

            }

            var obj = new {
                code=0,
                msg="",
                count=resultlist.Count,
                data=resultlist

            };

            return Json(obj,JsonRequestBehavior.AllowGet);

        }


        //////////////////////////////////////////////////////////////机试题题库////////////////////////////////////////////////////////////////////////////////////////////////



            /// <summary>
            /// 机试题题库主页
            /// </summary>
            /// <returns></returns>
        public ActionResult ComputerTestQuestionsIndex()
        {
            SpecialtyBusiness specialtyBusiness = new SpecialtyBusiness();
            ViewBag.Major = specialtyBusiness.GetSpecialties();

            return View();
        }


        /// <summary>
        /// 机试题数据表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ComputerTestQuestionTableData(int page, int limit)
        {
            List<MachTestQuesBank> list = db_computerTestQuestion.AllComputerTestQuestion();

            List<MachTestQuesBank> skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

            List<ComputerTestQuestionsView> resultlist = new List<ComputerTestQuestionsView>();

            foreach (var item in skiplist)
            {

                  var tempobj = db_computerTestQuestion.ConvertToComputerTestQuestionsView(item);

                resultlist.Add(tempobj);

            }

            var ojb = new {

                code=0,
                msg="",
                count=resultlist.Count,
                data=resultlist

            };

            return Json(ojb, JsonRequestBehavior.AllowGet);



        }


        /// <summary>
        /// 添加机试题视图
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult PropositionComputerTestQuestion()
        {
            //提供难度级别数据

            ViewBag.QuestionLevel = db_questionLevel.AllQuestionLevel();

            SpecialtyBusiness specialtyBusiness = new SpecialtyBusiness();
            ViewBag.Major = specialtyBusiness.GetSpecialties();

            return View();
        }


        /// <summary>
        /// 机试题提交
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PropositionComputerTestQuestion(MachTestQuesBank machTestQuesBank)
        {

            AjaxResult result = new AjaxResult();

            try
            {

                //判断是否重名
               var isExit = db_computerTestQuestion.AllComputerTestQuestion().Where(d => d.Title == machTestQuesBank.Title).FirstOrDefault();

                if (isExit != null)
                {
                    result.ErrorCode = 444;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var word = Request.Files[0];

                string filename = word.FileName;
                string Extension = Path.GetExtension(filename);

                string newfilename = DateTime.Now.ToLongDateString() + Extension;

                string path = Server.MapPath("~/uploadXLSXfile/ComputerTestQuestionsWord/" + newfilename);

                machTestQuesBank.CreateDate = DateTime.Now;
                machTestQuesBank.IsUsing = true;
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();

                var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

                machTestQuesBank.Proposition = teacher.TeacherID;
                machTestQuesBank.SaveURL = path;

                db_computerTestQuestion.Insert(machTestQuesBank);

                word.SaveAs(path);

                result.ErrorCode = 200;


            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
            }


           


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 机试题详细页面
        /// </summary>
        /// <param name="ids">机试题Id 以逗号分割</param>
        /// <returns></returns>
        
        public ActionResult ComputerTestQuestionDetailList(string ids)
        {
            string[] ary1 = ids.Split(',');

            var list = ary1.ToList();

            list.RemoveAt(ary1.Length - 1);

            var obj = db_computerTestQuestion.AllComputerTestQuestion().Where(d=>d.ID==int.Parse( list[0])).FirstOrDefault();

            var result = db_computerTestQuestion.ConvertToComputerTestQuestionsView(obj);

            ViewBag.ComputerTestQuestionIds = ids;


            //读取文件
            byte[] s = System.IO.File.ReadAllBytes(obj.SaveURL);

            ViewBag.ids = ids;

            var question = db_computerTestQuestion.Read_RTF_File(obj.SaveURL);

            ViewBag.Question = question;

            return View(result);


        }


        /// <summary>
        /// 启用或禁用
        /// </summary>
        /// <returns></returns>
        public ActionResult ComputerTestQuestionIsUsing(string ids)
        {
            string[] ary1 = ids.Split(',');

            var list = ary1.ToList();

            list.RemoveAt(ary1.Length - 1);
            AjaxResult result = new AjaxResult();

            try
            {
                db_computerTestQuestion.DisableOrEnable(list);
                result.ErrorCode=200;
            }
            catch (Exception ex)
            {

                result.ErrorCode=500;
            }

            return Json(result,JsonRequestBehavior.AllowGet);

        }

        public ActionResult RemoveComputerTestQuestion(int id)
        {
            var result = db_computerTestQuestion.Delete(id);

            return Json(result);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        public ActionResult Dowle(int id)
        {

           var obj = db_computerTestQuestion.AllComputerTestQuestion().Where(d => d.ID == id).FirstOrDefault();

           var filename = Path.GetFileName(obj.SaveURL);

            var path = Server.MapPath("/uploadXLSXfile/ComputerTestQuestionsWord/"+filename);

            FileStream fileStream = new FileStream(path, FileMode.Open);

            return File(fileStream, "application/octet-stream",Server.UrlEncode(filename));


        }
        [ValidateInput(false)]
        public ActionResult SearchComputerTestQuestion(string Title, int course, int page, int limit)
        {

            var list = db_computerTestQuestion.Search(Title, course);

            var skiplist = list.Skip((page - 1) * limit).Take(limit).ToList();

            //转换对象
            List<ComputerTestQuestionsView> resultlist = new List<ComputerTestQuestionsView>();

            foreach (var item in skiplist)
            {

                var tempobj = db_computerTestQuestion.ConvertToComputerTestQuestionsView(item);
                resultlist.Add(tempobj);

            }

            var obj = new
            {
                code = 0,
                msg = "",
                count = resultlist.Count,
                data = resultlist

            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

      

    }
}