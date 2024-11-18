using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public class FormManager
    {
        Storage<Form> forms = new Storage<Form>();

        private static readonly Lazy<FormManager> singleton = new Lazy<FormManager>(() => new FormManager(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        private FormManager() { }
        public static FormManager Instance { get { return singleton.Value; } }

        public void attach(Form form)
        {
            forms.SetRecord(form);
        }

        public IFormUpdate Query<T>() where T : Form
        {
            return forms.QueryItem<T>() as IFormUpdate;
        }
    }
}
