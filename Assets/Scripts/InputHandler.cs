using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance;

    #region Private Fields
    [SerializeField] private LevelProgression _levelProgression;
    #endregion

    private void Awake()
    {
        Instance = this;
    }
    #region Public Methods
    public void ChangeColor(AssociatedColor associated)
    {
        if (Input.touchCount > 1)
            return;
        _levelProgression.CurrentColor = associated.Color;
        GetShapePointsSpline.instance.needleAnimator.enabled = true;
        //_levelProgression.IsPainting = true;
    }
    public void StopPainting()
    {
        _levelProgression.IsPainting = false;
    }
    public void StartPainting()
    {
        _levelProgression.IsPainting = true;
    }
    public void Vibrate()
    {
        Vibration.SmallVibrate();
    }
    #endregion
}
