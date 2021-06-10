using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCubePosition : MonoBehaviour
{
    [SerializeField] float moveTime = 2f;
    private List<PointInTime> pointsInTime;
    private bool isRewinding = false;
    private bool isTargeting = false;
    private Rigidbody rb;
    private int index;

    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody>();
        index = GetComponent<Swipe>().Index;
    }

    
    void Update()
    {
        if (LevelGenerator.Instance.CubeIsReturning)
        {
            LevelGenerator.Instance.CubeIsReturning = false;
            StartRewind();
        }
    }

    private void FixedUpdate()
    {
        if(isRewinding)
        {
            Rewind();
        } 
        
        else if (isTargeting)
        {
            Record();
        }
    }


    private void OnMouseDown()
    {
        isTargeting = true;
    }


    void Record()
    {
        if (pointsInTime.Count < Mathf.Round(moveTime / Time.fixedDeltaTime) && index == LevelGenerator.Instance.LastCubeIndex)
        {
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
        }
        
    }

    void Rewind()
    {
        if(pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewinding();
        }

    }
    
    void StartRewind()
    {
        if (index == LevelGenerator.Instance.LastCubeIndex)
        {
            isRewinding = true;
        }
        rb.isKinematic = true;
    }

    void StopRewinding()
    {
        isRewinding = false;
        rb.isKinematic = false;        
    }
}
