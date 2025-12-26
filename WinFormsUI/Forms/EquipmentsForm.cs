using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;
using WinFormsUI.Forms.Edit;

namespace WinFormsUI.Forms
{
    public class EquipmentsForm : Form
    {
        private readonly EquipmentService _service = new EquipmentService();
        private DataGridView _grid;
        private Button _add, _edit, _del, _refresh;

        public EquipmentsForm()
        {
            Text = "Оборудование";
            Width = 1150; Height = 560;

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

        private Equipment Selected()
        {
            return _grid.CurrentRow?.DataBoundItem as Equipment;
        }

        private void LoadData()
        {
            _grid.DataSource = _service.GetAll();
            // hide navigation columns
            if (_grid.Columns["EquipmentType"] != null) _grid.Columns["EquipmentType"].Visible = false;
            if (_grid.Columns["Employee"] != null) _grid.Columns["Employee"].Visible = false;
            if (_grid.Columns["InstalledSoftwares"] != null) _grid.Columns["InstalledSoftwares"].Visible = false;
            if (_grid.Columns["Movements"] != null) _grid.Columns["Movements"].Visible = false;
        }

        private void Add()
        {
            var dlg = new EquipmentEditForm();
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Add(dlg.Equipment); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Edit()
        {
            var eq = Selected();
            if (eq == null) return;

            var dlg = new EquipmentEditForm(eq);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try { _service.Update(dlg.Equipment); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Delete()
        {
            var eq = Selected();
            if (eq == null) return;

            if (!UiHelpers.Confirm($"Удалить оборудование '{eq.InventoryNumber}'?")) return;

            try { _service.Delete(eq.Id); LoadData(); }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }
    }
}
