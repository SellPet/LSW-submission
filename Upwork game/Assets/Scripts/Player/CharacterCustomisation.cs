using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CharacterCustomisation : MonoBehaviour
{
    public Image[] changable;
    [SerializeField] 
    private GraphicRaycaster cc_Raycaster;
    PointerEventData cc_PointerEventData;
    [SerializeField] 
    private EventSystem cc_EventSystem;
    private Canvas canvas;
    public RectTransform selec_rec;
    public SelectionMenu selec_menu; 
    public bool toggleWritingState;
    private Transform hit_results;
    public int maxLet;
    string text = "";
    TMPro.TextMeshProUGUI gui_text;
    bool isOn;
    private PlayerInfoManager player_info_manager;
    public TMPro.TextMeshProUGUI Name, Surename;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        player_info_manager = FindObjectOfType<PlayerInfoManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isOn){
            // Hide Menu if Cursor is too far from It // scalefactor to keep the same size //
            Vector2 Vec_dist = (Input.mousePosition - selec_rec.position) / canvas.scaleFactor;
            float dist = Vec_dist.magnitude;
            if(dist > 35f){
                hideMenu();
            }
        }
        // Text //
        if(Input.GetMouseButtonDown(0)){

            hit_results = checkPointer();
            toggleWritingState = isWriting();
        }
        // writing state //
        if(toggleWritingState){
            if(hit_results != null){
                keyboardWrite();
            }
        }
        // selection // 
        if(Input.GetMouseButtonDown(1)){
            hit_results = checkPointer();
            if(hit_results.CompareTag("Hair"))
            ItemClicked(hit_results.GetComponent<Custom_Item>());
        }

    }
    public void ItemClicked(Custom_Item itemInfo){
        isOn = true;
        // Revealing Selection Menu // 
        selec_rec.gameObject.SetActive(true);

        itemInfo.c_customisation = this;
        // Changing Equip Buttons depending on if the item is equiped or not // Created different method to be able to change it from ItemInfo script too // Flexibility //
        if(!itemInfo.isEquiped){
            createEquipButton(itemInfo);
        }
        else{
            createUnequipButton(itemInfo);
        }

        // Interaction Menu Position // for it to appear At the corner of the pointer // Multiplying by ScaleFactor to not give a fuck about screen size change //
        Vector2 target_pos = Input.mousePosition + new Vector3(selec_rec.sizeDelta.x,-selec_rec.sizeDelta.y,0) * canvas.scaleFactor;
        selec_rec.position = target_pos;
    }
    private void hideMenu(){
        // Hiding selection Menu //
        isOn = false;
        selec_rec.gameObject.SetActive(false);
    }

    public void createEquipButton(Custom_Item itemInfo){
        // Changing buttons //
        selec_menu.doEquip();

        // Clearing All listeners // so no endless stacks //
        Button selectionButton = selec_menu.transform.Find("Eq").GetComponent<Button>();
        selectionButton.onClick.RemoveAllListeners(); 
        

        // Adding listener to the button // So when pressed it will trigger Item Info to change it's state //
        selectionButton.onClick.AddListener(itemInfo.Equip); 
        
    }
    public void changeName(){
        // Update Name in player info manager //
        player_info_manager._name = Name.text;
        player_info_manager._surename = Surename.text;
    }

    public void createUnequipButton(Custom_Item itemInfo){
        // Changing buttons //
        selec_menu.unEquip();

        Button selectionButton = selec_menu.transform.Find("Uneq").GetComponent<Button>();
        // Clearing All listeners // so no endless stacks //
        selectionButton.GetComponent<Button>().onClick.RemoveAllListeners();

        // Adding listener to the button // So when pressed it will trigger Item Info to change it's state //
        selectionButton.GetComponent<Button>().onClick.AddListener(itemInfo.UnEquip);   
    }
    
    private bool isWriting(){
        if(hit_results != null && hit_results.CompareTag("writable")){ gui_text = hit_results.GetComponent<TMPro.TextMeshProUGUI>(); text = gui_text.text; return true;}
        return false;
    }
    private void keyboardWrite(){
        // simple script for storing writen characters - into string //
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (text.Length != 0) text = text.Substring(0, text.Length - 1);                   
            }
            else if(text.Length < maxLet) text += c;
        }
        // display on gui
        gui_text.text = text;
    }
    // Creating simple method to track Pointer //
    private Transform checkPointer(){
        // Update List // 
        List<RaycastResult> results = new List<RaycastResult>();
        // Creating pointer event system, to be flexible //
        cc_PointerEventData = new PointerEventData(cc_EventSystem);
        cc_PointerEventData.position = Input.mousePosition;
        cc_Raycaster.Raycast(cc_PointerEventData, results);
        
        
        // if raycast hit - get the result back //
        if(results.Count > 0){
            Transform hit_tr;
            hit_tr = results[0].gameObject.transform;
            return hit_tr;
        }
        return null;

    }
}
