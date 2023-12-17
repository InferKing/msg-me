using UnityEngine;

public class PlayerHandler : MonoBehaviour, IInitializer
{
    private EventBus _bus;
    private bool _canInteract = true;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<PlayerInteractSignal>(OnPlayerInteract);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canInteract)
        {
            _bus.Invoke(new PlayerClickedSignal());
        }
    }
    private void OnPlayerInteract(PlayerInteractSignal signal)
    {
        _canInteract = signal.data;
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<PlayerInteractSignal>(OnPlayerInteract);
    }
}
