using MessagePipe;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Scripts.Core.Lifetime
{
    internal class ApplicationLifetimeScope : LifetimeScope
    {
        private ApplicationInstaller[] _installers;

        public void Construct(ApplicationInstaller[] installers)
        {
            _installers = installers;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            //MessagePipe
            builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c => 
                GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
            
            //Observable
            ObservableSystem.DefaultFrameProvider = UnityFrameProvider.Update;
            ObservableSystem.DefaultTimeProvider = UnityTimeProvider.Update;
            ObservableSystem.RegisterUnhandledExceptionHandler(e => Debug.LogException(e));
            builder.RegisterBuildCallback(c =>
                ObservableSystem.RegisterServiceProvider(c.AsServiceProvider()));
                
            foreach (var installer in _installers) 
                installer.Install(builder);

            _installers = null;
        }
    }
}