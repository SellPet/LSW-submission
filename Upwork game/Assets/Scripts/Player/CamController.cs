using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public Transform target;
    public bool _follow = true;
    public float smoothness = 10f;
    public float size;
    private Camera cam;
    void Awake()
    {
        cam = GetComponent<Camera>();
    }
    void LateUpdate(){
        // making smooooooth camera follow action // very smuuth :> // writing this in a cute way to save this shitcode //
        if(_follow){
            Vector3 goDir = new Vector3(target.localPosition.x, target.localPosition.y, -10);
            transform.localPosition = Vector3.Lerp(transform.localPosition, goDir, Time.deltaTime * smoothness);

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, Time.deltaTime * smoothness);
        }
    }
}
