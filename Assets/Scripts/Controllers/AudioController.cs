using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour, IInitializer
{
    [SerializeField] private AudioSource _fx, _music;
    private EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<NodeParsedDataSignal>(OnNodeParsedDataSignal);
    }
    private void OnNodeParsedDataSignal(NodeParsedDataSignal signal)
    {
        if (signal.data.music != null)
        {
            if (!_music.isPlaying) 
            {
                _music.clip = signal.data.music;
                _music.Play();
            }
            else
            {
                StartCoroutine(TransitionMusic(_music, signal.data.music));
            }
        }
        if (signal.data.fx != null && !_fx.isPlaying)
        {
            _fx.clip = signal.data.fx;
            _fx.Play();
        }
    }
    private IEnumerator TransitionMusic(AudioSource src, AudioClip clip)
    {
        float lastVolume = src.volume;
        for (float i = src.volume; i > float.Epsilon; i -= Time.deltaTime / 3) 
        {
            src.volume = i;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        src.Stop();
        src.clip = clip;
        src.Play();
        for (float i = 0; i < lastVolume; i += Time.deltaTime / 3)
        {
            src.volume = i;
            yield return null;
        }
        
    }
    private void OnDisable()
    {
        _bus.Unsubscribe<NodeParsedDataSignal>(OnNodeParsedDataSignal);
    }
}
