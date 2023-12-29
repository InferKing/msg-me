using System.Collections.Generic;
using YG;

public class YandexModel : IModel
{
    private Dictionary<string, int> _data; 
    public YandexModel() 
    {
        _data = new Dictionary<string, int>();
        LoadData();
    }
    public int GetValue(string key)
    {
        return _data.GetValueOrDefault(key, 0);
    }

    public void LoadData()
    {
        YandexGame.LoadProgress();
    }

    public void SaveData()
    {
        YandexGame.SaveProgress();
    }

    public void UpdateValue(KeyValuePair<string, int> pair)
    {
        
    }
}
