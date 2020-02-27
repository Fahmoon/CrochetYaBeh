using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ModelFilledUV.asset", menuName = "ScriptableObjects/Level/Model UV")]
public class ModelFilledUV : ScriptableObject
{
    #region Private Fields
    [SerializeField]
    private List<Vector2>   _uvS;
    [SerializeField]
    private List<Vector3>   _mappedPoints;
    [SerializeField]
    private Mesh            _mesh;
    [SerializeField]
    private Vector2         _texture2DResolution;
    #endregion
    #region Public Properties
    public List<Vector2> UVs
    {
        get
        {
            if (_uvS.Count > 0 || _uvS == null)
                return _uvS;
            else
                return UVFilledPoints();
        }
    }

    public List<Vector3> MappedPoints
    {
        get
        {
            if (_mappedPoints.Count <= 0 || _mappedPoints == null)
                UVFilledPoints();

                return _mappedPoints;
        }
    }
    #endregion
    #region Public Methods
    private List<Vector2> UVFilledPoints()
    {
        _uvS = new List<Vector2>();
        _mappedPoints = new List<Vector3>();

        int textureWidth = (int)_texture2DResolution.x,
            textureHeight = (int)_texture2DResolution.y;

        if (textureHeight != textureWidth)
            return _uvS;

        for (int i = 0; i < textureWidth; i++)
        {
            for (int j = 0; j < textureHeight; j++)
            {
                Vector3[] points = _mesh.GetMappedPoints(new Vector2(j * 1.0f / textureWidth, i * 1.0f / textureHeight));

                if (points.Length > 0)
                {
                    _uvS.Add(new Vector2(j * 1.0f / textureWidth, i * 1.0f / textureHeight));
                    _mappedPoints.Add(points[0]);
                }
            }
        }
        return _uvS;
    }
    #endregion
}
