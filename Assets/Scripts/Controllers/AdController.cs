using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class AdController : MonoBehaviour, IInitializer
{
    private EventBus _bus;
    private bool _isVideo = true;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<StartShowAdSignal>(OnStartShowAd);
    }
    public void AdClosed() => _bus.Invoke(new AfterShowAdSignal());
    private void OnStartShowAd(StartShowAdSignal signal)
    {
        if (_isVideo)
        {
            YandexGame.RewVideoShow(0);
        }
        else
        {
            YandexGame.FullscreenShow();
        }
        _isVideo = !_isVideo;
    }

    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<StartShowAdSignal>(OnStartShowAd);
    }
}
