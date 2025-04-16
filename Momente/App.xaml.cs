namespace Momente
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current!.UserAppTheme = Application.Current!.RequestedTheme; 
            InitializeComponent();            
            MainPage = new AppShell();           
        }
    }
}
