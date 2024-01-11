// @author M_Studio from website bilibili with link
// https://www.bilibili.com/video/BV1WJ411v7xD/?spm_id_from=333.788.recommend_more_video.0&vd_source=6a4ed8aa508c3eb47126d5dd4684892e
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    public Inventory myBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
        List<Item> itemList = myBag.itemList;
        foreach (Item item in myBag.itemList)
        {
            item.itemHeld = 0;
        }
        myBag.itemList.Clear();
    }
    // we are not likely to use this method if we don't hide the bag
    private void OnEnable()
    {
        RefreshItem();
    }

    private void Update()
    {
        Item itemConsumed = null;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemConsumed = myBag.updateItem("1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            itemConsumed = myBag.updateItem("2");
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            itemConsumed = myBag.updateItem("3");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            itemConsumed = myBag.updateItem("4");
        }
        if (itemConsumed != null)
        {
            GetComponent<AudioSource>().PlayOneShot(itemConsumed.audio);
        }

    }
    public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
        newItem.pressButton.text = item.pressButton;
    }

    public static void RefreshItem()
    {
        Transform slotGridTransform = instance.slotGrid.transform;
        int childCount = slotGridTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(slotGridTransform.GetChild(i).gameObject);

        }
        List<Item> bagItemList = instance.myBag.itemList;
        for (int i = 0; i < bagItemList.Count; i++)
        {
            CreateNewItem(instance.myBag.itemList[i]);
        }
    }
}
