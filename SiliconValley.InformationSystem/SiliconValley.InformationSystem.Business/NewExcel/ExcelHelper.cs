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
        public bool DaoruExcel(List<MyExcelClass> list ,string mypath)
        {
            bool IsCuss = false;
            object misValue = System.Reflection.Missing.Value;
            Application xlApp = new Application();
            Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            Worksheet xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

            PropertyInfo[] props = GetPropertyInfoArray();
            for (int i = 0; i < props.Length; i++)
            {
                xlWorkSheet.Cells[1, i + 1] = props[i].Name; //write the column name
            }
            for (int i = 0; i < list.Count; i++)
            {
                xlWorkSheet.Cells[i + 2, 1] = list[i].StuName;
                xlWorkSheet.Cells[i + 2, 2] = list[i].StuSex;
                xlWorkSheet.Cells[i + 2, 3] = list[i].StuPhone;
                xlWorkSheet.Cells[i + 2, 4] = list[i].StuSchoolName;
                xlWorkSheet.Cells[i + 2, 5] = list[i].StuAddress;
                xlWorkSheet.Cells[i + 2, 6] = list[i].Region_id;
                xlWorkSheet.Cells[i + 2, 7] = list[i].StuInfomationType_Id;
                xlWorkSheet.Cells[i + 2, 8] = list[i].StuEducational;
                xlWorkSheet.Cells[i + 2, 9] = list[i].Reak;
            }
            try
            {            
                xlWorkBook.SaveAs(mypath, XlFileFormat.xlExcel8, null, null, false, false, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                IsCuss = true;
            }
            catch (Exception ex)
            {
                return IsCuss;
            }
            return IsCuss;
        }
    }
}
