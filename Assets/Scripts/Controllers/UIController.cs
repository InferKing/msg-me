using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour, IInitializer
{
    [SerializeField] private Animator _outro;
    [SerializeField] private Image _characterImage, _panel;
    [SerializeField] private TMPro.TMP_Text _characterText, _characterName;
    private EventBus _bus;
    private Coroutine _corTypeText;
    private NodeData _tempData;
    private float _tempColorAlpha = 1, _sec = 0.7725f;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<NodeParsedDataSignal>(OnNodeParsedData);
        _bus.Subscribe<PlayerClickedSignal>(OnPlayerClicked);
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Subscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Subscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
        _bus.Subscribe<ToggleAutoTextSignal>(OnToggleAutoText);
        _bus.Subscribe<EndingGameSignal>((signal) => _outro.SetBool(Constants.pathBtnAnimatorParameter, true));

    }
    private void OnNodeParsedData(NodeParsedDataSignal signal)
    {
        _tempData = signal.data;
        // _characterImage.color = _tempData.sprite != null ? Color.white : Color.clear;
        if (_tempData.sprite == null && _characterImage.sprite != null)
        {
            StartCoroutine(FadeOut(_characterImage, 0.09f, true));
        }
        else if (_tempData.sprite != null && _characterImage.sprite == null) 
        {
            _characterImage.sprite = _tempData.sprite;
            StartCoroutine(FadeIn(_characterImage, 0.09f, true));
        }
        else
        {
            _characterImage.sprite = _tempData.sprite;
            _characterImage.SetNativeSize();
        }
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
    private void OnToggleAutoText(ToggleAutoTextSignal signal)
    {
        if (_corTypeText == null)
        {
            _bus.Invoke(new TypeTextFinishedSignal());
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
        _characterText.text = "";
        _characterName.text = "";
        StartCoroutine(FadeOut(_panel));
    }
    private void OnFinishChangeScene(FinishChangeSceneSignal signal)
    {
        StartCoroutine(FadeIn(_panel));
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
        _bus.Invoke(new TypeTextFinishedSignal());
    }
    private IEnumerator FadeOut(Image img, float speed = 0.09f, bool isNative = false)
    {
        if (isNative) { img.SetNativeSize(); }
        _tempColorAlpha = img.color.a;
        for (float i = img.color.a; i > 0; i -= speed)
        {
            yield return null;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Clamp(i, 0, 1));
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        if (isNative)
        {
            img.sprite = null;
        }
    }
    private IEnumerator FadeIn(Image img, float speed = 0.09f, bool isNative = false)
    {
        if (isNative) { img.SetNativeSize(); _tempColorAlpha = 1; }
        for (float i = 0; i < (isNative ? _tempColorAlpha : _sec); i += speed)
        {
            yield return null;
            img.color = new Color(img.color.r, img.color.g, img.color.b, Mathf.Clamp(i, 0, 1));
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, (isNative ? _tempColorAlpha : _sec));
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<NodeParsedDataSignal>(OnNodeParsedData);
        _bus.Unsubscribe<PlayerClickedSignal>(OnPlayerClicked);
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAd);
        _bus.Unsubscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Unsubscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
        _bus.Unsubscribe<ToggleAutoTextSignal>(OnToggleAutoText);
    }
}
