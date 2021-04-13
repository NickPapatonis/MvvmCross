// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Android.App;
using MvvmCross.Base;
using MvvmCross.Logging;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxAndroidMainThreadDispatcher : MvxMainThreadAsyncDispatcher
    {
        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        //public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        //{
        //    if (IsOnMainThread)
        //        ExceptionMaskedAction(action, maskExceptions);
        //    else
        //    {
        //        Application.SynchronizationContext.Post(ignored =>
        //        {
        //            ExceptionMaskedAction(action, maskExceptions);
        //        }, null);
        //    }

        //    return true;
        //}

        public override bool RequestMainThreadAction(Action action, bool maskExceptions = true)
        {
            Trace("Begin");

            if (IsOnMainThread)
                ExceptionMaskedAction(action, maskExceptions);
            else
            {
                Application.SynchronizationContext.Post(ignored =>
                {
                    try
                    {
                        Trace("Begin", $"{nameof(RequestMainThreadAction)}.SynchronizationContext.Post");
                        ExceptionMaskedAction(action, maskExceptions);
                        Trace("End", $"{nameof(RequestMainThreadAction)}.SynchronizationContext.Post");
                    }
                    catch (Exception ex)
                    {
                        ErrorException("Error calling ExceptionMaskedAction in sync context", ex);
                        throw;
                    }
                }, null);
            }

            Trace("End");
            return true;
        }

        new protected readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MvxAndroidMainThreadDispatcher>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}");
        }
        private void ErrorException(string msg, Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.ErrorException($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}", exception);
        }
    }
}
