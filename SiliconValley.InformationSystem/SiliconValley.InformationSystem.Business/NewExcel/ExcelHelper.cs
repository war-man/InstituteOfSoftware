using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using SiliconValley.InformationSystem.Entity.Entity;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;

namespace SiliconValley.InformationSystem.Business.NewExcel
{
   public class ExcelHelper
    {
        private PropertyInfo[] GetPropertyInfoArray()
        {
            PropertyInfo[] props = null;
            try
            {
                Type type = typeof(MyExcelClass);
                object obj = Activator.CreateInstance(type);
                props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception)
            {
                
            }
            return props;
        }
        /// <summary>
        /// 将疑似数据写入Excel中
        /// </summary>
        /// <param name="list">要写入Excel的数据</param>
        /// <param name="mypath">保存的路径</param>
        /// <returns></returns>
        public bool DaoruExcel(List<MyExcelClass> list ,string mypath,List<string> Head)
        {
            bool IsCuss = false;
            object misValue = System.Reflection.Missing.Value;
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            //PropertyInfo[] props = GetPropertyInfoArray();
            for (int i = 0; i < Head.Count; i++)
            {
                xlWorkSheet.Cells[1, i + 1] = Head[i]; //write the column name
            }
            try
            {            
                xlWorkBook.SaveAs(mypath, XlFileFormat.xlExcel8, null, null, false, false, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                IsCuss = true;
            }
            catch (Exception)
            {
                return IsCuss;
            }
            return IsCuss;
        }
    }
}
