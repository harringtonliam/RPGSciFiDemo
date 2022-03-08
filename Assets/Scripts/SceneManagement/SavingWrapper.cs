using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;
using RPG.Core;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeTime = 0.2f;

        const string defaultSaveFile = "autosave";
        const string quickSaveFile = "quicksave";

        GameConsole gameConsole;

        private void Start()
        {
            gameConsole = FindObjectOfType<GameConsole>();
        }

        private IEnumerator LoadLastScene(string savedGame)
        {
            yield return  GetComponent<SavingSystem>().LoadLastScene(savedGame);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeTime); ;
        }
    

        public void LoadSavedGame(string savedGame)
        {

            StartCoroutine(LoadLastScene(savedGame));
            WriteToConsole("Game loaded from save: " + savedGame);
        }


        public void Load()
        {

            GetComponent<SavingSystem>().Load(defaultSaveFile);  
        }

        public void Save(string fileName)
        {
            GetComponent<SavingSystem>().Save(fileName);
            WriteToConsole("Game saved: " + fileName);
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

        public void DeleteDefaultSaveFile()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }


        public Dictionary<string, DateTime> ListSaveFiles()
        {

            Dictionary<string, DateTime> allSaveFiles = GetComponent<SavingSystem>().ListAllSaveFiles();

            return allSaveFiles;

        }


        private void WriteToConsole(string textToWrite)
        {
            if (gameConsole == null) return;
            gameConsole.AddNewLine(textToWrite);
        }
    }



}
