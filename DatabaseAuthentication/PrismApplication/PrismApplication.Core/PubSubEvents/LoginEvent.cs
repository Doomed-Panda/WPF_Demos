using Business.Model;
using Prism.Events;

namespace PrismApplication.Core
{
    public class LoginEvent : PubSubEvent<UserModel>
    {
    }
}