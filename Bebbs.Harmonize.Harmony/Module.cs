﻿using Bebbs.Harmonize.Common;
using Bebbs.Harmonize.Harmony.Services;
using Ninject;
using Ninject.Modules;

namespace Bebbs.Harmonize.Harmony
{
    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<State.IFactory>().To<State.Factory>().InSingletonScope();
            Bind<Command.IFactory>().To<Command.Factory>().InSingletonScope();

            Bind<Hub.Configuration.Parser>().ToSelf();
            Bind<Hub.Configuration.IParser>().ToMethod(ctx => EventSourceProxy.TracingProxy.Create<Hub.Configuration.IParser>(ctx.Kernel.Get<Hub.Configuration.Parser>())).InSingletonScope();

            Bind<XmppService>().ToSelf();
            Bind<IXmppService>().ToMethod(ctx => EventSourceProxy.TracingProxy.Create<IXmppService>(ctx.Kernel.Get<XmppService>())).InSingletonScope();
            Bind<AuthenticationService>().ToSelf();
            Bind<IAuthenticationService, IInitializeAtStartup, ICleanupAtShutdown>().ToMethod(ctx => EventSourceProxy.TracingProxy.Create<IAuthenticationService>(ctx.Kernel.Get<AuthenticationService>())).InSingletonScope();
            Bind<HarmonyService>().ToSelf();
            Bind<IHarmonyService, IInitializeAtStartup, ICleanupAtShutdown>().ToMethod(ctx => EventSourceProxy.TracingProxy.Create<IHarmonyService>(ctx.Kernel.Get<HarmonyService>())).InSingletonScope();
            Bind<State.Machine>().ToSelf();
            Bind<State.IMachine, IInitializeAtStartup, ICleanupAtShutdown>().ToMethod(ctx => EventSourceProxy.TracingProxy.Create<State.IMachine>(ctx.Kernel.Get<State.Machine>())).InSingletonScope();
        }
    }
}