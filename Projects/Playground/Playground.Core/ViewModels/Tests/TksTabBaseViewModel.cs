#define useMvxNavVm

using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Playground.Core.G1T;

namespace Playground.Core.ViewModels.Tests
{
    public interface ITksTabBaseViewModel
    {
        Task<bool> WaitForViewToAppearAsync();
        bool IsViewAppeared { get; }
    }

#if useMvxNavVm
    public abstract class TksTabBaseViewModel : MvxNavigationViewModel, ITksTabBaseViewModel
#else
    public abstract class TksViewModel : MvxViewModel, ITksViewModel
#endif
    {
        #region [ Construction ]
#if useMvxNavVm
        public TksTabBaseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
#else
        public TksViewModel(IStatefulLogger logger, IMvxNavigationService navigationService)
        {
            Log = logger.WithTagForInstance(this);
            NavigationService = navigationService;
#endif
        }

        #endregion

        #region [ Private Properties ]

        private TaskCompletionSource<bool> _viewAppearedTaskCompletionSource { get; set; } = CreateViewAppearedTaskCompletionSource();
        // TODO: PUT CODE IN TO DISPOSE _viewDestroyedCancellationTokenSource
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

        #region [ ITksViewModel ]

        public bool IsViewAppeared => _viewAppearedTaskCompletionSource.Task.IsCompleted;

        public async Task<bool> WaitForViewToAppearAsync()
        {
            // TODO: DETERMINE IF TaskCreationOptions.RunContinuationsAsynchronously SHOULD BE USED IN CancellationTokenTaskSource
            // TODO: CHANGE WaitAsync TO SIMPLIFIED VERSION AND CancellationTokenTaskSource IS NO LONGER NEEDED
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

    public class TksTabBaseViewModelConfig
    {

    }
}
