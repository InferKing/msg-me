using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _monobehs;
    [SerializeField] private MonoBehaviour _introView;
    private ServiceLocator _locator;
    private EventBus _bus;
    private List<IInitializer> _initializers = new();
    private void Awake()
    {
        ServiceLocator.Initialize();
        _locator = ServiceLocator.Current;
        _bus = new EventBus();
        _locator.Register(_bus);
        YG.YandexGame.savesData.isAutoText = false;
        YG.YandexGame.SaveProgress();
        foreach (IInitializer pair in _monobehs)
        {
            _initializers.Add(pair);
        }
        (_introView as IInitializer).Initialize();
        _bus.Subscribe<IntroFinishedSignal>((item) =>
        {
            YG.YandexGame.GameReadyAPI();
            foreach (IInitializer pair in _initializers)
            {
                pair.Initialize();
            }
        });
    }
    private void OnDisable()
    {
        _locator.Unregister<EventBus>();
    }
}
