using UnityEngine;
using YG;

public class PlayerHandler : MonoBehaviour, IInitializer
{
    [SerializeField] private CheckMousePos _mousePos;
    private EventBus _bus;
    private bool _canInteract = true;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<PlayerInteractSignal>(OnPlayerInteract);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canInteract && _bus != null)
        {
            if (!_mousePos.IsUnderButton())
            {
                _bus.Invoke(new PlayerClickedSignal());
            }
        }
    }
    private void OnPlayerInteract(PlayerInteractSignal signal)
    {
        if (!YandexGame.savesData.isAutoText)
        {
            _canInteract = signal.data;
        }
        else
        {
            _canInteract = false;
        }
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<PlayerInteractSignal>(OnPlayerInteract);
    }

    
}
