using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using RPG.Attributes;


namespace RPG.DialogueControl
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialog;
        DialogueNode currentNode = null;
        bool isChoosing = false;
        AIConversant currentConversant;
        public event Action onConversationUpdated;

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



        public void StartDialogue(AIConversant newConverstant, Dialogue newDialogue)
        {
            currentConversant = newConverstant;
            currentDialog = newDialogue;
            currentNode = currentDialog.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();

        }

        public void Quit()
        {

            currentDialog = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialog != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }

            return currentNode.DialogueText;
        }

        internal string GetCurrentConversantName()
        {
            return currentConversant.ConversantName;
        }

        internal Sprite GetCurrentConversantPortrait()
        {
            return currentConversant.ConversantPortrait;
        }

        internal string GetPlayerConversantName()
        {
            return conversantName;
        }

        internal Sprite GetPlayerConversantPortrait()
        {
            return conversantPortrait;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialog.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialog.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            isChoosing = false;

            DialogueNode[] childNodes = currentDialog.GetAIChildren(currentNode).ToArray();
            TriggerExitAction();
            currentNode = childNodes[0];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            if (currentDialog.GetAllChildren(currentNode).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasPlayerChoicesNext()
        {
            if (currentDialog.GetPlayerChildren(currentNode).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null )
            {
                TriggerAction(currentNode.OnEnterAction);
            }
        }



        private void TriggerExitAction()
        {
            if (currentNode != null )
            {
                TriggerAction(currentNode.OnExitAction);
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "")
            {
                return;
            }
            DialogueTrigger[] dialogueTriggers = currentConversant.GetComponents<DialogueTrigger>();
            foreach (var trigger in dialogueTriggers)
            {
                trigger.Trigger(action);
            }
        }


    }


}


