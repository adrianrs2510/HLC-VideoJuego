using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class load : MonoBehaviour
{

    [SerializeField] private Menu menu;
    
    // Start is called before the first frame update
    void Start()
    {
        if (LoadState.load)
        {
            menu.Checkpoint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
