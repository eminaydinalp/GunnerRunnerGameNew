using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deneme : MonoBehaviour
{
    
    void Start()
    {
        InvokeRepeating(nameof(selam), 2, 3);
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CancelInvoke(nameof(selam));
        }
    }

    void selam()
    {
        Debug.Log("Selam");
        
    }
}
