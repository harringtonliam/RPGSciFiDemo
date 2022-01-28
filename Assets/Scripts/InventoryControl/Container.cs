using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.InventoryControl
{
    [RequireComponent(typeof(Inventory))]
    public class Container : MonoBehaviour,  IRaycastable
    {
        bool isOpen = false;
        ContainerLink containerLink = null;

        private void Start()
        {
            containerLink = FindObjectOfType<ContainerLink>();
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ContainerOpener containerOpener = playerController.transform.GetComponent<ContainerOpener>();
                if (containerOpener != null)
                {
                    containerOpener.StartOpenContainer(gameObject);
                }
            }
            return true;
        }

        public void OpenContainer()
        {
            if (isOpen) return;

            isOpen = true;
            if (containerLink != null)
            {
                containerLink.OpenContainer(this);
            }
            
        }

        public void CloseContainer()
        {
            if (!isOpen) return;

            isOpen = false;
            if (containerLink != null)
            {
                containerLink.CloseContainer();
            }
        }



    }
}

