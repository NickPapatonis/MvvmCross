using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.G1T;

namespace Playground.Core.ViewModels.Tests
{
    public class TabsHostFragmentViewModel : MvxNavigationViewModel, IDelegateViewModel
    {
        public TabsHostFragmentViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public Tab01ViewModel Tab01ViewModel { get; private set; }
        public Tab02ViewModel Tab02ViewModel { get; private set; }
        public Tab03ViewModel Tab03ViewModel { get; private set; }

        public override Task Initialize()
        {
            SetCurrentTabItemReceiver = new SetValueMessageReceiver<int>((msg) => CurrentItem = msg.Value);
            SetExecutingCommandReceiver = new SetValueMessageReceiver<bool>((msg) => _executingCommand = msg.Value);

            SetCountReceiver = new SetValueMessageReceiver<int>();
            GetTextMessageReceiver = new GetTextMessageReceiver();
            GetRevTextMessageReceiver = new GetValueMessageReceiver<string>();
            GetTextLengthMessageReceiver = new GetValueMessageReceiver<int>();

            Tab01ViewModel = Mvx.IoCProvider.IoCConstruct<Tab01ViewModel>();
            Tab01ViewModel.Prepare(new Tab01ViewModelConfig(
                SetCurrentTabItemReceiver.CreateSender(),
                SetCountReceiver,
                GetTextMessageReceiver,
                GetRevTextMessageReceiver,
                GetTextLengthMessageReceiver));

            Tab02ViewModel = Mvx.IoCProvider.IoCConstruct<Tab02ViewModel>();
            Tab02ViewModel.Prepare(new Tab02ViewModelConfig(
                SetCountReceiver.CreateSender(),
                GetTextMessageReceiver.CreateSender(),
                GetRevTextMessageReceiver.CreateSender(),
                GetTextLengthMessageReceiver.CreateSender(),
                SetExecutingCommandReceiver.CreateSender()));

            Tab03ViewModel = Mvx.IoCProvider.IoCConstruct<Tab03ViewModel>();

            return base.Initialize();
        }

        private SetValueMessageReceiver<int> SetCurrentTabItemReceiver { get; set; }
        private SetValueMessageReceiver<bool> SetExecutingCommandReceiver { get; set; }
        private SetValueMessageReceiver<int> SetCountReceiver { get; set; }
        private GetTextMessageReceiver GetTextMessageReceiver { get; set; }
        private GetValueMessageReceiver<string> GetRevTextMessageReceiver { get; set; }
        private GetValueMessageReceiver<int> GetTextLengthMessageReceiver { get; set; }

        private bool _executingCommand;

        public async Task<bool> HandleBackPressed()
        {
            if (_executingCommand) return true;

            var dialogService = Mvx.IoCProvider.Resolve<IDialogService>();
            //if (await dialogService.CanShowAsync())
            if (true)  // Placeholder until wait for view appeared is available in this view model
            {
                var message = new OKCancelMessage("Unsaved changes", "Warning");
                message.PositiveButton.Caption = "Continue working";
                message.NegativeButton.Caption = "Exit";
                Trace($"Dialog: {message.Message}");
                var result = await dialogService.ShowAsync(message);
                return result == SimpleDialogResponse.OK;
                //if (result == SimpleDialogResponse.OK)
                //{
                //    // Continue Working
                //}
                //else
                //{
                //    ShowConfirmBackPressDialog = false;
                //    Log.SystemAction("Navigate Close");
                //    await NavigationService.Close(this);
                //}
            }
        }

        private int _currentItem;
        public int CurrentItem
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                RaisePropertyChanged(() => CurrentItem);
            }
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
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

            SetCountReceiver.Dispose();
            SetCurrentTabItemReceiver.Dispose();
            GetTextMessageReceiver.Dispose();
            GetRevTextMessageReceiver.Dispose();
            GetTextLengthMessageReceiver.Dispose();
            base.ViewDestroy(viewFinishing);

            Trace("End");
        }
    }
}
