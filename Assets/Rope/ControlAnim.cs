using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAnim : MonoBehaviour
{
    Animator myanim;
    private void Start()
    {
        myanim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            myanim.Play("Building");

        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            myanim.StopPlayback();


        }
    }
}
