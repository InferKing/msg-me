using UnityEngine;

public class PlayerHandler : MonoBehaviour, IInitializer
{
    private EventBus _bus;
    private bool _canInteract = true;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Subscribe<AfterShowAdSignal>(OnAfterShowAd);
        _bus.Subscribe<ShowPathButtonsSignal>(OnShowPathButtons);
        _bus.Subscribe<HidePathButtonsSignal>(OnHidePathButtons);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canInteract)
        {
            _bus.Invoke(new PlayerClickedSignal());
        }
    }
    private void OnBeforeShowAd(BeforeShowAdSignal signal)
    {
        _canInteract = false;
    }
    private void OnAfterShowAd(AfterShowAdSignal signal)
    {
        _canInteract = true;
    }
    private void OnShowPathButtons(ShowPathButtonsSignal signal)
    {
        _canInteract = false;
    }
    private void OnHidePathButtons(HidePathButtonsSignal signal)
    {
        _canInteract = true;
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Unsubscribe<AfterShowAdSignal>(OnAfterShowAd);
        _bus.Unsubscribe<ShowPathButtonsSignal>(OnShowPathButtons);
        _bus.Unsubscribe<HidePathButtonsSignal>(OnHidePathButtons);
    }
}
