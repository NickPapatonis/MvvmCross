// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
    {
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxLog.Instance.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
                else
                    throw exception;
            }
            catch (Exception exception)
            {
                MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxLog.Instance.Warn("Exception masked " + exception.ToLongString());
                else
                    throw exception;
            }
        }

        //public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        //{
        //    Trace("Begin");

        //    try
        //    {
        //        Trace("Before calling action");
        //        action();
        //        Trace("After calling action");
        //    }
        //    catch (TargetInvocationException exception)
        //    {
        //        //MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
        //        ErrorException("Exception throw when invoking action via dispatcher", exception);
        //        if (maskExceptions)
        //            //MvxLog.Instance.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
        //            Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
        //        else
        //            throw exception;
        //    }
        //    catch (Exception exception)
        //    {
        //        //MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
        //        ErrorException("Exception throw when invoking action via dispatcher", exception);
        //        if (maskExceptions)
        //            //MvxLog.Instance.Warn("Exception masked " + exception.ToLongString());
        //            Warn("Exception masked " + exception.ToLongString());
        //        else
        //            throw exception;
        //    }

        //    Trace("End");
        //}

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }

        //protected static readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MvxMainThreadDispatcher>();
        //private static void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        //{
        //    Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}");
        //}
        //private static void ErrorException(string msg, Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        //{
        //    Log.ErrorException($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}", exception);
        //}
        //private static void Warn(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        //{
        //    Log.Warn($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}");
        //}
    }
}
