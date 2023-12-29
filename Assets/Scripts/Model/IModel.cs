using System.Collections.Generic;

public interface IModel
{
    void SaveData();
    void LoadData();
    int GetValue(string key);
    void UpdateValue(KeyValuePair<string, int> pair);
}
