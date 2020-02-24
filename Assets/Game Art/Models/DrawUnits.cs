using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class DrawUnits : MonoBehaviour
{
    [SerializeField] GameObject tile1Prefab;
    [SerializeField] GameObject tile2Prefab;
    [SerializeField] GameObject tile3Prefab;
    [SerializeField] Transform parent;
    List<Vector3> pos = new List<Vector3>();
    int count;
    private void Start()
    {
        DrawUnit();
    }
    void DrawUnit()
    {
        using (StreamReader sr = new StreamReader("Assets//Game Art//Models//VoxelModeltest.txt"))
        {
            int i = 0;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] Pos = line.Split(',');
                pos.Add(new Vector3(int.Parse(Pos[0]), int.Parse(Pos[1]), int.Parse(Pos[2])));
            }
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            GameObject temp = Instantiate(tile1Prefab, pos[count], Quaternion.identity, parent.transform);
            count++;
        }
     else   if (Input.GetKey(KeyCode.J))
        {
            GameObject temp = Instantiate(tile2Prefab, pos[count], Quaternion.identity, parent.transform);
            count++;
        }
       else if (Input.GetKey(KeyCode.L))
        {
            GameObject temp = Instantiate(tile3Prefab, pos[count], Quaternion.identity, parent.transform);
            count++;
        }


    }
}
