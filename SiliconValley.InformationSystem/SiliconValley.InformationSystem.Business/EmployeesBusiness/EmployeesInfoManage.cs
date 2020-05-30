using System;
using System.Collections.Generic;
using System.Linq;

namespace SiliconValley.InformationSystem.Business.EmployeesBusiness
{
    using SiliconValley.InformationSystem.Business.Channel;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.PositionBusiness;
    using SiliconValley.InformationSystem.Business.DepartmentBusiness;
    using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.Employment;
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    using SiliconValley.InformationSystem.Business.Consult_Business;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Business.FinanceBusiness;
    using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
    using SiliconValley.InformationSystem.Business.DormitoryBusiness;
    using NPOI.SS.UserModel;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.XSSF.UserModel;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.EmpTransactionBusiness;

    /// <summary>
    /// 员工业务类
    /// </summary>
    public class EmployeesInfoManage : BaseBusiness<EmployeesInfo>
    {
        RedisCache rc;
        /// <summary>
        /// 将员工信息表数据存储到redis服务器中
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetEmpInfoData() {
            rc = new RedisCache();
            rc.RemoveCache("InRedisEmpInfoData");
            List<EmployeesInfo> emplist = new List<EmployeesInfo>();
            if (emplist == null || emplist.Count() == 0) {
                emplist = this.GetIQueryable().ToList();
                rc.SetCache("InRedisEmpInfoData", emplist);
            }
            emplist = rc.GetCache<List<EmployeesInfo>>("InRedisEmpInfoData");
            return emplist;
        }

        /// <summary>
        ///  获取所属岗位对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Position GetPosition(int pid)
        {
            PositionManage pmanage = new PositionManage();
            var str = pmanage.GetEntity(pid);
            return str;
        }


        /// <summary>
        /// 根据员工编号获取所属岗位对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public Position GetPositionByEmpid(string empid) {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            PositionManage pmanage = new PositionManage();
            var pstr = pmanage.GetEntity(emanage.GetEntity(empid).PositionId);
            return pstr;
        }

        /// <summary>
        /// 根据员工编号获取所属部门对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public Department GetDeptByEmpid(string empid)
        {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            DepartmentManage dmanage = new DepartmentManage();
            var dstr = dmanage.GetEntity(GetPositionByEmpid(empid).DeptId);
            return dstr;
        }

