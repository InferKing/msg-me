using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class AdCountdown : MonoBehaviour, IInitializer
{
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private Image _countdownImage;
    [SerializeField, Range(2, 5)] private int _delay;
    private EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Subscribe<AfterShowAdSignal>(OnAfterShowAd);
    }
    private void OnBeforeShowAd(BeforeShowAdSignal signal)
    {
        _countdownText.gameObject.SetActive(true);
        _countdownImage.gameObject.SetActive(true);
        StartCoroutine(Countdown());
    }
    private void OnAfterShowAd(AfterShowAdSignal signal)
    {
        _countdownText.gameObject.SetActive(false);
        _countdownImage.gameObject.SetActive(false);
    }
    private IEnumerator Countdown()
    {
        for (float i = _delay; i > 0; i -= Time.deltaTime)
        {
            _countdownText.text = string.Format(Constants.adCountdown, Mathf.CeilToInt(i));
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _bus.Invoke(new StartShowAdSignal());
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Unsubscribe<AfterShowAdSignal>(OnAfterShowAd);

    }
}
