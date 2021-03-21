
using SaintSender.Core.Models;
using SaintSender.Core.Services;

namespace SaintSender.DesktopUI.ViewModels
{
    public class DetailWindowViewModel
    {
        private Email _email;

        public DetailWindowViewModel(Email email)
        {
            _email = email;
        }

        public Email Email
        {
            get => _email;
            set => _email = value;
        }
    }
}