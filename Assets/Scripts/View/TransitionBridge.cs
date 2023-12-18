using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBridge : MonoBehaviour, IInitializer
{
    private EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
    }
    public void UpdateScene()
    {
        _bus.Invoke(new ChangeSceneSignal());
    }

    public void EndOfAnimation()
    {
        _bus.Invoke(new FinishChangeSceneSignal());
        _bus.Invoke(new PlayerInteractSignal(true));
    }
}
