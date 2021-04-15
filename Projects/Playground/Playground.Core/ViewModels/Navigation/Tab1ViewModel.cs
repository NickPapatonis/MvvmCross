// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab1ViewModel : MvxNavigationViewModel<string>
    {
        public Tab1ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            Trace("Begin");

            OpenChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            OpenTab2Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab2ViewModel))));

            Trace("End");
        }

        public override async Task Initialize()
        {
            Trace("Begin");
            await Task.Delay(3000);
            Trace("End");
        }

        string para;
        public override void Prepare(string parameter)
        {
            Trace("Begin");
            para = parameter;
            Trace("End");
        }

        public IMvxAsyncCommand OpenChildCommand { get; private set; }

        public IMvxAsyncCommand OpenModalCommand { get; private set; }

        public IMvxAsyncCommand OpenNavModalCommand { get; private set; }

        public IMvxAsyncCommand OpenTab2Command { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

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
            base.ViewDestroy(viewFinishing);
            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
