//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public class NavMeshCorners : MonoBehaviour
//{
//	public float moveSpeed = 3.5f;
//    public float minDistanceCheck = 0.5f;

//	public Transform target;
//	private NavMeshPath _path;
//private NavMeshPath path
//{
//    get
//    {
//        return _path;
//    }
//    set
//    {
//        _path = value;
//        //if(_path == null)
//        //{
//        //    NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
//        //}
//    }
//}
//	private float elapsed = 0.0f;
//	private int currentPathIndex = 1;
//    private int lastCornersLength = 0;
//    private Vector3 offset;

//	void Start()
//	{
//		path = new NavMeshPath();
//		elapsed = 0.0f;
//        StartCoroutine(FindPath());
//    }

//    private IEnumerator FindPath()
//    {
//        while (true)
//        {
//            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
//            yield return new WaitForSeconds(1);
//        }

//    }

//    void Update()
//    {
//        if(path != null)
//        {
//            lastCornersLength = path.corners.Length;
//        }

//        for (int i = 0; i < path.corners.Length - 1; i++)
//        {
//            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
//        }

//        if(path.corners != null && path.corners.Length > 0)
//        {
//            offset = (path.corners[currentPathIndex] - this.transform.position);
//            offset.y = 0;



//            if (offset.x > offset.y && path.corners[currentPathIndex].y < this.transform.position.y)
//            {
//                direction = dirDown;
//                movement(direction);


//                //yield return new WaitForSeconds(randomTime);
//            }
//            else if (offset.x <= offset.y && path.corners[currentPathIndex].y >= this.transform.position.y)
//            {
//                direction = dirUp;
//                movement(direction);


//                //yield return new WaitForSeconds(randomTime);
//            }


//            //left&right
//            else if (offset.x <= offset.y && path.corners[currentPathIndex].x < this.transform.position.x)
//            {
//                direction = dirLeft;
//                movement(direction);


//                //yield return new WaitForSeconds(randomTime);
//            }
//            else if (offset.x > offset.y && path.corners[currentPathIndex].x >= this.transform.position.x)
//            {
//                direction = dirRight;
//                movement(direction);


//                //yield return new WaitForSeconds(randomTime);
//            }

//            //randomTime = Random.Range(0, 0.75f);
//            //playerOffset = Speler2D.transform.position - this.transform.position;
//        }

//        //Debug.Log("CURRENT PATH INDEX: " + currentPathIndex);
//        //Debug.Log("CORNERS LENGTH: " + path.corners.Length);
//        Debug.DrawLine(this.transform.position, this.transform.position + offset, Color.green);
//    }
//}
