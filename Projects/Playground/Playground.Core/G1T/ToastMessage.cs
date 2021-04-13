namespace Playground.Core.G1T
{
    public class ToastMessage : UIMessage
    {
        public ToastMessage(string message) : base(message)
        {
        }

        public ToastMessage(string message, bool longMessage) : base(message)
        {
            LongMessage = longMessage;
        }

        public bool LongMessage { get; set; } = false;
    }
}
