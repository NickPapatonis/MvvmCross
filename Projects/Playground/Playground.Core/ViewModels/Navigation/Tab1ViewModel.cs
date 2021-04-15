// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
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
            OpenChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            OpenTab2Command = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab2ViewModel))));
        }

        public override async Task Initialize()
        {
            await Task.Delay(3000);
        }

        string para;
        public override void Prepare(string parameter)
        {
            para = parameter;
        }

        public IMvxAsyncCommand OpenChildCommand { get; private set; }

        public IMvxAsyncCommand OpenModalCommand { get; private set; }

        public IMvxAsyncCommand OpenNavModalCommand { get; private set; }

        public IMvxAsyncCommand OpenTab2Command { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public override void ViewAppearing()
        {
            Log.Trace($"{nameof(Tab1ViewModel)}.Begin");
            base.ViewAppearing();
            Log.Trace($"{nameof(Tab1ViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewAppeared)} Begin");
            base.ViewAppeared();
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewDisappeared)} End");
        }

        public override void ViewCreated()
        {
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewCreated)} Begin");
            base.ViewCreated();
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewCreated)} End");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewDestroy)} Begin, {nameof(viewFinishing)} = {viewFinishing}");
            base.ViewDestroy(viewFinishing);
            Log.Trace($"{nameof(Tab1ViewModel)}.{nameof(ViewDestroy)} End");
        }

    }
}
