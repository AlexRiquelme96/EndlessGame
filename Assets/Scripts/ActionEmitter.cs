using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEmitter : MonoBehaviour
{
    public Transform lookAt;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = this.lookAt.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = this.lookAt.rotation;
        
    }
}
