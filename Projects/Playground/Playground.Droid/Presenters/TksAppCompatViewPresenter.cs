using System.Collections.Generic;
using System.Reflection;
using Android.Support.V4.App;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels.Tests;

namespace Playground.Droid.Presenters
{
    public class TksAppCompatViewPresenter : MvxAppCompatViewPresenter
    {
        public TksAppCompatViewPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        protected override void OnFragmentChanged(
            FragmentTransaction ft,
            Fragment fragment,
            MvxFragmentPresentationAttribute attribute,
            MvxViewModelRequest request)
        {
            if (fragment is MvxFragment mvxFragment && mvxFragment.ViewModel is IDelegateViewModel delegateViewModel)
            {
                if (attribute.FragmentHostViewType != null)
                {
                    var fragmentHost = GetFragmentByViewType(attribute.FragmentHostViewType);
                    if (fragmentHost is MvxFragment mvxHostFragment && mvxHostFragment is IDelegatingViewModel delegatingFragmentViewModel)
                    {
                        delegatingFragmentViewModel.DelegateViewModel = delegateViewModel;
                    }
                }
                else if (typeof(IDelegatingViewModel).IsAssignableFrom(attribute.ActivityHostViewModelType))
                {
                    if (CurrentActivity is MvxAppCompatActivity appCompatActivity && appCompatActivity.ViewModel is IDelegatingViewModel delegatingActivityViewModel)
                    {
                        delegatingActivityViewModel.DelegateViewModel = delegateViewModel;
                    }
                }
            }

            base.OnFragmentChanged(ft, fragment, attribute, request);
        }

        //protected override Task<bool> CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        //{
        //    if (CurrentActivity != null)
        //    {
        //        return base.CloseFragment(viewModel, attribute);
        //    }

        //    return Task.FromResult(false);
        //}
    }
}
