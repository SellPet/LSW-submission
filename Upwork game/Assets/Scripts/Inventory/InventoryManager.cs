using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Canvas inventoryCanvas;
    public Inventory inv;
    private PlayerController pc;
    [HideInInspector]
    public SpriteRenderer[] sr;

    void Start()
    {
        pc = GetComponent<PlayerController>();
        initialize();
    }

    public void initialize(){
        if(pc.sprite_r != null){
        sr = pc.sprite_r;
        }
        // inventory UI //
        inventoryCanvas.enabled = true;
        inv = inventoryCanvas.GetComponent<Inventory>();

        // looping through current character all sprite renderers and looping through "about me" character's changable items to change them after initialization so they are in sync (by same tags) //
        for(int y = 0; y < sr.Length; y++){
            
            for(int j = 0; j < inv.changable.Length; j++){

                if(sr[y].tag == inv.changable[j].tag){
                    
                    inv.changable[j].sprite = sr[y].sprite;
                }
            }
        }
    }

    void Update()
    {
        // looping through "about me" character's changable items - and current character sprite renderers - so characters items are changed in realtime after 'about me' character (by same tags)"
        for(int j = 0; j < inv.changable.Length; j++){
            for(int y = 0; y < sr.Length; y++){

                if(inv.changable[j].tag == sr[y].tag){
                        
                    sr[y].sprite = inv.changable[j].sprite;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.I)){
            CloseInventory();
        }
        
        
    }
    public void CloseInventory(){
        PlayerController pc = GetComponent<PlayerController>();
        pc.enabled = true;
        inventoryCanvas.enabled = false;
        this.enabled = false;
    }
}
