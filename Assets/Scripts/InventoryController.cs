using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {
    [HideInInspector]
    public Dictionary<int, Items> inventory;
    float inventoryWeight;

    int currentInventoryCount;
    public GameObject bagOverlay;
    Transform[] bagOverlayChildren;

    // Use this for initialization
    void Start () {
        inventoryWeight = 0;
        inventory = new Dictionary<int, Items>(30);
        AddItem("hands",1);

        bagOverlayChildren = bagOverlay.transform.GetComponentsInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void ToggleInv()
    {
        if (bagOverlay.activeSelf)
            bagOverlay.SetActive(false);
        else
            bagOverlay.SetActive(true);
    }
    
    public void AddItem(string item, int quantity)
    {
        Items itemAdding = Persistence.persistence.FindItem(item);
        if (itemAdding == null)
        {
            Debug.LogError("Item " + item + " not found");
            return;
        }

        if (currentInventoryCount == 9 && !itemAdding.isStackable)
            return;
                
        if (Persistence.persistence.itemsCount[itemAdding.itemName] < 1)
        {
            Persistence.persistence.itemsCount[itemAdding.itemName]+= quantity;
            if (itemAdding.isDroppable)
            {
                itemAdding.inventorySlot = FindFirstOpenBagSlot();
                inventory.Add(itemAdding.inventorySlot, itemAdding);

                AddBagSlotUI(itemAdding);
                Persistence.persistence.gm.UpdateHUDStats();
                currentInventoryCount++;
            }
            else
            {
                itemAdding.inventorySlot = FindFirstOpenSlot();
                inventory.Add(itemAdding.inventorySlot, itemAdding);
                //Add notweights to bag UI as list of items
            }
            AddWeight(itemAdding);
        }
        else if (Persistence.persistence.itemsCount[itemAdding.itemName] >= 1 && itemAdding.isStackable)
        {
            Persistence.persistence.itemsCount[itemAdding.itemName]+= quantity;
            UpdateBagSlotUINumber(itemAdding.inventorySlot, Persistence.persistence.itemsCount[itemAdding.itemName]);
            Persistence.persistence.gm.UpdateHUDStats();
        }
    }

    public void DropItem(int itemSlot)
    {
        Items item;
        inventory.TryGetValue(itemSlot, out item);
        if (item != null && Persistence.persistence.itemsCount[item.itemName] > 0)
        {
            Persistence.persistence.itemsCount[item.itemName]--;
            DropWeight(item);
            if (Persistence.persistence.itemsCount[item.itemName] > 0)
            {
                UpdateBagSlotUINumber(item.inventorySlot, Persistence.persistence.itemsCount[item.itemName]);
                Persistence.persistence.gm.UpdateHUDStats();
            }
            else
            {
                inventory.Remove(itemSlot);
                RemoveBagSlotUI(itemSlot);
                if (item.itemType == Items.iType.gun)
                    Persistence.persistence.gm.player.GetComponentInChildren<GunSwitch>().DropWeapon((int)item.gunType);
            }

            //CreateInWorld(item); TODO
        }
    }

    public void DropAllItems()
    {
        for (int i = 1; i < 10; i++)
            DropItem(i);
    }

    public int FindFirstOpenBagSlot()
    {
        for (int i = 1; i < 10; i++)
        {
            Items item;
            if (!inventory.TryGetValue(i, out item))
                return i;
        }
        return -1;
    }
    public int FindFirstOpenSlot()
    {
        for (int i = 10; i < 31; i++)
        {
            Items item;
            if (!inventory.TryGetValue(i, out item))
                return i;
        }
        return -1;
    }

    private void AddBagSlotUI(Items item)
    {
        GameObject slot = null;
        foreach (Transform child in bagOverlayChildren)
        {
            if (child.name == "slot" + item.inventorySlot.ToString())
                slot = child.gameObject;
        }
        if (slot == null)
            slot = bagOverlay.transform.GetChild(bagOverlay.transform.childCount).gameObject;

        GameObject go = (GameObject)Instantiate(Resources.Load("inv" + item.itemName));
        go.transform.SetParent(slot.transform);
        go.transform.localPosition = new Vector3(0, 0, 0);

        GameObject go2 = (GameObject)Instantiate(Resources.Load("invItemCount"));
        go2.name = "invItemCount" + item.inventorySlot.ToString();
        go2.transform.SetParent(slot.transform);
        go2.transform.localPosition = new Vector3(25, -25, 0);
        go2.GetComponent<Text>().text = Persistence.persistence.itemsCount[item.itemName].ToString();
    }

    private void RemoveBagSlotUI(int bagSlot)
    {
        GameObject slotIcon = null;
        foreach (Transform child in bagOverlayChildren)
        {
            if (child.name == "slot" + bagSlot.ToString())
                slotIcon = child.gameObject;
        }
        if (slotIcon == null)
            slotIcon = bagOverlay.transform.GetChild(bagOverlay.transform.childCount).gameObject;

        for (int i = 0; i < slotIcon.transform.childCount; i++)
        {
            Destroy(slotIcon.transform.GetChild(i).gameObject);
        }
    }

    public void UpdateBagSlotUINumber(int bagSlot, int num)
    {
        GameObject itemNum = null;
        foreach (Transform child in bagOverlayChildren)
        {
            if (child.name == "slot" + bagSlot.ToString())
            {
                itemNum = child.GetChild(1).gameObject;
            }

        }
        itemNum.GetComponent<Text>().text = num.ToString();
    }

    public float GetWeight()
    {
        return inventoryWeight;
    }

    public void AddWeight(Items item)
    {
        inventoryWeight += item.weight + (int)item.handability * .5f;
    }

    public void DropWeight(Items item)
    {
        inventoryWeight -= item.weight + (int)item.handability * .5f;
    }
}
