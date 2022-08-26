using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperManager : MonoBehaviour
{
    public bool close_enough;
    public Transform target_mark;
    private Vector3 curr_mark_pos;
    private Quaternion curr_mark_rot;
    private Market_system m_system;
    public Transform mark;
    [SerializeField]
    private float smoothness = 10;
    [HideInInspector]
    public GameObject player_go;
    public PlayerController pc;
    [HideInInspector]
    public Camera cam;


    void Awake()
    {
        curr_mark_pos = mark.localPosition;
        curr_mark_rot = mark.localRotation;
        m_system = GetComponent<Market_system>();
    }


    void Update()
    {
        if(close_enough){
            interaction();
        }
        else{
            closeInteraction();
        }
    }
    private void interaction(){
        
        // Exclamination mark Smooth appearance //
        mark.localPosition = Vector3.Slerp(mark.localPosition, target_mark.localPosition, Time.deltaTime * smoothness);
        mark.localRotation = Quaternion.Slerp(mark.localRotation, target_mark.localRotation, Time.deltaTime * smoothness / 2f);
    }
    private void closeInteraction(){

        // Exclamination mark Smooth dissappearance //
        mark.localPosition = Vector3.Lerp(mark.localPosition, curr_mark_pos, Time.deltaTime * smoothness / 1.5f);
        mark.localRotation = Quaternion.Slerp(mark.localRotation, curr_mark_rot, Time.deltaTime * smoothness / 2f);
    }
    public void OpenMarketMenu_keeper(){
        // IF Market has opened - interacted // enable market system //
        m_system.enabled = true;

        // set inventory items to market system //
        m_system.global_Inventory = pc.GetComponent<Inventory_Items>();
        m_system.camControll = cam.GetComponent<CamController>();
        m_system.StartCoroutine("Initialize");
    }
}
