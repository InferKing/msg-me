using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IInitializer
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private TMPro.TMP_Text _characterText, _characterName;
    private EventBus _bus;

    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<NodeParsedDataSignal>(OnNodeParsedData);
    }
    private void OnNodeParsedData(NodeParsedDataSignal signal)
    {
        _characterImage.sprite = signal.data.sprite;
        _characterName.text = signal.data.name;
        StartCoroutine(TypeText(signal.data.text));
    }
    private IEnumerator TypeText(string text)
    {
        
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<NodeParsedDataSignal>(OnNodeParsedData);
    }
}
