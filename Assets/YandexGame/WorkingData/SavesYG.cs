
using UnityEngine;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int andreyCount = 0, slaveCount = 0, teacherCount = 0;
        public bool isAutoText = false;
        public DialogueNode node;
        public AudioClip music;
        public Sprite background;
        public SavesYG()
        {
            
        }
    }
}
