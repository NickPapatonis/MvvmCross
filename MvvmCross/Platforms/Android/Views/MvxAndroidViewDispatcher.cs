// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxAndroidViewDispatcher
        : MvxAndroidMainThreadDispatcher
        , IMvxViewDispatcher
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public MvxAndroidViewDispatcher(IMvxAndroidViewPresenter presenter)
        {
            _presenter = presenter;
        }

        public async Task<bool> ShowViewModel(MvxViewModelRequest request)
        {
            await ExecuteOnMainThreadAsync(() => _presenter.Show(request));
            return true;
        }

        //public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        //{
        //    await ExecuteOnMainThreadAsync(() => _presenter.ChangePresentation(hint));
        //    return true;
        //}

        public async Task<bool> ChangePresentation(MvxPresentationHint hint)
        {
            Trace("Begin");
            await ExecuteOnMainThreadAsync(() =>
            {
                Trace("Begin", $"{nameof(ChangePresentation)}.action");

                try
                {
                    Trace("Before invoking presenter's ChangePresentation", $"{nameof(ChangePresentation)}.action");
                    return _presenter.ChangePresentation(hint);
                }
                catch (Exception ex)
                {
                    ErrorException("Error changing presentation", ex);
                    throw;
                }
            });
            Trace("End");
            return true;
        }

        new protected static readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MvxAndroidViewDispatcher>();
        private static void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {Instance.IsOnMainThread}] {msg}");
        }
        private static void ErrorException(string msg, Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.ErrorException($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}", exception);
        }
    }
}
