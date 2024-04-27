using System;
using System.Diagnostics;
using System.Drawing;
using WebFramework;
using SamsidParty_WiFi_Extract;
using WebFramework.PT;

public class Program
{

    public static ThemeBasedColor TitlebarColor;
    public static PasswordFinder Extractor = new();

    [STAThread]
    public static void Main(string[] args)
    {
        PTWindowProvider.Activate();
        AppManager.Validate(args, "SamsidParty", "SamsiNetExtractor");

        App();
    }

    public static async Task App()
    {

        //Disable In Production
        //DevTools.Enable();
        //DevTools.HotReload("http://127.0.0.1:25631");

        //Change Color Based On Theme (light, dark)
        TitlebarColor = new ThemeBasedColor(Color.FromArgb(255, 255, 255), Color.FromArgb(34, 34, 34));

        WindowManager.Options = new WindowOptions() {
            TitlebarColor = TitlebarColor,
            StartWidthHeight = new Rectangle(400, 600, 520, 780),
            LockWidthHeight = true,
            IconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WWW", "Image", "Icon.ico")
        };

        WebScript.Register<Frontend>("frontend");

        await AppManager.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WWW"), OnReady);
    }

    public static async Task OnReady(WebWindow w)
    {
        w.BackgroundColor = TitlebarColor;
    }

}