using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Tests
{
    public class Test01ViewModel : MvxNavigationViewModel
    {
        private bool? _lastCloseResult = null;
        private bool? _lastNavigateResult = null;

        public Test01ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        private IMvxAsyncCommand _closeCommand;
        public IMvxAsyncCommand CloseCommand => _closeCommand ??
        (
            _closeCommand = new MvxAsyncCommand(async () =>
            {
                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(CloseCommand)} Begin");

                Message = "Doing some work async, please wait...";
                await DoSomeWork();
                Message = $"Done doing some work async";

                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(CloseCommand)} Attempting to close view and navigate back");
                _lastCloseResult = await NavigationService.Close(this);
                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(CloseCommand)} Close result = {_lastCloseResult}");

                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(CloseCommand)} End");
            })
        );

        private IMvxAsyncCommand _workCommand;
        public IMvxAsyncCommand WorkCommand => _workCommand ??
        (
            _workCommand = new MvxAsyncCommand(async () =>
            {
                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(WorkCommand)} Begin");

                Message = "Doing some work async, please wait...";
                await DoSomeWork();
                Message = $"Done doing some work async";

                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(WorkCommand)} Attempting to navigate to dialog");
                _lastNavigateResult = await NavigationService.Navigate<Modal01ViewModel>();
                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(WorkCommand)} Navigate result = {_lastNavigateResult}");

                Log.Trace($"{nameof(Test01ViewModel)}.{nameof(WorkCommand)} End");
            })
        );

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public override void ViewAppearing()
        {
            Log.Trace($"{nameof(Test01ViewModel)}.Begin, Last close result = {_lastCloseResult}");
            base.ViewAppearing();
            Log.Trace($"{nameof(Test01ViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(ViewAppeared)} Begin, Last close result = {_lastCloseResult}");
            base.ViewAppeared();
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(ViewDisappeared)} End");
        }

        private async Task DoSomeWork()
        {
            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(DoSomeWork)} Begin");

            await Task.Delay(5000);

            Log.Trace($"{nameof(Test01ViewModel)}.{nameof(DoSomeWork)} End");
        }
    }

}
