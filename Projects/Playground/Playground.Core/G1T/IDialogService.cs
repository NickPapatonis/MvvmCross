using System;
using System.Threading.Tasks;

namespace Playground.Core.G1T
{
    public interface IDialogService
    {
        /// <summary>
        /// Indicates if there is currently a view that can be used to show a dialog.
        /// </summary>
        bool CanShow { get; }

        void Show(ToastMessage popUpMessage);

        /// <summary>
        /// Shows the dialog and returns the response.
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="dialogMessage"></param>
        /// <param name="viewModelType">The expected view model type.  It tries to wait for this view model to
        /// be the top activity.  When navigating, there seems to be a brief delay in making the new activity
        /// the top activity. Use when displaying a dialog during view model startup.</param>
        /// <returns></returns>
        Task<TResponse> ShowAsync<TResponse>(DialogMessage<TResponse> dialogMessage, Type viewModelType = null);
    }
}
