using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.InventoryControl;
using System;

namespace RPG.Combat
{
    public class AmmunitionStore : MonoBehaviour
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();

        private class DockedItemSlot
        {
            public Ammunition ammunition;
            public int number;
        }

        public event Action storeUpdated;


        public Ammunition GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].ammunition;
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
                if (object.ReferenceEquals(item, dockedItems[index].ammunition))
                {
                    dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.ammunition = item as Ammunition;
                slot.number = number;
                dockedItems[index] = slot;
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
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
            var actionItem = item as Ammunition;
            if (!actionItem) return 0;

            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].ammunition))
            {
                return 0;
            }
            if (actionItem.IsStackable)
            {
                return item.MaxNumberInStack;
            }
            if (dockedItems.ContainsKey(index))
            {
                return 0;
            }

            return 1;
        }

        public int FindAmmunitionType(AmmunitionType ammunitionType)
        {
            for (int i = 0; i < dockedItems.Count; i++)
            {
                if (dockedItems[i] != null && dockedItems[i].ammunition.AmmunitionType == ammunitionType)
                {
                    return i;
                }
            }
            return -1;
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
                record.itemID = pair.Value.ammunition.ItemID;
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
                AddAction(Ammunition.GetFromID(pair.Value.itemID) as Ammunition, pair.Key, pair.Value.number);
            }
        }

    }
}




