using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow
{
    public class Btn_Msg
    {
        public string Name;
        public bool isEnable;
        public Color backColor;

        public Btn_Msg(string Name, bool isEnable, Color backColor)
        {
            this.Name = Name;
            this.isEnable = isEnable;
            this.backColor = backColor;
        }
        public Btn_Msg(string Name, bool isEnable) : this(Name, isEnable, SystemColors.Control) { }
        public Btn_Msg(string Name, Color backColor) : this(Name, true, backColor) { }
        public Btn_Msg(string Name) : this(Name, true, SystemColors.Control) { }
    }

    public abstract class ListViewMsg
    {
        public string Name;

        public ListViewMsg(string Name)
        {
            this.Name = Name;
        }
        public abstract object GetFormat();
        public abstract Color GetColor();
    }

    public class ListViewInsertMsg : ListViewMsg
    {
        public string[] msg = default(string[]);
        public ListViewInsertMsg(string Name, string[] msg) : base(Name) { this.msg = msg; }

        public override object GetFormat() { return msg; }
        public override Color GetColor() { return default(Color); }
    }

    public class ListViewDeleteMsg : ListViewMsg
    {
        public string compare = default(string);
        public ListViewDeleteMsg(string Name, string compare) : base(Name) { this.compare = compare; }
        public override object GetFormat() { return compare; }
        public override Color GetColor() { return default(Color); }
    }

    public class ListViewClearMsg : ListViewMsg
    {
        public ListViewClearMsg(string Name) : base(Name) { }
        public override object GetFormat() { return default(object); }
        public override Color GetColor() { return default(Color); }
    }

    public class ListViewBackColorClearMsg : ListViewMsg
    {
        public Color color = default(Color);
        public ListViewBackColorClearMsg(string Name, Color color = default(Color)) : base(Name) { this.color = color; }
        public override object GetFormat() { return default(object); }
        public override Color GetColor() { return color; }
    }

    public class ListViewBackColorMsg : ListViewMsg
    {
        public string compare = default(string);

        public Color color = default(Color);
        public ListViewBackColorMsg(string Name, string compare, Color color = default(Color)) : base(Name) { this.color = color; this.compare = compare; }
        public override object GetFormat() { return compare; }
        public override Color GetColor() { return color; }
    }

}
