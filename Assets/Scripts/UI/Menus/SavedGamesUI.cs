using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using System;

namespace RPG.UI.Menus
{

    public class SavedGamesUI : MonoBehaviour
    {
        [SerializeField] SaveGameUI loadGameGamePrefab = null;

        SavingWrapper savingWrapper = null;

        

        // Start is called before the first frame update
        void Start()
        {
            Redraw();

        }

        public void Redraw()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper == null) return;

            Dictionary<string, DateTime> saveFiles = savingWrapper.ListSaveFiles();

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var saveFile in saveFiles)
            {
                var savedGameGameUI = Instantiate(loadGameGamePrefab, transform);
                savedGameGameUI.Setup(saveFile.Key, saveFile.Value.ToString());
            }
        }


    }

}


