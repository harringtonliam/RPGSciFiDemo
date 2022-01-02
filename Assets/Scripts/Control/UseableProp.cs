using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPG.Control
{

    public class UseableProp : MonoBehaviour, IRaycastable
    {
        [SerializeField] bool isActivated = false;
        [SerializeField] UnityEvent<bool> useProp;
        [SerializeField] UnityEvent<float> usePropFloat;
        [TextArea]
        [SerializeField] string mouseOverTextActive = "Click to deactivate this item";
        [TextArea]
        [SerializeField] string mouseOverTextDeactive = "Click to activate this item";
        [TextArea]
        [SerializeField] string activateText = "Item activated";
        [TextArea]
        [SerializeField] string deactivateText = "Item deactivated";
        [SerializeField] Text usePropText = null;
        [SerializeField] float deactivatedValue = 5f;
        [SerializeField] float activatedValue = 10f;
        [SerializeField] AudioSource activateSound;
        [SerializeField] AudioSource deactivateSound;


        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UseProp useProp = playerController.transform.GetComponent<UseProp>();
                if (useProp != null)
                {
                    useProp.StartUseProp(gameObject);
                }
            }
            return true;
        }

        public void UseProp()
        {
            isActivated = !isActivated;
            useProp.Invoke(isActivated);
            if (isActivated)
            {
                usePropFloat.Invoke(activatedValue);
            }
            else
            {
                usePropFloat.Invoke(deactivatedValue);
            }

            if (isActivated && activateSound != null)
            {
                activateSound.Play();
            }
            else if (!isActivated && deactivateSound != null)
            {
                deactivateSound.Play();
            }

            SetUsePropText();
        }

        private void OnMouseOver()
        {
            SetMouseOverText();
        }

        private void OnMouseExit()
        {
            if (usePropText == null) return;
            usePropText.text = string.Empty;
        }

        private void SetMouseOverText()
        {
            if (usePropText == null) return;
            if (usePropText.text != string.Empty) return;
            if (isActivated)
            {
                usePropText.text = mouseOverTextActive;
            }
            else
            {
                usePropText.text = mouseOverTextDeactive;
            }
        }

        private void SetUsePropText()
        {
            if (usePropText == null) return;
            if (isActivated)
            {
                usePropText.text = activateText;
            }
            else
            {
                usePropText.text = deactivateText;
            }
        }




    }
}

