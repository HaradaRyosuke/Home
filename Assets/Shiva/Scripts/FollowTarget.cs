using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target)
            transform.position = Vector3.Lerp(
                transform.position, 
                target.position + Vector3.forward * -10, 
                Time.deltaTime);
    }


}
