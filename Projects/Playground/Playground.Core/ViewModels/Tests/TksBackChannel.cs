using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;

namespace Playground.Core.ViewModels.Tests
{
    internal class MessageReceiverBase { }

    public abstract class MessageReceiverBase<TRequest> : IDisposable
        where TRequest : RequestMessage
    {
        #region [ ctor ]

        protected MessageReceiverBase()
        {
            Trace("Begin");

            ReceiverId = Guid.NewGuid();
            Trace($"Receiver Id = {ReceiverId}");

            Trace("End");
        }

        #endregion

        #region [ Private Properties ]

        private MvxSubscriptionToken SubscriptionToken { get; set; }

        #endregion

        #region [ Protected Properties ]

        protected IMvxMessenger Messenger => Mvx.IoCProvider.Resolve<IMvxMessenger>();
        protected Guid ReceiverId { get; private set; }

        #endregion

        #region [ Protected Methods ]

        protected void Subscribe(Action<TRequest> action)
        {
            SubscriptionToken = Messenger.Subscribe(action);
        }

        #endregion

        #region [ IDisposable ]

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            Trace("Begin");

            if (!_disposed)
            {
                if (disposing)
                {
                    if (SubscriptionToken != null)
                    {
                        Trace("Unsubscribe");
                        Messenger.Unsubscribe<TRequest>(SubscriptionToken);
                        Trace("Dispose subscription token");
                        SubscriptionToken.Dispose();
                        SubscriptionToken = null;
                    }
                }
                _disposed = true;
            }

            Trace("End");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MessageReceiverBase>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    internal class MessageReceiver { }

    public abstract class MessageReceiver<TRequest, TMessageSender, TResponse, TResult> : MessageReceiverBase<TRequest>
        where TRequest : RequestResponseMessage<TResult, TResponse>
        where TMessageSender : MessageSender<TRequest, TResult, TResponse>
        where TResponse : ResponseMessage<TResult>
    {
        #region [ ctor ]

        public MessageReceiver()
        {
        }

        public MessageReceiver(Func<TRequest, TResult> func) : base()
        {
            Trace("Begin");
            ShimHandlerAndSubscribe(func);
            Trace("End");
        }

        #endregion

        #region [ Public Methods ]

        public abstract TMessageSender CreateSender();

        public void SetHandler(Func<TRequest, TResult> func)
        {
            Trace("Begin");
            ShimHandlerAndSubscribe(func);
            Trace("End");
        }

        public void SetAsyncHandler(Func<TRequest, Task<TResult>> func)
        {
            Trace("Begin");
            ShimAsyncHandlerAndSubscribe(func);
            Trace("End");
        }

        #endregion

        #region [ Private Methods ]

        private void ShimHandlerAndSubscribe(Func<TRequest, TResult> func)
        {
            Trace("Begin");

            Action<TRequest> shimAction = (msg) =>
            {
                Trace("Begin", "ShimHandlerAndSubscribe.shimAction");
                if (msg.ReceiverId == ReceiverId)
                {
                    Trace("Receiver Id match", "ShimHandlerAndSubscribe.shimAction");

                    var result = func(msg);
                    Trace($"result = {result}", "ShimHandlerAndSubscribe.shimAction");

                    Trace("Creating/publishing response", "ShimHandlerAndSubscribe.shimAction");
                    var response = msg.CreateResponseMessage(this, result);
                    response.ReceiverId = ReceiverId;
                    Messenger.Publish(response);
                }
                else
                {
                    Trace($"Ignoring request for Receiver Id {msg.ReceiverId}", "ShimHandlerAndSubscribe.shimAction");
                }
                Trace("End", "ShimHandlerAndSubscribe.shimAction");
            };
            Subscribe(shimAction);

            Trace("End");
        }

        private void ShimAsyncHandlerAndSubscribe(Func<TRequest, Task<TResult>> func)
        {
            Trace("Begin");

            Action<TRequest> shimAction = async (msg) =>
            {
                Trace("Begin", "ShimHandlerAndSubscribe.shimAction");
                if (msg.ReceiverId == ReceiverId)
                {
                    Trace("Receiver Id match", "ShimHandlerAndSubscribe.shimAction");

                    var result = await func(msg);
                    Trace($"result = {result}", "ShimHandlerAndSubscribe.shimAction");

                    Trace("Creating/publishing response", "ShimHandlerAndSubscribe.shimAction");
                    var response = msg.CreateResponseMessage(this, result);
                    response.ReceiverId = ReceiverId;
                    Messenger.Publish(response);
                }
                else
                {
                    Trace($"Ignoring request for Receiver Id {msg.ReceiverId}", "ShimHandlerAndSubscribe.shimAction");
                }
                Trace("End", "ShimHandlerAndSubscribe.shimAction");
            };
            Subscribe(shimAction);

            Trace("End");
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MessageReceiver>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public abstract class MessageReceiver<TRequest, TMessageSender> : MessageReceiverBase<TRequest>
        where TRequest : RequestMessage
        where TMessageSender : MessageSender<TRequest>
    {
        #region [ ctor ]

        public MessageReceiver()
        {
        }

        protected MessageReceiver(Action<TRequest> action)
        {
            Trace("Begin");
            ShimHandlerAndSubscribe(action);
            Trace("End");
        }

        #endregion

        #region [ Public Methods ]

        public abstract TMessageSender CreateSender();

        public void SetHandler(Action<TRequest> action)
        {
            Trace("Begin");
            ShimHandlerAndSubscribe(action);
            Trace("End");
        }

        #endregion

        #region [ Private Methods ]

        private void ShimHandlerAndSubscribe(Action<TRequest> action)
        {
            Trace("Begin");

            Action<TRequest> shimAction = (msg) =>
            {
                Trace("Begin", "ShimHandlerAndSubscribe.shimAction");
                if (msg.ReceiverId == ReceiverId)
                {
                    Trace("Receiver Id match", "ShimHandlerAndSubscribe.shimAction");
                    action(msg);
                }
                else
                {
                    Trace($"Ignoring request for Receiver Id {msg.ReceiverId}", "ShimHandlerAndSubscribe.shimAction");
                }
                Trace("End", "ShimHandlerAndSubscribe.shimAction");
            };
            Subscribe(shimAction);

            Trace("End");
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MessageReceiver>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public abstract class MessageSender
    {
        #region [ ctor ]

        public MessageSender(Guid receiverId)
        {
            ReceiverId = receiverId;
        }

        #endregion

        #region [ Protected Properties ]

        protected Guid ReceiverId { get; set; }
        protected IMvxMessenger Messenger => Mvx.IoCProvider.Resolve<IMvxMessenger>();

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public abstract class MessageSender<TRequest> : MessageSender
        where TRequest : RequestMessage
    {
        #region [ ctor ]

        public MessageSender(Guid receiverId) : base(receiverId)
        {
        }

        #endregion

        #region [ Protecte Methods ]

        protected void SendRequest(TRequest request)
        {
            Trace("Begin");
            request.ReceiverId = ReceiverId;
            Messenger.Publish(request);
            Trace("End");
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public abstract class MessageSender<TRequest, TResult, TResponse> : MessageSender, IDisposable
        where TRequest : RequestMessage
        where TResponse : ResponseMessage<TResult>
    {
        #region [ ctor ]

        public MessageSender(Guid receiverId) : base(receiverId)
        {
        }

        #endregion

        #region [ Private Properties ]

        private MvxSubscriptionToken SubscriptionToken { get; set; }
        private TaskCompletionSource<TResult> ResultTaskCompletionSource { get; set; }

        #endregion

        #region [ Private Methods ]

        private void Subscribe(Action<TResponse> action)
        {
            SubscriptionToken = Messenger.Subscribe(action);
        }

        #endregion

        #region [ Protected Methods ]

        protected async Task<TResult> SendRequestAsync(TRequest request)
        {
            try
            {
                Trace("Begin");

                if (ResultTaskCompletionSource != null && !ResultTaskCompletionSource.Task.IsCompleted)
                {
                    throw new InvalidOperationException("Still awaiting previous result");
                }

                Trace("Create new task completion source");
                ResultTaskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

                Trace("Prepare response callback action");
                Action<TResponse> responseAction = (response) =>
                {
                    if (response.ReceiverId == ReceiverId)
                    {
                        Trace("Received response message", "SendResultAsync.responseAction");
                        ResultTaskCompletionSource.TrySetResult(response.Result);
                    }
                    else
                    {
                        Trace($"Ignoring response from Receiver Id {response.ReceiverId}", "SendResultAsync.shimAction");
                    }
                };
                Trace("Subscribe for response message");
                Subscribe(responseAction);

                request.ReceiverId = ReceiverId;
                Messenger.Publish(request);

                // TODO: DETERMINE IF WE WANT TO AWAIT HERE
                return await ResultTaskCompletionSource.Task;
            }
            finally
            {
                Trace("End");
            }
        }

        #endregion

        #region [ IDisposable ]

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            Trace("Begin");

            if (!_disposed)
            {
                if (disposing)
                {
                    if (SubscriptionToken != null)
                    {
                        Trace("Unsubscribe");
                        Messenger.Unsubscribe<TRequest>(SubscriptionToken);
                        Trace("Dispose subscription token");
                        SubscriptionToken.Dispose();
                        SubscriptionToken = null;
                    }
                }
                _disposed = true;
            }

            Trace("End");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public abstract class IdentifiableMessage : MvxMessage
    {
        public IdentifiableMessage(object sender) : base(sender)
        {
        }

        public Guid ReceiverId { get; set; }
    }

    public abstract class RequestMessage : IdentifiableMessage
    {
        public RequestMessage(object sender) : base(sender)
        {
        }
    }

    public abstract class ResponseMessage<TResult> : IdentifiableMessage
    {
        public ResponseMessage(object sender, TResult result) : base(sender)
        {
            Result = result;
        }

        public TResult Result { get; private set; }
    }

    public abstract class RequestResponseMessage : RequestMessage
    {
        public RequestResponseMessage(object sender) : base(sender)
        {
        }
    }

    public abstract class RequestResponseMessage<TResult, TResponse> : RequestResponseMessage
        where TResponse : ResponseMessage<TResult>
    {
        public RequestResponseMessage(object sender) : base(sender)
        {
        }

        public abstract TResponse CreateResponseMessage(object sender, TResult result);
    }

    public class UpdateCountMessage : RequestMessage
    {
        public UpdateCountMessage(object sender, int count) : base(sender)
        {
            Count = count;
        }

        public int Count { get; private set; }
    }

    public class UpdateCountMessageReceiver : MessageReceiver<UpdateCountMessage, UpdateCountMessageSender>
    {
        public UpdateCountMessageReceiver(Action<UpdateCountMessage> action) : base(action)
        {
        }

        public override UpdateCountMessageSender CreateSender()
        {
            try
            {
                Trace("Begin");
                return new UpdateCountMessageSender(ReceiverId);
            }
            finally
            {
                Trace("End");
            }
        }

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<UpdateCountMessageReceiver>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public class UpdateCountMessageSender : MessageSender<UpdateCountMessage>
    {
        #region [ ctor ]

        public UpdateCountMessageSender(Guid receiverId) : base(receiverId)
        {
        }

        #endregion

        #region [ Public Methods ]

        public void Update(int count)
        {
            Trace("Begin");
            SendRequest(new UpdateCountMessage(this, count));
            Trace("End");
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<UpdateCountMessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public class GetTextRequestMessage : RequestResponseMessage<string, GetTextResponseMessage>
    {
        public GetTextRequestMessage(object sender) : base(sender)
        {
        }

        public override GetTextResponseMessage CreateResponseMessage(object sender, string result)
        {
            return new GetTextResponseMessage(sender, result);
        }
    }

    public class GetTextResponseMessage : ResponseMessage<string>
    {
        public GetTextResponseMessage(object sender, string text) : base(sender, text)
        {
        }
    }

    public class GetTextMessageReceiver : MessageReceiver<GetTextRequestMessage, GetTextMessageSender, GetTextResponseMessage, string>
    {
        public GetTextMessageReceiver()
        {
        }

        public GetTextMessageReceiver(Func<GetTextRequestMessage, string> func) : base(func)
        {
        }

        public override GetTextMessageSender CreateSender()
        {
            try
            {
                Trace("Begin");
                return new GetTextMessageSender(ReceiverId);
            }
            finally
            {
                Trace("End");
            }
        }

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<GetTextMessageReceiver>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public class GetTextMessageSender : MessageSender<GetTextRequestMessage, string, GetTextResponseMessage>
    {
        #region [ ctor ]

        public GetTextMessageSender(Guid receiverId) : base(receiverId)
        {
        }

        #endregion

        #region [ Public Methods ]

        public Task<string> GetAsync()
        {
            try
            {
                Trace("Begin");
                return SendRequestAsync(new GetTextRequestMessage(this));
            }
            finally
            {
                Trace("End");
            }
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<GetTextMessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public class GetValueRequestMessage<TValue> : RequestResponseMessage<TValue, GetValueResponseMessage<TValue>>
    {
        public GetValueRequestMessage(object sender) : base(sender)
        {
        }

        public override GetValueResponseMessage<TValue> CreateResponseMessage(object sender, TValue value)
        {
            return new GetValueResponseMessage<TValue>(sender, value);
        }
    }

    public class GetValueResponseMessage<TValue> : ResponseMessage<TValue>
    {
        public GetValueResponseMessage(object sender, TValue value) : base(sender, value)
        {
        }
    }

    internal class GetValueMessageReceiver { }
    public class GetValueMessageReceiver<TValue> : MessageReceiver<GetValueRequestMessage<TValue>, GetValueMessageSender<TValue>, GetValueResponseMessage<TValue>, TValue>
    {
        public GetValueMessageReceiver() : base()
        {
        }

        public GetValueMessageReceiver(Func<GetValueRequestMessage<TValue>, TValue> func) : base(func)
        {
        }

        public override GetValueMessageSender<TValue> CreateSender()
        {
            try
            {
                Trace("Begin");
                return new GetValueMessageSender<TValue>(ReceiverId);
            }
            finally
            {
                Trace("End");
            }
        }

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<GetValueMessageReceiver>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    internal class GetValueMessageSender { }
    public class GetValueMessageSender<TValue> : MessageSender<GetValueRequestMessage<TValue>, TValue, GetValueResponseMessage<TValue>>
    {
        #region [ ctor ]

        public GetValueMessageSender(Guid receiverId) : base(receiverId)
        {
        }

        #endregion

        #region [ Public Methods ]

        public Task<TValue> GetAsync()
        {
            try
            {
                Trace("Begin");
                return SendRequestAsync(new GetValueRequestMessage<TValue>(this));
            }
            finally
            {
                Trace("End");
            }
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<GetValueMessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    public class SetValueMessage<TValue> : RequestMessage
    {
        public SetValueMessage(object sender, TValue value) : base(sender)
        {
            Value = value;
        }

        public TValue Value { get; private set; }
    }

    internal class SetValueMessageReceiver { }
    public class SetValueMessageReceiver<TValue> : MessageReceiver<SetValueMessage<TValue>, SetValueMessageSender<TValue>>
    {
        public SetValueMessageReceiver() : base()
        {
        }

        public SetValueMessageReceiver(Action<SetValueMessage<TValue>> action) : base(action)
        {
        }

        public override SetValueMessageSender<TValue> CreateSender()
        {
            try
            {
                Trace("Begin");
                return new SetValueMessageSender<TValue>(ReceiverId);
            }
            finally
            {
                Trace("End");
            }
        }

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<SetValueMessageReceiver>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }

    internal class SetValueMessageSender { }
    public class SetValueMessageSender<TValue> : MessageSender<SetValueMessage<TValue>>
    {
        #region [ ctor ]

        public SetValueMessageSender(Guid receiverId) : base(receiverId)
        {
        }

        #endregion

        #region [ Public Methods ]

        public void Send(TValue value)
        {
            Trace("Begin");
            SendRequest(new SetValueMessage<TValue>(this, value));
            Trace("End");
        }

        #endregion

        private readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<SetValueMessageSender>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
