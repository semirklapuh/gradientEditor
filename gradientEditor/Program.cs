using gradientEditor.Helpers;
using System;
using System.Windows.Forms;

namespace gradientEditor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IColorHelper colorHelper = new ColorHelper();
            Application.Run(new formTest(colorHelper));
        }
    }
}
