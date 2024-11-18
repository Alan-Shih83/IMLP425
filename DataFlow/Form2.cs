using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public partial class Form2 : Form
    {
        FormInfo formInfo = default;
        public Form2()
        {
            InitializeComponent();
            formInfo = new FormInfo(this);
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            if (!Equals(formInfo, default))
                formInfo.ControlResize(this);
        }
    }
}
