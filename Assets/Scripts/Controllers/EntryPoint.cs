using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _monobehs;
    private ServiceLocator _locator;
    private EventBus _bus;
    private List<IInitializer> _initializers = new();
    private void OnValidate()
    {
        foreach (IInitializer pair in _monobehs)
        {
            _initializers.Add(pair);
        }
    }
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
