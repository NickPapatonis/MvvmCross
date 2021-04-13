using MvvmCross;
using MvvmCross.Plugin.Messenger;
using System;
using System.Threading.Tasks;

namespace Playground.Core.G1T
{
    public class MessageEvent<TMessage> : ManualResetEventEx, IDisposable
      where TMessage : MvxMessage
    {
        #region =====[ ctor ]==========================================================================================

        public MessageEvent()
          : this(null, false)
        {
        }

        public MessageEvent(bool set)
          : this(null, set)
        {
        }

        public MessageEvent(IMvxMessenger messenger)
          : this(messenger, false)
        {
        }

        public MessageEvent(IMvxMessenger messenger, bool set)
          : base(set)
        {
            Messenger = messenger ?? Mvx.IoCProvider.Resolve<IMvxMessenger>();
            SubscriptionToken = Messenger.SubscribeOnThreadPoolThread<TMessage>(HandleMessage);

            Message = null;
        }

        #endregion

        #region =====[ Private Properties ]============================================================================

        private IMvxMessenger Messenger { get; set; }
        private MvxSubscriptionToken SubscriptionToken { get; set; }

        #endregion

        #region =====[ Public Properties ]=============================================================================

        public TMessage Message { get; private set; }

        #endregion

        #region =====[ Protected Methods ]=============================================================================

        protected virtual Task AfterSetEvent(TMessage msg) { return Task.CompletedTask; }
        protected virtual Task BeforeSetEvent(TMessage msg) { return Task.CompletedTask; }

        #endregion

        #region =====[ Private Methods ]===============================================================================

        private async void HandleMessage(TMessage msg)
        {
            Message = msg;

            try { await BeforeSetEvent(msg).ConfigureAwait(false); }
            catch { }

            Set();

            try { await AfterSetEvent(msg).ConfigureAwait(false); }
            catch { }
        }

        #endregion

        #region =====[ IDisposable ]===================================================================================

        private bool m_disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    SubscriptionToken.Dispose();
                }
                m_disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
