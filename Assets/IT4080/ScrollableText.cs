using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;

public class ScrollableText : MonoBehaviour
{
    public TMPro.TMP_Text txtText;

    // Start is called before the first frame update
    void Start()
    {
        txtText.text = "Hello World";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
