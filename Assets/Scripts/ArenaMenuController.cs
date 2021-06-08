using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaMenuController : MonoBehaviour
{


    public GameObject mainMenu;
    public GameObject arena;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateArena()
    {
        arena.gameObject.SetActive(true);
    }
    
    public void GoBack()
    {
        arena.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
