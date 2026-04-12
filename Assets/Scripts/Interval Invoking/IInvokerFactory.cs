using System;

public interface IInvokerFactory
{
    IInvoker StartRepeatInvoking(float interval, Action callback, object caller = null);
}
