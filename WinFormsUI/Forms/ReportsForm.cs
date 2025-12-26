using BLL.Services;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms
{
    public class ReportsForm : Form
    {
        private readonly ReportsService _reports = new ReportsService();
        private readonly EmployeeService _employees = new EmployeeService();

        private TabControl _tabs;
        private DataGridView _grid1, _grid2;
        private ComboBox _employee;
        private Button _refresh1, _refresh2;

        public ReportsForm()
        {
            Text = "Отчеты";
            Width = 1200; Height = 600;

            Build();
            LoadEmployees();
            LoadReport1();
            LoadReport2();
        }

        private void Build()
        {
            _tabs = new TabControl { Dock = DockStyle.Fill };

            // Tab 1
            var tab1 = new TabPage("Оборудование по подразделениям");
            var top1 = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(10), FlowDirection = FlowDirection.RightToLeft };
            _refresh1 = new Button { Text = "Обновить", Width = 110 };
            _refresh1.Click += (s, e) => LoadReport1();
            top1.Controls.Add(_refresh1);

            _grid1 = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            tab1.Controls.Add(_grid1);
            tab1.Controls.Add(top1);

            // Tab 2
            var tab2 = new TabPage("ПО на компьютерах сотрудника");
            var top2 = new TableLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10), ColumnCount = 3 };
            top2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            top2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            top2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));

            _employee = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _employee.SelectedIndexChanged += (s, e) => LoadReport2();

            _refresh2 = new Button { Text = "Обновить", Dock = DockStyle.Fill };
            _refresh2.Click += (s, e) => { LoadEmployees(); LoadReport2(); };

            top2.Controls.Add(new Label { Text = "Сотрудник:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            top2.Controls.Add(_employee, 1, 0);
            top2.Controls.Add(_refresh2, 2, 0);

            _grid2 = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            tab2.Controls.Add(_grid2);
            tab2.Controls.Add(top2);

            _tabs.TabPages.Add(tab1);
            _tabs.TabPages.Add(tab2);

            Controls.Add(_tabs);
        }

        private void LoadEmployees()
        {
            var emps = _employees.GetAll();
            _employee.DisplayMember = "FullName";
            _employee.ValueMember = "Id";
            _employee.DataSource = emps;
        }

        private void LoadReport1()
        {
            _grid1.DataSource = _reports.EquipmentByDepartments();
        }

        private void LoadReport2()
        {
            if (_employee.SelectedValue == null) { _grid2.DataSource = null; return; }
            _grid2.DataSource = _reports.SoftwareForEmployee((int)_employee.SelectedValue);
        }
    }
}
