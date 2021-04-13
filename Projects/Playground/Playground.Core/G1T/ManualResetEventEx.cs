using System;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.G1T
{
    public class ManualResetEventEx
    {
        #region =====[ Private Fields ]================================================================================

        private readonly object m_mutex;
        private TaskCompletionSource<object> m_tcs;
        private int m_setCount;

        #endregion

        #region =====[ ctor ]==========================================================================================

        public ManualResetEventEx()
          : this(false)
        {
        }

        public ManualResetEventEx(bool set)
        {
            m_mutex = new object();
            m_setCount = 0;

            Reset();
            if (set) Set();
        }

        #endregion

        #region =====[ Public Methods ]================================================================================

        public void Reset()
        {
            lock (m_mutex)
            {
                TryReset(m_setCount);
            }
        }

        public void Set()
        {
            lock (m_mutex)
            {
                m_tcs.TrySetResult(null);
                m_setCount++;
            }
        }

        public int SetCount
        {
            get
            {
                lock (m_mutex)
                {
                    return m_setCount;
                }
            }
        }

        public bool TryReset(int testCount)
        {
            var result = false;

            lock (m_mutex)
            {
                if (m_setCount == testCount)
                {
                    if (m_tcs == null || m_tcs.Task.IsCompleted)
                    {
                        m_tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
                    }
                    m_setCount = 0;
                    result = true;
                }
            }

            return result;
        }

        public Task WaitAsync()
        {
            lock (m_mutex)
            {
                return m_tcs.Task;
            }
        }

        public async Task WaitAsync(CancellationToken cancellationToken)
        {
            var waitTask = WaitAsync();
            if (waitTask.IsCompleted) return;

            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(
              s => ((TaskCompletionSource<object>)s).TrySetCanceled(cancellationToken),
              tcs))
            {
                if (waitTask != await Task.WhenAny(waitTask, tcs.Task).ConfigureAwait(false))
                    throw new OperationCanceledException(cancellationToken);

                await waitTask.ConfigureAwait(false);
            }
        }

        #endregion
    }
}
