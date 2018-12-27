using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProxyOnOff
{
    class Program
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption
          (IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;

        static void Main(string[] args)
        {
            int a=-1;
            bool settingsReturn, refreshReturn;

            Console.WriteLine("*************Proxy ON OFF*************\n");
            Console.WriteLine("1 for ON\n");
            Console.WriteLine("0 for OFF\n");

            //a = Convert.ToInt32(Console.ReadLine());

            RegistryKey registry = Registry.CurrentUser.OpenSubKey
               ("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);

            if((int)registry.GetValue("ProxyEnable", 0) == 0)
            {
                a = 1;
            }
            else if((int)registry.GetValue("ProxyEnable", 1) == 1)
            {
                a = 0;
            }

            switch (a)
            {
                case 1:
                    {
                        registry.SetValue("ProxyEnable", 1);
                        registry.SetValue
                        ("ProxyServer", "proxy.cognizant.com:6050");
                        if ((int)registry.GetValue("ProxyEnable", 0) == 0)
                            Console.WriteLine("Unable to enable the proxy.");
                        else
                            Console.WriteLine("The proxy has been turned on.");
                        break;
                    }
                case 0:
                    {
                        registry.SetValue("ProxyEnable", 0);
                        //registry.SetValue("ProxyServer", 0);
                        if ((int)registry.GetValue("ProxyEnable", 1) == 1)
                            Console.WriteLine("Unable to disable the proxy.");
                        else
                            Console.WriteLine("The proxy has been turned off.");

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid Argument!");
                        Console.ReadKey();
                        return;
                    }
            }
            registry.Close();
            settingsReturn = InternetSetOption
            (IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption
            (IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}