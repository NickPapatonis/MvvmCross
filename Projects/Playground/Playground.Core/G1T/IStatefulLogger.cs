using System;

namespace Playground.Core.G1T
{
    public interface IBaseLogger
    {
        void Verbose(string message, string tag, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void Information(string message, string tag, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void Warning(string message, string tag, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void Error(string message, string tag, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
    }

    public interface ILogger : IBaseLogger
    {
    }

    public interface IStatefulLogger
    {
        // IMPORTANT:
        // Please update the StatefulLoggerBuilder if you add a method to this interface, so that the method logs the output in unit tests.

        ILogger Logger { get; }
        string Tag { get; }

        void Details(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void Enter([System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void EnterDetails(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void Error(Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void ErrorDetails(Exception exception, string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void ErrorDetails(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void Exit([System.Runtime.CompilerServices.CallerMemberName]string caller = null);
        void ExitDetails(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        void GotHere([System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        /// <summary>
        /// Intended to log the processing of an entity event by an interested recipient.
        /// </summary>
        void EntityEvent(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        /// <summary>
        /// Intended to log important state information about the system, a class or method.
        /// </summary>
        void State(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        /// <summary>
        /// Intended to log actions initiated by the system.
        /// </summary>
        void SystemAction(string message = null, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        /// <summary>
        /// Intended to log actions initiated by the user.
        /// </summary>
        void UserAction(string message = null, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        /// <summary>
        /// Intended to log a view lifecycle event.
        /// </summary>
        void ViewLifecycle(string message = null, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        void Warning(string message, [System.Runtime.CompilerServices.CallerMemberName]string caller = null);

        /// <summary>
        /// Additional context to include in the message.
        /// </summary>
        /// <remarks>
        /// Especially useful when there are multiple instances of the same view model, such as for list items.
        /// </remarks>
        IStatefulLogger WithExtraContext(string value);

        /// <summary>
        /// Use during active development to log informational and verbose messages as warnings
        /// in order to reduce the flood of messages to focus on what's important.
        /// </summary>
        IStatefulLogger WithDevelopmentLevel();

        /// <summary>
        /// Sets the logger tag to the type name of this instance.
        /// </summary>
        IStatefulLogger WithTagForInstance(object instance);

    }
}
