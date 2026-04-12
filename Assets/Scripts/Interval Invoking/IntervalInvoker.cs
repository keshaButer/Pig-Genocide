using System;

public class IntervalInvoker : IInvokerFactory
{
    private readonly CoroutineOwner _owner;

    public IntervalInvoker(CoroutineOwner owner)
    {
        _owner = owner;
    }

    public IInvoker StartRepeatInvoking(float interval, Action callback, object caller = null)
    {
        return new Invoker(_owner, interval, callback, caller);
    }
}
