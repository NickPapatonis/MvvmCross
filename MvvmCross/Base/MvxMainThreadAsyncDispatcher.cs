// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadAsyncDispatcher : MvxMainThreadDispatcher, IMvxMainThreadAsyncDispatcher
    {
        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            var asyncAction = new Func<Task>(() =>
            {
                action();
                return Task.CompletedTask;
            });
            return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
        }

        //public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        //{
        //    var completion = new TaskCompletionSource<bool>();
        //    var syncAction = new Action(async () =>
        //    {
        //        await action();
        //        completion.SetResult(true);
        //    });
        //    RequestMainThreadAction(syncAction, maskExceptions);

        //    // If we're already on main thread, then the action will
        //    // have already completed at this point, so can just return
        //    if (completion.Task.IsCompleted)
        //        return;

        //    // Make sure we don't introduce weird locking issues  
        //    // blocking on the completion source by jumping onto
        //    // a new thread to wait
        //    await Task.Run(async () => await completion.Task);
        //}

        //public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        //{
        //    Trace("Begin");

        //    var asyncAction = new Func<Task>(() =>
        //    {
        //        Trace("Before action", $"{nameof(ExecuteOnMainThreadAsync)}.asyncAction");
        //        action();
        //        Trace("After action", $"{nameof(ExecuteOnMainThreadAsync)}.asyncAction");

        //        Trace("End", $"{nameof(ExecuteOnMainThreadAsync)}.asyncAction");
        //        return Task.CompletedTask;
        //    });

        //    try
        //    {
        //        Trace("Before ExecuteOnMainThreadAsync(asyncAction)");
        //        var x = ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
        //        Trace("After ExecuteOnMainThreadAsync(asyncAction)");

        //        Trace("End");
        //        return x;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorException("Error executing asyncAction with ExecuteOnMainThreadAsync(asyncAction)", ex);
        //        throw;
        //    }
        //}

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            Trace("Begin");

            var completion = new TaskCompletionSource<bool>();
            var syncAction = new Action(async () =>
            {
                try
                {
                    Trace("Awaiting action", $"{nameof(ExecuteOnMainThreadAsync)}.syncAction");
                    await action();
                    Trace("Finished awaiting action", $"{nameof(ExecuteOnMainThreadAsync)}.syncAction");

                    Trace("Setting task completion result to true", $"{nameof(ExecuteOnMainThreadAsync)}.syncAction");
                    completion.SetResult(true);

                    Trace("End", $"{nameof(ExecuteOnMainThreadAsync)}.syncAction");
                }
                catch (Exception ex)
                {
                    ErrorException("Error invoking/awaiting syncAction", ex);
                    throw;
                }

            });
            RequestMainThreadAction(syncAction, maskExceptions);

            // If we're already on main thread, then the action will
            // have already completed at this point, so can just return
            Trace($"completion.Task.IsCompleted = {completion.Task.IsCompleted}");
            if (completion.Task.IsCompleted)
                return;

            // Make sure we don't introduce weird locking issues  
            // blocking on the completion source by jumping onto
            // a new thread to wait
            await Task.Run(async () => await completion.Task);

            Trace("End");
        }

        public abstract override bool IsOnMainThread { get; }

        new protected readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MvxMainThreadAsyncDispatcher>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}");
        }

        private void ErrorException(string msg, Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.ErrorException($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}", exception);
        }
    }
}
