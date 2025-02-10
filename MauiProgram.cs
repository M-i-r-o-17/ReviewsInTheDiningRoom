
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Newtonsoft.Json;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microcharts.Maui;
using System.Diagnostics;


#if WINDOWS
using System.Diagnostics;
using Microsoft.UI;
using Microsoft.UI.Windowing;
#endif

namespace ReviewsInTheDiningRoom
{
    public static class MauiProgram
    {
        private static string[] passwords =
        {
            "56789215", //root
            "98425647", //user
            "92749823", //power off
        };
        #if WINDOWS
        public static bool exploer
        { 
            get
            {

                Process[] processInfo = Process.GetProcessesByName("explorer");
                return processInfo != null;

            }
            
        }
        #endif

        public static bool isDebug = false;
        public static void ReadSave(string date, out int like, out int dislike)
        {
            string path = $"./save/{date}.json";

            if (!Directory.Exists("./save"))
            {
                Directory.CreateDirectory("./save");
            }

            if (!File.Exists(path))
            {
                like = 0;
                dislike = 0;
                WriteSave(0, 0);
                return;
            }
            var json = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<DataProgram>(json);
            like = data == null ? 0 : data.like;
            dislike = data == null ? 0 : data.dislike;

        }

        public static void PowerOff()
        {
            Process.Start($"shutdown", "/s /t 5");
            Application.Current?.Quit();
        }
        public static void ReadSave(out int like, out int dislike)
        {
            ReadSave(DateTime.Now.ToString("d"), out like, out dislike);
        }

        public static void WriteSave(string date, int like, int dislike)
        {
            string path = $"./save/{date}.json";
            if (!Directory.Exists("./save"))
            {
                Directory.CreateDirectory("./save");
            }

            var data = new DataProgram(like, dislike);
            var json = JsonConvert.SerializeObject(data);

            File.WriteAllText(path, json);
        }

        public static void WriteSave(int like, int dislike)
        {
            WriteSave(DateTime.Now.ToString("d"), like, dislike);

        }

        public static int AccesUser(string password)
        {
            for (int i = 0; i < passwords.Length; i++) { if(password ==  passwords[i]) return i; }
            return -1;
        }

        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
            events.AddWindows(w =>
            {
                w.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = true; //If you need to completely hide the minimized maximized close button, you need to set this value to false.
                    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
                    var _appWindow = AppWindow.GetFromWindowId(myWndId);
                    _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                });
            });
            events.AddWindows(windows => windows.OnPlatformMessage((window, args) =>
                               {
                                   if (args.MessageId == Convert.ToUInt32("0010", 16))
                                   {
                                        do{
                                            Process.Start("explorer");
                                       }while(!exploer);
                                   }
                               }));
#endif
            });



            return builder.Build();
        }
    }
}
