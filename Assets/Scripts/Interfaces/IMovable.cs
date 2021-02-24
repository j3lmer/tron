using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    void directionChanger(Vector3 direction);
    void spawnWall();

    void fitColliderBetween(Collider2D co, Vector2 a, Vector2 b);
    
}
