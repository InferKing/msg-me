using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using YG;

public class AutoText : MonoBehaviour, IInitializer
{
    private EventBus _bus;
    private Coroutine _delay;
    private bool _wasEnabled;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<ToggleAutoTextSignal>(OnToggleAutoText);
        _bus.Subscribe<TypeTextFinishedSignal>(OnTypeTextFinished);
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Subscribe<AfterShowAdSignal>(OnAfterShowAd);
        _bus.Subscribe<EndingGameSignal>(OnEndingGameSignal);
    }
    private void OnToggleAutoText(ToggleAutoTextSignal signal)
    {
        Debug.Log("AutoText");
        YandexGame.savesData.isAutoText = signal.data;
        YandexGame.SaveProgress();
    }
    private void OnTypeTextFinished(TypeTextFinishedSignal signal)
    {
        if (!YandexGame.savesData.isAutoText) return;
        if (_delay != null) StopCoroutine(_delay);
        _delay = StartCoroutine(Delay());
    }
    private void OnEndingGameSignal(EndingGameSignal signal)
    {
        YandexGame.savesData.isAutoText = false;
        YandexGame.SaveProgress();
        if (_delay != null) StopCoroutine(_delay);
    }
    private void OnBeforeShowAd(BeforeShowAdSignal signal)
    {
        _wasEnabled = YandexGame.savesData.isAutoText;
        YandexGame.savesData.isAutoText = false;
        if (_delay != null) StopCoroutine(_delay);
    }
    private void OnAfterShowAd(AfterShowAdSignal signal)
    {
        YandexGame.savesData.isAutoText = _wasEnabled;
    }
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        if (YandexGame.savesData.isAutoText)
        {
            _bus.Invoke(new NextNodeSignal());
        }
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<ToggleAutoTextSignal>(OnToggleAutoText);
        _bus.Unsubscribe<TypeTextFinishedSignal>(OnTypeTextFinished);
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Unsubscribe<AfterShowAdSignal>(OnAfterShowAd);
        _bus.Unsubscribe<EndingGameSignal>(OnEndingGameSignal);
    }
}
