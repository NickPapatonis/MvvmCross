namespace Playground.Core.G1T
{
    public class OKCancelMessage : DialogMessage<SimpleDialogResponse>
    {
        public OKCancelMessage(string message) : this(message, null)
        {
        }

        public OKCancelMessage(string message, string title) : base(message, title)
        {
            PositiveButton = new DialogButton<SimpleDialogResponse>("OK", SimpleDialogResponse.OK);
            NegativeButton = new DialogButton<SimpleDialogResponse>("Cancel", SimpleDialogResponse.Cancel);
        }
    }
}
