using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    /// <summary>
    /// 获取设备信息辅助类
    /// </summary>
   public class DeviceInformationHelp
    {
        /// <summary>
        /// 获取系统信息的公共方法
        /// </summary>
        /// <param name="objquery">查询语句</param>
        /// <param name="infoname">系统信息字段</param>
        /// <returns></returns>
        public static string getSystemInfoObject(ObjectQuery objquery, string infoname)
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(objquery);
                var systeminfo = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    systeminfo = mo[infoname].ToString();
                }
                return systeminfo;
            }
            catch
            {
                return "";
            }
        }

        // PC序列号
        public static string getPCNumber()
        {
            ObjectQuery objquery = new ObjectQuery("Select * From Win32_BIOS");
            string infoname = "SerialNumber";
            return getSystemInfoObject(objquery, infoname);
        }


        // 计算机名  
        public static string getPCName()
        {
            return Environment.MachineName;
        }

        // 磁盘序列号
        public static string getDiskNumber()
        {
            ObjectQuery objquery = new ObjectQuery("Select * From Win32_DiskDrive");//Win32_PhysicalMedia
            string infoname = "SerialNumber"; //SerialNumber
            return getSystemInfoObject(objquery, infoname);
        }

        //磁盘驱动器
        public static string getDisks()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            var disks = "";
            foreach (ManagementObject mo in searcher.Get())
            {
                disks += mo["Caption"].ToString() + "；";
            }
            return disks;
        }

        // 操作系统  
        public static string getPCSystem()
        {
            ObjectQuery objquery = new ObjectQuery("select * from Win32_OperatingSystem");
            string infoname = "Name";
            return getSystemInfoObject(objquery, infoname);
        }
        // 操作系统位数
        public static string getPCBit()
        {
            ObjectQuery objquery = new ObjectQuery("select * from Win32_OperatingSystem");
            string infoname = "OSArchitecture";
            return getSystemInfoObject(objquery, infoname);
        }
        // 操作系统语言
        public static string getPCLanguage()
        {
            return CultureInfo.InstalledUICulture.NativeName;
        }

        //PC模块
        public static string getPCType()
        {
            ObjectQuery objquery = new ObjectQuery("select * from Win32_ComputerSystem");
            string infoname = "Model";
            return getSystemInfoObject(objquery, infoname);
        }

        // CPU
        public static string getPCCPU()
        {
            ObjectQuery objquery = new ObjectQuery("select * from Win32_Processor");
            string infoname = "Name";
            return getSystemInfoObject(objquery, infoname);
        }

        // 内存
        public static string getPCMemory()
        {
            ObjectQuery objquery = new ObjectQuery("select * from Win32_ComputerSystem");
            string infoname = "TotalPhysicalMemory";
            return getSystemInfoObject(objquery, infoname);
        }

        //硬盘
        public static string getPCDisk()
        {
            long lsum = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                //判断是否是固定磁盘  
                if (drive.DriveType == DriveType.Fixed)
                {
                    lsum += drive.TotalSize;
                }
            }
            var disk = lsum / (1024 * 1024 * 1024);
            if (disk > 300)
            {
                return "SSD " + disk.ToString();
            }
            else
            {
                return "HDD " + disk.ToString();
            }
        }
    }
}
