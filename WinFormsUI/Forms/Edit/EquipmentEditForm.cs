using BLL.Services;
using DAL.Entities;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms.Edit
{
    public class EquipmentEditForm : Form
    {
        public Equipment Equipment { get; private set; }

        private readonly EquipmentService _service = new EquipmentService();

        private TextBox _inv, _name, _serial;
        private ComboBox _type, _employee, _status;
        private DateTimePicker _registered;

        public EquipmentEditForm()
        {
            Text = "Оборудование";
            Width = 650; Height = 360;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            Equipment = new Equipment();
            Build();
            LoadLists();
            _status.SelectedIndex = 0;
            _registered.Value = DateTime.Now;
        }

        public EquipmentEditForm(Equipment e) : this()
        {
            Equipment = new Equipment
            {
                Id = e.Id,
                InventoryNumber = e.InventoryNumber,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                RegisteredAt = e.RegisteredAt,
                Status = e.Status,
                EquipmentTypeId = e.EquipmentTypeId,
                EmployeeId = e.EmployeeId
            };

            _inv.Text = Equipment.InventoryNumber;
            _name.Text = Equipment.Name;
            _serial.Text = Equipment.SerialNumber;
            _registered.Value = Equipment.RegisteredAt == default ? DateTime.Now : Equipment.RegisteredAt;
            _status.SelectedIndex = Equipment.Status;

            _type.SelectedValue = Equipment.EquipmentTypeId;
            if (Equipment.EmployeeId == null) _employee.SelectedIndex = 0;
            else _employee.SelectedValue = Equipment.EmployeeId.Value;
        }

        private void LoadLists()
        {
            var types = _service.GetTypes();
            _type.DisplayMember = "Name";
            _type.ValueMember = "Id";
            _type.DataSource = types;

            var employees = _service.GetEmployees();
            // добавим "Не назначен"
            employees.Insert(0, new Employee { Id = 0, FullName = "(не назначен)" });
            _employee.DisplayMember = "FullName";
            _employee.ValueMember = "Id";
            _employee.DataSource = employees;
        }

        private void Build()
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 7 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _inv = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _name = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _serial = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _type = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList };
            _employee = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList };
            _registered = new DateTimePicker { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _status = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, DropDownStyle = ComboBoxStyle.DropDownList };
            _status.Items.AddRange(new object[] { "В работе", "На списании", "В ремонте" });

            AddRow(table, 0, "Инвентарный №:", _inv);
            AddRow(table, 1, "Название:", _name);
            AddRow(table, 2, "Тип:", _type);
            AddRow(table, 3, "Серийный №:", _serial);
            AddRow(table, 4, "Ответственный:", _employee);
            AddRow(table, 5, "Дата учета:", _registered);
            AddRow(table, 6, "Статус:", _status);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12), Height = 55 };
            var ok = new Button { Text = "OK", Width = 90 };
            var cancel = new Button { Text = "Отмена", Width = 90 };

            ok.Click += (s, e) => Ok();
            cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttons.Controls.AddRange(new Control[] { ok, cancel });

            Controls.Add(table);
            Controls.Add(buttons);
        }

        private void AddRow(TableLayoutPanel t, int row, string label, Control control)
        {
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            t.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
            t.Controls.Add(control, 1, row);
        }

        private void Ok()
        {
            if (string.IsNullOrWhiteSpace(_inv.Text)) { MessageBox.Show("Введите инвентарный номер."); return; }
            if (string.IsNullOrWhiteSpace(_name.Text)) { MessageBox.Show("Введите название."); return; }
            if (_type.SelectedValue == null) { MessageBox.Show("Выберите тип."); return; }
            if (_status.SelectedIndex < 0) { MessageBox.Show("Выберите статус."); return; }

            Equipment.InventoryNumber = _inv.Text.Trim();
            Equipment.Name = _name.Text.Trim();
            Equipment.SerialNumber = string.IsNullOrWhiteSpace(_serial.Text) ? null : _serial.Text.Trim();
            Equipment.RegisteredAt = _registered.Value;
            Equipment.Status = _status.SelectedIndex;
            Equipment.EquipmentTypeId = (int)_type.SelectedValue;

            var empId = (int)_employee.SelectedValue;
            Equipment.EmployeeId = empId == 0 ? null : empId;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
