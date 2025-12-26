using BLL.Services;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms
{
    public class MovementsForm : Form
    {
        private readonly EquipmentService _equipmentService = new EquipmentService();
        private readonly MovementService _movementService = new MovementService();

        private ComboBox _equipment;
        private DataGridView _grid;
        private Button _refresh;

        public MovementsForm()
        {
            Text = "История перемещений";
            Width = 950; Height = 520;

            Build();
            LoadEquipments();
        }

        private void Build()
        {
            var top = new TableLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10), ColumnCount = 3 };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));

            _equipment = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            _equipment.SelectedIndexChanged += (s, e) => LoadMovements();

            _refresh = new Button { Text = "Обновить", Dock = DockStyle.Fill };
            _refresh.Click += (s, e) => { LoadEquipments(); LoadMovements(); };

            top.Controls.Add(new Label { Text = "Оборудование:", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            top.Controls.Add(_equipment, 1, 0);
            top.Controls.Add(_refresh, 2, 0);

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

        private void LoadEquipments()
        {
            var eq = _equipmentService.GetAll();
            _equipment.DisplayMember = "InventoryNumber";
            _equipment.ValueMember = "Id";
            _equipment.DataSource = eq;
        }

        private int SelectedEquipmentId()
        {
            return _equipment.SelectedValue == null ? 0 : (int)_equipment.SelectedValue;
        }

        private void LoadMovements()
        {
            var eqId = SelectedEquipmentId();
            if (eqId <= 0) { _grid.DataSource = null; return; }
            _grid.DataSource = _movementService.GetMovements(eqId);
        }
    }
}
