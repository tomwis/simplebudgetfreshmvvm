using FreshMvvm;
using SimpleBudgetFreshMvvm.Pages;
using SimpleBudgetFreshMvvm.PlatformServices;
using SimpleBudgetFreshMvvm.Services;
using SimpleBudgetFreshMvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SimpleBudgetFreshMvvm
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var fs = DependencyService.Get<IFileService>();
            FreshIOC.Container.Register(fs);
            FreshIOC.Container.Register<IDbService, DbService>();
            var page = FreshPageModelResolver.ResolvePageModel<DashboardViewModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
