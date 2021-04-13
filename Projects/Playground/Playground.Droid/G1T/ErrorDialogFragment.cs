using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.Core.G1T;
using V4 = Android.Support.V4.App;

namespace Playground.Droid.G1T
{
    public class ErrorDialogFragment : V4.DialogFragment
    {

        private TextView ShortErrorTextView;
        private TextView LongErrorTextView;

        public ErrorDialogFragment(ErrorMessage errorMessage, TaskCompletionSource<ErrorDialogResponse> tcs) : base()
        {
            ErrorMessage = errorMessage;
            TCS = tcs;
        }

        private ErrorMessage ErrorMessage { get; set; }
        private TaskCompletionSource<ErrorDialogResponse> TCS { get; set; }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var builder = new AlertDialog.Builder(Activity);
            var inflater = Activity.LayoutInflater;
            var dialogView = inflater.Inflate(Resource.Layout.FragmentErrorDialog, null);

            ShortErrorTextView = dialogView.FindViewById<TextView>(Resource.Id.shortError);
            ShortErrorTextView.Text = ErrorMessage.Message;

            if (!String.IsNullOrEmpty(ErrorMessage.LongMessage))
            {
                LongErrorTextView = dialogView.FindViewById<TextView>(Resource.Id.longError);
                LongErrorTextView.Text = ErrorMessage.LongMessage;
                var neutralBtn = ErrorMessage.NeutralButton;
                builder.SetNeutralButton(neutralBtn.Caption, (sender, args) => { });
            }

            builder.SetTitle(ErrorMessage.Title);
            builder.SetView(dialogView);

            var positiveBtn = ErrorMessage.PositiveButton;
            builder.SetPositiveButton(positiveBtn.Caption, (sender, args) => { TCS.SetResult(positiveBtn.Response); });

            var dialog = builder.Create();
            dialog.SetCanceledOnTouchOutside(false);

            return dialog;
        }

        public override void OnStart()
        {
            base.OnStart();

            AlertDialog dialog = Dialog as AlertDialog;
            Button neutralButton = dialog.GetButton((int)DialogButtonType.Neutral);
            neutralButton.Click += (object sender, EventArgs args) => {
                OnClickMoreDetail(neutralButton);
            };
        }

        private void OnClickMoreDetail(Button detailBtn)
        {
            detailBtn.Text = ErrorMessage.GetDetailButtonCaption(detailBtn.Text);

            if (LongErrorTextView.Visibility == ViewStates.Visible)
            {
                LongErrorTextView.Visibility = ViewStates.Gone;
            }
            else if (LongErrorTextView.Visibility == ViewStates.Gone)
            {
                LongErrorTextView.Visibility = ViewStates.Visible;
            }
        }
    }
}
