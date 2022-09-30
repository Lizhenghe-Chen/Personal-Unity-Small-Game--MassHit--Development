using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine;
namespace UIElements
{
    public class Splash : GlobalUIFunctions
    {
        public TMP_Text loadingText;
        public string playerName_;
        private void Awake()
        {
            playerName_ = PlayerPrefs.GetString("PlayerName");
        }
        public void UpdateText()
        {
            if (string.IsNullOrEmpty(playerName_) && loadingText) { loadingText.GetComponentInChildren<LocalizeStringEvent>().SetEntry("NullPlayerName"); }
            else { loadingText.GetComponentInChildren<LocalizeStringEvent>().SetEntry("PlayerWelecome"); }
        }
    }
}

