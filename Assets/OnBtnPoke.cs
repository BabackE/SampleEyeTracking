using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBtnPoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to execute when the button is pressed
    public void OnBZeroPoke()
    {
        Debug.Log("Button Pressed!"); // Replace with your custom action
        // Example: Change color of the button
        GetComponent<Renderer>().material.color = Color.green;
    }

    public void OnBTwoPoke()
    {
        Debug.Log("Button Pressed!"); // Replace with your custom action
        // Example: Change color of the button
        GetComponent<Renderer>().material.color = Color.green;
    }
}
