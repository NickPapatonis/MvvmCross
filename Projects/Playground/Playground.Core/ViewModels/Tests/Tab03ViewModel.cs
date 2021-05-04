using System.Collections.Generic;
using System.Threading;
using MvvmCross.Base;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Playground.Core.G1T;

namespace Playground.Core.ViewModels.Tests
{
    public class Tab03ViewModel : ViewSafeMvxViewModel
    {
        public Tab03ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            Trace("Begin");
            DashboardSections = CreateDashboardSections();
            Trace("End");
        }

        #region [ Dashboard Sections ]

        private List<IDashboardSectionViewModel> _dashboardSections;
        public List<IDashboardSectionViewModel> DashboardSections
        {
            get { return _dashboardSections; }
            set { SetProperty(ref _dashboardSections, value); }
        }

        private List<IDashboardSectionViewModel> CreateDashboardSections()
        {
            Trace("Begin");
            var dashboardSections = new List<IDashboardSectionViewModel>();

            Trace("Adding thing section");
            var thingRows = new List<IDashboardButtonRowViewModel>();
            thingRows.Add(new ThingButtonRowViewModel(NavigationService));
            dashboardSections.Add(new DashboardSectionViewModel(thingRows)
            {
                HeaderName = "Things",
                IconName = null
            });

            Trace("End");
            return dashboardSections;
        }

        #endregion

        public override void ViewAppearing()
        {
            Trace("Begin");
            base.ViewAppearing();
            Trace("End");
        }

        public override void ViewAppeared()
        {
            Trace("Begin");
            base.ViewAppeared();
            Trace("End");
        }

        public override void ViewDisappearing()
        {
            Trace("Begin");
            base.ViewDisappearing();
            Trace("End");
        }

        public override void ViewDisappeared()
        {
            Trace("Begin");
            base.ViewDisappeared();
            Trace("End");
        }

        public override void ViewCreated()
        {
            Trace("Begin");
            base.ViewCreated();
            Trace("End");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Trace($"Begin, {nameof(viewFinishing)} = {viewFinishing}");
            base.ViewDestroy(viewFinishing);
            Trace("End");
        }

        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}, {MvxMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
