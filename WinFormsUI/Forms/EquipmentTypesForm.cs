using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;
using WinFormsUI.Forms.Edit;

namespace WinFormsUI.Forms
{
    public class EquipmentTypesForm : Form
    {
        private readonly EquipmentTypeService _service = new EquipmentTypeService();
        private DataGridView _grid;
        private Button _add, _edit, _del, _refresh;

        public EquipmentTypesForm()
        {
            Text = "Типы оборудования";
            Width = 700; Height = 460;

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

        private EquipmentType Selected()
        {
            return _grid.CurrentRow?.DataBoundItem as EquipmentType;
        }

        private void LoadData()
        {
            _grid.DataSource = _service.GetAll();
            if (_grid.Columns["Equipments"] != null) _grid.Columns["Equipments"].Visible = false;
        }

        private void Add()
        {
            var dlg = new SimpleNameEditForm("Тип оборудования");
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Add(new EquipmentType { Name = dlg.Value }); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Edit()
        {
            var t = Selected();
            if (t == null) return;

            var dlg = new SimpleNameEditForm("Тип оборудования", t.Name);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Update(new EquipmentType { Id = t.Id, Name = dlg.Value }); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Delete()
        {
            var t = Selected();
            if (t == null) return;

            if (!UiHelpers.Confirm($"Удалить '{t.Name}'?")) return;

            try { _service.Delete(t.Id); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }
    }
}
