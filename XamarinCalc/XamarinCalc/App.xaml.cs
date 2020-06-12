using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinCalc.Views;

namespace XamarinCalc
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CalcPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
