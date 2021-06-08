using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Something was hit");
    }
}
