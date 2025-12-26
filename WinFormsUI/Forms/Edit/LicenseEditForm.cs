using DAL.Entities;
using System;
using System.Windows.Forms;

namespace WinFormsUI.Forms.Edit
{
    public class LicenseEditForm : Form
    {
        public SoftwareLicense License { get; private set; }

        private TextBox _name, _man, _key;
        private DateTimePicker _exp;

        public LicenseEditForm()
        {
            Text = "Лицензия ПО";
            Width = 650; Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            License = new SoftwareLicense { ExpirationDate = DateTime.Today.AddYears(1) };
            Build();
        }

        public LicenseEditForm(SoftwareLicense l) : this()
        {
            License = new SoftwareLicense
            {
                Id = l.Id,
                SoftwareName = l.SoftwareName,
                Manufacturer = l.Manufacturer,
                LicenseKey = l.LicenseKey,
                ExpirationDate = l.ExpirationDate
            };

            _name.Text = License.SoftwareName;
            _man.Text = License.Manufacturer;
            _key.Text = License.LicenseKey;
            _exp.Value = License.ExpirationDate == default ? DateTime.Today.AddYears(1) : License.ExpirationDate;
        }

        private void Build()
        {
            var table = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 5 };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            _name = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _man = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _key = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };
            _exp = new DateTimePicker { Anchor = AnchorStyles.Left | AnchorStyles.Right };

            AddRow(table, 0, "Название ПО:", _name);
            AddRow(table, 1, "Производитель:", _man);
            AddRow(table, 2, "Ключ лицензии:", _key);
            AddRow(table, 3, "Дата окончания:", _exp);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var ok = new Button { Text = "OK", Width = 90 };
            var cancel = new Button { Text = "Отмена", Width = 90 };

            ok.Click += (s, e) => Ok();
            cancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            buttons.Controls.AddRange(new Control[] { ok, cancel });
            table.Controls.Add(buttons, 0, 4);
            table.SetColumnSpan(buttons, 2);

            Controls.Add(table);
        }

        private void AddRow(TableLayoutPanel t, int row, string label, Control control)
        {
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            t.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left }, 0, row);
            t.Controls.Add(control, 1, row);
        }

        private void Ok()
        {
            if (string.IsNullOrWhiteSpace(_name.Text)) { MessageBox.Show("Введите название ПО."); return; }
            if (string.IsNullOrWhiteSpace(_man.Text)) { MessageBox.Show("Введите производителя."); return; }
            if (string.IsNullOrWhiteSpace(_key.Text)) { MessageBox.Show("Введите ключ."); return; }

            License.SoftwareName = _name.Text.Trim();
            License.Manufacturer = _man.Text.Trim();
            License.LicenseKey = _key.Text.Trim();
            License.ExpirationDate = _exp.Value.Date;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
