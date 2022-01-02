using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toogleKey = KeyCode.Escape;
        [SerializeField] GameObject uiCanvas = null;

        // Start is called before the first frame update
        void Start()
        {
            uiCanvas.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toogleKey))
            {
                uiCanvas.SetActive(!uiCanvas.activeSelf);
            }
        }
    }



}

