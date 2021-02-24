using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelerController : MonoBehaviour, ISpelerControllable
{
    Speler speler;

    KeyCode up, left, down, right;

    private void Start()
    {
        speler = gameObject.GetComponent<Speler>();
    }

    public void setKeyCodes(List<KeyCode> keycodes)
    {
        up = keycodes[0];
        left = keycodes[1];
        down = keycodes[2];
        right = keycodes[3];
    }
}
