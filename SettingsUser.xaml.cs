using static System.Net.Mime.MediaTypeNames;
using Microcharts;
using SkiaSharp;

namespace ReviewsInTheDiningRoom
{


    public partial class SettingsUser : ContentPage
    {
        public static List<String> SortAscending(List<String> list)
        {
            for (int i = 0; i < list.Count; i++)                                //Удаляем у всех начало строки и добовляем 0
            {
                list[i] = list[i].Remove(0, 7);
                list[i] = list[i].Remove(list[i].Length - 5);

                if (list[i].Split(".")[0].Length == 1) list[i] = $"0{list[i]}";
            }

            list.Sort((a, b) => DateTime.Compare(DateTime.Parse(a), DateTime.Parse(b)));

            list.Reverse();

            return list;
        }

        ChartEntry[] entry;
        int tLike = 0, tDisLike = 0;
        string background = "";
        public SettingsUser()
        {
        #if WINDOWS
            InitializeComponent();
            MauiProgram.ReadSave(out tLike, out tDisLike);
            ChangeChart(tLike, tDisLike);

            string[] files = Directory.GetFiles("./save/", "*.json");

            var f = files.ToList();
            SortAscending(f);
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
                background = button.BackgroundColor.ToHex();
                rightPanel.Children.Add(button);
            }
            dateLabel.Text = DateTime.Now.ToString("D");
        #endif

        }

        private void ChangeChart(int like, int dislike)
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
            donatChart.Chart = new PieChart { Entries = entry, BackgroundColor = SKColor.Parse("#512BD4"), LabelMode = LabelMode.RightOnly};
        }

        public void OnButtonClicked(object sender, System.EventArgs e)
        {
            int like = 0, dislike = 0;
            Button button = (Button)sender;

            dateLabel.Text = button.Text;

            MauiProgram.ReadSave(DateTime.Parse(button.Text).ToString("d"), out like, out dislike);

            ChangeChart(like, dislike);            

        }
    }

}
