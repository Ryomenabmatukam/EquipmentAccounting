using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms.Edit
{
    public class EmployeeEditForm : Form
    {
        public Employee Employee { get; private set; }

        private readonly EmployeeService _service = new EmployeeService();
        private TextBox _fullName, _position;
        private ComboBox _dep;

        public EmployeeEditForm()
        {
            Text = "Сотрудник";
            Width = 520; Height = 260;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            Employee = new Employee();
            Build();
            LoadDeps();
        }

        public EmployeeEditForm(Employee e) : this()
        {
            Employee = new Employee { Id = e.Id, FullName = e.FullName, Position = e.Position, DepartmentId = e.DepartmentId };

            _fullName.Text = Employee.FullName;
            _position.Text = Employee.Position;
            _dep.SelectedValue = Employee.DepartmentId;
        }

        private void LoadDeps()
        {
            var deps = _service.GetDepartments();
            _dep.DisplayMember = "Name";
            _dep.ValueMember = "Id";
            _dep.DataSource = deps;
        }

        private void Build()
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 4 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            table.Controls.Add(new Label { Text = "ФИО:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            _fullName = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(_fullName, 1, 0);

            table.Controls.Add(new Label { Text = "Должность:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
            _position = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            table.Controls.Add(_position, 1, 1);

            table.Controls.Add(new Label { Text = "Подразделение:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 2);
            _dep = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList };
            table.Controls.Add(_dep, 1, 2);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var ok = new Button { Text = "OK", Width = 90 };
            var cancel = new Button { Text = "Отмена", Width = 90 };

            ok.Click += (s, e) => Ok();
            cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttons.Controls.AddRange(new Control[] { ok, cancel });
            table.Controls.Add(buttons, 0, 3);
            table.SetColumnSpan(buttons, 2);

            Controls.Add(table);
        }

        private void Ok()
        {
            if (string.IsNullOrWhiteSpace(_fullName.Text))
            {
                MessageBox.Show("Введите ФИО.");
                return;
            }
            if (_dep.SelectedValue == null)
            {
                MessageBox.Show("Выберите подразделение.");
                return;
            }

            Employee.FullName = _fullName.Text.Trim();
            Employee.Position = string.IsNullOrWhiteSpace(_position.Text) ? null : _position.Text.Trim();
            Employee.DepartmentId = (int)_dep.SelectedValue;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
