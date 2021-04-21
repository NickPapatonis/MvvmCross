// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels;
using Playground.Droid.Activities;

namespace Playground.Droid.Fragments
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 2", ActivityHostViewModelType = typeof(TabsRootViewModel))]
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 2", FragmentHostViewType = typeof(TabsRootBView))]
    [Register(nameof(Tab2View))]
    public class Tab2View : MvxFragment<Tab2ViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            Trace("Begin");

            base.OnCreate(savedInstanceState);

            Trace("End");
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                Trace("Begin");

                base.OnCreateView(inflater, container, savedInstanceState);

                var view = this.BindingInflate(Resource.Layout.Tab2View, null);

                return view;
            }
            finally
            {
                Trace("End");
            }
        }

        public override void OnStart()
        {
            Trace("Begin");
            base.OnStart();
            Trace("End");
        }

        public override void OnResume()
        {
            Trace("Begin");
            base.OnResume();
            Trace("End");
        }

        public override void OnPause()
        {
            Trace("Begin");
            base.OnPause();
            Trace("End");
        }

        public override void OnStop()
        {
            Trace("Begin");
            base.OnStop();
            Trace("End");
        }

        public override void OnDestroy()
        {
            Trace("Begin");
            base.OnDestroy();
            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            MvxAndroidLog.Instance.Trace($"({nameof(Tab2View)}) {caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
