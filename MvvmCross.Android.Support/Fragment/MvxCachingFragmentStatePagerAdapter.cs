// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Java.Lang;
using MvvmCross.Core;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using String = Java.Lang.String;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxCachingFragmentStatePagerAdapter")]
    public class MvxCachingFragmentStatePagerAdapter
		: MvxCachingFragmentPagerAdapter
    {
        private readonly Context _context;
        private readonly Type _activityType;

        protected MvxCachingFragmentStatePagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Trace("Begin");
            _activityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
            Trace("End");
        }

		public MvxCachingFragmentStatePagerAdapter(Context context, FragmentManager fragmentManager,
            List<MvxViewPagerFragmentInfo> fragmentsInfo) : base(fragmentManager)
        {
            Trace("Begin");

            _context = context;
            FragmentsInfo = fragmentsInfo;
            _activityType = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();
            Trace($"FragmentInfo count = {FragmentsInfo.Count}, activityType = {_activityType?.Name ?? "null"}");

            Trace("End");
        }

        public override int Count => FragmentsInfo?.Count() ?? 0;

        public List<MvxViewPagerFragmentInfo> FragmentsInfo { get; }

        public override Fragment GetItem(int position, Fragment.SavedState fragmentSavedState = null)
        {
            try
            {
                Trace("Begin");

                var fragInfo = FragmentsInfo.ElementAt(position);
                var fragment = Fragment.Instantiate(_context, fragInfo.FragmentType.FragmentJavaName());

                var mvxFragment = fragment as IMvxFragmentView;
                if (mvxFragment == null)
                {
                    Trace("Not an MvxFragment");
                    return fragment;
                }

                if (mvxFragment.GetType().IsFragmentCacheable(_activityType) && fragmentSavedState != null)
                {
                    Trace("Fragment is cacheable and has saved state");
                    return fragment;
                }

                Trace($"Setting fragment view model from {(fragInfo.ViewModel != null ? "fragInfo" : "CreateViewModel")}");
                var viewModel = fragInfo.ViewModel ?? CreateViewModel(position);
                mvxFragment.ViewModel = viewModel;

                return fragment;
            }
            finally
            {
                Trace("End");
            }
        }

        public override int GetItemPosition(Java.Lang.Object @object)
        {
            return PagerAdapter.PositionNone;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new String(FragmentsInfo.ElementAt(position).Title);
        }

        protected override string GetTag(int position)
        {
            return FragmentsInfo.ElementAt(position).Tag;
        }

        private IMvxViewModel CreateViewModel(int position)
        {
            try
            {
                Trace("Begin");

                var fragInfo = FragmentsInfo.ElementAt(position);

                MvxBundle mvxBundle = null;
                if (fragInfo.ParameterValuesObject != null)
                    mvxBundle = new MvxBundle(fragInfo.ParameterValuesObject.ToSimplePropertyDictionary());

                var request = new MvxViewModelRequest(fragInfo.ViewModelType, mvxBundle, null);

                return Mvx.IoCProvider.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
            }
            finally
            {
                Trace("End");
            }
        }

        protected static readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<MvxCachingFragmentStatePagerAdapter>();
        private static void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            Log.Trace($"{caller} [{System.Threading.Thread.CurrentThread.ManagedThreadId}, {MvxAndroidMainThreadDispatcher.Instance.IsOnMainThread}] {msg}");
        }
    }
}
