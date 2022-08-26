using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Market_system : MonoBehaviour
{

    [HideInInspector]
    public CamController camControll;
    public Canvas marketCanvas;
    public float camSize;
    public float waitTime;

    private bool isOn;
    public bool refreshCatalog;
    private ShopkeeperManager keeper_manager;

    public Inventory_Items global_Inventory;

    public List<Transform> marketCatalog = new List<Transform>();

    public Transform inventItems_parent;
    
    [HideInInspector] public List<Transform> inventItems = new List<Transform>();
    public Transform marketItems_parent;
    
    [HideInInspector] public List<Transform> marketItems = new List<Transform>();

    [SerializeField] 
    private GraphicRaycaster cc_Raycaster;
    PointerEventData cc_PointerEventData;
    [SerializeField] 
    private EventSystem cc_EventSystem;
    [SerializeField] 
    private RectTransform canvasRect;
    private SelectionMenu selec_menu;
    public RectTransform selec_rec;
    private ItemInfo II;
    private PlayerController pc;

    void Start()
    {
        keeper_manager = GetComponent<ShopkeeperManager>();
        selec_menu = selec_rec.GetComponent<SelectionMenu>();
        pc = keeper_manager.pc;

    }
    void Awake(){
        // get all invent spots //
        inventItems = new List<Transform>();
        marketItems = new List<Transform>();
        for (int i = 0; i < inventItems_parent.childCount; i++)
        {
            inventItems.Add(inventItems_parent.GetChild(i));
        }
        // get all market spots //
        for (int i = 0; i < marketItems_parent.childCount; i++)
        {
            marketItems.Add(marketItems_parent.GetChild(i));
        }

    }
    void Update()
    {
        // Exit Market //
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)){
            marketExit();
        }
        // if we refresh our inventory in global we refresh in market too // Up to date //
        if(global_Inventory != null && global_Inventory.refreshMarketInventory){
            RefreshInvent();
        }
       
        if(isOn){
            // Hide Menu if Cursor is too far from It // scalefactor to keep the same size //
            Vector2 Vec_dist = (Input.mousePosition - selec_rec.position) / marketCanvas.scaleFactor;
            float dist = Vec_dist.magnitude;
            if(dist > 35f){
                hideMenu();
            }
        }
        
        if(Input.GetMouseButtonDown(1)){
            Transform hit_results = checkPointer();
            if(hit_results != null){
                // if hit the inventory item button // And it has an Item //
                if(hit_results.CompareTag("Invent_Item") && hit_results.childCount == 1){
                    II = hit_results.GetComponentInChildren<ItemInfo>();
                    ItemClicked(II);
                    CreateSellButton(II);
                }

                // if hit the Market item button // And it has an Item //
                if(hit_results.CompareTag("Market_Item") && hit_results.childCount == 1){
                    II = hit_results.GetComponentInChildren<ItemInfo>();
                    ItemClicked(II);
                    CreateBuyButton(II);
                }
            }
        }
        if(refreshCatalog && marketItems.Count != 0){
            renewCatalog();
        }
    }
    private void renewCatalog(){
        // Update catalog based on Market Items //
        for (int i = 0; i < marketItems.Count; i++)
        {
            // Destroy if it already has an Item //
            if(marketItems[i].childCount != 0){
                Destroy(marketItems[i].GetChild(0).gameObject);
            }
            // Instantiate new Item //
            if(i < marketCatalog.Count){
                RectTransform instrec = Instantiate(marketCatalog[i].gameObject, marketItems[i].transform).GetComponent<RectTransform>();
                instrec.sizeDelta = marketItems[i].GetComponent<RectTransform>().sizeDelta;
            }
            
        }
        refreshCatalog = false;
    }
    public void updateCharacter(string sprite_name){
        for (int i = 0; i < pc.sprite_r.Length; i++)
        {
            if(pc.sprite_r[i].sprite.name == sprite_name){
                pc.sprite_r[i].sprite = (Sprite)Resources.Load("otherMeshes/Trans", typeof(Sprite));
            } 
        }
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

        // Interaction Menu Position // for it to appear At the corner of the pointer // Multiplying by ScaleFactor to not give a fuck about screen size change //
        Vector2 target_pos = Input.mousePosition + new Vector3(selec_rec.sizeDelta.x,-selec_rec.sizeDelta.y,0) * marketCanvas.scaleFactor;
        selec_rec.position = target_pos;
        isOn = true;
    }
    public void hideMenu(){
        // Hiding selection Menu //
        isOn = false;
        selec_rec.gameObject.SetActive(false);
    }
    public void CreateSellButton(ItemInfo itemInfo){
        // Changing buttons //
        selec_menu.doEquip();

        // change price to selection menu display //
        selec_menu.changePrice(itemInfo.price);

        Button selectionButton = selec_menu.transform.Find("Sell").GetComponent<Button>();
        // Clearing All listeners // so no endless stacks //
        selectionButton.onClick.RemoveAllListeners(); 
        
        // Adding listener to the button // So when pressed it will trigger Item Info to change it's state //
        selectionButton.onClick.AddListener(itemInfo.Sell);

    }

    public void CreateBuyButton(ItemInfo itemInfo){
        // Changing buttons //
        selec_menu.unEquip();

        // change price to selection menu display //
        selec_menu.changePrice(itemInfo.price);

        Button selectionButton = selec_menu.transform.Find("Buy").GetComponent<Button>();
        // Clearing All listeners // so no endless stacks //
        selectionButton.onClick.RemoveAllListeners(); 
        
        // Adding listener to the button // So when pressed it will trigger Item Info to change it's state //
        selectionButton.onClick.AddListener(itemInfo.Buy);
    }
    public void RefreshInvent(){
        // refreshing inventory items with global item update // Up to date //
        for (int i = 0; i < inventItems.Count; i++)
        {
            if(inventItems[i].childCount != 0){
                Destroy(inventItems[i].GetChild(0).gameObject);
            }
            if(i < global_Inventory.itemPrefabs.Count && global_Inventory.itemPrefabs[i] != null){
                GameObject inst = Instantiate(global_Inventory.itemPrefabs[i].gameObject, inventItems[i].transform);
                RectTransform instrec = inst.GetComponent<RectTransform>();
                instrec.sizeDelta = inventItems[i].GetComponent<RectTransform>().sizeDelta;
            }
            
            
        }
        // stop refreshing //
        global_Inventory.refreshMarketInventory = false;
    }
    public IEnumerator Initialize(){
        // changing camera target and target size //
        camControll.size = camSize;
        camControll.target = this.transform;

        // smooth appearance could be done by animation // however i am limited by time // so i chose easier solution //
        yield return new WaitForSeconds(waitTime);
        marketCanvas.enabled = true;

        // To keep inventory with market inventory up to date // + catalog //
        RefreshInvent();
        refreshCatalog = true;
    }
    void marketExit(){
        // if escape before the coroutine // to stop canvas appearing //
        StopCoroutine("Initialize");
        
        marketCanvas.enabled = false;

        camControll.target = keeper_manager.pc.transform;
        camControll.size = keeper_manager.pc.standardCamSize;

        keeper_manager.pc.enabled = true;
        this.enabled = false;
    }
}
