using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;
using WinFormsUI.Forms.Edit;

namespace WinFormsUI.Forms
{
    public class DepartmentsForm : Form
    {
        private readonly DepartmentService _service = new DepartmentService();
        private DataGridView _grid;
        private Button _add, _edit, _del, _refresh;

        public DepartmentsForm()
        {
            Text = "Подразделения";
            Width = 900; Height = 500;

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

        private Department Selected()
        {
            return _grid.CurrentRow?.DataBoundItem as Department;
        }

        private void LoadData()
        {
            _grid.DataSource = _service.GetAll();
        }

        private void Add()
        {
            var dlg = new DepartmentEditForm();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Add(dlg.Department); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Edit()
        {
            var d = Selected();
            if (d == null) return;

            var dlg = new DepartmentEditForm(d);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Update(dlg.Department); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Delete()
        {
            var d = Selected();
            if (d == null) return;

            if (!UiHelpers.Confirm($"Удалить подразделение '{d.Name}'?")) return;

            try { _service.Delete(d.Id); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }
    }
}
