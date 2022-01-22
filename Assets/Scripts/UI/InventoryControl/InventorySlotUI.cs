using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Dragging;
using RPG.InventoryControl;

namespace RPG.UI.InventoryControl
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        //Configuration
        [SerializeField] InventoryItemIcon icon = null;

        //State
        int index;
        InventoryItem item;
        RPG.InventoryControl.Inventory inventory;



        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index),  inventory.GetNumberInSlot(index));
        }

        public void AddItems(InventoryItem item, int number)
        {
            Debug.Log("InventorySlotUI AddItems = " + item.name + " " + number.ToString());
            inventory.AddItemToSlot(index, item, number);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            Debug.Log("InventorySlotUI Max Acceptable item = " + item.name);
            if (inventory.HasSpaceFor(item))
            {
                return item.MaxNumberInStack;
            }
            return 0;
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }


    }

}


