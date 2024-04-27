using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFramework;
using WebFramework.Backend;
using WebFramework.PT;

namespace SamsidParty_WiFi_Extract
{
    public class Frontend : WebScript
    {
        public override async Task DOMContentLoaded()
        {
            while (Program.Extractor.Passwords.Count < 1)
            {
                await Task.Delay(100);
            }

            await Document.RunFunction("window.AddEntries", Program.Extractor.Passwords.ToArray());
        }

        //Called By JavaScript
        //Expands The Window Size So The Share UI Fits
        public static void SharePassword()
        {
            (WindowManager.MainWindow as PTWebWindow).Native.Size = new System.Drawing.Size(1280, 720);
        }


        //Called By JavaScript
        //Returns The Window To A Normal Size
        public static void FinishSharePassword()
        {
            (WindowManager.MainWindow as PTWebWindow).Native.Size = new System.Drawing.Size(520, 780);
        }
    }
}
