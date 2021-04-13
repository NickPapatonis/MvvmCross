using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Playground.Core.G1T;
using V4 = Android.Support.V4.App;

namespace Playground.Droid.G1T
{
    public class DialogFragment<TResponse> : V4.DialogFragment
    {

        public DialogFragment(DialogMessage<TResponse> dialogMessage, TaskCompletionSource<TResponse> tcs) : base()
        {
            TCS = tcs;
            DialogMessage = dialogMessage;
        }

        private DialogMessage<TResponse> DialogMessage { get; set; }
        private TaskCompletionSource<TResponse> TCS { get; set; }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var builder = new AlertDialog.Builder(Activity);

            if (DialogMessage.Title == null)
                SetStyle(StyleNoTitle, 0);
            else
                builder.SetTitle(DialogMessage.Title);

            builder.SetMessage(DialogMessage.Message);

            var positiveBtn = DialogMessage.PositiveButton;
            var negativeBtn = DialogMessage.NegativeButton;
            var neutralBtn = DialogMessage.NeutralButton;

            if (positiveBtn != null)
            {
                builder.SetPositiveButton(positiveBtn.Caption, (sender, args) => { TCS.TrySetResult(positiveBtn.Response); });
            }

            if (negativeBtn != null)
            {
                builder.SetNegativeButton(negativeBtn.Caption, (sender, args) => { TCS.TrySetResult(negativeBtn.Response); });
            }

            if (neutralBtn != null)
            {
                builder.SetNeutralButton(neutralBtn.Caption, (sender, args) => { TCS.TrySetResult(neutralBtn.Response); });
            }

            var dialog = builder.Create();
            dialog.SetCanceledOnTouchOutside(false);

            return dialog;
        }
    }
}
