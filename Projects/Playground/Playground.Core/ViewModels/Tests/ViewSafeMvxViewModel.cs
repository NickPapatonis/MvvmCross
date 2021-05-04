#define useMvxNavVm

using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.G1T;

namespace Playground.Core.ViewModels.Tests
{
    public interface IViewSafeMvxViewModel
    {
        Task<bool> WaitForViewToAppearAsync();
        bool IsViewAppeared { get; }
    }

#if useMvxNavVm
    public abstract class ViewSafeMvxViewModel : MvxNavigationViewModel, IViewSafeMvxViewModel
#else
    public abstract class ViewSafeMvxViewModel : MvxViewModel, ITksViewModel
#endif
    {
        #region [ Construction ]
#if useMvxNavVm
        public ViewSafeMvxViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }
#else
        public ViewSafeMvxViewModel(IStatefulLogger logger)
        {
          Log = logger.WithTagForInstance(this);
        }

        public ViewSafeMvxViewModel(IStatefulLogger logger, IMvxNavigationService navigationService)
          : this(logger)
        {
          NavigationService = navigationService;
        }
#endif

        #endregion

        #region [ Private Properties ]

        private TaskCompletionSource<bool> _viewAppearedTaskCompletionSource { get; set; } = CreateViewAppearedTaskCompletionSource();
        // TODO: Need to dispose _viewDestroyedCancellationTokenSource. Probably not safe to dispose with ordinary Dispose pattern
        // or in the ViewDestroy method. May have to setup timer and destroy after one or two second delay.
        private CancellationTokenSource _viewDestroyedCancellationTokenSource { get; set; } = new CancellationTokenSource();

        #endregion

        #region [ Protected Properties]

        protected bool IsViewDestroyed => _viewDestroyedCancellationTokenSource.IsCancellationRequested;
        protected CancellationToken ViewDestroyedCancellationToken => _viewDestroyedCancellationTokenSource.Token;

#if !useMvxNavVm
        protected IStatefulLogger Log { get; set; }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public IMvxNavigationService NavigationService
        {
            get
            {
                if (m_NavigationService == null)
                {
                    m_NavigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
                }
                return m_NavigationService;
            }
            set
            {
                m_NavigationService = value;
            }
        }
        private IMvxNavigationService m_NavigationService { get; set; }
#endif

        #endregion

        #region [ IViewSafeMvxViewModel ]

        public bool IsViewAppeared => _viewAppearedTaskCompletionSource.Task.IsCompleted;

        public async Task<bool> WaitForViewToAppearAsync()
        {
            bool appeared = false;
            try
            {
                // Yield in case main thread is needed to invoke ViewDisappearing.  We want to ensure any 
                // pending callbacks occur before checking the view appeared completion task.
                await Task.Yield();

                await _viewAppearedTaskCompletionSource.Task.WaitAsync(_viewDestroyedCancellationTokenSource.Token);
                appeared = true;
            }
            catch (TaskCanceledException)
            {
                // Ignore
            }
            return appeared;
        }

        #endregion

        #region [ MvxViewModel Overrides ]

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            _viewAppearedTaskCompletionSource.SetResult(true);
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();
            if (_viewAppearedTaskCompletionSource.Task.IsCompleted)
            {
                _viewAppearedTaskCompletionSource = CreateViewAppearedTaskCompletionSource();
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing)
            {
                // throwOnFirstException = false ensures the callback registered by CancellationTokenTaskSource will be executed 
                // since our await on _viewAppearedTaskCompletionSource might need to be cancelled
                _viewDestroyedCancellationTokenSource.Cancel(throwOnFirstException: false);
            }
            base.ViewDestroy(viewFinishing);
        }

        #endregion

        #region [ Private Methods ]

        private static TaskCompletionSource<bool> CreateViewAppearedTaskCompletionSource()
        {
            return new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        #endregion
    }

    public class ViewSafeMvxViewModelConfig
    {

    }
}
