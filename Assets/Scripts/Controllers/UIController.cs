using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IInitializer
{
    [SerializeField] private Image _characterImage, _panel;
    [SerializeField] private TMPro.TMP_Text _characterText, _characterName;
    private EventBus _bus;
    private Coroutine _corTypeText;
    private NodeData _tempData;
    private float _tempColorAlpha;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<NodeParsedDataSignal>(OnNodeParsedData);
        _bus.Subscribe<PlayerClickedSignal>(OnPlayerClicked);
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Subscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Subscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
    }
    private void OnNodeParsedData(NodeParsedDataSignal signal)
    {
        _tempData = signal.data;
        _characterImage.color = _tempData.sprite != null ? Color.white : Color.clear;
        _characterImage.sprite = _tempData.sprite;
        _characterImage.SetNativeSize();
        _characterName.text = _tempData.characterInfo.name;
        _characterName.color = Constants.characterColors.GetValueOrDefault(
            _tempData.characterInfo.character, 
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
    private void OnStartChangeScene(StartChangeSceneSignal signal)
    {
        StartCoroutine(FadeOut());
    }
    private void OnFinishChangeScene(FinishChangeSceneSignal signal)
    {
        StartCoroutine(FadeIn());
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
        if (_tempData.isNextPath)
        {
            _bus.Invoke(new NextNodeSignal());
        }
        _corTypeText = null;
    }
    private IEnumerator FadeOut()
    {
        _tempColorAlpha = _panel.color.a;
        for (float i = _panel.color.a; i > 0; i -= Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, Mathf.Clamp(i, 0, 1));
        }
        _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, 0);
    }
    private IEnumerator FadeIn()
    {
        for (float i = 0; i < _tempColorAlpha; i += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, Mathf.Clamp(i, 0, 1));
        }
        _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, _tempColorAlpha);
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<NodeParsedDataSignal>(OnNodeParsedData);
        _bus.Unsubscribe<PlayerClickedSignal>(OnPlayerClicked);
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Unsubscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Unsubscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
    }
}
