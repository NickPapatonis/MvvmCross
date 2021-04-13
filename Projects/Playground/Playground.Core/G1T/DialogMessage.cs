namespace Playground.Core.G1T
{
    public class DialogMessage<TResponse> : UIMessage
    {
        public DialogMessage(string message, string title) : base(message)
        {
            Title = title;
        }

        public string Title { get; set; }
        public DialogButton<TResponse> PositiveButton { get; set; }
        public DialogButton<TResponse> NegativeButton { get; set; }
        public DialogButton<TResponse> NeutralButton { get; set; }
    }
}
