using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.G1T;

namespace Playground.Core.ViewModels.Tests
{
    public class SafeNavigationViewModel : MvxNavigationViewModel
    {
        private Func<CancellationToken, Task> _deferredExecFunc;

        public SafeNavigationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        protected async Task TopSafeExec(
            Func<CancellationToken, Task> execFunc,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var canExec = Mvx.IoCProvider.Resolve<IDialogService>().CanShow;
            if (canExec)
            {
                await execFunc(cancellationToken);
            }
            else
            {
                _deferredExecFunc = execFunc;
            }
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            if (_deferredExecFunc != null)
            {
                InvokeOnMainThreadAsync(async () =>
                {
                    await _deferredExecFunc(CancellationToken.None);
                });
            }
        }
    }

    public class Test02ViewModel : SafeNavigationViewModel
    {
        private bool? _lastCloseResult = null;
        private bool? _lastNavigateResult = null;

        public Test02ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        private IMvxAsyncCommand _closeCommand;
        public IMvxAsyncCommand CloseCommand => _closeCommand ??
        (
            _closeCommand = new MvxAsyncCommand(async () =>
            {
                Log.Trace($"{nameof(Test02ViewModel)}.{nameof(CloseCommand)} Begin");

                Message = "Doing some work async, please wait...";
                await DoSomeWork();
                Message = $"Done doing some work async";

                await TopSafeExec(async (ct) =>
                {
                    Log.Trace($"{nameof(Test02ViewModel)}.{nameof(CloseCommand)} Attempting to close view and navigate back");
                    _lastCloseResult = await NavigationService.Close(this);
                    Log.Trace($"{nameof(Test02ViewModel)}.{nameof(CloseCommand)} Close result = {_lastCloseResult}");

                    Log.Trace($"{nameof(Test02ViewModel)}.{nameof(CloseCommand)} End");
                });
            })
        );

        private IMvxAsyncCommand _workCommand;
        public IMvxAsyncCommand WorkCommand => _workCommand ??
        (
            _workCommand = new MvxAsyncCommand(async () =>
            {
                Log.Trace($"{nameof(Test02ViewModel)}.{nameof(WorkCommand)} Begin");

                Message = "Doing some work async, please wait...";
                await DoSomeWork();
                Message = $"Done doing some work async";

                await TopSafeExec(async (ct) =>
                {
                    Log.Trace($"{nameof(Test02ViewModel)}.{nameof(WorkCommand)} Attempting to navigate to dialog");

                    var message = new OKCancelMessage("Press Continue or Exit", "Warning");
                    message.PositiveButton.Caption = "Continue";
                    message.NegativeButton.Caption = "Exit";
                    _lastNavigateResult = null;
                    var result = await Mvx.IoCProvider.Resolve<IDialogService>().ShowAsync(message);
                    _lastNavigateResult = result == SimpleDialogResponse.OK;

                    Log.Trace($"{nameof(Test02ViewModel)}.{nameof(WorkCommand)} Navigate result = {_lastNavigateResult}");

                    Log.Trace($"{nameof(Test02ViewModel)}.{nameof(WorkCommand)} End");
                });
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
            Log.Trace($"{nameof(Test02ViewModel)}.Begin, Last close result = {_lastCloseResult}");
            base.ViewAppearing();
            Log.Trace($"{nameof(Test02ViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(ViewAppeared)} Begin, Last close result = {_lastCloseResult}");
            base.ViewAppeared();
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(ViewDisappeared)} End");
        }

        private async Task DoSomeWork()
        {
            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(DoSomeWork)} Begin");

            await Task.Delay(5000);

            Log.Trace($"{nameof(Test02ViewModel)}.{nameof(DoSomeWork)} End");
        }
    }

}
