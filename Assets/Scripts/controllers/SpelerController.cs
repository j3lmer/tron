using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelerController : MonoBehaviour
{
    Speler speler;

    KeyCode up, left, down, right;



    private void Start()
    {
        speler = gameObject.GetComponent<Speler>();
    }   

    private void Update()
    {
        checkInputs();       
    }

    void checkInputs()
    {        
        if (Input.GetKeyDown(up))
        {
            if (speler.lastdir != Vector3.down)
            {
                speler.directionChanger(Vector3.up);
            }
        }

        if (Input.GetKeyDown(down))
        {
            if (speler.lastdir != Vector3.up)
            {
                speler.directionChanger(Vector3.down);
            }
        }

        if (Input.GetKeyDown(left))
        {
            if (speler.lastdir != Vector3.right)
            {
                speler.directionChanger(Vector3.left);
            }
        }
        if (Input.GetKeyDown(right))
        {
            if (speler.lastdir != Vector3.left)
            {
                speler.directionChanger(Vector3.right);
            }
        }               
    }


    public void setKeyCodes(KeyCode[] keycodes)
    {
        up = keycodes[0];
        left = keycodes[1];
        down = keycodes[2];
        right = keycodes[3];
    }
}
