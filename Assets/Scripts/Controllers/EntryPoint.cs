using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _monobehs;
    [SerializeField] private MonoBehaviour _introView, _transition;
    private ServiceLocator _locator;
    private EventBus _bus;
    private List<IInitializer> _initializers = new();
    private void Awake()
    {
        ServiceLocator.Initialize();
        _locator = ServiceLocator.Current;
        _bus = new EventBus();
        _locator.Register(_bus);
        YG.YandexGame.LoadProgress();
        foreach (IInitializer pair in _monobehs)
        {
            _initializers.Add(pair);
        }
        (_introView as IInitializer).Initialize();
        (_transition as IInitializer).Initialize();
        _bus.Subscribe<IntroFinishedSignal>((item) =>
        {
            foreach (IInitializer pair in _initializers)
            {
                pair.Initialize();
            }
            _bus.Invoke(new PlayerInteractSignal(!YG.YandexGame.savesData.isAutoText));
            YG.YandexGame.GameReadyAPI();
        });
    }
    private void OnDisable()
    {
        _locator.Unregister<EventBus>();
    }
}
