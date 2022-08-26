using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsMovement : MonoBehaviour
{
    private Animator anim;
    private PlayerController pc; 
    private Rigidbody2D pc_rb;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        pc = GetComponentInParent<PlayerController>();
        pc.rb = pc.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        // making relationship with moving speed and animation speed (for pretty and smooth animation) //
        if(pc.rb.velocity.magnitude > .03f){
            anim.SetBool("walk", true);
            anim.SetFloat("speed", pc.rb.velocity.magnitude);
        }
        else{
            anim.SetBool("walk", false);
        }
    }
}
