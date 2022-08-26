using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class Inventory : MonoBehaviour
{
    public Image[] changable;
    public List<Transform> AllItemPlaces = new List<Transform>();
    public TMPro.TextMeshProUGUI _name, _surename;
    public PlayerInfoManager pim;
    public RectTransform selec_rec;
    public SelectionMenu selec_menu; 
    public bool isOn;

    [SerializeField] 
    private GraphicRaycaster cc_Raycaster;
    PointerEventData cc_PointerEventData;
    [SerializeField] 
    private EventSystem cc_EventSystem;
    [SerializeField] 
    private RectTransform canvasRect;
    private Canvas canvas;

    public ItemInfo II;
    public bool syncCharacterWithInvent;
    void Awake()
    {
        pim = FindObjectOfType<PlayerInfoManager>();
        _name.text = pim._name;
        _surename.text = pim._surename;
        canvas = GetComponent<Canvas>();
        selec_menu = selec_rec.GetComponent<SelectionMenu>();
    }

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
        
        if(Input.GetMouseButtonDown(1)){
            // if hit the inventory item button // And it has an Item //
            Transform hit_results = checkPointer();
            if(hit_results != null && hit_results.CompareTag("Invent_Item") && hit_results.childCount == 1){
                II = hit_results.GetComponentInChildren<ItemInfo>();
                ItemClicked(II);
            }
        }
        if(syncCharacterWithInvent){
            //checkIfInInventory();
        }
    }
    void checkIfInInventory(){
        for (int i = 0; i < changable.Length; i++)
        {
            int a = 0;
            for (int j = 0; j < AllItemPlaces.Count; j++)
            {
                if(AllItemPlaces[j].childCount != 0){
                    ItemInfo iInfo = AllItemPlaces[j].GetComponentInChildren<ItemInfo>();
                    if(a == 0){
                    if(iInfo.isEquiped){
                
                        changable[i].sprite = iInfo.txtre;
                        a = 1;
                    }
                    else{
                        print(iInfo.name);
                        changable[i].sprite = (Sprite)Resources.Load("otherMeshes/Trans", typeof(Sprite));
                    }
                    }
                    
                }
                else{
        
                    print(AllItemPlaces.Count);
                    if(j == AllItemPlaces.Count - 1 && a == 0){
                        changable[i].sprite = (Sprite)Resources.Load("otherMeshes/Trans", typeof(Sprite));
                    }
                }     
            }
            
        }
        syncCharacterWithInvent = false;
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
    public void ItemClicked(ItemInfo itemInfo){
        // Revealing Selection Menu // 
        selec_rec.gameObject.SetActive(true);

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
        isOn = true;
    }
    private void hideMenu(){
        // Hiding selection Menu //
        isOn = false;
        selec_rec.gameObject.SetActive(false);
    }

    public void createEquipButton(ItemInfo itemInfo){
        // Changing buttons //
        selec_menu.doEquip();
        Button selectionButton = selec_menu.transform.Find("Eq").GetComponent<Button>();
        // Clearing All listeners // so no endless stacks //
        selectionButton.onClick.RemoveAllListeners(); 
        

        // Adding listener to the button // So when pressed it will trigger Item Info to change it's state //
        selectionButton.onClick.AddListener(itemInfo.Equip); 
        
    }

    public void createUnequipButton(ItemInfo itemInfo){
        // Changing buttons //
        selec_menu.unEquip();
        Button selectionButton = selec_menu.transform.Find("Uneq").GetComponent<Button>();
        // Clearing All listeners // so no endless stacks //
        selectionButton.onClick.RemoveAllListeners();

        // Adding listener to the button // So when pressed it will trigger Item Info to change it's state //
        selectionButton.onClick.AddListener(itemInfo.UnEquip);   
    }
}
