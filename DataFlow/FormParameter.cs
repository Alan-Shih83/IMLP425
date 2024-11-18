using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{

    [AttributeUsage(System.AttributeTargets.Property)]
    public class SectionAttribute : System.Attribute
    {
        public string Section { get; set; }
        public SectionAttribute(string Section) { this.Section = Section; }
    }
    public class FormParameter
    {
        private string _IP   = default(string);
        private string _Port = default(string);
        private string _COM  = default(string);
        private string _RetryCount = default(string);
        private string _RetryTime = default(string);
        private string _setTestMode = "0";

        [SectionAttribute("Mode")]
        public string setTestMode
        {
            get
            {
                return _setTestMode;
            }
            set
            {
                if (value != default)
                {
                    try
                    {
                        Convert.ToInt32(value.Trim());
                        _setTestMode = value.Trim();
                    }
                    catch (Exception) { ClsMsgBox.MyWarning("setTestMode 設定錯誤. "); throw; }
                }
            }
        }
        [SectionAttribute("PLC")]
        public string IP 
        {
            get
            {
                return _IP;
            }
            set
            {
                if (value != default)
                {
                    try
                    {
                        System.Net.IPAddress.Parse(value.Trim());
                        _IP = value.Trim();
                    }
                    catch (Exception) { ClsMsgBox.MyWarning("IP 設定錯誤. "); throw; }
                }
            }
        }

        [SectionAttribute("PLC")]
        public string Port
        {
            get
            {
                return _Port;
            }
            set
            {
                if (value != default)
                {
                    try
                    {
                        Convert.ToInt32(value);
                        _Port = value.Trim();
                    }
                    catch (Exception) { ClsMsgBox.MyWarning("Port 設定錯誤. "); throw; }
                }
            }
        } 

        [SectionAttribute("RS232")]
        public string COM
        {
            get
            {
                return _COM;
            }
            set
            {
                if (value != default)
                {
                    try
                    {
                        Convert.ToInt32(value);
                        _COM = value.Trim();
                    }
                    catch (Exception) { ClsMsgBox.MyWarning("COM 設定錯誤. "); throw; }
                }
            }
        }

        [SectionAttribute("RS232")]
        public string RetryCount
        {
            get
            {
                return _RetryCount;
            }
            set
            {
                if (value != default)
                {
                    try
                    {
                       // Convert.ToInt32(value);
                        _RetryCount = value.Trim();
                    }
                    catch (Exception) { ClsMsgBox.MyWarning("RetryCount 設定錯誤. "); throw; }
                }
            }
        }

        [SectionAttribute("RS232")]
        public string RetryTime
        {
            get
            {
                return _RetryTime;
            }
            set
            {
                if (value != default)
                {
                    try
                    {
                       // Convert.ToInt32(value);
                        _RetryTime = value.Trim();
                    }
                    catch (Exception) { ClsMsgBox.MyWarning("RetryTime 設定錯誤. "); throw; }
                }
            }
        }

        public FormParameter() { }

        public FormParameter(string IP, string Port, string COM, string RetryCount, string RetryTime, string setTestMode)
        {
            this.IP = IP;
            this.Port = Port;
            this.COM = COM;
            this.RetryCount = RetryCount;
            this.RetryTime = RetryTime;
            this.setTestMode = setTestMode;
        }

        public IEnumerable<SectionAttribute> GetAttributes()
        {
            List<SectionAttribute> sections = new List<SectionAttribute>();
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                SectionAttribute section = property.GetCustomAttributes(typeof(SectionAttribute), false).FirstOrDefault(sec => !string.IsNullOrEmpty((sec as SectionAttribute).Section)) as SectionAttribute;
                if (!Equals(section, default(SectionAttribute)))
                {
                    if (sections.Where(s => string.CompareOrdinal(section.Section, s.Section) == 0).Count() == 0)
                        sections.Add(section);
                }
            }
            return sections;
        }

    }
}
