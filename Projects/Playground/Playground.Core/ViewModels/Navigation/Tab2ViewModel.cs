﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using MvvmCross.Base;
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
            Trace("Begin");

            ShowRootViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this));

            Trace("End");
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; private set; }

        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }

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
