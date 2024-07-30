using System.Collections.Generic;
using Core.Controllers.Core.Controllers;
using Core.Controllers.Core.Interfaces;
using Features.AppStarter.Scripts.Controllers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.AppStarter.Scripts
{
 public class AppLifetimeScope : LifetimeScope
    {
        private readonly List<IInstaller> _installers = new()
        {
            //add installers here for base features
        };

        public void Start()
        {
            name = $"DI {nameof(AppLifetimeScope)}";
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EntryPoints.AppStarter>();
            DontDestroyOnLoad(this);
            builder.RegisterEntryPointExceptionHandler(Debug.LogException);

            RegisterInstallers(builder);
            RegisterControllers(builder);
            RegisterServices(builder);
            RegisterModels(builder);
        }

        private void RegisterInstallers(IContainerBuilder builder)
        {
            foreach (var installer in _installers)
            {
                installer.Install(builder);
            }
        }

        private static void RegisterServices(IContainerBuilder builder)
        {
            // init services here
        }

        private void RegisterModels(IContainerBuilder builder)
        {
            // init models here
        }

        private static void RegisterControllers(IContainerBuilder builder)
        {
            //controllers base
            builder.Register<IControllerFactory, ControllerFactory>(Lifetime.Scoped);

            //common part
            builder.Register<RootController>(Lifetime.Transient);
            builder.Register<LaunchAppController>(Lifetime.Transient);

        }

    }
}