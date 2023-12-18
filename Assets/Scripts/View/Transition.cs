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
        _bus.Subscribe<ChangeSceneSignal>(OnChangeSceneSignal);
    }
    private void OnStartChangeScene(StartChangeSceneSignal signal)
    {
        _sprite = signal.sprite;
        _transition.SetBool(Constants.transitionAnimatorParameter, true);
    }
    private void OnChangeSceneSignal(ChangeSceneSignal signal)
    {
        _image.sprite = _sprite;
    }
    private void OnFinishChangeScene(FinishChangeSceneSignal signal)
    {
        _transition.SetBool(Constants.transitionAnimatorParameter, false);
        _bus.Invoke(new NextNodeSignal());
    }
    
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Unsubscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
        _bus.Unsubscribe<ChangeSceneSignal>(OnChangeSceneSignal);
    }
}
