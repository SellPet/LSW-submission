using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Inventory_Items : MonoBehaviour
{
    // ! This script is Global Character Inventory System // which will be the real inventory items state ! //
    public List<GameObject> itemPrefabs = new List<GameObject>();
    private PlayerController pc;
    
    // Trigger Used to refresh invent //
    public bool refreshInventory;
    public bool refreshMarketInventory;
    private Inventory inv;
    public float Money;
    public TMPro.TextMeshProUGUI money_gui;
    public List<bool> savedgo;
    void Awake()
    {
        inv = GetComponent<InventoryManager>().inv;   
    }

    void Start(){
        pc = GetComponent<PlayerController>();
        itemPrefabs.Add(FindObjectOfType<PlayerInfoManager>().currentlyOn[0]);

        // We set Characters Items from Dont Destroy PlayerInfoManager // so if we choose type of hair in the menu it goes to the level //
        if(itemPrefabs[0] != null){
            itemPrefabs[0].GetComponent<ItemInfo>().isEquiped = true;
        }
        else{
            itemPrefabs = new List<GameObject>();
        }

        for(int y = 0; y < pc.sprite_r.Length; y++){

            if(itemPrefabs.Count > 0 && itemPrefabs[0].tag == pc.sprite_r[y].tag){
             
                pc.sprite_r[y].sprite = itemPrefabs[0].GetComponent<ItemInfo>().txtre;  
            } 
        }
    }
    void Update()
    {
        // If Signal to refresh invent We Refresh //
        if(refreshInventory){
            RefreshInvent();
        }

        // setting Money to the gui // Up to date //
        money_gui.text = Money.ToString() + "$";
    }
    void RefreshInvent(){
        refreshMarketInventory = true;
        // Checking All existing Item places to Replace them with Current Inventory Items // Flexibility // 
        savedgo = new List<bool>();
        for (int i = 0; i < inv.AllItemPlaces.Count; i++)
        {
            // Destroy if allready has //
            if(inv.AllItemPlaces[i].childCount != 0){

                savedgo = new List<bool>();
                savedgo.Add(inv.AllItemPlaces[i].GetChild(0).GetComponent<ItemInfo>().isEquiped);
                Destroy(inv.AllItemPlaces[i].GetChild(0).gameObject);
            }
            // if it does not exceed current items lenght then instantiate them in inventory //
            if(i < itemPrefabs.Count && itemPrefabs[i] != null){  
                  
                GameObject newChild = Instantiate(itemPrefabs[i], inv.AllItemPlaces[i].transform);
                ItemInfo iInfo = newChild.GetComponent<ItemInfo>();
                if(savedgo.Count != 0){
                iInfo.isEquiped = savedgo[0];
                }
                newChild.transform.SetParent(inv.AllItemPlaces[i]);
                savedgo = new List<bool>();
            }
            
        }
        refreshInventory = false;
    }
}
