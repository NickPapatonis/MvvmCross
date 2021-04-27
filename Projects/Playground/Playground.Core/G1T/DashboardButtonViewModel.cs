using MvvmCross.ViewModels;
using System;
using System.Windows.Input;

namespace Playground.Core.G1T
{
    public class DashboardButtonViewModel : MvxViewModel, IDashboardButtonViewModel
    {
        public DashboardButtonViewModel() { }

        #region Default View Property Functions

        /// <summary>
        /// Underlying function which is called when getting the Text property. Overridable.
        /// </summary>
        public Func<string> TextFunc = () => string.Empty;

        /// <summary>
        /// Underlying function which is called when getting the Navigate property. Overridable.
        /// </summary>
        public Func<ICommand> NavigateFunc = () => null;

        /// <summary>
        /// Underlying function which is called when getting the IsVisible property. Overridable.
        /// </summary>
        public Func<bool> IsVisibleFunc = () => true;

        /// <summary>
        /// Underlying function which is called when getting the IsClickable property. Overridable.
        /// </summary>
        public Func<bool> IsClickableFunc = () => true;

        /// <summary>
        /// Underlying function which is called when getting the IconName property. Overridable.
        /// </summary>
        public Func<string> IconNameFunc = () => string.Empty;

        #endregion

        #region View Properties

        public string Text => TextFunc();
        public ICommand Navigate => NavigateFunc();
        public bool IsVisible => IsVisibleFunc();
        public bool IsClickable => IsClickableFunc();
        public string IconName => IconNameFunc();

        #endregion

        public void UpdateIsClickable()
        {
            RaisePropertyChanged(() => IsClickable);
        }
    }

}
