using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    [SerializeField]
    private string trig_layer;
    private ShopkeeperManager keeper_manager;

    void Start()
    {
        keeper_manager = GetComponentInParent<ShopkeeperManager>();
    }
    void OnTriggerEnter2D(Collider2D other){
        // if close to keeper //
        if(other.gameObject.layer == LayerMask.NameToLayer(trig_layer)){
            keeper_manager.close_enough = true;

            keeper_manager.player_go = other.gameObject;
            keeper_manager.pc = other.gameObject.GetComponent<PlayerController>();
            keeper_manager.pc.keeper_manager = keeper_manager;
        }
        else{
            keeper_manager.close_enough = false;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        keeper_manager.close_enough = false;
    }
}
