using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
    public Item updateItem(string input)
    {
        foreach (Item item in itemList)
        {
            if (item.pressButton.Contains(input))
            {
                if (input.Equals("2")) BulletController.increaseHarm(); 
                if (input.Equals("3")) GameObject.Find("Player").GetComponent<HealthManager>().ApplyHealing(); 
                if (input.Equals("1")) GameObject.Find("Player").GetComponent<PlayerController>().speedUp(); 
                item.itemHeld -= 1;
                if (item.itemHeld == 0)
                {
                    itemList.Remove(item);
                }
                InventoryManager.RefreshItem();
                return item;
            }
        }
        return null;
    }
}
