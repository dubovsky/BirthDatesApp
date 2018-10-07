using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BirthDates
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }
        public override void Uninstall(IDictionary savedState)
        {
            if (savedState != null)
            {
                base.Uninstall(savedState);
            }

            string targetDir = @"D:\BirthDates\BirthDates\";
            

            try
            {
                // delete temp files (you can as well delete all files: "*.*")
                foreach (FileInfo f in new DirectoryInfo(targetDir).GetFiles("*.txt"))
                {
                    f.Delete();
                }

                // delete entire temp folder
                if (Directory.Exists(targetDir))
                {
                    Directory.Delete(targetDir, true);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                
            }
        }
    }
}
