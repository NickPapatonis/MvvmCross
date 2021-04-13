using System;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android;
using Playground.Core.G1T;
using V4 = Android.Support.V4.App;

namespace Playground.Droid.G1T
{
    public class DialogService : IDialogService
    {
        protected readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<DialogService>();

        #region =====[ ctor ]==========================================================================

        public DialogService()
        {
        }

        #endregion

        Activity TryGetTopActivity()
        {
            try
            {
                IMvxAndroidCurrentTopActivity currentTopActivity = null;
                Activity resultActivity = null;

                bool success = Mvx.IoCProvider.TryResolve<IMvxAndroidCurrentTopActivity>(out currentTopActivity);
                if (success && currentTopActivity != null)
                {
                    resultActivity = currentTopActivity.Activity;
                }
                if (resultActivity == null)
                {
                    Log.Error("Current top activity is null");
                }
                return resultActivity;
            }
            catch (Exception ex)
            {
                Log.ErrorException("Error", ex);
                return null;
            }
        }

        #region =====[ IDialogService ]======================================================================

        public bool CanShow
        {
            get
            {
                var activity = TryGetTopActivity() as MvxAppCompatActivity;
                return activity != null;
            }
        }

        public void Show(ToastMessage toastMessage)
        {
            var activity = TryGetTopActivity();
            if (activity != null)
            {
                activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(activity.ApplicationContext, toastMessage.Message, toastMessage.LongMessage ? ToastLength.Long : ToastLength.Short).Show();
                });
            }
        }

        public Task<TResponse> ShowAsync<TResponse>(DialogMessage<TResponse> dialogMessage, Type viewModelType = null)
        {
            if (dialogMessage is ErrorMessage)
            {
                return ShowDialogMessageAsync<ErrorDialogResponse>((tcs) => new ErrorDialogFragment(dialogMessage as ErrorMessage, tcs), viewModelType) as Task<TResponse>;
            }
            //else if (dialogMessage is TimePickerRequest)
            //{
            //    return ShowDialogMessageAsync<TimeSpan?>((tcs) => new TimePickerDialogFragment(dialogMessage as TimePickerRequest, tcs), viewModelType) as Task<TResponse>;
            //}
            //else if (dialogMessage is DatePickerRequest)
            //{
            //    return ShowDialogMessageAsync<DateTimeOffset?>((tcs) => new DatePickerDialogFragment(dialogMessage as DatePickerRequest, tcs), viewModelType) as Task<TResponse>;
            //}
            else
            {
                return ShowDialogMessageAsync<TResponse>((tcs) => new DialogFragment<TResponse>(dialogMessage, tcs), viewModelType);
            }
        }

        private Task<TResponse> ShowDialogMessageAsync<TResponse>(
            Func<TaskCompletionSource<TResponse>, V4.DialogFragment> CreateDialogFragment,
            Type viewModelType = null)
        {
            // We aren't using TryGetTopActivity to safetly retrieve the top activity here
            // We would rather crash than try and handle cancelling the dialog and handling that throughout the app
            // If a ShowAsync crash is occuring due to the top activity missing, the root cause of that issue should
            // be the target of a fix

            var tcs = new TaskCompletionSource<TResponse>();
            var topActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
            var activity = topActivity.Activity as MvxAppCompatActivity;

            activity.RunOnUiThread(async () =>
            {
                if (viewModelType != null && activity.ViewModel?.GetType() != viewModelType)
                {
                    // Try to wait for the expected view model to be the top activity.
                    // When navigating, there seems to be a brief delay in making the new activity the top activity.
                    for (int i = 0; i < 3; i++)
                    {
                        await Task.Delay(100);
                        topActivity = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
                        activity = topActivity.Activity as MvxAppCompatActivity;
                        if (activity.ViewModel?.GetType() == viewModelType)
                        {
                            break;
                        }
                    }
                    if (activity.ViewModel?.GetType() != viewModelType)
                    {
                        Log.Warn($"Unexpected view model: '{activity.ViewModel?.GetType().Name}'");
                    }
                }
                var dialogFragment = CreateDialogFragment(tcs);
                dialogFragment.Show(activity.SupportFragmentManager, "dialogfragment");
            });
            return tcs.Task;
        }

        #endregion

    }
}
