using UnityEngine;
using YG;

public class AdController : MonoBehaviour, IInitializer
{
    private EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<StartShowAdSignal>(OnStartShowAd);
    }
    public void AdClosed() 
    {
        _bus.Invoke(new AfterShowAdSignal());
        _bus.Invoke(new PlayerInteractSignal(!YandexGame.savesData.isAutoText));
    }
    private void OnStartShowAd(StartShowAdSignal signal)
    {
        YandexGame.FullscreenShow();
    }

    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<StartShowAdSignal>(OnStartShowAd);
    }
}
