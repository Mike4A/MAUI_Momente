using System.Threading.Tasks;

namespace Momente
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void AddMomentButton_Clicked(object sender, EventArgs e)
        {
            //testing
            await Navigation.PushModalAsync(new MomentPage(0, false));
        }
    }
}
