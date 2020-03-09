using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GetShapePointsSpline : MonoBehaviour
{
    [SerializeField] Animator yarnBallAnimator;
    [SerializeField] Animator needleAnimator;
    public LevelProgression myScriptableObject;
    public static GetShapePointsSpline instance;
    public Transform anchor;
    private void Awake()
    {
        instance = this;
    }

    bool knitting;
    private void Update()
    {
        if (myScriptableObject.IsPainting && !knitting)
        {
            knitting = true;
            yarnBallAnimator.enabled = true;
            needleAnimator.enabled = true;
        }
        if (!myScriptableObject.IsPainting && knitting)
        {
            knitting = false;
            yarnBallAnimator.enabled = false;
            needleAnimator.enabled = false;
        }
    }
  
    void FixedUpdate()
    {
        if (knitting)
        {
            anchor.position = LevelHandler.CURRENT_PIXEL_POSITION;
        }
    }
}