using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeTime = 0.2f;

        const string defaultSaveFile = "autosave";
        const string quickSaveFile = "quicksave";

        private void Awake()
        {
            //Liam Harring removed to proper Save/Load screen can be developed
            //StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return  GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeTime); ;
        }
    

        public void LoadSavedGame(string savedGame)
        {
            StartCoroutine(LoadLastScene());
        }


        public void Load()
        {

            GetComponent<SavingSystem>().Load(defaultSaveFile);  
        }

        public void Save(string fileName)
        {
            GetComponent<SavingSystem>().Save(fileName);
        }

        public void QuickSave()
        {
            GetComponent<SavingSystem>().Save(quickSaveFile);
        }

        public void AutoSave()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete(string filename)
        {
            GetComponent<SavingSystem>().Delete(filename);
        }


        public Dictionary<string, DateTime> ListSaveFiles()
        {
            Debug.Log("saveing wrapper list all save files");

            Dictionary<string, DateTime> allSaveFiles = GetComponent<SavingSystem>().ListAllSaveFiles();

            return allSaveFiles;

        }
    }



}
