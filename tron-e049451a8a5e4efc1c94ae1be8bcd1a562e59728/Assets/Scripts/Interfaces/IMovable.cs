using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    void directionChanger(Vector3 direction);
    void spawnWall();
    void fitColliderBetween(Collider2D co, Vector3 a, Vector3 b);
    void die();    
}
