using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Transition : MonoBehaviour, IInitializer
{
    [SerializeField] private Animator _transition;
    [SerializeField] private UnityEngine.UI.Image _image;
    private EventBus _bus;
    private Sprite _sprite;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Subscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
    }
    private void OnStartChangeScene(StartChangeSceneSignal signal)
    {
        _sprite = signal.sprite;
        _transition.SetBool(Constants.transitionAnimatorParameter, true);
    }
    private void OnFinishChangeScene(FinishChangeSceneSignal signal)
    {
        _transition.SetBool(Constants.transitionAnimatorParameter, false);
        _bus.Invoke(new NextNodeSignal());
    }
    
    public void UpdateScene()
    {
        _image.sprite = _sprite;
    }
    public void EndOfAnimation()
    {
        _bus.Invoke(new FinishChangeSceneSignal());
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Unsubscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
    }
}
