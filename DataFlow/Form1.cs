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
    public partial class Form1 : Form
    {
        public FormInfo formInfo = default;
        public Form1()
        {
            InitializeComponent();
            formInfo = new FormInfo(this);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (!Equals(formInfo, default))
                formInfo.ControlResize(this);
        }
    }
}
