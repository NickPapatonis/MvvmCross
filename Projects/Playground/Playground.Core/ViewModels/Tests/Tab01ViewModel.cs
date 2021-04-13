using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Tests
{
    public class Tab01ViewModel : MvxNavigationViewModel<Tab01ViewModelConfig>
    {
        public Tab01ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public override void Prepare(Tab01ViewModelConfig viewModelConfig)
        {
            ViewModelConfig = viewModelConfig;

            ViewModelConfig.SetCountReceiver.SetHandler(SetCount);
            ViewModelConfig.GetTextMessageReceiver.SetHandler(GetUserTextHandler);
            ViewModelConfig.GetRevTextMessageReceiver.SetAsyncHandler(GetUserRevTextHandler);
            ViewModelConfig.GetTextLengthMessageReceiver.SetHandler(GetUserTextLengthHandler);

            OpenChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            //OpenTab2Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab02ViewModel))));

            OpenTab2Command = new MvxAsyncCommand(() => { ViewModelConfig.SetCurrentTabItemSender.Send(1); return Task.CompletedTask; });
        }

        public override async Task Initialize()
        {
            //await Task.Delay(3000);
        }

        private Tab01ViewModelConfig ViewModelConfig { get; set; }

        public IMvxAsyncCommand OpenChildCommand { get; private set; }

        public IMvxAsyncCommand OpenModalCommand { get; private set; }

        public IMvxAsyncCommand OpenNavModalCommand { get; private set; }

        public IMvxAsyncCommand OpenTab2Command { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }

        public void SetCount(SetValueMessage<int> request)
        {
            Trace("Begin");
            Message = $"Count from Tab 2 is {request.Value}";
            Trace("End");
        }

        private string GetUserTextHandler(GetTextRequestMessage request)
        {
            try
            {
                Trace("Begin", "GetUserTextHandler");
                return UserText;
            }
            finally
            {
                Trace("End", "GetUserTextHandler");
            }
        }

        private async Task<string> GetUserRevTextHandler(GetValueRequestMessage<string> request)
        {
            try
            {
                Trace("Begin", "GetUserRevTextHandler");
                await Task.Delay(4000);
                return new string(UserText.Reverse().ToArray());
            }
            finally
            {
                Trace("End", "GetUserRevTextHandler");
            }
        }

        private int GetUserTextLengthHandler(GetValueRequestMessage<int> request)
        {
            try
            {
                Trace("Begin", "GetUserTextLengthHandler");
                return UserText.Length;
            }
            finally
            {
                Trace("End", "GetUserTextLengthHandler");
            }
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

        private string _userText;
        public string UserText
        {
            get => _userText;
            set
            {
                _userText = value;
                RaisePropertyChanged(() => UserText);
            }
        }

        public override void ViewAppearing()
        {
            Trace("Begin");
            base.ViewAppearing();
            Trace("End");
        }

        public override void ViewAppeared()
        {
            Trace("Begin");
            base.ViewAppeared();
            Trace("End");
        }

        protected override void SaveStateToBundle(IMvxBundle bundle)
        {
            base.SaveStateToBundle(bundle);
        }

        public override void ViewDisappearing()
        {
            Trace("Begin");
            base.ViewDisappearing();
            Trace("End");
        }

        public override void ViewDisappeared()
        {
            Trace("Begin");
            base.ViewDisappeared();
            Trace("End");
        }

        public override void ViewCreated()
        {
            Trace("Begin");
            base.ViewCreated();
            Trace("End");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Trace($"Begin, {nameof(viewFinishing)} = {viewFinishing}");
            ViewModelConfig.Dispose();
            base.ViewDestroy(viewFinishing);
            Trace("End");
        }
    }

    public class Tab01ViewModelConfig : IDisposable
    {
        public Tab01ViewModelConfig(
            SetValueMessageSender<int> setCurrentTabItemSender,
            SetValueMessageReceiver<int> setCountReceiver,
            GetTextMessageReceiver getTextMessageReceiver,
            GetValueMessageReceiver<string> getRevTextMessageReceiver,
            GetValueMessageReceiver<int> getTextLengthMessageReceiver)
        {
            SetCurrentTabItemSender = setCurrentTabItemSender;
            SetCountReceiver = setCountReceiver;
            GetTextMessageReceiver = getTextMessageReceiver;
            GetRevTextMessageReceiver = getRevTextMessageReceiver;
            GetTextLengthMessageReceiver = getTextLengthMessageReceiver;
        }

        public SetValueMessageSender<int> SetCurrentTabItemSender { get; private set; }
        public SetValueMessageReceiver<int> SetCountReceiver { get; private set; }
        public GetTextMessageReceiver GetTextMessageReceiver { get; private set; }
        public GetValueMessageReceiver<string> GetRevTextMessageReceiver { get; private set; }
        public GetValueMessageReceiver<int> GetTextLengthMessageReceiver { get; private set; }

        #region [ IDisposable ]

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    SetCurrentTabItemSender.DisposeIfDisposable();
                    SetCountReceiver.Dispose();
                    GetTextMessageReceiver.Dispose();
                    GetRevTextMessageReceiver.Dispose();
                    GetTextLengthMessageReceiver.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
