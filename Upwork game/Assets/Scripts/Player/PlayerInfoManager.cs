using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public string _name,_surename;
    public GameObject[] currentlyOn;
    public static PlayerInfoManager instance;
    void Awake()
    {
        // Dont Destroy On Load system //
        if(instance == null){
            instance = this;
        }else {Destroy(gameObject); return;}
        DontDestroyOnLoad(gameObject);
    }
}
