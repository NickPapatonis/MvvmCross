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
    public class TabsRootViewModel : MvxNavigationViewModel
    {
        public TabsRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
            ShowTabsRootBCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootBViewModel>());
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsRootBCommand { get; private set; }

        private async Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
            tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
            tasks.Add(NavigationService.Navigate<Tab3ViewModel>());
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
            Log.Trace($"{nameof(TabsRootViewModel)}.Begin");
            base.ViewAppearing();
            Log.Trace($"{nameof(TabsRootViewModel)}.End");
        }

        public override void ViewAppeared()
        {
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewAppeared)} Begin");
            base.ViewAppeared();
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewAppeared)} End");
        }

        public override void ViewDisappearing()
        {
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewDisappearing)} Begin");
            base.ViewDisappearing();
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewDisappearing)} End");
        }

        public override void ViewDisappeared()
        {
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewDisappeared)} Begin");
            base.ViewDisappeared();
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewDisappeared)} End");
        }

        public override void ViewCreated()
        {
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewCreated)} Begin");
            base.ViewCreated();
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewCreated)} End");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewDestroy)} Begin, {nameof(viewFinishing)} = {viewFinishing}");
            base.ViewDestroy(viewFinishing);
            Log.Trace($"{nameof(TabsRootViewModel)}.{nameof(ViewDestroy)} End");
        }
    }
}
