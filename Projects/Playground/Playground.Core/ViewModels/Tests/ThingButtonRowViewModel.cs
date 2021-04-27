using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using Playground.Core.G1T;

namespace Playground.Core.ViewModels.Tests
{
    class ThingButtonRowViewModel : IDashboardButtonRowViewModel
    {
        IMvxNavigationService NavigationService;
        //ISessionService SessionService;
        //ICategoryService CategoryService;
        bool busy;

        public ThingButtonRowViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
            //SessionService = sessionService;
            //CategoryService = categoryService;
        }

        public IDashboardButtonViewModel Button0 => new DashboardButtonViewModel()
        {
            TextFunc = () => "Things",
            NavigateFunc = () => new MvxAsyncCommand(async () =>
            {
                busy = true;
                await DisplayMessage("First Button Pressed");
                busy = false;
            }, () => !busy),
            //IsClickableFunc = () => !busy,
            IsClickableFunc = () => false,
            IconNameFunc = () => null,
        };

        public IDashboardButtonViewModel Button1 => new DashboardButtonViewModel()
        {
            TextFunc = () => string.Format("{0} Observation", "Thing"),
            NavigateFunc = () => new MvxAsyncCommand(async () =>
            {
                busy = true;
                (Button0 as DashboardButtonViewModel).UpdateIsClickable();
                (Button1 as DashboardButtonViewModel).UpdateIsClickable();
                (Button2 as DashboardButtonViewModel).UpdateIsClickable();
                //await Task.Delay(5000);
                await DisplayMessage("Second Button Pressed");
                busy = false;
                (Button0 as DashboardButtonViewModel).UpdateIsClickable();
                (Button1 as DashboardButtonViewModel).UpdateIsClickable();
                (Button2 as DashboardButtonViewModel).UpdateIsClickable();
            }, () => !busy),
            IsClickableFunc = () => !busy,
            IconNameFunc = () => null,
        };

        public IDashboardButtonViewModel Button2 => new DashboardButtonViewModel()
        {
            TextFunc = () => string.Format("{0} Handout", "Thing"),
            NavigateFunc = () => new MvxAsyncCommand(async () =>
            {
                busy = true;
                await DisplayMessage("Third Button Pressed");
                busy = false;
            }, () => !busy),
            IsClickableFunc = () => !busy,
            IconNameFunc = () => null,
        };

        private async Task DisplayMessage(string msg)
        {
            var dialogService = Mvx.IoCProvider.Resolve<IDialogService>();
            var message = new OKCancelMessage(msg, "Message");
            message.PositiveButton.Caption = "Ok";
            message.NegativeButton.Caption = "Cancel";
            var result = await dialogService.ShowAsync(message);
        }
    }
}
