using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;
using WinFormsUI.Forms.Edit;

namespace WinFormsUI.Forms
{
    public class EmployeesForm : Form
    {
        private readonly EmployeeService _service = new EmployeeService();
        private DataGridView _grid;
        private Button _add, _edit, _del, _refresh;

        public EmployeesForm()
        {
            Text = "Сотрудники";
            Width = 1000; Height = 520;

            Build();
            LoadData();
        }

        private void Build()
        {
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(10) };

            _add = new Button { Text = "Добавить", Width = 110 };
            _edit = new Button { Text = "Изменить", Width = 110 };
            _del = new Button { Text = "Удалить", Width = 110 };
            _refresh = new Button { Text = "Обновить", Width = 110 };

            _add.Click += (s, e) => Add();
            _edit.Click += (s, e) => Edit();
            _del.Click += (s, e) => Delete();
            _refresh.Click += (s, e) => LoadData();

            top.Controls.AddRange(new Control[] { _add, _edit, _del, _refresh });

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Controls.Add(_grid);
            Controls.Add(top);
        }

        private Employee Selected()
        {
            return _grid.CurrentRow?.DataBoundItem as Employee;
        }

        private void LoadData()
        {
            _grid.DataSource = _service.GetAll();
            if (_grid.Columns["Department"] != null) _grid.Columns["Department"].Visible = false;
            if (_grid.Columns["Equipments"] != null) _grid.Columns["Equipments"].Visible = false;
        }

        private void Add()
        {
            var dlg = new EmployeeEditForm();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Add(dlg.Employee); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Edit()
        {
            var e = Selected();
            if (e == null) return;

            var dlg = new EmployeeEditForm(e);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Update(dlg.Employee); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Delete()
        {
            var e = Selected();
            if (e == null) return;

            if (!UiHelpers.Confirm($"Удалить сотрудника '{e.FullName}'?")) return;

            try { _service.Delete(e.Id); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }
    }
}
