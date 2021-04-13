// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters;
using Playground.Core;
using Playground.Core.G1T;
using Playground.Droid.Bindings;
using Playground.Droid.Controls;
using Playground.Droid.G1T;
using Playground.Droid.Presenters;
using Serilog;

namespace Playground.Droid
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                string msg = $"AppDomain.CurrentDomain.UnhandledException: IsTerminating: {e.IsTerminating}";
                ErrorException(msg, e.ExceptionObject as Exception);
            };

            Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (s, e) =>
            {
                string msg = $"AndroidEnvironment.UnhandledExceptionRaiser: Handled: {e.Handled}";
                ErrorException(msg, e.Exception);
            };

            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                string msg = $"TaskScheduler.UnobservedTaskException: Observed: {e.Observed}";
                ErrorException(msg, e.Exception);
            };
        }

        //protected readonly IMvxLog Log = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor<Setup>();
        private void Trace(string msg, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            SetupLog.Trace($"{caller} [{Thread.CurrentThread.ManagedThreadId}] {msg}");
        }
        private void ErrorException(string msg, Exception exception, [System.Runtime.CompilerServices.CallerMemberName]string caller = null)
        {
            if (exception is AggregateException aggregateException)
            {
                exception = aggregateException.InnerException;
            }
            SetupLog.ErrorException($"{caller} [{Thread.CurrentThread.ManagedThreadId}] {msg}", exception);
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies =>
            new List<Assembly>(base.AndroidViewAssemblies)
            {
                typeof(MvxRecyclerView).Assembly
            };

        //public override MvxLogProviderType GetDefaultLogProviderType() => MvxLogProviderType.Serilog;

        //protected override IMvxLogProvider CreateLogProvider()
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .MinimumLevel.Debug()
        //        .WriteTo.AndroidLog()
        //        .CreateLogger();

        //    return base.CreateLogProvider();
        //}

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new TksAppCompatViewPresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<BinaryEdit>(
                "MyCount",
                (arg) => new BinaryEditTargetBinding(arg));

            base.FillTargetFactories(registry);
        }

        protected override void InitializePlatformServices()
        {
            base.InitializePlatformServices();
            Mvx.IoCProvider.RegisterSingleton<IDialogService>(Mvx.IoCProvider.IoCConstruct<DialogService>());
        }
    }
}
