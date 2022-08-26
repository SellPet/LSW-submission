using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectionMenu : MonoBehaviour
{
    public GameObject eq, uneq;
    public TMPro.TextMeshProUGUI price_gui;
    // change price //
    public void changePrice(float price){
        price_gui.text = price.ToString() + "$";
    }
    public void doEquip(){
        eq.SetActive(true);
        uneq.SetActive(false);
    }
    public void unEquip(){
        eq.SetActive(false);
        uneq.SetActive(true);
    }
}
