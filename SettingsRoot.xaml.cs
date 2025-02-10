using System.Diagnostics;
using Microcharts;
using SkiaSharp;
using static System.Net.Mime.MediaTypeNames;
using Application = Microsoft.Maui.Controls.Application;

namespace ReviewsInTheDiningRoom
{


    public partial class SettingsRoot : ContentPage
    {
        ChartEntry[] entry;
        int like, dislike;
        string curDate = DateTime.Now.ToString("d");
        public SettingsRoot()
        {
        #if WINDOWS
            InitializeComponent();
            curDate = DateTime.Now.ToString("d");
            dateLabel.Text = DateTime.Now.ToString("D");
            MauiProgram.ReadSave(curDate,out like,out dislike);
            ChangeChart();

            string[] files = Directory.GetFiles("./save/", "*.json");

            var f = files.ToList();
            SettingsUser.SortAscending(f);
            files = f.ToArray();

            foreach (var file in files)
            {
                Button button = new Button
                {
                    Text = DateTime.Parse(file).ToString("D"),
                    FontSize = 22,
                    HorizontalOptions = LayoutOptions.Fill,
                    Margin = 10
                };

                button.Clicked += OnButtonClicked;
                rightPanel.Children.Add(button);
            }

            likeCountLabel.Text = $"{like}";
            dislikeCountLabel.Text = $"{dislike}";

#endif

        }

        private void ChangeChart()
        {
            entry = new[] {
                new ChartEntry(like)
                {
                    Label = "Понравилось",
                    ValueLabel = $"{like}",
                    Color = SKColor.Parse("#77d065"),
                    TextColor = SKColors.WhiteSmoke, OtherColor = SKColors.WhiteSmoke, ValueLabelColor = SKColors.WhiteSmoke
                },
                new ChartEntry(dislike)
                {
                    Label = "Не понравилось",
                    ValueLabel = $"{dislike}",
                    Color = SKColor.Parse("#FF1493"),
                    TextColor = SKColors.WhiteSmoke, OtherColor = SKColors.WhiteSmoke, ValueLabelColor = SKColors.WhiteSmoke
                },
            };
            donatChart.Chart = new PieChart { Entries = entry, BackgroundColor = SKColor.Parse("#512BD4"), LabelMode = LabelMode.RightOnly };
        }

        public void OnButtonClicked(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;

            dateLabel.Text = button.Text;
            curDate = DateTime.Parse(button.Text).ToString("d");
            MauiProgram.ReadSave(curDate, out like, out dislike);
            likeCountLabel.Text = $"{like}";
            dislikeCountLabel.Text = $"{dislike}";
            ChangeChart();

        }

        private void ChangedCount()
        {
            likeCountLabel.Text = $"{like}";
            dislikeCountLabel.Text = $"{dislike}";
            MauiProgram.WriteSave(curDate, like, dislike); 
            ChangeChart();
        }

        public void OnButtonLikeAddClicked(object sender, System.EventArgs e) { like++; ChangedCount(); }
        public void OnButtonLikeMinClicked(object sender, System.EventArgs e) { like--; ChangedCount(); }
        public void OnButtonDisLikeAddClicked(object sender, System.EventArgs e) { dislike++; ChangedCount(); }
        public void OnButtonDisLikeMinClicked(object sender, System.EventArgs e) { dislike--; ChangedCount(); }

       public void OnPowerOfClicked(object sender, System.EventArgs e)
        {
            MauiProgram.PowerOff();
        }

    }

}
