using System;
using System.Collections;
using UnityEngine;

public class Invoker : IInvoker
{
    private Coroutine _coroutine;
    private CoroutineOwner _owner;

    public Invoker(CoroutineOwner owner, float interval, Action callback, object caller = null)
    {
        _owner = owner;
        _coroutine = _owner.StartCoroutine(InvokingCorotine(interval, callback, caller));
    }

    private IEnumerator InvokingCorotine(float interval, Action callback, object caller = null)
    {
        var wait = new WaitForSeconds(interval);

        UnityEngine.Object unityObject = caller as UnityEngine.Object;
        bool isUnityObject = unityObject != null;

        while (true)
        {
            if (isUnityObject && unityObject == null)
            {
                Stop();
                yield break;
            }

            callback?.Invoke();
            yield return wait;
        }
    }

    public void Stop()
    {
        if (_coroutine != null && _owner != null)
            _owner.StopCoroutine(_coroutine);
        _coroutine = null;
    }
}