using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAnimationTriggers : MonoBehaviour
{
   
    public void StopPainting()
    {

        InputHandler.Instance.StopPainting();
    }
    public void StartPainting()
    {
        InputHandler.Instance.StartPainting();
    }
    public void Vibrate()
    {
        InputHandler.Instance.Vibrate();
    }
}
