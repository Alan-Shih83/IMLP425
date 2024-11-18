using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class Ini
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern uint GetPrivateProfileSection(string lpAppName, IntPtr lpReturnedString, uint nSize, string lpFileName);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileSection", SetLastError = true)]
        public static extern uint WritePrivateProfileSection(string section, string lpString, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static Dictionary<string, string> GetSectionDic(string iniFile, string section)
        {
            uint MAX_BUFFER = 32767;
            string[] items = null;

            IntPtr pReturnedString = System.Runtime.InteropServices.Marshal.AllocCoTaskMem((int)MAX_BUFFER * sizeof(char));
            uint bytesReturned = GetPrivateProfileSection(section, pReturnedString, MAX_BUFFER, iniFile);
            if (!(bytesReturned == MAX_BUFFER - 2) || (bytesReturned == 0))
            {
                string returnedString = System.Runtime.InteropServices.Marshal.PtrToStringAuto(pReturnedString, (int)bytesReturned);
                items = returnedString.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(pReturnedString);

            if (items == null)
            {
                return null;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string item in items)
            {
                if (!item.Contains("="))
                {
                    continue;
                }
                string[] part = item.Split('=');
                if (part[1] != "")
                {
                    dic.Add(part[0], part[1]);
                }

            }
            return dic;
        }

    }
}
