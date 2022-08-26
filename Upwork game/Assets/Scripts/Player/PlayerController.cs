using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 move_direction, previous_direcrion;
    [HideInInspector]
    public Rigidbody2D rb;
    [SerializeField]
    public float c_speed, c_accel,c_currentSpeed;
    public ShopkeeperManager keeper_manager;
    public Camera cam;
    public float standardCamSize;
    [HideInInspector]
    public Inventory inv;
    private InventoryManager invent_manager;
    public SpriteRenderer[] sprite_r;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inv = GetComponent<Inventory>();
        invent_manager = GetComponent<InventoryManager>();
        sprite_r = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Flexible Input System //
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // calculate direction //
        move_direction = new Vector3(x, y, 0);

        // if moving //
        if(x != 0 || y != 0){
            // creating previous direction for it to not get zero-ed (so i can slow down character smoothly with speed) //
            previous_direcrion = move_direction;

            // if both pressed // moving diagonaly // dividing by Sqrt(2) so no velocity add up //
            if(x != 0 && y != 0){
                c_currentSpeed = Mathf.Lerp(c_currentSpeed, c_speed / Mathf.Sqrt(2), Time.deltaTime * c_accel);  
            }
            else{
                c_currentSpeed = Mathf.Lerp(c_currentSpeed, c_speed, Time.deltaTime * c_accel);
            }

        }
        // slow down smoothly //
        else{
            c_currentSpeed = Mathf.Lerp(c_currentSpeed, 0, Time.deltaTime * c_accel * 2);
        }
        // open inventory //
        if(Input.GetKeyDown(KeyCode.I)){
            OpenInventory();
        }
        if(keeper_manager != null && keeper_manager.close_enough){
            if(Input.GetKeyDown(KeyCode.E)){
                OpenMarketMenu_player();
            }
        }
        // movement // No need for fixed update // constant - no physics based //
        rb.velocity = previous_direcrion * c_currentSpeed;
    }

    public void OpenInventory(){
        // stopping player //
        c_currentSpeed = 0;
        rb.velocity = Vector2.zero;

        // inventory manager enabled and initialized // this (player) script turned off //

        invent_manager.enabled = true;
        invent_manager.initialize();
        this.enabled = false;
    }
    public void OpenMarketMenu_player(){
        // stopping player //
        c_currentSpeed = 0;
        rb.velocity = Vector2.zero;

        // Letting them know Current player cam info // Flexibility // 
        
        keeper_manager.cam = cam;
        keeper_manager.OpenMarketMenu_keeper();
        this.enabled = false;
    }
}
