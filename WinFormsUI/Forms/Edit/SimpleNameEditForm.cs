using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms.Edit
{
    public class SimpleNameEditForm : Form
    {
        public string Value { get; private set; }

        private TextBox _tb;

        public SimpleNameEditForm(string title, string initial = null)
        {
            Text = title;
            Width = 420; Height = 170;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            Build(initial);
        }

        private void Build(string initial)
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 2 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            table.Controls.Add(new Label { Text = "Название:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            _tb = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Text = initial ?? "" };
            table.Controls.Add(_tb, 1, 0);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var ok = new Button { Text = "OK", Width = 90 };
            var cancel = new Button { Text = "Отмена", Width = 90 };

            ok.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_tb.Text))
                {
                    MessageBox.Show("Введите название.");
                    return;
                }
                Value = _tb.Text.Trim();
                DialogResult = DialogResult.OK;
                Close();
            };
            cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttons.Controls.AddRange(new Control[] { ok, cancel });
            table.Controls.Add(buttons, 0, 1);
            table.SetColumnSpan(buttons, 2);

            Controls.Add(table);
        }
    }
}
