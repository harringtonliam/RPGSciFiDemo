using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.DialogueControl
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        string conversantName;
        Sprite conversantPortrait;


        public string ConversantName
        {
            get { return conversantName; }
        }

        public Sprite ConversantPortrait
        {
            get { return conversantPortrait; }
        }

        private void Start()
        {
            CharacterSheet characterSheet = GetComponent<CharacterSheet>();
            if (characterSheet == null) 
            {
                conversantName = "unknown";
                conversantPortrait = null;
            }
            else
            {
                conversantName = characterSheet.CharacterName;
                conversantPortrait = characterSheet.Portrait;
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Dialog;
        }

     
        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null)
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {

                PlayerConversant playerConversant = callingController.GetComponent<PlayerConversant>();
                playerConversant.StartDialogue( this, dialogue);
   
            }
            return true;
        }


    }

}


