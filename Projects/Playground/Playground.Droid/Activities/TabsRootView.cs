// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
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

        public override View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            try
            {
                Trace("Begin");
                Trace($"Parent tag: {parent?.Tag ?? "None"}, name: {name}");
                return base.OnCreateView(parent, name, context, attrs);
            }
            finally
            {
                Trace("End");
            }
        }

        protected override void OnStart()
        {
            Trace("Begin");
            base.OnStart();
            Trace("End");
        }

        protected override void OnResume()
        {
            Trace("Begin");
            base.OnResume();
            Trace("End");
        }

        protected override void OnPause()
        {
            Trace("Begin");
            base.OnPause();
            Trace("End");
        }

        protected override void OnStop()
        {
            Trace("Begin");
            base.OnStop();
            Trace("End");
        }

        protected override void OnDestroy()
        {
            Trace("Begin");
            base.OnDestroy();
            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            MvxAndroidLog.Instance.Trace($"({nameof(TabsRootView)}) {caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
