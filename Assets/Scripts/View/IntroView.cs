using UnityEngine;

public class IntroView : MonoBehaviour, IInitializer
{
    [SerializeField] private AudioSource _sound;
    private EventBus _bus;
    public void EndOfAnimation()
    {
        _bus.Invoke(new IntroFinishedSignal());
    }
    public void PlaySound()
    {
        _sound.Play();
    }
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
    }
}
