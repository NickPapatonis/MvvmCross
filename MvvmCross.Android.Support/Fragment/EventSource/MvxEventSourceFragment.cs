// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Views;

namespace MvvmCross.Droid.Support.V4.EventSource
{
    public class MvxEventSourceFragment
        : Fragment, IMvxEventSourceFragment
    {
        public event EventHandler<MvxValueEventArgs<Context>> AttachCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        public event EventHandler<MvxValueEventArgs<MvxCreateViewParameters>> CreateViewCalled;

        public event EventHandler StartCalled;

        public event EventHandler ResumeCalled;

        public event EventHandler PauseCalled;

        public event EventHandler StopCalled;

        public event EventHandler DestroyViewCalled;

        public event EventHandler DestroyCalled;

        public event EventHandler DetachCalled;

        public event EventHandler DisposeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;

        public MvxEventSourceFragment()
        {
        }

        public MvxEventSourceFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnAttach(Context context)
        {
            Trace("Begin");
            AttachCalled.Raise(this, context);
            base.OnAttach(context);
            Trace("End");
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            Trace("Begin");
            CreateWillBeCalled.Raise(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            CreateCalled.Raise(this, savedInstanceState);
            Trace("End");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                Trace("Begin");
                CreateViewCalled.Raise(this, new MvxCreateViewParameters(inflater, container, savedInstanceState));
                return base.OnCreateView(inflater, container, savedInstanceState);
            }
            finally
            {
                Trace("End");
            }
        }

        public override void OnStart()
        {
            Trace("Begin");
            StartCalled.Raise(this);
            base.OnStart();
            Trace("End");
        }

        public override void OnResume()
        {
            Trace("Begin");
            ResumeCalled.Raise(this);
            base.OnResume();
            Trace("End");
        }

        public override void OnPause()
        {
            Trace("Begin");
            PauseCalled.Raise(this);
            base.OnPause();
            Trace("End");
        }

        public override void OnStop()
        {
            Trace("Begin");
            StopCalled.Raise(this);
            base.OnStop();
            Trace("End");
        }

        public override void OnDestroyView()
        {
            Trace("Begin");
            DestroyViewCalled.Raise(this);
            base.OnDestroyView();
            Trace("End");
        }

        public override void OnDestroy()
        {
            Trace("Begin");
            DestroyCalled.Raise(this);
            base.OnDestroy();
            Trace("End");
        }

        public override void OnDetach()
        {
            Trace("Begin");
            DetachCalled.Raise(this);
            base.OnDetach();
            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            MvxAndroidLog.Instance.Trace($"({nameof(MvxEventSourceFragment)}) {caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }
    }
}
