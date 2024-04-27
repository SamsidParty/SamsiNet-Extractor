using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SamsidParty_WiFi_Extract
{
    public class PasswordFinder
    {
        public Dictionary<string, string> Passwords = new Dictionary<string, string>();
        public string LastExtraction;

        public PasswordFinder() 
        {
            LastExtraction = Util.SimpleCMD("netsh wlan show profile");
            var result = LastExtraction;

            if (!result.Contains("User profiles"))
            {
                return;
            }

            var networks = Util.ExtractFromString(result, "All User Profile :", "\n");

            foreach (var network in networks)
            {
                var netInfo = Util.SimpleCMD("netsh wlan show profile name=\"" + network + "\" key=clear");
                var password = Util.ExtractFromString(netInfo, "Key Content :", "\n");

                if (password.Count > 0)
                {
                    Passwords[network] = password[0];
                }
                else
                {
                    Passwords[network] = "None";
                }
            }
        }
    }
}
