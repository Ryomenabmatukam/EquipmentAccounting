using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;
using WinFormsUI.Forms.Edit;

namespace WinFormsUI.Forms
{
    public class LicensesForm : Form
    {
        private readonly LicenseService _service = new LicenseService();
        private DataGridView _grid;
        private Button _add, _edit, _del, _refresh;

        public LicensesForm()
        {
            Text = "Лицензии ПО";
            Width = 1100; Height = 520;

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

        private SoftwareLicense Selected()
        {
            return _grid.CurrentRow?.DataBoundItem as SoftwareLicense;
        }

        private void LoadData()
        {
            _grid.DataSource = _service.GetAll();
            if (_grid.Columns["InstalledSoftwares"] != null) _grid.Columns["InstalledSoftwares"].Visible = false;
        }

        private void Add()
        {
            var dlg = new LicenseEditForm();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Add(dlg.License); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Edit()
        {
            var l = Selected();
            if (l == null) return;

            var dlg = new LicenseEditForm(l);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Update(dlg.License); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Delete()
        {
            var l = Selected();
            if (l == null) return;

            if (!UiHelpers.Confirm($"Удалить лицензию '{l.SoftwareName}'?")) return;

            try { _service.Delete(l.Id); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }
    }
}
