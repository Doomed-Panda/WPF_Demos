using Business.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using PrismApplication.Core;
using System.Windows.Controls;

namespace PrismApplication.WpfApplication.ViewModels
{
    public class LoginFormViewModel : BindableBase
    {
        private readonly IAuthenticationContext _authenticationContext;
        private readonly IEventAggregator _eventAggregator;

        private string _username;

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public DelegateCommand<PasswordBox> LoginCommand { get; set; }

        //TODO: Add temporary lockout after n failed attempts in x time
        private void ExecuteLogin(PasswordBox passwordBoxParameter)
        {
            //TODO: Validation rules (null, whitespace, required lengths)
            if (Username.Length == 0 || passwordBoxParameter.SecurePassword.Length == 0)
            {
                return;
            }
            //AuthenticationContext will return default UserModel if authentication fails
            UserModel currentUser = _authenticationContext.AuthenticateUser(Username, passwordBoxParameter.SecurePassword);

            if (currentUser.IsAuthenticated)
            {
                _eventAggregator.GetEvent<LoginEvent>().Publish(currentUser);
            }
            else
            {
                //TODO: Add error "Could not verify login credentials"
                Username = "";
                passwordBoxParameter.Clear();
            }
        }

        public LoginFormViewModel(IAuthenticationContext authenticationContext, IEventAggregator eventAggregator)
        {
            _authenticationContext = authenticationContext;
            _eventAggregator = eventAggregator;

            LoginCommand = new DelegateCommand<PasswordBox>(ExecuteLogin);
        }
    }
}