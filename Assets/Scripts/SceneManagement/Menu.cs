using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RPG.UI.SceneManagement
{


    public class Menu : MonoBehaviour
    {
        [SerializeField] int startSceneIndex = 1;
        [SerializeField] GameObject loadGameCanvas = null;

        public void LoadStartScenee()
        {
            SceneManager.LoadScene(startSceneIndex);
        }

        public void Quit()
        {
            Application.Quit();   
        }

        public void ShowLoadGameCanvas()
        {
            if (loadGameCanvas == null) return;

            loadGameCanvas.SetActive(true);
            Debug.Log("ShowloadGameCanvas");
            
        }



    }

}



