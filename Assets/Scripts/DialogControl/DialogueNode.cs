using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;


namespace RPG.DialogueControl
{

    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        bool isPlayerSpeaking = false;
        [SerializeField]
        private string dialogueText;
        [SerializeField]
        private List<string> children = new List<string>();
        [SerializeField]
        private Rect rect = new Rect(0, 0, 200, 100);
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;




        public string DialogueText
        {
            get
            {
                return dialogueText;
            }
        }

        public List<string> Children
        {
            get
            {
                return children;
            }
        }

        public Rect DialogRect
        {
            get
            {
                return rect;
            }
        }
        public bool IsPlayerSpeaking
        {
            get { return isPlayerSpeaking; }
        }

        public string OnEnterAction
        {
            get
            {
                return onEnterAction;
            }
        }

        public string  OnExitAction
        {
            get
            {
                return onExitAction;
            }
        }


#if UNITY_EDITOR

        public void SetDialogueText(string newText)
        {
            if (dialogueText != newText)
            {
                Undo.RecordObject(this, "Update Dialog Text");
                dialogueText = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Move Dialog Node");
            rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childId)
        {
            Undo.RecordObject(this, "Add Dialog Link");
            children.Add(childId);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childId)
        {
            Undo.RecordObject(this, "Remove Dialog Link");
            children.Remove(childId);
            EditorUtility.SetDirty(this);
        }

        public void SetIsPlayerSpeaking(bool newIsPlayerSpeaking)
        {
            Undo.RecordObject(this, "Change Dialog Speaker");
            isPlayerSpeaking = newIsPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}

