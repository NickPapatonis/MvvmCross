using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels.Tests;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation()]
    [Activity(Theme = "@style/AppTheme")]
    public class TabsHostActivityView : MvxAppCompatActivity<TabsHostActivityViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TabsHostActivityView);

            //if (bundle == null)
            //{
            //    ViewModel.ShowInitialViewModelsCommand.Execute();
            //}
        }

        public override async void OnBackPressed()
        {
            var navigateBack = true;

            if (ViewModel is IDelegatingViewModel delegatingViewModel)
            {
                if (await delegatingViewModel.DelegateViewModel.HandleBackPressed())
                {
                    navigateBack = false;
                }
            }

            if (navigateBack) base.OnBackPressed();
        }

        //protected override async void OnPause()
        //{
        //    Trace("Begin");
        //    base.OnPause();
        //    await Task.Delay(3000);
        //    Trace("Simulate Stop/Destroy with Finish");
        //    Finish();
        //    Trace("End");
        //}

        //private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        //{
        //    MvxAndroidLog.Instance.Trace($"({nameof(TabsHostActivityView)}) {caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        //}
    }

    internal static class MvxAndroidLog
    {
        internal static IMvxLog Instance { get; } = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("MvxAndroid");
    }
}
