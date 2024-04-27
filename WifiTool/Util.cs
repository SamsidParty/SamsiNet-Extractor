using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SamsidParty_WiFi_Extract
{
    public class Util
    {

        /// <summary>
        /// Runs A Command Prompt Command And Returns The Output
        /// </summary>
        /// <param name="cmdString"></param>
        /// <returns></returns>
        public static string SimpleCMD(string cmdString)
        {
            var command = "/c " + cmdString;
            var cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Verb = "runas";
            cmdsi.Arguments = command;
            cmdsi.RedirectStandardOutput = true;
            cmdsi.UseShellExecute = false;
            cmdsi.CreateNoWindow = true;
            var cmd = Process.Start(cmdsi);
            var output = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();

            output = (new Regex("[ ]{2,}", RegexOptions.None)).Replace(output, " "); //Remove Double Spaces
            return output;
        }


        public static List<string> ExtractFromString(string source, string start, string end)
        {
            var results = new List<string>();

            string pattern = string.Format(
                "{0}({1}){2}",
                Regex.Escape(start),
                ".+?",
                 Regex.Escape(end));

            foreach (Match m in Regex.Matches(source, pattern))
            {
                results.Add(m.Groups[1].Value.Trim().Replace("\n", ""));
            }

            return results;
        }


    }
}
