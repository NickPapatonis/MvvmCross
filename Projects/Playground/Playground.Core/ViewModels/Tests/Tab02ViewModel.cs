using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Tests
{
    public class Tab02ViewModel : TksTabBaseViewModel, IMvxViewModel<Tab02ViewModelConfig>
    {
        public Tab02ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new MvxAsyncCommand(HandleCloseCommand);
            UpdateCountCommand = new MvxAsyncCommand(HandleUpdateCountCommand);
            GetTextCommand = new MvxAsyncCommand(HandleGetTextCommand);
            GetRevTextCommand = new MvxAsyncCommand(HandleGetRevTextCommand, () => !_executingHandleGetRevTextCommand);
            GetTextLengthCommand = new MvxAsyncCommand(HandleGetTextLengthCommand);

            //CloseViewModelCommand = new MvxCommand(() => MvxNotifyTask.Create(() => Task.Run(HandleCloseCommand), (ex) => { error(ex); }));
            //CloseViewModelCommand = new MvxCommand(() => MvxNotifyTask.Create(() => HandleCloseCommand(), (ex) => { error(ex); }));
        }

        private int Count { get; set; }
        private Tab02ViewModelConfig ViewModelConfig { get; set; }

        public void Prepare(Tab02ViewModelConfig viewModelConfig)
        {
            ViewModelConfig = viewModelConfig;
        }

        private async Task HandleCloseCommand()
        {
            try
            {
                Trace("Begin");

                // First workflow
                //Message = "Doing some work async, please wait...";
                await DoSomeWork();
                //Message = $"Done doing some work async";

                Trace("(01) Start awaiting ViewAppeared");
                await WaitForViewToAppearAsync();
                Trace("(01) Finished awaiting ViewAppeared");

                Trace($"(01) {nameof(IsViewAppeared)} = {IsViewAppeared}");
                Trace($"(01) {nameof(IsViewDestroyed)} = {IsViewDestroyed}");
                if (IsViewDestroyed)
                {
                    Trace("(01) End - Cancellation Requested");
                    return;
                }

                Trace("(01) Attempting to close view and navigate back");
                var closeResult = await NavigationService.Close(this);
                Trace($"(01) Close result = {closeResult}");

                // Second workflow
                Trace("(02) Start awaiting ViewAppeared");
                await WaitForViewToAppearAsync();
                Trace("(02) Finished awaiting ViewAppeared");

                Trace($"(02) {nameof(IsViewAppeared)} = {IsViewAppeared}");
                Trace($"(02) {nameof(IsViewDestroyed)} = {IsViewDestroyed}");
                if (IsViewDestroyed)
                {
                    Trace("(02) End - Cancellation Requested");
                    return;
                }

                Trace("(02) Attempting to close view and navigate back");
                closeResult = await NavigationService.Close(this);
                Trace($"(02) Close result = {closeResult}");

                Trace("Before GC");
                await Task.Delay(1000);
                GC.Collect();
                await Task.Delay(1000);
                GC.Collect();
                Trace("After GC");

                Trace("End");
            }
            catch (Exception ex)
            {
                Error(ex, nameof(HandleCloseCommand));
            }
        }

        private bool useGoodSender = true;
        private Task HandleUpdateCountCommand()
        {
            Trace("Begin");

            try
            {
                Count += 1;
                Message = $"Count is {Count}";

                if (useGoodSender)
                {
                    ViewModelConfig.SetCountMessageSender.Send(Count);
                }
                else
                {
                    var badSender = new UpdateCountMessageSender(Guid.NewGuid());
                    badSender.Update(Count);
                }

                useGoodSender = !useGoodSender;
                return Task.CompletedTask;
            }
            finally
            {
                Trace("End");
            }
        }

        private async Task HandleGetTextCommand()
        {
            Trace("Begin");

            try
            {
                var text = await ViewModelConfig.GetTextMessageSender.GetAsync();
                Message = text;
            }
            finally
            {
                Trace("End");
            }
        }

        private bool _executingHandleGetRevTextCommand;
        private async Task HandleGetRevTextCommand()
        {
            if (_executingHandleGetRevTextCommand) return;

            Trace("Begin");

            try
            {
                _executingHandleGetRevTextCommand = true;
                ViewModelConfig.SetExecutingCommandSender.Send(true);
                Message = "Getting reverse text from Tab 1...";
                var text = await ViewModelConfig.GetRevTextMessageSender.GetAsync();
                Message = text;
            }
            finally
            {
                ViewModelConfig.SetExecutingCommandSender.Send(false);
                _executingHandleGetRevTextCommand = false;
                Trace("End");
            }
        }

        private async Task HandleGetTextLengthCommand()
        {
            Trace("Begin");

            try
            {
                var length = await ViewModelConfig.GetTextLengthMessageSender.GetAsync();
                Message = $"Text length = {length}";
            }
            finally
            {
                Trace("End");
            }
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }

        private void Error(Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Error($"{nameof(CloseViewModelCommand)} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {exception.Message}\r\n, {exception.StackTrace}");
        }

        public IMvxCommand CloseViewModelCommand { get; private set; }
        public IMvxCommand UpdateCountCommand { get; private set; }
        public IMvxCommand GetTextCommand { get; private set; }
        public IMvxCommand GetRevTextCommand { get; private set; }
        public IMvxCommand GetTextLengthCommand { get; private set; }

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

        private async Task DoSomeWork()
        {
            Trace("Begin");
            await Task.Delay(5000);
            Trace("End");
        }
    }

    public class Tab02ViewModelConfig : IDisposable
    {
        public Tab02ViewModelConfig(
            SetValueMessageSender<int> setCountMessageSender,
            GetTextMessageSender getTextMessageSender,
            GetValueMessageSender<string> getRevTextMessageSender,
            GetValueMessageSender<int> getTextLengthMessageSender,
            SetValueMessageSender<bool> setExecutingCommandSender)
        {
            SetCountMessageSender = setCountMessageSender;
            GetTextMessageSender = getTextMessageSender;
            GetRevTextMessageSender = getRevTextMessageSender;
            GetTextLengthMessageSender = getTextLengthMessageSender;
            SetExecutingCommandSender = setExecutingCommandSender;
        }

        public SetValueMessageSender<int> SetCountMessageSender { get; private set; }
        public GetTextMessageSender GetTextMessageSender { get; private set; }
        public GetValueMessageSender<string> GetRevTextMessageSender { get; private set; }
        public GetValueMessageSender<int> GetTextLengthMessageSender { get; private set; }
        public SetValueMessageSender<bool> SetExecutingCommandSender { get; private set; }

        #region [ IDisposable ]

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    SetCountMessageSender.DisposeIfDisposable();
                    GetTextMessageSender.Dispose();
                    GetRevTextMessageSender.Dispose();
                    GetTextLengthMessageSender.Dispose();
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
