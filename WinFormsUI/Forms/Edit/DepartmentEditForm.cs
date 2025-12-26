using DAL.Entities;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms.Edit
{
    public class DepartmentEditForm : Form
    {
        public Department Department { get; private set; }

        private TextBox _name, _manager;

        public DepartmentEditForm()
        {
            Text = "Подразделение";
            Width = 420; Height = 210;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            Department = new Department();

            Build();
        }

        public DepartmentEditForm(Department d) : this()
        {
            Department = new Department { Id = d.Id, Name = d.Name, Manager = d.Manager };
            _name.Text = Department.Name;
            _manager.Text = Department.Manager;
        }

        private void Build()
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 3 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            table.Controls.Add(new Label { Text = "Название:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            _name = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(_name, 1, 0);

            table.Controls.Add(new Label { Text = "Руководитель:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
            _manager = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(_manager, 1, 1);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var ok = new Button { Text = "OK", Width = 90 };
            var cancel = new Button { Text = "Отмена", Width = 90 };

            ok.Click += (s, e) => Ok();
            cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttons.Controls.AddRange(new Control[] { ok, cancel });
            table.Controls.Add(buttons, 0, 2);
            table.SetColumnSpan(buttons, 2);

            Controls.Add(table);
        }

        private void Ok()
        {
            if (string.IsNullOrWhiteSpace(_name.Text))
            {
                MessageBox.Show("Введите название.");
                return;
            }
            if (string.IsNullOrWhiteSpace(_manager.Text))
            {
                MessageBox.Show("Введите руководителя.");
                return;
            }

            Department.Name = _name.Text.Trim();
            Department.Manager = _manager.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
