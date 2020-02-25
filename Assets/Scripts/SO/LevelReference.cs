using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelReference.asset", menuName = "ScriptableObjects/Level/Reference")]
public class LevelReference : ScriptableObject
{
    #region Private Fields
    [SerializeField] private Sprite                _sprite;
    [SerializeField] private List<ColorAndPercent> _colorAndPercents;
    [SerializeField, Range(0,100)] private float   _clearnce = 5f;
    #endregion
    #region Public Properties
    public Sprite RefSprite
    {
        get => _sprite;
    }
    public List<ColorAndPercent> RefColorsWithPercents
    {
        get => _colorAndPercents;
    }
    #endregion
    #region Public Methods
    public int MatchingReferenceColors(List<ColorAndPercent> colorAndPercents)
    {
        int counter = 0, iterator = colorAndPercents.Count;

        if (iterator != _colorAndPercents.Count)
            return -1;

        for (int i = 0; i < iterator; i++)
            if (!colorAndPercents[i].compared && Mathf.Abs(colorAndPercents[i].percent - _colorAndPercents[i].percent) <= _clearnce)
            {
                colorAndPercents[i].compared = true;
                counter++;
            }

        return counter;
    }
    #endregion
}
