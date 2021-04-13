// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels
{
    public class TabsRootBViewModel : MvxNavigationViewModel
    {
        public TabsRootBViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
            tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
            await Task.WhenAll(tasks);
        }

        private int _itemIndex;

        public int ItemIndex
        {
            get { return _itemIndex; }
            set
            {
                if (_itemIndex == value) return;
                _itemIndex = value;
                Log.Trace("Tab item changed to {0}", _itemIndex.ToString());
                RaisePropertyChanged(() => ItemIndex);
            }
        }

        public override void ViewAppearing()
        {
            Log.Trace($"{nameof(TabsRootBViewModel)}.Begin");
            base.ViewAppearing();
            Log.Trace($"{nameof(TabsRootBViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewAppeared)} Begin");
            base.ViewAppeared();
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewDisappeared)} End");
        }

        public override void ViewCreated()
        {
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewCreated)} Begin");
            base.ViewCreated();
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewCreated)} End");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewDestroy)} Begin, {nameof(viewFinishing)} = {viewFinishing}");
            base.ViewDestroy(viewFinishing);
            Log.Trace($"{nameof(TabsRootBViewModel)}.{nameof(ViewDestroy)} End");
        }
    }
}
