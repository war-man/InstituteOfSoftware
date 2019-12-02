using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 栋楼层基础控制器
    /// </summary>
    public class BasicsController : Controller
    {
        private TungFloorBusiness dbtungfloor;
        private DormitoryfloorBusiness dbfloor;
        private TungBusiness dbtung;
        private DormInformationBusiness dbdorm;
        private StaffAccdationBusiness dbstaffacc;
        private AccdationinformationBusiness dbacc;
        private RoomdeWithPageXmlHelp dbroomxml;
        // GET: Dormitory/Basics
        public ActionResult BasicsIndex()
        {
            return View();
        }

        /// <summary>
        /// 验证名字是否存在
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult verifyname(string param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            dbtung = new TungBusiness();
            try
            {
                if (dbtung.verifyname(param0))
                {
                    ajaxResult.Success = false;
                }
                else
                {
                    ajaxResult.Success = true;
                }
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "false";
            }
            
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddFloorPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFloorPage(string floorids, bool isagain, int tungid)
        {

            dbtungfloor = new TungFloorBusiness();
            dbdorm = new DormInformationBusiness();
            dbfloor = new  DormitoryfloorBusiness                                                                         ();
            AjaxResult ajaxResult = new AjaxResult();
            if (!string.IsNullOrWhiteSpace(floorids))
            {
                if (isagain)
                {
                    try
                    {
                        if (dbfloor.verifyname(floorids))
                        {
                            ajaxResult.Msg = "摸瞎几把乱搞";
                            ajaxResult.Success = false;
                        }
                        else
                        {
                            Dormitoryfloor floor = new Dormitoryfloor();
                            var now = DateTime.Now;
                            floor.CreationTime = now;
                            floor.FloorName = floorids;
                            floor.IsDelete = false;
                            floor.Remark = "被创建于" + now.Year + "年" + now.Month + "月" + now.Day + "日";
                            dbfloor.Insert(floor);
                            dbfloor = new DormitoryfloorBusiness();
                            Dormitoryfloor qyeryfloor = dbfloor.GetDormitoryfloors().Where(a => a.CreationTime.ToString() == now.ToString()).FirstOrDefault();

                            TungFloor tungFloor = new TungFloor();
                            tungFloor.CreationTime = DateTime.Now;
                            tungFloor.FloorId = qyeryfloor.ID;
                            tungFloor.IsDel = false;
                            tungFloor.Remark = "ID为" + tungid + "的栋,于" + tungFloor.CreationTime.ToString() + "时创建";
                            tungFloor.TungId = tungid;
                            dbtungfloor.Insert(tungFloor);

                            ajaxResult.Success = true;
                        }
                       

                    }
                    catch (Exception ex)
                    {

                        ajaxResult.Success = false;
                        ajaxResult.Msg = "请联系信息部。";
                    }
                }
                else
                {
                    string[] sArray = Regex.Split(floorids, ",", RegexOptions.IgnoreCase);
                    List<TungFloor> querylist = dbtungfloor.GetTungFloorByTungID(tungid);
                    List<string> newlist = sArray.ToList();
                    try
                    {
                        for (int i = newlist.Count - 1; i >= 0; i--)
                        {
                            foreach (var item in querylist)
                            {
                                if (int.Parse(newlist[i]) == item.FloorId)
                                {
                                    newlist.Remove(newlist[i]);
                                    break;
                                }
                            }
                        }
                        if (newlist.Count != 0)
                        {
                            foreach (var item in newlist)
                            {
                                int newfloorid = int.Parse(item);
                                var vv = dbtungfloor.GetIQueryable().Where(a => a.TungId == tungid && a.FloorId == newfloorid).FirstOrDefault();
                                if (vv != null)
                                {
                                    vv.IsDel = false;
                                    dbtungfloor.Update(vv);
                                    var ar = dbdorm.GetIQueryable().Where(a => a.TungFloorId == vv.Id).ToList();
                                    foreach (var item1 in ar)
                                    {
                                        item1.IsDelete = false;
                                        item1.ProhibitRemark = string.Empty;
                                        dbdorm.Update(item1);
                                    }
                                }
                                else
                                {
                                    TungFloor tungFloor = new TungFloor();
                                    tungFloor.CreationTime = DateTime.Now;
                                    tungFloor.FloorId = newfloorid;
                                    tungFloor.IsDel = false;
                                    tungFloor.Remark = "ID为" + tungid + "的栋,于" + tungFloor.CreationTime.ToString() + "时创建";
                                    tungFloor.TungId = tungid;
                                    dbtungfloor.Insert(tungFloor);
                                }

                            }
                            ajaxResult.Success = true;
                        }
                        else
                        {
                            ajaxResult.Success = false;
                            ajaxResult.Msg = "请勿重复点击！";
                        }


                    }
                    catch (Exception ex)
                    {
                        ajaxResult.Success = false;
                        ajaxResult.Msg = "请联系信息部。";
                    }
                }
            }
            else
            {
                ajaxResult.Msg = "摸瞎几把乱搞";
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="TungName"></param>
        /// <param name="tungid"></param>
        /// <returns></returns>
        public ActionResult updatetung(string TungName, int tungid)
        {
            dbtung = new TungBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                Tung querytung = dbtung.GetTungByTungID(tungid);
                querytung.TungName = TungName;
                dbtung.Update(querytung);
                ajaxResult.Success = true;

            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部。";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult del(int tungid, int floorid, bool istung)
        {
            dbtung = new TungBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbacc = new AccdationinformationBusiness();
            dbstaffacc = new StaffAccdationBusiness();
            dbdorm = new DormInformationBusiness();
            AjaxResult ajaxResult = new AjaxResult();

            //删除的前提是这个楼层或者是这个栋没有人居住才可以进行的一个删除

            try
            {
                //是栋
                if (istung)
                {
                    //栋楼层类
                    List<TungFloor> tungFloors = dbtungfloor.GetTungFloorByTungID(tungid);

                    if (tungFloors.Count != 0)
                    {
                        AjaxResult a = new AjaxResult();
                        
                        foreach (var item in tungFloors)
                        {
                            ajaxResult = Zhixing(item.Id, tungid, istung);
                            if (!ajaxResult.Success)
                            {
                                break;
                            }
                            else
                            {
                                if (item.Id== tungFloors.LastOrDefault().Id)
                                {
                                    Tung protung = dbtung.GetTungByTungID(tungid);
                                    protung.IsDel = true;
                                    dbtung.Update(protung);
                                    ajaxResult.Success = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        Tung protung = dbtung.GetTungByTungID(tungid);
                        protung.IsDel = true;
                        dbtung.Update(protung);
                        ajaxResult.Success = true;
                    }

                }
                else
                {
                    TungFloor tungFloor = dbtungfloor.GetTungFloorByTungIDAndFloorID(tungid, floorid);
                    ajaxResult = Zhixing(tungFloor.Id, tungid, istung);
                }
            }
            catch (Exception ex)
            {

                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部。";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 处理房间是否存在居住信息，如果不存在进行一个删除处理
        /// </summary>
        /// <param name="tungfloorid"></param>
        /// <param name="tungid"></param>
        /// <returns></returns>
        public AjaxResult Zhixing(int tungfloorid, int tungid, bool istung)
        {
            dbdorm = new DormInformationBusiness();
            dbstaffacc = new StaffAccdationBusiness();
            dbacc = new AccdationinformationBusiness();
            dbtung = new TungBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbroomxml = new RoomdeWithPageXmlHelp();
            AjaxResult ajaxResult = new AjaxResult();
            dbfloor = new DormitoryfloorBusiness();
            TungFloor proquerytungfloor = dbtungfloor.GetTungFloorByID(tungfloorid);

            try
            {
                int staffroomid = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StaffRoom);
                int studentroomid = dbroomxml.GetRoomType(RoomTypeEnum.RoomType.StudentRoom);
                List<DormInformation> querydorm = dbdorm.GetDormsByTungFloorID(tungfloorid);

                //排除存在居住信息的房间
                for (int i = querydorm.Count - 1; i >= 0; i--)
                {
                    List<StaffAccdation> querystaffacc = dbstaffacc.GetStaffAccdationsByDorminfoID(querydorm[i].ID);
                    List<Accdationinformation> queryacc = dbacc.GetAccdationinformationByDormId(querydorm[i].ID);
                    if (querystaffacc.Count == 0 && queryacc.Count == 0)
                    {
                        querydorm.Remove(querydorm[i]);
                    }
                }

                if (querydorm.Count == 0)
                {
                    TungFloor tungFloor = dbtungfloor.GetTungFloorByID(tungfloorid);
                    var aa=dbdorm.GetDormsByTungFloorID(tungfloorid);
                    dbdorm = new DormInformationBusiness();
                    foreach (var item in aa)
                    {
                        item.IsDelete = true;
                        item.ProhibitRemark =dbfloor.GetEntity(tungFloor.FloorId).FloorName+"被禁用！";
                        dbdorm.Update(item);
                    }
                    tungFloor.IsDel = true;
                    dbtungfloor.Update(tungFloor);
                    ajaxResult.Success = true;

                }
                else
                {
                    ajaxResult.Success = false;
                    Dormitoryfloor proqueryfloor = dbfloor.GetDormitoryfloorByFloorID(proquerytungfloor.FloorId);
                    ajaxResult.Msg = proqueryfloor.FloorName + "</br>";

                    List<DormInformation> staffroomlist = querydorm.Where(a => a.RoomStayTypeId == staffroomid).ToList();

                    List<DormInformation> studentroomlist = querydorm.Where(a => a.RoomStayTypeId == studentroomid).ToList();

                    if (staffroomlist.Count != 0)
                    {
                        string resultroomname = "";
                        foreach (var item2 in staffroomlist)
                        {
                            DormInformation dormInformation = dbdorm.GetDormByDorminfoID(item2.ID);
                            resultroomname = resultroomname + dormInformation.DormInfoName + "寝室  ";
                        }
                        ajaxResult.Msg = ajaxResult.Msg + "员工寝室:" + resultroomname + "</br>";

                    }

                    if (studentroomlist.Count != 0)
                    {
                        string resultroomname = "";
                        foreach (var item in studentroomlist)
                        {
                            DormInformation dormInformation = dbdorm.GetDormByDorminfoID(item.ID);
                            resultroomname = resultroomname + dormInformation.DormInfoName + "寝室  ";
                        }
                        ajaxResult.Msg = ajaxResult.Msg + "学生寝室:" + resultroomname + "</br>";

                    }



                }
            }
            catch (Exception ex)
            {

                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部。";
            }


            return ajaxResult;
        }


        [HttpGet]
        public ActionResult AddTungPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTungPage(string TungName, string TungAddress,string Remark)
        {

            AjaxResult result = new AjaxResult();

            dbtung = new TungBusiness();

            try
            {
                if (!string.IsNullOrWhiteSpace(TungName))
                {
                    if (dbtung.verifyname(TungName))
                    {
                        result.Msg = "摸瞎几把乱搞";
                        result.Success = false;
                    }
                    else
                    {
                        Tung tung = new Tung();
                        tung.CreationTime = DateTime.Now;
                        tung.IsDel = false;
                        tung.Remark = Remark;
                        tung.TungName = TungName;
                        tung.TungAddress = TungAddress;

                        dbtung.Insert(tung);
                        BusHelper.WriteSysLog("添加数据位置于Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据);
                        result.Msg = "添加成功";
                        result.Success = true;
                    }
                }
                else
                {
                    result.Msg = "摸瞎几把乱搞";
                    result.Success = false;
                }


            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message + "Dormitory/DormitoryInfo/AddTungPage", Entity.Base_SysManage.EnumType.LogType.添加数据error);
                result.Msg = "添加失败";
                result.Success = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        /// <summary>
        ///表单数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SeachData(int page, int limit) {
            dbtung = new TungBusiness();
            dbdorm = new DormInformationBusiness();
            dbtungfloor = new TungFloorBusiness();
            dbacc = new AccdationinformationBusiness();
            List<TungView> list = new List<TungView>();
            dbstaffacc = new StaffAccdationBusiness();
            var t= dbtung.GetTungs();
            
            foreach (var item in t)
            {
                TungView tungView = new TungView();
                tungView.CreationTime = item.CreationTime;
                tungView.Remark = item.Remark;
                tungView.TungAddress = item.TungAddress;
                tungView.TungName = item.TungName;
                tungView.studentcount = 0;
                tungView.staffcount = 0;
                var dd = dbtungfloor.GetTungFloorByTungID(item.Id);
                foreach (var item1 in dd)
                {

                    tungView.studentcount = tungView.studentcount + dbacc.GetAccdationinformationsByTungFloorID(item1.Id).Count;
                    tungView.staffcount = tungView.staffcount + dbstaffacc.GHetstaffaccByTungfloorid(item1.Id).Count;
                }
                list.Add(tungView);
            }
            var resultdata = list.OrderByDescending(a => a.CreationTime).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = resultdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
    }
}