namespace Playground.Core.G1T
{
    public class DialogButton<TResponse>
    {

        public DialogButton(string caption, TResponse response)
        {
            Caption = caption;
            Response = response;
        }

        public string Caption { get; set; }
        public TResponse Response { get; set; }
    }
}
