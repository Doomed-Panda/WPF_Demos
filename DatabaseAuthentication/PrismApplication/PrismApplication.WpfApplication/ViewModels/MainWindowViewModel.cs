using Business.Model;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using PrismApplication.Core;
using PrismApplication.WpfApplication.Views;

namespace PrismApplication.WpfApplication.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private UserModel _currentUser;

        private string _title = "LoB-AuthenticationDemo";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            //Prism Regions View Registration
            _regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(LoginForm));
            _regionManager.RegisterViewWithRegion(RegionNames.MainContentRegion, typeof(ApplicationHome));

            //Prism PubSubEvent Subscriptions
            _eventAggregator.GetEvent<LoginEvent>().Subscribe(NewLoginEventHandler);

            _currentUser = new UserModel();

            //Load LoginForm into MainContentRegion
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, NavigationPaths.LoginForm);
        }

        public void NewLoginEventHandler(UserModel currentUser)
        {
            _currentUser = currentUser;
            _regionManager.RequestNavigate(RegionNames.MainContentRegion, NavigationPaths.ApplicationHome);
        }
    }
}