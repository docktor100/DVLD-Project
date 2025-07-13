using DVLD_Project.Small_Forms;
using System;
using System.Windows.Forms;

namespace DVLD_Project
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            while (true)
            {
                frmLogin frmlogin = new frmLogin();
                frmlogin.ShowDialog();

                if (frmlogin.IsSignIn)
                {
                    frmMainForm frmmain = new frmMainForm();
                    frmmain.ShowDialog();

                    if (!frmmain.IsSignOut) break; // if closed the form without signing out
                }
                else
                    break;
            }
        }
    }
}
