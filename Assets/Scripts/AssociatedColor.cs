using UnityEngine;
using UnityEngine.UI;

public class AssociatedColor : MonoBehaviour
{
    #region Private Fields
    [SerializeField] private Color _color;
    #endregion
    #region Public Property
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            GetComponent<Image>().color = _color;
        }
    }
    #endregion
}
