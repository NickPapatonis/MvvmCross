using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Fragments
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(Modal02View))]
    public class Modal02View : MvxDialogFragment<Modal02ViewModel>
    {
        public Modal02View()
        {
        }

        protected Modal02View(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ModalNNView, null);

            return view;
        }

        public override void OnPause()
        {
            var top = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
            var activity = top.Activity;

            base.OnPause();
        }
    }
}
