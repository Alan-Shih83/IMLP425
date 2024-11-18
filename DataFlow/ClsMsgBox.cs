using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataFlow
{
    public class ClsMsgBox
    {
        // 錯誤訊息
        public static void MyError(string strMessage)
        {
            // MessageBox.Show(strMessage.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 確認訊息
        public static bool MyConfirmation(string strMessage)
        {
            bool IsConfirm = true;
            if (MessageBox.Show(strMessage, "Confirmation", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                IsConfirm = false;
            }
            return IsConfirm;
        }

        // 提示訊息
        public static void MyInformation(string strMessage)
        {
            MessageBox.Show(strMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 警告訊息
        public static void MyWarning(string strMessage)
        {
            MessageBox.Show(strMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
