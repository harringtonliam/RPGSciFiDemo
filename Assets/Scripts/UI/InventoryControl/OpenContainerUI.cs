using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;

namespace RPG.UI.InventoryControl
{
    public class OpenContainerUI : MonoBehaviour
    {
        [SerializeField] GameObject uiCanvas = null;
        [SerializeField] InventoryUI containerInventoryUI = null;


        void Start()
        {
            uiCanvas.SetActive(false);
        }

        public void OpenContainer(Inventory inventory)
        {
            uiCanvas.SetActive(true);
            if (containerInventoryUI != null)
            {
                containerInventoryUI.SetInventoryObject(inventory);
            }
        }

        public void CloseContainer()
        {
            uiCanvas.SetActive(false);
        }

    }

}


