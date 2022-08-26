using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Item : MonoBehaviour
{
    public bool isEquiped; 
    public Sprite txtre;
    public CharacterCustomisation c_customisation;
    public Custom_Item custom_redirect;
    public GameObject prefab;
    private PlayerInfoManager infoManager;
    public string modtag;
    void Start()
    {
        modtag = transform.tag;
        infoManager = FindObjectOfType<PlayerInfoManager>();
    }

    public void Equip(){
        // Going Thru Changable objects in the Canvas character // 
        for(int j = 0; j < c_customisation.changable.Length; j++){
            // If current item tag == to the canvas character Item tag //
            if(c_customisation.changable[j].CompareTag(modtag)){
                infoManager.currentlyOn[0] = prefab;
                // Setting Item Redirect //

                custom_redirect = c_customisation.changable[j].GetComponent<itemInfo_Redirect>().cust_Item;

                // if the old item is not this item //
                if(custom_redirect != null && custom_redirect != this){
                    // then the old one is disabled //
                    custom_redirect.Replace_Equip();
                }

                // changing sprite // setting this to Equiped // and setting redirect to new Item //
                c_customisation.changable[j].sprite = txtre;
                c_customisation.changable[j].GetComponent<itemInfo_Redirect>().cust_Item = this;
                
                // changing button to Unequiped // So you don't need to hide the menu and reapear it for the buttons to change // 
                isEquiped = true; 
                
                c_customisation.createUnequipButton(this);
                
            }
        }
        
        
    }
    public void UnEquip(){
        isEquiped = false; 
        for(int j = 0; j < c_customisation.changable.Length; j++){

            if(c_customisation.changable[j].CompareTag(modtag)){
                infoManager.currentlyOn[0] = null;
                // Setting Redirect to null //
                custom_redirect = c_customisation.changable[j].GetComponent<itemInfo_Redirect>().cust_Item;
                custom_redirect = null;

                // This is Just an Unequip, so nothing will go here // Meaning Transparent texture will take place // e.x Bald Head :) //
                Sprite txt = (Sprite)Resources.Load("otherMeshes/Trans", typeof(Sprite));
                c_customisation.changable[j].sprite = txt;

                // changing button to equiped // So you don't need to hide the menu and reapear it for the buttons to change //
                isEquiped = false;
               
                
                c_customisation.createEquipButton(this); 
            }
        }
    }
    public void Replace_Equip(){
        // unequip // without deleting anything // In case of Replacment //
        isEquiped = false;
    }
}
