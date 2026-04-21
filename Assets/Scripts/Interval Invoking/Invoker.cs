using System;
using System.Collections;
using UnityEngine;

public class Invoker : IInvoker
{
    private readonly CoroutineOwner _owner;
    private Coroutine _coroutine;
    private readonly float _interval;
    private readonly Action _callback;
    private readonly object _caller;

    public Invoker(CoroutineOwner owner, float interval, Action callback, object caller = null)
    {
        _owner = owner;
        _interval = interval;
        _callback = callback;
        _caller = caller;

        Start();
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

    public void Start()
    {
        if (_owner != null)
        {
            if (_coroutine != null)
                _owner.StopCoroutine(_coroutine);

            _coroutine = _owner.StartCoroutine(InvokingCorotine(_interval, _callback, _caller));
        }
    }
}