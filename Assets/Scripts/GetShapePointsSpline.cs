using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PathCreation.Examples;

public class ComparePositions : IComparer<Vector3>
{
    public int Compare(Vector3 pos1, Vector3 pos2)
    {
        int ret;
        if (Mathf.Abs(pos1.y - pos2.y) < 0.001f)
        {
            ret = 0;
        }
        else if (pos1.y < pos2.y)
            ret = -1;
        else
            ret = 1;
        if (ret == 0)
        {
            ret = (Vector3.SignedAngle(Vector3.left, pos2, Vector3.up) + 180).CompareTo(Vector3.SignedAngle(Vector3.left, pos1, Vector3.up) + 180);
        }
        if (ret == 0)
        {
            if (Mathf.Abs(pos1.z - pos2.z) < 0.001f)
            {
                ret = 0;
            }
            else if (pos1.z < pos2.z)
                ret = -1;
            else
                ret = 1;
        }
        return ret;
    }
}
public class GetShapePointsSpline : MonoBehaviour
{
    public LevelProgression myScriptableObject;
    public Mesh myMesh;
    public PathCreation.PathCreator ourCreator;
    public static GetShapePointsSpline instance;
    public float speed;
    public int vertexSpeed;
    public float accuracy;
    public Transform anchor;
    float distanceTravelled;
    public List<Vector3> verticies = new List<Vector3>();
    int currentVertex;
    int i;
    LevelHandler deleteMeGame;
    public PathCreation.EndOfPathInstruction endOfPathInstruction;
    float radiusModifier;
    public float radiusModifierFactor;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        deleteMeGame = GetComponentInParent<LevelHandler>();
        myMesh.GetVertices(verticies);
        currentVertex = 0;
        ComparePositions sc = new ComparePositions();
        verticies.Sort(sc);
        anchor.position = verticies[0] + transform.position;
        ourCreator.bezierPath.SetPoint(0, verticies[0] + transform.position, true);
        ourCreator.bezierPath.SetPoint(ourCreator.bezierPath.NumPoints - 1, verticies[1] + transform.position, true);
        ourCreator.bezierPath.IsClosed = true;
        ourCreator.bezierPath.IsClosed = false;
        for (int i = 2; i < verticies.Count; i++)
        {

            ourCreator.bezierPath.AddSegmentToEnd(verticies[i] + transform.position);

        }
  
   //     StartCoroutine(draw());
    }

    IEnumerator draw()
    {
        Color[] myColor = { Color.red, Color.yellow, Color.blue };
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        if (i < verticies.Count)
			temp.transform.position = verticies[i];

        temp.transform.localScale = Vector3.one * 0.003f;
        yield return new WaitForSeconds(0.05f);
        i++;
        StartCoroutine(draw());
    }

    bool knitting;
    private void Update()
    {
        if (myScriptableObject.IsPainting)
        {
            knitting = true;
        }
        if (!myScriptableObject.IsPainting)
        {
            knitting = false;
        }
    }
    IEnumerator SmoothScale()
    {
        float t = 0f;
        while (t <= 1.0)
        {
            if (knitting)
            {
                if (anchor.position != verticies[currentVertex] + transform.position)
                {
                    t += Time.fixedDeltaTime / speed;
                    anchor.position = Vector3.Lerp(anchor.position, verticies[currentVertex] + transform.position, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, t))));
                }
                else
                {
                    currentVertex++;
                    t = 0;
                }
            }
            yield return null;
        }
    }
    void FixedUpdate()
    {
        if (LevelHandler.CURRENT_PIXEL_POSITION!=Vector3.zero)
        {

            //distanceTravelled += Mathf.Clamp(speed - radiusModifierFactor/(Vector3.Distance(Vector3.zero, new Vector3(anchor.position.x, 0, anchor.position.z))+0.000001f) , 0, 100000);
            //anchor.position = ourCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            anchor.position = LevelHandler.CURRENT_PIXEL_POSITION;
        }
    }
}