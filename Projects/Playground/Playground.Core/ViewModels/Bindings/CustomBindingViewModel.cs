using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Bindings
{
    public class CustomBindingViewModel
        : MvxNavigationViewModel
    {
        private bool? _lastCloseResult = null;
        private bool? _lastNavigateResult = null;

        public CustomBindingViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }

        private string _hello = "Hello MvvmCross";
        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        private IMvxAsyncCommand _closeCommand;
        public IMvxAsyncCommand CloseCommand => _closeCommand ??
        (
            _closeCommand = new MvxAsyncCommand(async () =>
            {
                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(CloseCommand)} Begin");

                Message = "Doing some work async, please wait...";
                var tip = await DoSomeWork();
                Message = $"Done doing some work async";

                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(CloseCommand)} Attempting to close view and navigate back");
                _lastCloseResult = await NavigationService.Close(this);
                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(CloseCommand)} Close result = {_lastCloseResult}");

                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(CloseCommand)} End");
            })
        );

        private IMvxAsyncCommand _workCommand;
        public IMvxAsyncCommand WorkCommand => _workCommand ??
        (
            _workCommand = new MvxAsyncCommand(async () =>
            {
                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(WorkCommand)} Begin");

                Message = "Doing some work async, please wait...";
                var tip = await DoSomeWork();
                Message = $"Done doing some work async";

                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(WorkCommand)} Attempting to navigate to dialog");
                _lastNavigateResult = await NavigationService.Navigate<ModalViewModel>();
                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(WorkCommand)} Navigate result = {_lastNavigateResult}");

                Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(WorkCommand)} End");
            })
        );

        private int _counter = 2;
        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

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
            Log.Trace($"{nameof(CustomBindingViewModel)}.Begin, Last close result = {_lastCloseResult}");
            base.ViewAppearing();
            Log.Trace($"{nameof(CustomBindingViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(ViewAppeared)} Begin, Last close result = {_lastCloseResult}");
            base.ViewAppeared();
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(ViewDisappeared)} End");
        }

        private async Task<double> DoSomeWork()
        {
            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(DoSomeWork)} Begin");

            await Task.Delay(5000);
            //var tip = _calculationService.TipAmount(SubTotal, Generosity);
            var tip = 3.33;

            Log.Trace($"{nameof(CustomBindingViewModel)}.{nameof(DoSomeWork)} End");
            return tip;
        }
    }
}
