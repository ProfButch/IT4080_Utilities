using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogThings : MonoBehaviour
{

    float timeBetweenPrints = 0.5f;
    float timeSinceLastPrint = 0.0f;
    float timesPrinted = 0;



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
            timesPrinted += 1;
            Debug.Log($"I'm printing things {timesPrinted}");
            timeSinceLastPrint = 0.0f;
        }
    }
}
