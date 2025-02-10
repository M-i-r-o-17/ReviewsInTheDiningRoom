using static System.Net.Mime.MediaTypeNames;

namespace ReviewsInTheDiningRoom
{


    public partial class Settings : ContentPage
    {
        private string password = "";
        public Settings()
        {
        #if WINDOWS
                    InitializeComponent();
        #endif
            password = "";
        }

        private async void OnButtonClicked(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;

            if(button.Text == "enter")
            {
                int acces = MauiProgram.AccesUser(password);
                password = "";
                passwordLabel.Text = "";

                switch(acces)
                {
                    case 0: await Navigation.PushAsync(new SettingsRoot()); break; //Уровень доступа Администратор
                    case 1: await Navigation.PushAsync(new SettingsUser()); break; //Уровень доступа Пользователь
                    case 2: 
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    default: await Shell.Current.GoToAsync("//MainPage"); break;
                }
            }
            else if(button.Text == "c")
            {
                password = password.Remove(password.Length - 1);
                passwordLabel.Text = passwordLabel.Text.Remove(passwordLabel.Text.Length - 1);
            }
            else if(password.Length + 1 <= 8)
            {
                password += button.Text;
                passwordLabel.Text += MauiProgram.isDebug ? button.Text : "*";
            }

        }


    }

}
