using DAL;
using System;
using System.Windows.Forms;

namespace WinFormsUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // создаём БД и тестовые справочники при первом запуске
            DbInitializer.EnsureCreatedAndSeed();

            Application.Run(new MainForm());
        }
    }
}
