using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;

namespace SiliconValley.InformationSystem.Business
{
    interface ILogger
    {
        void WriteSysLog(Base_SysLog log);
        List<Base_SysLog> GetLogList(
            string logContent,
            string logType,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime,
            Pagination pagination);
    }
}
