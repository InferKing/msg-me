using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class ButtonController : MonoBehaviour, IInitializer
{
    [SerializeField] private Button[] _buttons;
    private Dictionary<Button, PathNodeData> _dataButtons;
    private EventBus _bus;
    public void Initialize()
    {
        _bus = ServiceLocator.Current.Get<EventBus>();
        _bus.Subscribe<ShowPathButtonsSignal>(OnShowPathButtons);
        _bus.Subscribe<HidePathButtonsSignal>(OnHidePathButtons);
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
            _buttons[i].gameObject.SetActive(true);
            _buttons[i].GetComponent<TMPro.TMP_Text>().text = signal.data[i].text;
            _dataButtons.Add(_buttons[i], signal.data[i]);
        }
    }
    private void OnHidePathButtons(HidePathButtonsSignal signal)
    {
        _dataButtons.Clear();
        foreach (var button in _buttons)
        {
            button.gameObject.SetActive(false);
        }
        _bus.Invoke(new NextNodeSignal());
    }
    public void ClickedButton(Button button)
    {
        if (!_dataButtons.TryGetValue(button, out PathNodeData result))
        {
            Debug.LogWarning("An unknown button was passed as an argument");
            return;
        }
        switch (result.chr)
        {
            case Character.Andrey:
                YandexGame.savesData.andreyCount += result.value;
                break;
            case Character.Slave:
                YandexGame.savesData.slaveCount += result.value;
                break;
            case Character.Teacher: 
                YandexGame.savesData.teacherCount += result.value;
                break;
        }
        YandexGame.SaveProgress();
        _bus.Invoke(new HidePathButtonsSignal());
    }
    private void OnDisable()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<ShowPathButtonsSignal>(OnShowPathButtons);
        _bus.Unsubscribe<HidePathButtonsSignal>(OnHidePathButtons);

    }
}
