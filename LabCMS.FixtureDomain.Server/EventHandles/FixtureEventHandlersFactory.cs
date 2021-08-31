﻿
using System.Reflection;
using Castle.DynamicProxy;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureEventHandlersFactory
{
    private class FixtureEventHandlerInterceptor : IInterceptor
    {
        private readonly static Assembly _eventAssembly = typeof(FixtureEvent).Assembly;
        public void Intercept(IInvocation invocation)
        {
            MethodInfo targetMethod = invocation.Method;

            if (targetMethod.Name == nameof(FixtureEventHandler.HandleAsync))
            {
                object[] args = invocation.Arguments;
                string handlerTypeName = targetMethod!.DeclaringType!.Name!;
                string eventTypeName = handlerTypeName.Substring(0, handlerTypeName.Length - "Handler".Length);
                Type targetEventType = _eventAssembly.GetExportedTypes().First(item=>item.Name==eventTypeName);
                Type argType = args[0].GetType();
                if (!argType.IsAssignableTo(targetEventType))
                {
                    throw new ArgumentException($"Ivalid kind of fixture event is given");
                }

                Func<ValueTask> actionWrap = async () =>
                {
                    //var proceed = invocation.CaptureProceedInfo();
                    FixtureEventHandler target = (invocation.InvocationTarget as FixtureEventHandler)!;
                    FixtureEvent fixtureEvent = (args[0] as FixtureEvent)!;
                    await target.HandleAsync(fixtureEvent);
                    
                    //dynamic ret = invocation.Method!.Invoke(invocation.InvocationTarget, invocation.Arguments)!;
                    //await ret;
                    //FixtureEventHandler target = (invocation.InvocationTarget as FixtureEventHandler)!;
                    //FixtureEvent fixtureEvent = (args[0] as FixtureEvent)!;
                    await target.Repository.FixtureEventsInDatabase.AddAsync(
                        FixtureEventInDatabase.GetEntity(fixtureEvent));
                    await target.Repository.SaveChangesAsync();
                    //proceed.Invoke();
                };
                invocation.ReturnValue = actionWrap.Invoke();
            }
            else { invocation.Proceed(); }
        }
    }
    private static ProxyGenerator _generator = new();
    private static Type[] _ctorArgTypes = new[] { typeof(FixtureDomainRepository)};
    public TFixtureEventHandler Create<TFixtureEventHandler>(
        FixtureDomainRepository repository)
        where TFixtureEventHandler : FixtureEventHandler
    {
        ConstructorInfo constructorInfo = typeof(TFixtureEventHandler).GetConstructor( 
            BindingFlags.Instance|BindingFlags.NonPublic,_ctorArgTypes)!;
        TFixtureEventHandler target = (constructorInfo.Invoke(new object[] { repository }) as TFixtureEventHandler)!;
        return _generator.CreateClassProxyWithTarget<TFixtureEventHandler>(
            target, new FixtureEventHandlerInterceptor());
    }
}
