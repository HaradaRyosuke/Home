using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyItem(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DestroyItem(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
        yield return null;
    }
}
