using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using UnityEngine.UI;

namespace RPG.UI.InventoryControl
{
    public class OpenContainerUI : MonoBehaviour
    {
        [SerializeField] GameObject uiCanvas = null;
        [SerializeField] InventoryUI containerInventoryUI = null;
        [SerializeField] ScrollRect containerscrollRect;
        [SerializeField] ScrollRect playerscrollRect;


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

            if (containerscrollRect != null)
            {
                containerscrollRect.verticalNormalizedPosition = 1f;
                Canvas.ForceUpdateCanvases();
            }
            if (playerscrollRect != null)
            {
                playerscrollRect.verticalNormalizedPosition = 1f;
                Canvas.ForceUpdateCanvases();
            }
        }

        public void CloseContainer()
        {
            uiCanvas.SetActive(false);
        }

    }

}


