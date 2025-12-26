using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms
{
    public static class UiHelpers
    {
        public static void ErrorBox(Exception ex)
        {
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool Confirm(string text)
        {
            return MessageBox.Show(text, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }
    }
}
