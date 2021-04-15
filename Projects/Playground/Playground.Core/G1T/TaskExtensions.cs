using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.G1T
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Asynchronously waits for the task to complete, or for the cancellation token to be canceled.
        /// </summary>
        /// <param name="this">The task to wait for. May not be <c>null</c>.</param>
        /// <param name="cancellationToken">The cancellation token that cancels the wait.</param>
        public static async Task WaitAsync(this Task task, CancellationToken cancellationToken)
        {
            var delayTask = Task.Delay(-1, cancellationToken);
            var completedTask = await Task.WhenAny(task, delayTask);
            if (completedTask.IsCanceled)
            {
                await completedTask;
            }
            await task;
        }

        /// <summary>
        /// Asynchronously waits for the task to complete, or for the cancellation token to be canceled.
        /// </summary>
        /// <typeparam name="TResult">The type of the task result.</typeparam>
        /// <param name="task">The task to wait for. May not be <c>null</c>.</param>
        /// <param name="cancellationToken">The cancellation token that cancels the wait.</param>
        public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, CancellationToken cancellationToken)
        {
            var delayTask = Task.Delay(-1, cancellationToken);
            var completedTask = await Task.WhenAny(task, delayTask);
            if (completedTask.IsCanceled)
            {
                await completedTask;
            }
            return await task;
        }
    }
}
