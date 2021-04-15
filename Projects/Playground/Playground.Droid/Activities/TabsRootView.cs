// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class TabsRootView : MvxAppCompatActivity<TabsRootViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            Trace("Begin");

            base.OnCreate(bundle);

            Trace("SetContentView");
            SetContentView(Resource.Layout.TabsRootView);

            if (bundle == null)
            {
                Trace("Before executing ViewModel.ShowInitialViewModelsCommand");
                ViewModel.ShowInitialViewModelsCommand.Execute();
                Trace("After executing ViewModel.ShowInitialViewModelsCommand");
            }

            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            MvxAndroidLog.Instance.Trace($"({nameof(TabsRootView)}) {caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
