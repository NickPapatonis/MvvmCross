using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels.Tests;
using Playground.Droid.Activities;
using static Android.Support.Design.Widget.TabLayout;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(TabsHostActivityViewModel), Resource.Id.content_frame, false)]
    public class TabsHostFragmentView : MvxFragment<TabsHostFragmentViewModel>
    {
        private TabLayout TabLayout { get; set; }
        private ViewPager ViewPager { get; set; }
        private TabLayout.IOnTabSelectedListener TabSelectedListener { get; set; }
        private MvxCachingFragmentStatePagerAdapterEx ViewPagerAdapter { get; set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.TabsHostFragmentView, null);

            if (Activity != null)
            {
                if (Activity is TabsHostActivityView tabsHostActivityView)
                {
                    tabsHostActivityView.ViewModel.FragmentViewModel = ViewModel;
                }
            }

            ViewPager = view.FindViewById<ViewPager>(Resource.Id.test_viewpager);
            InitializeViewPager();

            TabLayout = view.FindViewById<TabLayout>(Resource.Id.test_tabs);
            InitializeTabLayout();

            //var set = this.CreateBindingSet<TabbedSelectorFragment<TViewModel>, TViewModel>();
            //set.Bind(this).For(vw => vw.UpdateTabsInteraction).To(vm => vm.UpdateTabsInteraction).OneWay();
            //set.Apply();

            return view;
        }

        private void InitializeViewPager()
        {
            var ViewPagerFragments = new List<MvxViewPagerFragmentInfo>();

            ViewPagerFragments.Add(new MvxViewPagerFragmentInfo("Tab 1", "Tab01Tag", typeof(Tab01View), ViewModel.Tab01ViewModel));
            ViewPagerFragments.Add(new MvxViewPagerFragmentInfo("Tab 2", "Tab02Tag", typeof(Tab02View), ViewModel.Tab02ViewModel));

            /*
                MvxFragmentPagerAdapter (obsolete)
                    - Subclasses : FragmentPagerAdapter
                    - Creates view model each time using FragmentInfo.ViewModelType

                MvxFragmentStatePagerAdapter (obsolete)
                    - Subclasses : FragmentStatePagerAdapter
                    - Creates view model each time using FragmentInfo.ViewModelType

                MvxCachingFragmentPagerAdapter (abstract)
                    - Subclasses : PagerAdapter
                    - Does not create view model
                    - Implements own fragment state save/restore rather than using FragmentStatePagerAdapter

                MvxCachingFragmentStatePagerAdapter
                    - Subclasses : MvxCachingFragmentPagerAdapter
                    - Creates view model only if FragmentInfo.ViewModel is null
            */

            //Keep all 4 possible tabs in memory
            ViewPager.OffscreenPageLimit = ViewPagerFragments.Count;

            // NOTE: MvxCachingFragmentStatePagerAdapterEx is  a custom subclass at the end of the file, not an MvvmCross class
            //TODO:STABILITY: 7.20 SP1 most common error here, null exception
            ViewPagerAdapter = new MvxCachingFragmentStatePagerAdapterEx(Activity, ChildFragmentManager, ViewPagerFragments);
            ViewPager.Adapter = ViewPagerAdapter;
        }

        private void InitializeTabLayout()
        {
            TabLayout.SetupWithViewPager(ViewPager);
            TabSelectedListener = new NoScrollViewPagerOnTabSelectedListener(ViewPager);
            TabLayout.AddOnTabSelectedListener(TabSelectedListener);
            //TabLayout.TabSelected += TabLayout_TabSelected;
            //TabLayout.TabUnselected += TabLayout_TabUnselected;
            //SetTabLayoutAppearance();
        }
    }

    public class MvxCachingFragmentStatePagerAdapterEx : MvxCachingFragmentStatePagerAdapter
    {
        protected MvxCachingFragmentStatePagerAdapterEx(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MvxCachingFragmentStatePagerAdapterEx(Context context, FragmentManager fragmentManager, List<MvxViewPagerFragmentInfo> fragmentsInfo) : base(context, fragmentManager, fragmentsInfo)
        {
        }

        public override void SetPrimaryItem(ViewGroup container, int position, Java.Lang.Object objectValue)
        {
            FinishUpdate(container);
            base.SetPrimaryItem(container, position, objectValue);
        }
    }

    public class NoScrollViewPagerOnTabSelectedListener : ViewPagerOnTabSelectedListener
    {
        #region =====[ Private Fields ]================================================================================

        private ViewPager _viewPager;

        #endregion

        #region =====[ ctor ]==========================================================================================

        public NoScrollViewPagerOnTabSelectedListener(ViewPager viewPager)
          : base(viewPager)
        {
            _viewPager = viewPager;
        }

        protected NoScrollViewPagerOnTabSelectedListener(IntPtr javaReference, JniHandleOwnership transfer)
          : base(javaReference, transfer)
        {
        }

        #endregion

        #region =====[ Public Methods ]================================================================================

        public override void OnTabSelected(Tab tab)
        {
            _viewPager.SetCurrentItem(tab.Position, false);
        }

        #endregion
    }
}
