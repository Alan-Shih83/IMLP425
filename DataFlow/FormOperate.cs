using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public class FormInfo
    {
        Form form = default;

        float _Width = default;

        float _Height = default;

        List<Control> ControlList = new List<Control>();
        public FormInfo(Form form)
        {
            this.form = form;
            _Width = form.Width;
            _Height = form.Height;
            ControlList = ChangeFormSize.GetAllControls(form);
            setTag(form);
        }

        public float Width_Multiple
        {
            get
            {
                return (form.Width) / _Width;
            }
        }
        public float Height_Multiple
        {
            get
            {
                return (form.Height) / _Height;
            }
        }

        public void ControlResize(Control control)
        {
            var GroupList = ChangeFormSize.GetGroupList(ControlList, control);
            ChangeFormSize.setControls(GroupList, Width_Multiple, Height_Multiple);
        }

        private void setTag(Control controls)
        {
            foreach (Control control in controls.Controls)
            {
                control.Tag = control.Width + ";" + control.Height + ";" + control.Left + ";" + control.Top + ";" + control.Font.Size;
                if (control.Controls.Count > 0)
                {
                    setTag(control);
                }
            }
        }
    }

    public static class FormOperate
    {
        public static void TabControlIndexHide(TabControl tabControl)
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        public static void ComboBoxItemSet(ComboBox comboBox, Enum @enum)
        {
            comboBox.DisplayMember = "Description";
            comboBox.ValueMember = "Value";
            comboBox.DataSource = Enum.GetValues(@enum.GetType())
                .Cast<Enum>()
                .Select(value => new
                {
                    (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                    value
                })
                .OrderBy(item => item.value)
                .ToList();
        }

        public static void ListViewItenInsert(ListView listView, string[] item)
        {
            if (!Equals(item, default))
            {
                ListViewItem ListViewItem = new ListViewItem(item);
                ListViewItem.EnsureVisible();
                listView.Items.Add(ListViewItem);
            }
        }

        public static void ListViewClearAll(ListView listView)
        {
            listView.Clear();
        }

        public static void ListViewBackColorClear(ListView listView, Color color = default(Color))
        {
            var item = listView.Items.Cast<ListViewItem>().Where(it => !Equals(color, it.BackColor));
            foreach (var _item in item)
            {
                int index = listView.Items.IndexOf(_item);
                listView.Items[index].BackColor = color;
            }
        }

        public static void ListViewBackColor(ListView listView, int index, string search, Color color)
        {
            if (index >= 0 && index <= listView.Columns.Count)
            {
                ListViewItem indexer = listView.Items.Cast<ListViewItem>().FirstOrDefault(item => item.SubItems[index].Text == search);
                if(!Equals(indexer, default(ListViewItem)))
                    indexer.BackColor = color;
            }
        }

        public static void ListViewItemDelete(ListView listView, int index, string compare)
        {
            if(index >= 0 && index <= listView.Columns.Count)
            {
                var indexer = listView.Items.Cast<ListViewItem>().FirstOrDefault(item => item.SubItems[index].Text == compare);
                listView.Items.Remove(indexer);
            }
            //if(index >= 0 && index <= listView.Columns.Count)
            //{
            //    for (int j = 0; j < listView.Items.Count; j++)
            //    {
            //        if (listView.Items[j].SubItems[index].Text.Equals(compare))
            //        {
            //            listView.Items.Remove(listView.Items[j]);
            //            j--;
            //        }
            //    }
            //} 
        }

        public static void ListBoxItemInterval(ListBox listBox, int interval = 200)
        {
            if (listBox.Items.Count > interval)
            {
                listBox.Items.RemoveAt(interval - 1);
            }
            listBox.Refresh();
        }

        public static void ListBoxItemInsert(ListBox listBox, params object[] item)
        {
            if (!Equals(item, default) && item.Length > 0)
            {
                var iter = item.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (!Equals(iter.Current, default))
                    {
                        listBox.Items.Insert(0, iter.Current);
                        ListBoxItemInterval(listBox);
                    }
                }
            }
        }

    }
}
