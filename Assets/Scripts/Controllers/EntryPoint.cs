using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private IInitializer[] _initializers;
    private ServiceLocator _locator;
    private EventBus _bus;
    private void Awake()
    {
        ServiceLocator.Initialize();
        _locator = ServiceLocator.Current;
        _bus = new EventBus();
        _locator.Register(_bus);
        foreach (var item in _initializers)
        {
            item.Initialize();
        }
    }
    private void OnDisable()
    {
        _locator.Unregister<EventBus>();
    }
}
