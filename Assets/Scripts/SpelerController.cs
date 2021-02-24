using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelerController : MonoBehaviour
{
    Speler speler;

    private void Start()
    {
        speler = gameObject.GetComponent<Speler>();
    }

}
