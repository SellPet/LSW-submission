using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject menu, charactermenu;
    public void toCharacterCustom(){
        // Go to Character customisation //
        menu.SetActive(false);
        charactermenu.SetActive(true);
    }
    public void toMenu(){
        // go to menu //
        menu.SetActive(true);
        charactermenu.SetActive(false);
    }
    public void loadLevel(){
        // load game level //
        SceneManager.LoadScene(1);
    }
}
