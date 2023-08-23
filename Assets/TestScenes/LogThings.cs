using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogThings : MonoBehaviour
{

    float timeBetweenPrints = 0.5f;
    float timeSinceLastPrint = 0.0f;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastPrint += Time.deltaTime;
        if(timeSinceLastPrint >= timeBetweenPrints)
        {
            Debug.Log("I'm printing things");
            timeSinceLastPrint = 0.0f;
        }
    }
}
