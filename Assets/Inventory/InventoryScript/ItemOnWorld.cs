using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerInventory;

    public void AddNewItem()
    {

        if (!playerInventory.itemList.Contains(thisItem))
        {
            playerInventory.itemList.Add(thisItem);
            thisItem.itemHeld++;
        }
        else
        {
            thisItem.itemHeld++;
        }
        InventoryManager.RefreshItem();
    }
}
