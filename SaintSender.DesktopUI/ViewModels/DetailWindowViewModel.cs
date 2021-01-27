
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
    }
}