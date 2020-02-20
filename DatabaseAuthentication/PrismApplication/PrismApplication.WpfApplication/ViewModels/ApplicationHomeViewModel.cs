using Prism.Mvvm;

namespace PrismApplication.WpfApplication.ViewModels
{
    public class ApplicationHomeViewModel : BindableBase
    {
        public string Message { get; set; }

        public ApplicationHomeViewModel()
        {
            Message = "Success!";
        }
    }
}