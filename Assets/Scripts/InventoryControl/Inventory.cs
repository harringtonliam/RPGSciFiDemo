using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.InventoryControl
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [Tooltip("Maximum Inventory Size")]
        [SerializeField] int inventorySize = 12;

        InventorySlot[] inventorySlots;



 

        public struct InventorySlot
        {
            public InventoryItem inventoryItem;
            public int number;

        }

        public event Action inventoryUpdated;

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }


        public int GetSize()
        {
            return inventorySlots.Length;
        }

        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            inventorySlots[i].inventoryItem = item;
            inventorySlots[i].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].inventoryItem, item))
                {
                    return true;
                }
            }
            return false;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return inventorySlots[slot].inventoryItem;
        }

        public int GetNumberInSlot(int slot)
        {
            return inventorySlots[slot].number;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            inventorySlots[slot].number -= number;
            if (inventorySlots[slot].number <= 0)
            {
                inventorySlots[slot].number = 0;
                inventorySlots[slot].inventoryItem = null;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }


        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (inventorySlots[slot].inventoryItem != null)
            {
                return AddToFirstEmptySlot(item, number); ;
            }

            var i = FindStack(item);
            if (i >= 0)
            {
                slot = i;
            }

            inventorySlots[slot].inventoryItem = item;
            inventorySlots[slot].number += number;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        private void Start()
        {
            AddToFirstEmptySlot(InventoryItem.GetFromID("440c070e-d32e-4e7d-b261-e73515de5a0b"), 1);
        }


        private void Awake()
        {
            inventorySlots = new InventorySlot[inventorySize];
        }

        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].inventoryItem == null)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable)
            {
                return -1;
            }

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].inventoryItem, item))
                {
                    return i;
                }
            }
            return -1;
        }


        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }

        public object CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (inventorySlots[i].inventoryItem != null)
                {
                    slotStrings[i].itemID = inventorySlots[i].inventoryItem.ItemID;
                    slotStrings[i].number = inventorySlots[i].number;
                }
            }
            return slotStrings;
        }

        public void RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots[i].inventoryItem = InventoryItem.GetFromID(slotStrings[i].itemID);
                inventorySlots[i].number = slotStrings[i].number;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }


    }

}

