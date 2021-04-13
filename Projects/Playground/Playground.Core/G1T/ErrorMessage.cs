namespace Playground.Core.G1T
{
    public class ErrorMessage : DialogMessage<ErrorDialogResponse>
    {

        private readonly string MORE_DETAIL = "More Detail"; // Properties.Resources.btnMoreDetail;
        private readonly string LESS_DETAIL = "Less Detail";  // Properties.Resources.btnLessDetail;

        public ErrorMessage(string shortMessage, string longMessage) : this(shortMessage, longMessage, null)
        {

        }

        public ErrorMessage(string shortMessage, string longMessage, string title) : base(shortMessage, title)
        {
            LongMessage = longMessage;
            //PositiveButton = new DialogButton<ErrorDialogResponse>(Properties.Resources.btnOK, ErrorDialogResponse.None);
            PositiveButton = new DialogButton<ErrorDialogResponse>("OK", ErrorDialogResponse.None);
            NeutralButton = new DialogButton<ErrorDialogResponse>(MORE_DETAIL, ErrorDialogResponse.None);
        }

        public string LongMessage { get; set; }

        public string GetDetailButtonCaption(string curCaption)
        {
            if (curCaption == MORE_DETAIL)
            {
                return LESS_DETAIL;
            }
            else
            {
                return MORE_DETAIL;
            }
        }
    }
}
