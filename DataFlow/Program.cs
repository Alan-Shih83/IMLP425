using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        static string appGuid = "{B19DAFCB-729C-43A6-8232-F3C31BB4E404}";

        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (mutex.WaitOne(0, false))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    _FormMain formMain = new _FormMain();
                    formMain.Tag = false;
                    Application.Run(formMain);
                }
            }
        }
    }
}