        /// <summary>
        /// 获取所属岗位的所属部门对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Department GetDept(int pid)
        {
            DepartmentManage deptmanage = new DepartmentManage();
            var str = deptmanage.GetEntity(GetPosition(pid).DeptId);
            return str;
        }
        /// <summary>
        /// 根据部门编号获取部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Department GetDeptById(int id) {
            DepartmentManage deptmanage = new DepartmentManage();
            var dept = deptmanage.GetEntity(id);
            return dept;
        }
        /// <summary>
        /// 根据岗位编号获取岗位对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Position GetPobjById(int id) {
            PositionManage pmanage = new PositionManage();
            return pmanage.GetEntity(id);
        }

        //根据岗位编号获取该岗位的员工
        public List<EmployeesInfo> GetEmpByPid(int pid) {
            List<EmployeesInfo> emplist = new List<EmployeesInfo>();
            foreach (var item in this.GetEmpInfoData())
            {
                if (item.PositionId == pid) {
                    emplist.Add(item);
                }
            }
            return emplist;
        }


        /// <summary>
        /// 通过岗位编号获取该岗位所属部门
        /// </summary>
        /// <returns></returns>
        public Department GetDeptByPid(int pid)
        {
            PositionManage pmanage = new PositionManage();
            DepartmentManage dmanage = new DepartmentManage();
            var deptid = pmanage.GetEntity(pid);
            return dmanage.GetEntity(deptid.DeptId);
        }

        /// <summary>
        /// 根据类型编号获取员工异动类型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MoveType GetETById(int id) {
            MoveTypeManage mtmanage = new MoveTypeManage();
            return mtmanage.GetEntity(id);
        }

        /// <summary>
        /// 根据部门编号获取该部门下的所有员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetEmpsByDeptid(int deptid)
        {
            return this.GetAll().Where(a => this.GetDeptByPid(a.PositionId).DeptId == deptid).ToList();
        }


        /// <summary>
        /// 渠道
        /// </summary>
        private ChannelStaffBusiness dbchannel;
        /// <summary>
        /// 获取所有没有离职的员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetAll() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据员工编号获取员工对象
        /// </summary>
        /// <param name="empinfoid">员工编号</param>
        /// <returns></returns>
        public EmployeesInfo GetInfoByEmpID(string empinfoid) {
            return this.GetEntity(empinfoid);
        }

        /// <summary>
        /// 添加员工借资
        /// </summary>
        /// <param name="debit"></param>
        /// <returns></returns>
        public bool Borrowmoney(Debit debit) {
            BaseBusiness<Debit> dbdebit = new BaseBusiness<Debit>();
            bool result = false;
            try
            {
                dbdebit.Insert(debit);
                result = true;
            }
            catch (Exception)
            {

            }
            return result;
        }


        /// <summary>
        /// 查询是市场主任员工集合
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetChannelStaffZhuren() {
            return this.GetAll().Where(a => this.GetPositionByEmpid(a.EmployeeId).PositionName == "市场主任").ToList();

        }

        /// <summary>
        /// 获取市场副主任
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetChannelStaffFuzhuren() {
            return this.GetAll().Where(a => this.GetPositionByEmpid(a.EmployeeId).PositionName == "市场副主任").ToList();
        }

        /// <summary>
        /// 根据渠道员工id获取员工对象
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public EmployeesInfo GetInfoByChannelID(int ChannelID) {
            dbchannel = new ChannelStaffBusiness();
            var channel = dbchannel.GetChannelByID(ChannelID);
            return this.GetInfoByEmpID(channel.EmployeesInfomation_Id);
        }
        /// <summary>
        /// 杨校(常务副校长岗位)
        /// </summary>
        /// <returns></returns>
        public EmployeesInfo GetYangxiao() {
            return this.GetAll().Where(a => this.GetPositionByEmpid(a.EmployeeId).PositionName == "常务副校长").FirstOrDefault();
        }
        /// <summary>
        /// 判断是否是渠道主任
        /// </summary>
        /// <param name="empinfoid">员工编号</param>
        /// <returns></returns>
        public bool IsChannelZhuren(string empinfoid) {
            bool iszhuren = false;
            var data = this.GetChannelStaffZhuren();
            foreach (var item in data)
            {
                if (item.EmployeeId == empinfoid)
                {
                    iszhuren = true;
                }
            }
            return iszhuren;
        }
        /// <summary>
        /// 判断是不是主任
        /// </summary>
        /// <param name="empinfo"></param>
        /// <returns></returns>
        public bool IsChannelZhuren(EmployeesInfo empinfo) {
            bool iszhuren = false;
            var query = this.GetChannelStaffZhuren().Where(a => a.EmployeeId == empinfo.EmployeeId).FirstOrDefault();
            if (query != null)
            {
                iszhuren = true;
            }
            return iszhuren;
        }

        /// <summary>
        /// 判断是不是副主任
        /// </summary>
        /// <param name="empinfo"></param>
        /// <returns></returns>
        public bool IsFuzhiren(EmployeesInfo empinfo)
        {
            bool isfuzhuren = false;
            var query = this.GetChannelStaffFuzhuren().Where(a => a.EmployeeId == empinfo.EmployeeId).FirstOrDefault();
            if (query != null)
            {
                isfuzhuren = true;
            }
            return isfuzhuren;
        }

        #region tangmin--Write
        /// <summary>
        /// 根据员工Id或者员工名称查询名称
        /// </summary>
        /// <param name="name">员工编号或员工名称</param>
        /// <param name="key">true---按编号查，false---按名称查</param>
        /// <returns></returns>
        public EmployeesInfo FindEmpData(string name, bool key)
        {
            EmployeesInfo employees = new EmployeesInfo();
            if (key)
            {
                employees = this.GetEntity(name);
            }
            else
            {
                employees = this.GetEmpInfoData().Where(e => e.EmpName == name).FirstOrDefault();
            }
            return employees;
        }
        #endregion

        /// <summary>
        /// 将员工加入对相应的部门
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public bool AddEmpToCorrespondingDept(EmployeesInfo emp) {
            bool result = true;
            var dname = this.GetDept(emp.PositionId).DeptName;//获取该员工所属部门名称
            var pname = this.GetPosition(emp.PositionId).PositionName;//获取该员工所属岗位名称
            if (dname.Equals("就业部"))
            {
                EmploymentStaffBusiness esmanage = new EmploymentStaffBusiness();
                result = esmanage.AddEmploystaff(emp.EmployeeId);//给就业部员工表添加员工
            }
            if (dname.Equals("市场部"))
            {
                ChannelStaffBusiness csmanage = new ChannelStaffBusiness();
                result = csmanage.AddChannelStaff(emp.EmployeeId);
            }//给市场部员工表添加员工
            if ((dname.Equals("s1、s2教质部") || dname.Equals("s3教质部")) && !pname.Equals("教官"))
            {
                HeadmasterBusiness hm = new HeadmasterBusiness();
                result = hm.AddHeadmaster(emp.EmployeeId);
            }//给两个教质部员工表添加除教官外的员工
            if ((dname.Equals("s1、s2教质部") || dname.Equals("s3教质部")) && pname.Equals("教官"))
            {
                InstructorListBusiness itmanage = new InstructorListBusiness();
                result = itmanage.AddInstructorList(emp.EmployeeId);
            }//给教官员工表添加教官
            if (pname.Equals("咨询师") || pname.Equals("咨询主任"))
            {
                ConsultTeacherManeger cmanage = new ConsultTeacherManeger();
                result = cmanage.AddConsultTeacherData(emp.EmployeeId);
            }//给咨询部员工表添加除咨询助理外的员工
            if (dname.Equals("s1、s2教学部") || dname.Equals("s3教学部") || dname.Equals("s4教学部"))//给三个教学部员工表添加员工
            {
                TeacherBusiness teamanage = new TeacherBusiness();
                Teacher tea = new Teacher();
                tea.EmployeeId = emp.EmployeeId;
                result = teamanage.AddTeacher(tea);
            }
            if (dname.Equals("财务部"))
            {
                FinanceModelBusiness fmmanage = new FinanceModelBusiness();
                result = fmmanage.AddFinancialstaff(emp.EmployeeId);
            }//给财务部员工表添加员工
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            result = esemanage.AddEmpToEmpSalary(emp.EmployeeId);//往员工工资体系表添加员工
            return result;
        }


        /// <summary>
        /// 将导过来的excel数据赋给考勤视图类中
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public List<EmployeeInfoView> CreateExcelData(ISheet sheet)
        {
            List<EmployeeInfoView> result = new List<EmployeeInfoView>();
            int num = 0;
            AjaxResult ajaxresult = new AjaxResult();
            try
            {
                while (true)
                {
                    EmployeeInfoView empview = new EmployeeInfoView();
                    num++;
                    //循环获取num行的数据
                    var getrow = sheet.GetRow(num);
                    if (string.IsNullOrEmpty( Convert.ToString(getrow)))
                    {
                        break;
                    }
                    #region 获取excel中的每一列数据
                    //获取第num行"姓名"列的数据
                    string name = getrow.GetCell(1).ToString();
                    //获取第num行"部门"列的数据
                    string dept = getrow.GetCell(2).ToString();
                    //获取第num行"岗位"列的数据
                    string position = getrow.GetCell(3).ToString();
                    //获取第num行"工号"列的数据
                    string ddid = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(4))) ? null : getrow.GetCell(4).ToString();
                    //获取第num行"招聘来源"列的数据
                    string original = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(5))) ? null : getrow.GetCell(5).ToString();
                    //获取第num行"身份证号码"列的数据
                    // var idnum = System.Text.RegularExpressions.Regex.Replace(getrow.GetCell(6).StringCellValue, @"[^0-9]+", "");
                    string idcardnum = getrow.GetCell(6).ToString();
                    //获取第num行"电话号码"列的数据
                    string phonenum = getrow.GetCell(7).ToString();
                    //获取第num行"性别"列的数据
                    string empsex = getrow.GetCell(8).ToString();
                    //获取第num行"年龄"列的数据
                    string empage = getrow.GetCell(9).ToString();
                    //获取第num行"民族"列的数据
                    string nation = getrow.GetCell(10).ToString();
                    //获取第num行"入职时间"列的数据
                    string entertime = getrow.GetCell(11).ToString();
                    //获取第num行"转正时间"列的数据
                    string positivetime = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(12))) ? null : getrow.GetCell(12).ToString();
                  //  string positivetime = getrow.GetCell(12)?.StringCellValue ?? null;
                    //获取第num行"试用期工资"列的数据
                    string probationsalary = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(13))) ? null : getrow.GetCell(13).ToString();
                    //获取第num行"转正后工资"列的数据
                    string salary = getrow.GetCell(14).ToString();
                    //获取第num行"学历"列的数据
                    string education = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(15))) ? "大专" : getrow.GetCell(15).ToString();
                    //获取第num行"合同起始日期"列的数据
                    string contractStartTime = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(17))) ? null : getrow.GetCell(17).ToString();
                    //获取第num行"合同终止日期"列的数据
                    string contractEndTime = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(18))) ? null : getrow.GetCell(18).ToString();
                    //获取第num行"生日"列的数据
                    string birthday = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(19))) ? null : getrow.GetCell(19).ToString();
                    //获取第num行"紧急联系电话"列的数据
                    string urgentphone = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(20))) ? null : getrow.GetCell(20).ToString();
                    //获取第num行"户籍地址"列的数据
                    string domicileAddress = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(21))) ? null : getrow.GetCell(21).ToString();
                    //获取第num行"现地址"列的数据
                    string address = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(22))) ? null : getrow.GetCell(22).ToString();
                    //获取第num行"婚姻状况"列的数据
                    string maritalStatus = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(23))) ? "未婚" : getrow.GetCell(23).ToString();
                    //获取第num行"身份证有效期"列的数据
                    string idcardIndate = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(24))) ? null : getrow.GetCell(24).ToString();
                    //获取第num行"政治面貌"列的数据
                    string politicsStatus = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(25))) ? "党员" : getrow.GetCell(25).ToString();
                    //获取第num行"社保起始月份"列的数据
                    string SSstartTime= string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(26))) ?null:getrow.GetCell(26).ToString(); 
                    //获取第num行"银行卡号"列的数据
                    string bankCardnum = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(27))) ? null : getrow.GetCell(27).ToString();
                    //获取第num行"纸质材料"列的数据
                    string paperyMaterial = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(28))) ? null : getrow.GetCell(28).ToString();
                    // ICell leaveddays_cell = sheet.GetRow(num).GetCell(3);
                    //获取第num行"备注"列的数据
                    string remark = string.IsNullOrEmpty(Convert.ToString(getrow.GetCell(29))) ? null : getrow.GetCell(29).ToString();
                    #endregion
                    #region 将excel中拿过来的数据赋给员工视图对象
                    empview.name = name;
                    empview.dept = dept;
                    empview.position = position;
                    empview.ddid =int.Parse(ddid);
                    empview.original = original;
                    empview.idcardnum = idcardnum;
                    empview.phonenum = phonenum; 
                    empview.empsex = empsex;
                    empview.nation = nation;
                    empview.entertime = Convert.ToDateTime(entertime);
                    if (!string.IsNullOrEmpty(positivetime)) {
                        empview.positivetime=Convert.ToDateTime(positivetime);
                    }
                    empview.probationsalary =Convert.ToDecimal(probationsalary);
                    empview.salary =Convert.ToDecimal(salary);
                    empview.education = education;
                    empview.contractStartTime = Convert.ToDateTime(contractStartTime);
                    empview.contractEndTime = Convert.ToDateTime(contractEndTime);
                    empview.birthday = birthday;
                    empview.urgentphone = urgentphone;
                    empview.domicileAddress = domicileAddress;
                    empview.address = address;
                    empview.maritalStatus = maritalStatus=="已婚"?true:false;
                    empview.idcardIndate = Convert.ToDateTime(idcardIndate);
                    empview.politicsStatus = politicsStatus;
                    if (!string.IsNullOrEmpty(SSstartTime) && !SSstartTime.Equals("/")) {
                        // DateTime.ParseExact(str, "yyyyMMdd", null);
                        DateTime stime=DateTime.ParseExact(SSstartTime,"yyyyMM",null);
                        empview.SSstartTime = stime;
                    }
                    empview.bankCardnum = bankCardnum;
                    empview.paperyMaterial = paperyMaterial;
                    empview.Remark = remark;
                    #endregion
                    result.Add(empview);
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;

        }

        /// <summary>
        /// 将excel数据类的数据存入到数据库的考勤表中
        /// </summary>
        /// <returns></returns>
        public AjaxResult ExcelImportEmpSql(ISheet sheet)
        {
            var ajaxresult = new AjaxResult();
            List<EmpErrorDataView> emperrorlist = new List<EmpErrorDataView>();
            try
            {
                var mateviewlist = CreateExcelData(sheet);
                foreach (var item in mateviewlist)
                {
                    EmployeesInfo emp = new EmployeesInfo();
                    EmpErrorDataView emperror = new EmpErrorDataView();
                    var deptobj = GetDeptByDname(item.dept);
                    if (DDidIsExist((int)item.ddid) == true)
                    {
                        emperror.ddid = item.ddid;
                        emperror.ename = item.name;
                        emperror.errorExplain = "原因是该钉钉号已存在！";
                        emperrorlist.Add(emperror);

                    }
                    else {
                        if (deptobj == null)
                        {
                            emperror.ddid = item.ddid;
                            emperror.ename = item.name;
                            emperror.errorExplain = "原因是不存在"+item.dept+"该部门!";
                            emperrorlist.Add(emperror);
                        }
                        else
                        {
                            if (deptobj != null && GetPositionByDeptidPname(deptobj.DeptId, item.position) == null)
                            {
                                emperror.errorExplain = "原因是" + item.dept + "中不存在"+item.position+"该岗位!";
                                emperror.ddid = item.ddid;
                                emperror.ename = item.name;
                                emperrorlist.Add(emperror);
                            }
                            else
                            {
                                emp.EmployeeId = EmpId();
                                emp.DDAppId = item.ddid;
                                emp.EmpName = item.name;
                                emp.PositionId = GetPositionByDeptidPname(deptobj.DeptId, item.position).Pid;
                                emp.Sex = item.empsex;
                                emp.IdCardIndate = item.idcardIndate;

                                emp.IdCardNum = item.idcardnum;
                                if (emp.IdCardNum != null)
                                {
                                    emp.Birthdate = DateTime.Parse(GetBirth(emp.IdCardNum));
                                }
                                if (emp.Birthdate != null)
                                {
                                    emp.Age = Convert.ToInt32(this.GetAge((DateTime)emp.Birthdate, DateTime.Now));
                                }
                                emp.Nation = item.nation;
                                emp.Phone = item.phonenum;
                                emp.ContractStartTime = item.contractStartTime;
                                emp.ContractEndTime = item.contractEndTime;
                                emp.EntryTime = item.entertime;
                                emp.Birthday = item.birthday;
                                emp.PositiveDate = item.positivetime;
                                emp.UrgentPhone = item.urgentphone;
                                emp.DomicileAddress = item.domicileAddress;
                                emp.Address = item.address;
                                emp.Education = item.education;
                                emp.MaritalStatus = item.maritalStatus;
                                emp.IdCardIndate = item.idcardIndate;
                                emp.PoliticsStatus = item.politicsStatus;
                                emp.ProbationSalary = item.probationsalary;
                                emp.Salary = item.salary;
                                emp.SSStartMonth = item.SSstartTime;
                                emp.BCNum = item.bankCardnum;
                                emp.Material = item.paperyMaterial;
                                emp.Remark = item.Remark;
                              
                                this.Insert(emp);
                                rc.RemoveCache("InRedisEmpInfoData");
                                AddEmpToCorrespondingDept(emp);

                            }
                        }
                      
                    }           
                }
                if (mateviewlist.Count() - emperrorlist.Count() == mateviewlist.Count())
                {//说明没有出错数据，导入的数据全部添加成功
                    ajaxresult.Success = true;
                    ajaxresult.ErrorCode = 100;
                    ajaxresult.Msg = mateviewlist.Count().ToString();
                    ajaxresult.Data = emperrorlist;
                }
                else
                {//说明有出错数据，导入的数据条数就是导入的数据总数-错误数据总数
                    ajaxresult.Success = true;
                    ajaxresult.ErrorCode = 200;
                    ajaxresult.Msg = (mateviewlist.Count() - emperrorlist.Count()).ToString();
                    ajaxresult.Data = emperrorlist;
                }
            }
            catch (Exception ex)
            {
                ajaxresult.Success = false;
                ajaxresult.ErrorCode = 500;
                ajaxresult.Msg = ex.Message;
                ajaxresult.Data = 0;
            }
            return ajaxresult;
        }


        public AjaxResult ImportDataFormExcel(Stream stream, string contentType)
        {
            var ajaxresult = new AjaxResult();
            IWorkbook workbook = null;

            if (contentType == "application/vnd.ms-excel")
            {
                workbook = new HSSFWorkbook(stream);
            }

            if (contentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                workbook = new XSSFWorkbook(stream);
            }

            ISheet sheet = workbook.GetSheetAt(0);
            var result = ExcelImportEmpSql(sheet);
            stream.Close();
            stream.Dispose();
            workbook.Close();

            return result;
        }

        /// <summary>
        /// 根据部门的名称获取部门对象
        /// </summary>
        /// <param name="dname"></param>
        /// <returns></returns>
        public Department GetDeptByDname(string dname)
        {
            DepartmentManage dmanage = new DepartmentManage();
            var dept = dmanage.GetDepartmentByName(dname);
            return dept;
        }
        /// <summary>
        /// 获取指定部门某岗位对象
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="pname"></param>
        /// <returns></returns>
        public Position GetPositionByDeptidPname(int deptid,string pname) {
            PositionManage pmanage = new PositionManage();
            var plist = pmanage.GetPositionByDepeID(deptid);
            var position = plist.Where(s => s.PositionName == pname).FirstOrDefault();
            return position;


        }

        /// <summary>
        ///计算员工年龄
        /// </summary>
        /// <param name="dtBirthday"></param>
        /// <param name="dtNow"></param>
        /// <returns></returns>
        public  string GetAge(DateTime dtBirthday, DateTime dtNow)
        {
            string strAge = string.Empty; // 年龄的字符串表示
            int intYear = 0; // 岁
            int intMonth = 0; // 月
            int intDay = 0; // 天

            // 如果没有设定出生日期, 返回空
            if (dtBirthday == null)
            {
                return string.Empty;
            }

            // 计算天数
            intDay = dtNow.Day - dtBirthday.Day;
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);
                intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            }

            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;

            // 格式化年龄输出
            if (intYear >= 1) // 年份输出
            {
                strAge = intYear.ToString();
            }

            //if (intMonth > 0 && intYear <= 5) // 五岁以下可以输出月数
            //{
            //    strAge += intMonth.ToString() + "月";
            //}

            //if (intDay >= 0 && intYear < 1) // 一岁以下可以输出天数
            //{
            //    if (strAge.Length == 0 || intDay > 0)
            //    {
            //        strAge += intDay.ToString() + "日";
            //    }
            //}

            return strAge;
        }
        /// <summary>
        ///生成员工编号
        /// </summary>
        /// <returns></returns>
        public string EmpId()
        {
            string mingci = string.Empty;
            // DateTime date = Convert.ToDateTime(Date());
            DateTime date = DateTime.Now;
            string n = date.Year.ToString();//获取年份
            string y = MonthAndDay(Convert.ToInt32(date.Month)).ToString();//获取月份
            string d = MonthAndDay(Convert.ToInt32(date.Day)).ToString();//获取日期

            EmployeesInfoManage empinfo = new EmployeesInfoManage();
            var lastobj = empinfo.GetEmpInfoData().LastOrDefault();
            if (lastobj == null)
            {
                mingci = "0001";
            }
            else
            {
                string laststr = lastobj.EmployeeId;
                string startfournum = laststr.Substring(0, 4);
                string endfournum = laststr.Substring(laststr.Length - 4, 4);
                if (int.Parse(n) > int.Parse(startfournum))
                {
                    mingci = "0001";
                }
                else
                {
                    string newstr = (int.Parse(endfournum) + 1).ToString();
                    if (int.Parse(newstr) < 10)
                    {
                        mingci = "000" + newstr;
                    }
                    else if (int.Parse(newstr) >= 10 && int.Parse(newstr) < 100)
                    {
                        mingci = "00" + newstr;
                    }
                    else if (int.Parse(newstr) >= 100 && int.Parse(newstr) < 1000)
                    {
                        mingci = "0" + newstr;
                    }
                    else
                    {
                        mingci = newstr;
                    }
                }
            }
            string EmpidResult = n + y + d + mingci;
            return EmpidResult;
        }
        //月份及日期前面加个零
        public string MonthAndDay(int a)
        {
            if (a < 10)
            {
                return "0" + a;
            }
            string c = a.ToString();
            return c;
        }
        /// <summary>
        /// 根据身份证号码获取出生日期
        /// </summary>
        /// <param name="idnum"></param>
        /// <returns></returns>
        public  string GetBirth(string idnum)
        {
            string year = idnum.Substring(6, 4);
            string month = idnum.Substring(10, 2);
            string date = idnum.Substring(12, 2);
            string result = year + "-" + month + "-" + date;
            return result;
        }
        /// <summary>
        /// 判断某钉钉号是否已存在
        /// </summary>
        /// <param name="ddid"></param>
        /// <returns></returns>
        public bool DDidIsExist(int ddid) {
            bool result = false;
           
                var emp = this.GetEmpInfoData().Where(s => s.DDAppId == ddid).FirstOrDefault();
            if (emp != null)
            {
                result = true;
            }
            else {
                result = false;
            }
            return result;
           
        }
    }
}
