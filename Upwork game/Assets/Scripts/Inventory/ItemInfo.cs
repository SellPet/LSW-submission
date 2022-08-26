using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ItemInfo : MonoBehaviour
{
    public bool isEquiped; 
    public int price;
    public Sprite txtre;
    public Inventory inv;
    public ItemInfo itemInfo_redirect;
    // for the global inventory to not lose game object after refresh //
    public GameObject itself_prefab;
    // Which of the modules is it - Hair, instruments, eyes, nose, or other -- made it to be very flexible //
    public string modtag;
    void Start()
    {
        modtag = transform.tag;
        inv = transform.root.GetComponent<Inventory>();
    }

    public void Equip(){
        // Going Thru Changable objects in the Canvas character // 
        for(int j = 0; j < inv.changable.Length; j++){
            // If current item tag == to the canvas character Item tag //
            if(inv.changable[j].CompareTag(modtag)){

                // Setting Item Redirect //

                itemInfo_redirect = inv.changable[j].GetComponent<itemInfo_Redirect>().Iinfo;

                // if the old item is not this item //
                if(itemInfo_redirect != null && itemInfo_redirect != this){
                    // then the old one is disabled //
                    itemInfo_redirect.Replace_Equip();
                }

                // changing sprite // setting this to Equiped // and setting redirect to new Item //
                inv.changable[j].sprite = txtre;
                inv.changable[j].GetComponent<itemInfo_Redirect>().Iinfo = this;
                
                // changing button to Unequiped // So you don't need to hide the menu and reapear it for the buttons to change // 
                isEquiped = true; 
                
                inv.createUnequipButton(this);
                
            }
        }
        
        
    }
    public void UnEquip(){
        isEquiped = false; 
        for(int j = 0; j < inv.changable.Length; j++){

            if(inv.changable[j].CompareTag(modtag)){
                // Setting Redirect to null //
                itemInfo_redirect = inv.changable[j].GetComponent<itemInfo_Redirect>().Iinfo;
                itemInfo_redirect = null;

                // This is Just an Unequip, so nothing will go here // Meaning Transparent texture will take place // e.x Bald Head :) //
                Sprite txt = (Sprite)Resources.Load("otherMeshes/Trans", typeof(Sprite));
                inv.changable[j].sprite = txt;

                // changing button to equiped // So you don't need to hide the menu and reapear it for the buttons to change //
                isEquiped = false;
               
                
                inv.createEquipButton(this); 
            }
        }
    }
    public void Replace_Equip(){
        // unequip //
        isEquiped = false;
    }
    public void Sell(){
        Market_system m_s = transform.root.GetComponent<Market_system>();

        // If sold the item character already has on // we should take it off of the character //
        m_s.updateCharacter(GetComponent<Image>().sprite.name);

        Inventory_Items global_Inventory = m_s.global_Inventory;

        global_Inventory.itemPrefabs.RemoveAt(Convert.ToInt16(transform.parent.name) - 1);
        global_Inventory.refreshInventory = true;
        
        global_Inventory.Money += price;

        m_s.hideMenu();
    }
    public void Buy(){
        Market_system m_s = transform.root.GetComponent<Market_system>();

        Inventory_Items global_Inventory = m_s.global_Inventory;
        if(global_Inventory.Money >= price){
            global_Inventory.Money -= price;
        if(global_Inventory.itemPrefabs.Count < 9){
            global_Inventory.itemPrefabs.Add(itself_prefab);
        }

        global_Inventory.refreshInventory = true;

        }
        m_s.hideMenu();
    }
}
