// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class Tab2ViewModel : MvxNavigationViewModel
    {
        public Tab2ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; private set; }

        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }

        public override void ViewAppearing()
        {
            Log.Trace($"{nameof(Tab2ViewModel)}.Begin");
            base.ViewAppearing();
            Log.Trace($"{nameof(Tab2ViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewAppeared)} Begin");
            base.ViewAppeared();
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewDisappeared)} End");
        }

        public override void ViewCreated()
        {
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewCreated)} Begin");
            base.ViewCreated();
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewCreated)} End");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewDestroy)} Begin, {nameof(viewFinishing)} = {viewFinishing}");
            base.ViewDestroy(viewFinishing);
            Log.Trace($"{nameof(Tab2ViewModel)}.{nameof(ViewDestroy)} End");
        }

    }
}
