using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelProgression.asset", menuName = "ScriptableObjects/Level/Progression")]
public class LevelProgression : ScriptableObject
{
    #region Events
    public delegate void opOnStarsValueChanging(int stars);
    public event  opOnStarsValueChanging Stars_Changed = delegate {};

    public delegate void opOnColorValueChanging(ColorAndPercent color);
    public event opOnColorValueChanging Color_Changed = delegate { };
    #endregion
    #region Private Fields
    /// <summary>
    /// Returns a value between 0 and 100 that indicates the level progress *Not affected by the player accuracy*.
    /// </summary>
    [SerializeField] private float                  _progress;
    /// <summary>
    /// Determines how the player is close to the reference;
    /// </summary>
    [SerializeField] private int                    _starsCount;
    [SerializeField] private ColorAndPercent        _currentColor;
    [SerializeField] private bool                   _isPainting;
    [SerializeField] private List<ColorAndPercent>  _colorAndPercents;
    #endregion
    #region Public Properties
    public int StarsCount
    {
        get => _starsCount;
        set
        {
            if (value == _starsCount)
                return;
            if (value == -1)
            {
                _starsCount = -1;
                return;
            }
            _starsCount = Mathf.Clamp(value,0,3);
            Stars_Changed(_starsCount);
        }
    }
    public float Progress
    {
        get => _progress;
        set => _progress = value;
    }
    public Color CurrentColor
    {
        get => _currentColor.color;
        set
        {
            if (value == _currentColor.color) return;

            _currentColor.color = value;
            Color_Changed(_colorAndPercents.Find(x => x.color == value));
        }
    }
    public bool IsPainting
    {
        get => _isPainting;
        set => _isPainting = value;
    }
    public List<ColorAndPercent> ColorAndPercents
    {
        get => _colorAndPercents;
        set => _colorAndPercents = value;
    }
    #endregion
}
