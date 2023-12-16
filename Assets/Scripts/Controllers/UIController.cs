using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IInitializer
{
    [SerializeField] private Image _characterImage;
    [SerializeField] private TMPro.TMP_Text _characterText, _characterName;
    private EventBus _bus;
    private Coroutine _corTypeText;
    private NodeData _tempData;

    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<NodeParsedDataSignal>(OnNodeParsedData);
        _bus.Subscribe<PlayerClickedSignal>(OnPlayerClicked);
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAd);
    }
    private void OnNodeParsedData(NodeParsedDataSignal signal)
    {
        _tempData = signal.data;
        _characterImage.color = signal.data.sprite != null ? Color.white : Color.clear;
        _characterImage.sprite = signal.data.sprite;
        _characterImage.SetNativeSize();
        _characterName.text = signal.data.characterInfo.name;
        _characterName.color = Constants.characterColors.GetValueOrDefault(
            signal.data.characterInfo.character, 
            Color.white
        );
        _corTypeText = StartCoroutine(TypeText(signal.data.text));
    }
    private void OnPlayerClicked(PlayerClickedSignal signal)
    {
        if (_corTypeText != null)
        {
            StopCoroutine(_corTypeText);
            _characterText.text = _tempData.text;
            _corTypeText = null;
        }
        else
        {
            _bus.Invoke(new NextNodeSignal());
        }
    }
    private void OnBeforeShowAd(BeforeShowAdSignal signal)
    {
        NodeData data = new();
        data.text = Constants.adText;
        data.characterInfo = new CharacterInfo();
        data.characterInfo.name = Constants.developerName;
        data.characterInfo.character = Character.Developer;
        OnNodeParsedData(new NodeParsedDataSignal(data));
    }
    private IEnumerator TypeText(string text)
    {
        _characterText.text = "";
        foreach (var item in text)
        {
            _characterText.text += item;
            bool isPunctuation = Constants.punctutation.Contains(item);
            yield return new WaitForSeconds(isPunctuation ? Constants.delayPunctuation : Constants.delayLetter);
        }
        _corTypeText = null;
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<NodeParsedDataSignal>(OnNodeParsedData);
        _bus.Unsubscribe<PlayerClickedSignal>(OnPlayerClicked);
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAd);
    }
}
