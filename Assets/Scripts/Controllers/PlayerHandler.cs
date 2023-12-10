using UnityEngine;

public class PlayerHandler : MonoBehaviour, IInitializer
{
    private EventBus _bus;

    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _bus.Invoke(new PlayerClickedSignal());
        }
    }
}
