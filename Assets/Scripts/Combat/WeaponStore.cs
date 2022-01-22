using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;
using RPG.InventoryControl;

namespace RPG.Combat
{
    public class WeaponStore : MonoBehaviour, ISaveable
    {
        Dictionary<int, DockedItemSlot> dockedItems = new Dictionary<int, DockedItemSlot>();
        
        private class DockedItemSlot
        {
            public WeaponConfig weaponConfig;
            public int number;
            public bool isActive;
        }

        public event Action storeUpdated;


        public WeaponConfig GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].weaponConfig;
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


        public void AddAction(InventoryItem item, int index, int number, bool isActive)
        {
            if (dockedItems.ContainsKey(index))
            {
                if (object.ReferenceEquals(item, dockedItems[index].weaponConfig))
                {
                    dockedItems[index].number += number;
                }
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.weaponConfig = item as WeaponConfig;
                slot.number = number;
                slot.isActive = isActive;
                dockedItems[index] = slot;
            }
            SetActiveWeapon(index);
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
        public WeaponConfig GetActiveWeapon()
        {
            foreach (var dockedItem  in dockedItems)
            {
                if (dockedItem.Value.isActive)
                {
                    return dockedItem.Value.weaponConfig;
                }
            }

            return null;
        }

        public void SetActiveWeapon(int slot)
        {
            foreach (var dockedItem in dockedItems)
            {
                dockedItem.Value.isActive = false;
            }

            if (dockedItems.ContainsKey(slot))
            {
                if (dockedItems[slot] != null)
                {
                    dockedItems[slot].isActive = true;
                }
            }


            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }


        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as WeaponConfig;
            if (!actionItem) return 0;

            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].weaponConfig))
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
                record.itemID = pair.Value.weaponConfig.ItemID;
                record.number = pair.Value.number;
                record.isActive = pair.Value.isActive;
                state[pair.Key] = record;
            }
            return state;
        }
            
        public void RestoreState(object state)
        {
            var stateDict = (Dictionary<int, DockedItemRecord>)state;
            foreach (var pair in stateDict)
            {
                AddAction(WeaponConfig.GetFromID(pair.Value.itemID) as WeaponConfig, pair.Key, pair.Value.number, pair.Value.isActive);
            }
        }

    }

}


