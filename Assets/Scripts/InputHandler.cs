using UnityEngine;

public class InputHandler : MonoBehaviour
{
    #region Private Fields
    [SerializeField] private LevelProgression _levelProgression;
    #endregion
    #region Public Methods
    public void ChangeColor(AssociatedColor associated)
    {
        if (Input.touchCount > 1)
            return;
        _levelProgression.CurrentColor = associated.Color;
        _levelProgression.IsPainting = true;
    }
    public void StopPainting()
    {
        _levelProgression.IsPainting = false;
    }
    #endregion
}
