using Autofac;
using DBase.Abstact;
using DBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Core.DI
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Context()).As<IDbContext>().InstancePerDependency();
            base.Load(builder);
        }
    }
}