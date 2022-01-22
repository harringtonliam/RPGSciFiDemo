using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;


namespace RPG.InventoryControl
{
    public class QuickItemStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        private class DockedItemSlot
        {
            public ActionItem actionItem;
            public int number;
        }

        public event Action storeUpdated;


        public ActionItem GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].actionItem;
            }
            return null;
        }

        public int GetNumber(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].number;
            }
            return 0;
        }


        public void AddAction(InventoryItem item, int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (object.ReferenceEquals(item, dockedItems[index].actionItem))
                {
                    dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.actionItem = item as ActionItem;
                slot.number = number;
                dockedItems[index] = slot;
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].actionItem.Use(user);
                if (dockedItems[index].actionItem.IsConsumable)
                {
                    RemoveItems(index, 1);
                }
                return true;
            }
            return false;
        }

        public void RemoveItems(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems.Remove(index);
                }
                if (storeUpdated != null)
                {
                    storeUpdated();
                }
            }
        }


        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if (!actionItem) return 0;

            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].actionItem))
            {
                return 0;
            }
            if (actionItem.IsConsumable)
            {
                return item.MaxNumberInStack;
            }
            if (dockedItems.ContainsKey(index))
            {
                return 0;
            }

            return 1;
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
            public bool isActive;
        }

        public object CaptureState()
        {
            var state = new Dictionary<int, DockedItemRecord>();
            foreach (var pair in dockedItems)
            {
                var record = new DockedItemRecord();
                record.itemID = pair.Value.actionItem.ItemID;
                record.number = pair.Value.number;
                state[pair.Key] = record;
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>)state;
            foreach (var pair in stateDict)
            {
                AddAction(ActionItem.GetFromID(pair.Value.itemID) as ActionItem, pair.Key, pair.Value.number);
            }
        }
    }
}


