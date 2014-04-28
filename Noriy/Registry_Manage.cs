using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Noriy
{
    public static class Registry_Manage
    {
        public static string GetUsername()
        {
            RegistryKey RKey = Registry.CurrentUser.OpenSubKey("Noriy", true);
            if (RKey == null)
            {
                //Creates registry key
                Registry.CurrentUser.CreateSubKey("Noriy");
                RKey = Registry.CurrentUser.OpenSubKey("Noriy", true);
                RKey.SetValue("username", "null");
                RKey.SetValue("RegisterTraffic", "true");
                return "null";
            }
            else
            {
                return RKey.GetValue("username").ToString(); 
            }

        }

        public static bool GetRegisterTraffic()
        {
            RegistryKey RKey = Registry.CurrentUser.OpenSubKey("Noriy", true);
            if (RKey == null)
            {
                //Creates registry key
                Registry.CurrentUser.CreateSubKey("Noriy");
                RKey = Registry.CurrentUser.OpenSubKey("Noriy", true);
                RKey.SetValue("username", "null");
                RKey.SetValue("RegisterTraffic", "true");
                return true;
            }
            else
            {
                if (RKey.GetValue("RegisterTraffic").ToString() == "true")
                    return true;
                else return false;
            }
        }


    }
}
