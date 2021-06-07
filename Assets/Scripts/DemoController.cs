using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    public GameObject[] allObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirstClick()
    {
        foreach (GameObject item in allObjects)
        {
            if (item.name.Equals("Cube"))
            {
                item.SetActive(true);

            }
        }
    }
}
