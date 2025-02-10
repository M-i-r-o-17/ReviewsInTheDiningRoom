using AsyncAwaitBestPractices;
using System.Text;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui;
using System.Diagnostics;

namespace ReviewsInTheDiningRoom
{
    public class DataProgram
    {
        public DataProgram() { like = 0; dislike = 0; }
        public DataProgram(int like, int dislike)  { this.like = like;  this.dislike = dislike; }

        public int like { get; set; }
        public int dislike { get; set; }
    }

    public partial class MainPage : ContentPage
    {
        private int like, dislike;

        public MainPage()
        {
            InitializeComponent();
            thnk.Scale = 0;
            AsyncUpdateDate().SafeFireAndForget();
#if WINDOWS
            KillExploerr().SafeFireAndForget();
#endif
        }

        private async void OnLikeButtonClicked(object sender, System.EventArgs e)
        {
            LikeButton.IsEnabled = false;
            DislikeButton.IsEnabled = false;
            MauiProgram.ReadSave(out like, out dislike);
            like += 1;
            MauiProgram.WriteSave(like, dislike);

            await DislikeButton.ScaleTo(0.0f);
            await LikeButton.TranslateTo(350, -100);
            await thnk.ScaleTo(1.0f);
            await Task.Delay(1000);
            await thnk.ScaleTo(0.0f);
            await LikeButton.TranslateTo(0, 0);
            await DislikeButton.ScaleTo(1.0f);

            LikeButton.IsEnabled = true;
            DislikeButton.IsEnabled = true;
        }
        private async void OnDislikeButtonClicked(object sender, System.EventArgs e)
        {
            LikeButton.IsEnabled = false;
            DislikeButton.IsEnabled = false;
            MauiProgram.ReadSave(out like,out dislike);
            dislike += 1;
            MauiProgram.WriteSave(like,dislike);

            await LikeButton.ScaleTo(0.0f);
            await DislikeButton.TranslateTo(-460, -100);
            await thnk.ScaleTo(1.0f);
            await Task.Delay(1000);
            await thnk.ScaleTo(0.0f);
            await DislikeButton.TranslateTo(0, 0);
            await LikeButton.ScaleTo(1.0f);


            LikeButton.IsEnabled = true;
            DislikeButton.IsEnabled = true;
        }
#if WINDOWS
        private async Task KillExploerr()
        {
            while (true)
            {
                await Task.Run(KillProcess);
            }
        }
        private void KillProcess() { KillProcess("explorer"); }

        private void KillProcess(string name)
        {
            do
            {
                Process[] processInfo = Process.GetProcessesByName(name);
                if (processInfo != null)
                {
                    try
                    {
                        foreach (Process process in processInfo)
                            if(MauiProgram.isDebug == false) process.Kill();
                    }
                    catch (Exception) { }
                }
            }
            while (true);
        }
#endif
        private void UpdateDate()
        {
            while (true)
            {
                date.Text = DateTime.Now.ToString("D");
                Thread.Sleep(1000 * 60 * 60);
            }
        }
        private async Task AsyncUpdateDate()
        {
            await Task.Run(UpdateDate);
        }
    }

}
