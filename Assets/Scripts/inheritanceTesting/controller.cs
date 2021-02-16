using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var thisplayer = new player();
        thisplayer.moveLeft();
    }

  
}
