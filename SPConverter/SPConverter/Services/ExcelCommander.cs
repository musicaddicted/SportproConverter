using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using SPConverter.Model;

namespace SPConverter.Services
{
    public class ExcelCommander: IDisposable
    {
        private Microsoft.Office.Interop.Excel.Application _app;
        private Workbook _workbook;
        private Worksheet _activeWorksheet;

        public ExcelCommander()
        {
            //Type objClassType;
            //objClassType = Type.GetTypeFromProgID("Excel.Application");
            //oApp = Activator.CreateInstance(objClassType);
        }

        public void OpenFile(Income income)
        {
            _app = new Microsoft.Office.Interop.Excel.Application();
            _app.Visible = true;

            _workbook = _app.Workbooks.Open(income.FilePath);
            _activeWorksheet = _workbook.ActiveSheet;

        }

        #region private Wrappers
        private void SetProperty(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            obj.GetType().InvokeMember(sProperty, BindingFlags.SetProperty, null, obj, oParam);
        }
        private object GetProperty(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            return obj.GetType().InvokeMember
                (sProperty, BindingFlags.GetProperty, null, obj, oParam);
        }
        private object GetProperty(object obj, string sProperty, object oValue1, object oValue2)
        {
            object[] oParam = new object[2];
            oParam[0] = oValue1;
            oParam[1] = oValue2;
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.GetProperty, null, obj, oParam);
        }
        private object GetProperty(object obj, string sProperty)
        {
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.GetProperty, null, obj, null);
        }
        private object InvokeMethod(object obj, string sProperty, object[] oParam)
        {
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }
        private object InvokeMethod(object obj, string sProperty, object oValue)
        {
            object[] oParam = new object[1];
            oParam[0] = oValue;
            return obj.GetType().InvokeMember
            (sProperty, BindingFlags.InvokeMethod, null, obj, oParam);
        }
        #endregion

        public void Dispose()
        {
            _app.Quit();

            //Marshal.ReleaseComObject(oBooks);

            //Marshal.ReleaseComObject(oApp);
            //GC.Collect();
        }
    }
}
