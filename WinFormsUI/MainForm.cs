using System;
using System.Windows.Forms;
using WinFormsUI.Forms;

namespace WinFormsUI
{
    public class MainForm : Form
    {
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        public MainForm()
        {
            Text = "Учет оборудования";
            Width = 1200;
            Height = 700;
            IsMdiContainer = true;

            BuildMenu();
            BuildToolbar();
            BuildStatus();
        }

        private void BuildMenu()
        {
            var menu = new MenuStrip();

            var mDirectories = new ToolStripMenuItem("Справочники");
            var miDepartments = new ToolStripMenuItem("Подразделения");
            var miEmployees = new ToolStripMenuItem("Сотрудники");
            var miTypes = new ToolStripMenuItem("Типы оборудования");
            var miLicenses = new ToolStripMenuItem("Лицензии ПО");

            miDepartments.Click += (s, e) => OpenChild(new DepartmentsForm());
            miEmployees.Click += (s, e) => OpenChild(new EmployeesForm());
            miTypes.Click += (s, e) => OpenChild(new EquipmentTypesForm());
            miLicenses.Click += (s, e) => OpenChild(new LicensesForm());

            mDirectories.DropDownItems.AddRange(new ToolStripItem[] { miDepartments, miEmployees, miTypes, miLicenses });

            var mAccounting = new ToolStripMenuItem("Учет");
            var miEquipment = new ToolStripMenuItem("Оборудование");
            var miInstalled = new ToolStripMenuItem("Установленное ПО");
            var miMovements = new ToolStripMenuItem("История перемещений");

            miEquipment.Click += (s, e) => OpenChild(new EquipmentsForm());
            miInstalled.Click += (s, e) => OpenChild(new InstalledSoftwareForm());
            miMovements.Click += (s, e) => OpenChild(new MovementsForm());

            mAccounting.DropDownItems.AddRange(new ToolStripItem[] { miEquipment, miInstalled, miMovements });

            var mReports = new ToolStripMenuItem("Отчеты");
            var miReports = new ToolStripMenuItem("Открыть отчеты");
            miReports.Click += (s, e) => OpenChild(new ReportsForm());
            mReports.DropDownItems.Add(miReports);

            menu.Items.AddRange(new ToolStripItem[] { mDirectories, mAccounting, mReports });
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private void BuildToolbar()
        {
            var ts = new ToolStrip();
            ts.GripStyle = ToolStripGripStyle.Hidden;

            var b1 = new ToolStripButton("Подразделения");
            b1.Click += (s, e) => OpenChild(new DepartmentsForm());

            var b2 = new ToolStripButton("Сотрудники");
            b2.Click += (s, e) => OpenChild(new EmployeesForm());

            var b3 = new ToolStripButton("Оборудование");
            b3.Click += (s, e) => OpenChild(new EquipmentsForm());

            var b4 = new ToolStripButton("Отчеты");
            b4.Click += (s, e) => OpenChild(new ReportsForm());

            ts.Items.AddRange(new ToolStripItem[] { b1, b2, new ToolStripSeparator(), b3, new ToolStripSeparator(), b4 });
            ts.Dock = DockStyle.Top;
            Controls.Add(ts);
        }

        private void BuildStatus()
        {
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Готово");
            statusStrip.Items.Add(statusLabel);
            Controls.Add(statusStrip);
        }

        private void OpenChild(Form f)
        {
            f.MdiParent = this;
            f.Show();
            statusLabel.Text = "Открыто: " + f.Text;
        }
    }
}
