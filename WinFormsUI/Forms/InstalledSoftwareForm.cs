using BLL.Services;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms
{
    public class InstalledSoftwareForm : Form
    {
        private readonly SoftwareInstallService _service = new SoftwareInstallService();

        private ComboBox _equipment, _license;
        private DateTimePicker _installedAt;
        private DataGridView _grid;
        private Button _install, _uninstall, _refresh;

        public InstalledSoftwareForm()
        {
            Text = "Установленное ПО";
            Width = 1100; Height = 560;

            Build();
            LoadLists();
        }

        private void Build()
        {
            var top = new TableLayoutPanel { Dock = DockStyle.Top, Height = 95, Padding = new Padding(10), ColumnCount = 6 };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));

            _equipment = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _license = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _installedAt = new DateTimePicker { Dock = DockStyle.Fill, Value = DateTime.Now };

            _install = new Button { Text = "Установить", Dock = DockStyle.Fill };
            _uninstall = new Button { Text = "Удалить", Dock = DockStyle.Fill };
            _refresh = new Button { Text = "Обновить", Dock = DockStyle.Fill };

            _equipment.SelectedIndexChanged += (s, e) => LoadInstalled();
            _install.Click += (s, e) => Install();
            _uninstall.Click += (s, e) => Uninstall();
            _refresh.Click += (s, e) => { LoadLists(); LoadInstalled(); };

            top.Controls.Add(new Label { Text = "Оборудование:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            top.Controls.Add(_equipment, 1, 0);
            top.SetColumnSpan(_equipment, 5);

            top.Controls.Add(new Label { Text = "Лицензия:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 1);
            top.Controls.Add(_license, 1, 1);

            top.Controls.Add(new Label { Text = "Дата:", AutoSize = true, Anchor = AnchorStyles.Left }, 2, 1);
            top.Controls.Add(_installedAt, 3, 1);

            top.Controls.Add(_install, 4, 1);
            top.Controls.Add(_uninstall, 5, 1);

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

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 45, Padding = new Padding(10), FlowDirection = FlowDirection.RightToLeft };
            bottom.Controls.Add(_refresh);
            Controls.Add(bottom);
        }

        private void LoadLists()
        {
            var eq = _service.GetEquipments();
            _equipment.DisplayMember = "InventoryNumber";
            _equipment.ValueMember = "Id";
            _equipment.DataSource = eq;

            var lic = _service.GetLicenses();
            _license.DisplayMember = "SoftwareName";
            _license.ValueMember = "Id";
            _license.DataSource = lic;

            LoadInstalled();
        }

        private int SelectedEquipmentId()
        {
            return _equipment.SelectedValue == null ? 0 : (int)_equipment.SelectedValue;
        }

        private void LoadInstalled()
        {
            var eqId = SelectedEquipmentId();
            if (eqId <= 0) { _grid.DataSource = null; return; }
            _grid.DataSource = _service.GetInstalledForEquipment(eqId);
        }

        private void Install()
        {
            var eqId = SelectedEquipmentId();
            if (eqId <= 0) return;
            if (_license.SelectedValue == null) { MessageBox.Show("Выберите лицензию."); return; }

            try
            {
                _service.Install(eqId, (int)_license.SelectedValue, _installedAt.Value);
                LoadInstalled();
            }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }

        private void Uninstall()
        {
            var eqId = SelectedEquipmentId();
            if (eqId <= 0) return;

            if (_grid.CurrentRow == null) return;

            var licIdObj = _grid.CurrentRow.Cells["SoftwareLicenseId"]?.Value;
            if (licIdObj == null) return;

            var licId = Convert.ToInt32(licIdObj);

            if (!UiHelpers.Confirm("Удалить выбранную установку?")) return;

            try
            {
                _service.Uninstall(eqId, licId);
                LoadInstalled();
            }
            catch (Exception ex) { UiHelpers.ErrorBox(ex); }
        }
    }
}
