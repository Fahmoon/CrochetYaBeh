using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct RopeSegment
{
    public Vector3 currentPos;
    public Vector3 prevPos;
    public RopeSegment(Vector3 pos)
    {
        currentPos = pos;
        prevPos = pos;
    }
}
public class Rope : MonoBehaviour
{
    [SerializeField] float ropeSegmentLength = 0.25f;
    [SerializeField] int ropeLength = 35;
    [SerializeField] float lineWidth = 0.1f;
    [SerializeField] float ropeGravity = 9.81f;
    [SerializeField] Transform anchor;
    [SerializeField] Transform start;
    LineRenderer myLR;
    List<RopeSegment> ropeSegments = new List<RopeSegment>();
    Camera myCam;
    bool firstTime;
    private void Start()
    {
        myCam = Camera.main;
        myLR = GetComponent<LineRenderer>();
        Vector3 startPoint = anchor.position;
        for (int i = 0; i < ropeLength; i++)
        {
            ropeSegments.Add(new RopeSegment(startPoint));
            startPoint.y -= ropeSegmentLength;
        }
        gameObject.SetActive(false);
    }
 
    void SimulateRope()
    {
        Vector3 gravity = new Vector2(0, -ropeGravity);
        //Simulation

            for (int i = 0; i < ropeLength; i++)
            {
                RopeSegment firstSegment = ropeSegments[i];
                Vector3 velocity = firstSegment.currentPos - firstSegment.prevPos;
                firstSegment.prevPos = firstSegment.currentPos;
                firstSegment.currentPos += velocity + (gravity * Time.deltaTime);
                ropeSegments[i] = firstSegment;
            }

        //Constraints

        if (!firstTime)
        {
            for (int i = 0; i < 20; i++)
            {
                ApplyConstraints();
            }
        }
        else
        {
            for (int i = 0; i < 200; i++)
            {
                ApplyConstraints();
            }
            firstTime = true;
        }
    
    }

    private void ApplyConstraints()
    {

        RopeSegment firstSegment = ropeSegments[0];

        firstSegment.currentPos = anchor.position;
        ropeSegments[0] = firstSegment;
        RopeSegment lastSegment = ropeSegments[ropeSegments.Count - 1];
        lastSegment.currentPos = start.position;
        ropeSegments[ropeSegments.Count - 1] = lastSegment;
        for (int i = 0; i < ropeLength - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];
            float dist = (ropeSegments[i].currentPos - ropeSegments[i + 1].currentPos).magnitude;
            float error = dist - ropeSegmentLength;

            Vector3 changeDir = (firstSeg.currentPos - secondSeg.currentPos).normalized;

            Vector3 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.currentPos = Vector3.MoveTowards(firstSeg.currentPos, firstSeg.currentPos - changeAmount * 0.5f, 0.5f);
                ropeSegments[i] = firstSeg;
                secondSeg.currentPos = Vector3.MoveTowards(secondSeg.currentPos, secondSeg.currentPos + changeAmount * 0.5f, 0.5f);
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.currentPos = Vector3.MoveTowards(secondSeg.currentPos, secondSeg.currentPos + changeAmount, 1);
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    void DrawRope()
    {
        float lineWidth = this.lineWidth;
        myLR.startWidth = lineWidth;
        myLR.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[ropeLength];
        for (int i = 0; i < ropePositions.Length; i++)
        {
            ropePositions[i] = ropeSegments[i].currentPos;
        }
        myLR.positionCount = ropePositions.Length;
        myLR.SetPositions(ropePositions);
    }
    private void Update()
    {
        DrawRope();
        SimulateRope();
    }

}
