using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    
    public class ControlEvent : EventArgs
    {
        public string[] Infor { get; set; }

        public float Newx;

        public float Newy;
    }

    public class ChangeFormSize
    {

        public static List<Control> GetAllControls(Form form)
        {
            return GetAllControls(ToList(form.Controls));
        }

        private static List<Control> ToList(Control.ControlCollection controls)
        {
            List<Control> controlList = new List<Control>();
            foreach (Control control in controls)
                controlList.Add(control);
            return controlList;
        }

        public static List<Control> GetGroupList(List<Control> inputList, Control controls)
        {
            IEnumerable<Control> containers = from control in inputList
                                       where control.Parent == controls
                                       select control;

            return GetAllControls(containers.ToList());
        }

        public static List<Control> GetAssignControlList(List<Control> inputList, Type type)
        {
            IEnumerable<Control> containers = from control in inputList
                                              where control.GetType() == type
                                              select control;

            return containers.ToList();
        }

        private static List<Control> GetAllControls(List<Control> inputList)
        {
            
            List<Control> outputList = new List<Control>(inputList);

           
            IEnumerable<Control> containers = from control in inputList
                                     where control is GroupBox |
                                           control is TabControl |
                                           control is Panel |
                                           control is FlowLayoutPanel |
                                           control is TableLayoutPanel |
                                           control is ContainerControl 
                                           select control;

            foreach (Control container in containers)
            {
                outputList.AddRange(GetAllControls(ToList(container.Controls)));
            }
            return outputList;
        }

        public static void setControls(List<Control> inputList, float newx, float newy)
        {

            Parallel.ForEach(inputList, con =>
            {
                if (con.Tag != null)
                {
                    var _ControlEvent = new ControlEvent()
                    {
                        Infor = con.Tag.ToString().Split(new char[] { ';' }),
                        Newx = newx,
                        Newy = newy
                    };

                    if (con.IsHandleCreated)
                    {
                        con.BeginInvoke(new Action(() =>
                        {
                            con.Width  = Convert.ToInt32(System.Convert.ToSingle(_ControlEvent.Infor[0]) * _ControlEvent.Newx);
                            con.Height = Convert.ToInt32(System.Convert.ToSingle(_ControlEvent.Infor[1]) * _ControlEvent.Newy);
                            con.Left   = Convert.ToInt32(System.Convert.ToSingle(_ControlEvent.Infor[2]) * _ControlEvent.Newx);
                            con.Top    = Convert.ToInt32(System.Convert.ToSingle(_ControlEvent.Infor[3]) * _ControlEvent.Newy);
                            if(con.Width > 1 && con.Height > 1 && con.Left > 1 && con.Top > 1)
                            {
                                Single currentSize = System.Convert.ToSingle(_ControlEvent.Infor[4]) * _ControlEvent.Newy + System.Convert.ToSingle(0.5);
                                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                            }
                            

                        }));
                    }
                   
                }
            });

        }

    }
}
