using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BtnController : MonoBehaviour, IInitializer
{
    [SerializeField] private AudioSource _src;
    [SerializeField] private Button[] _buttons;
    private bool _locked = false;

    private Dictionary<Button, PathNodeData> _dataButtons;
    private EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<ShowPathButtonsSignal>(OnShowPathButtons);
        _bus.Subscribe<HidePathButtonsSignal>(OnHidePathButtons);
        _bus.Subscribe<BeforeShowAdSignal>(OnBeforeShowAdSignal);
        _bus.Subscribe<AfterShowAdSignal>(OnAfterShowAdSignal);
        _bus.Subscribe<StartChangeSceneSignal>(OnStartChangeScene);
        _bus.Subscribe<FinishChangeSceneSignal>(OnFinishChangeScene);
    }
    public void AutoButton()
    {
        if (_bus == null || _locked) return;
        Debug.Log("AutoButton");
        YG.YandexGame.savesData.isAutoText = !YG.YandexGame.savesData.isAutoText;
        _src.Play();
        _bus.Invoke(new ToggleAutoTextSignal(YG.YandexGame.savesData.isAutoText));
        _bus.Invoke(new PlayerInteractSignal(!YG.YandexGame.savesData.isAutoText));
    }
    private void OnStartChangeScene(StartChangeSceneSignal signal)
    {
        _locked = true;
    }
    private void OnFinishChangeScene(FinishChangeSceneSignal signal)
    {
        _locked = false;
    }
    private void OnBeforeShowAdSignal(BeforeShowAdSignal signal)
    {
        _locked = true;
    }
    private void OnAfterShowAdSignal(AfterShowAdSignal signal)
    {
        _locked = false;
    }
    private void OnShowPathButtons(ShowPathButtonsSignal signal)
    {
        if (_buttons.Length < signal.data.Count)
        {
            Debug.LogWarning("More data than the number of buttons was specified");
            return;
        }
        _dataButtons = new Dictionary<Button, PathNodeData>();
        for (int i = 0; i < signal.data.Count; i++)
        {
            _buttons[i].GetComponentInChildren<TMPro.TMP_Text>().text = signal.data[i].text;
            _dataButtons.Add(_buttons[i], signal.data[i]);
        }
        StartCoroutine(ToggleButtons(true));
    }
    public void RestartGame()
    {
        YG.YandexGame.ResetSaveProgress();
        YG.YandexGame.savesData.isAutoText = false;
        YG.YandexGame.savesData.node = null;
        YG.YandexGame.savesData.background = null;
        YG.YandexGame.savesData.music = null;
        YG.YandexGame.SaveProgress();
        SceneManager.LoadScene(0);
    }
    private void OnHidePathButtons(HidePathButtonsSignal signal)
    {
        StartCoroutine(ToggleButtons(false));
        _bus.Invoke(new NextNodeSignal());
    }
    public void ClickedButton(Button button)
    {
        _src.Play();
        if (!_dataButtons.TryGetValue(button, out PathNodeData result))
        {
            Debug.LogWarning("An unknown button was passed as an argument");
            return;
        }
        switch (result.chr)
        {
            case Character.Andrey:
                YG.YandexGame.savesData.andreyCount += result.value;
                break;
            case Character.Slave:
                YG.YandexGame.savesData.slaveCount += result.value;
                break;
            case Character.Teacher:
                YG.YandexGame.savesData.teacherCount += result.value;
                break;
            default:
                Debug.LogWarning($"Character {result.chr} is not used to change the ending");
                break;
        }
        YG.YandexGame.SaveProgress();
        _bus.Invoke(new HidePathButtonsSignal());
    }
    private IEnumerator ToggleButtons(bool toggle)
    {
        foreach (var button in _dataButtons.Keys)
        {
            button.GetComponent<Animator>().SetBool(Constants.pathBtnAnimatorParameter, toggle);
            yield return new WaitForSeconds(Constants.delayBtnAnimation);
        }
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<ShowPathButtonsSignal>(OnShowPathButtons);
        _bus.Unsubscribe<HidePathButtonsSignal>(OnHidePathButtons);
        _bus.Unsubscribe<BeforeShowAdSignal>(OnBeforeShowAdSignal);
        _bus.Unsubscribe<AfterShowAdSignal>(OnAfterShowAdSignal);
    }
}
