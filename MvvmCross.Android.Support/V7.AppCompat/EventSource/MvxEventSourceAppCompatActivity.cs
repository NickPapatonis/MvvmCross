// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat.EventSource
{
    public abstract class MvxEventSourceAppCompatActivity
        : AppCompatActivity, IMvxEventSourceActivity
    {
        protected MvxEventSourceAppCompatActivity()
        {
        }

        protected MvxEventSourceAppCompatActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected override void OnCreate(Bundle bundle)
        {
            Trace("Begin");
            CreateWillBeCalled.Raise(this, bundle);
            base.OnCreate(bundle);
            CreateCalled.Raise(this, bundle);
            Trace("End");
        }

        protected override void OnDestroy()
        {
            Trace("Begin");
            DestroyCalled.Raise(this);
            base.OnDestroy();
            Trace("End");
        }

        protected override void OnNewIntent(Intent intent)
        {
            Trace("Begin");
            base.OnNewIntent(intent);
            NewIntentCalled.Raise(this, intent);
            Trace("End");
        }

        protected override void OnResume()
        {
            Trace("Begin");
            base.OnResume();
            ResumeCalled.Raise(this);
            Trace("End");
        }

        protected override void OnPause()
        {
            Trace("Begin");
            PauseCalled.Raise(this);
            base.OnPause();
            Trace("End");
        }

        protected override void OnStart()
        {
            Trace("Begin");
            base.OnStart();
            StartCalled.Raise(this);
            Trace("End");
        }

        protected override void OnRestart()
        {
            Trace("Begin");
            base.OnRestart();
            RestartCalled.Raise(this);
            Trace("End");
        }

        protected override void OnStop()
        {
            Trace("Begin");
            StopCalled.Raise(this);
            base.OnStop();
            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            MvxAndroidLog.Instance.Trace($"({nameof(MvxEventSourceAppCompatActivity)}) {caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            SaveInstanceStateCalled.Raise(this, outState);
            base.OnSaveInstanceState(outState);
        }

        public override void StartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResultCalled.Raise(this, new MvxStartActivityForResultParameters(intent, requestCode));
            base.StartActivityForResult(intent, requestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            ActivityResultCalled.Raise(this, new MvxActivityResultParameters(requestCode, resultCode, data));
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler DisposeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateWillBeCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> CreateCalled;

        public event EventHandler DestroyCalled;

        public event EventHandler<MvxValueEventArgs<Intent>> NewIntentCalled;

        public event EventHandler ResumeCalled;

        public event EventHandler PauseCalled;

        public event EventHandler StartCalled;

        public event EventHandler RestartCalled;

        public event EventHandler StopCalled;

        public event EventHandler<MvxValueEventArgs<Bundle>> SaveInstanceStateCalled;

        public event EventHandler<MvxValueEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;

        public event EventHandler<MvxValueEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}
