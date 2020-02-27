using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ColorAndPercent
{
    public Color color;
    public float percent;
    [HideInInspector] public float pixelsCounter;
    [HideInInspector] public bool compared;

    public ColorAndPercent(ColorAndPercent col)
    {
        this.color = col.color;
        this.percent = col.percent;
        this.pixelsCounter = col.pixelsCounter;
        this.compared = col.compared;
    }
}
public class LevelHandler : MonoBehaviour
{
    #region Static Public Fields
    public static Vector3           CURRENT_PIXEL_POSITION;
    #endregion
    #region Private Fields
    [Header("LEVEL CONSTs")]
    [SerializeField, Range(1, 100)]
    private int                     _knittingStep;
    [SerializeField]
    private List<Image>             _levelButtons;
    [SerializeField]
    private LevelProgression        _levelProgression;
    [SerializeField]
    private LevelReference          _levelReference;
    [SerializeField]
    private LineRenderer            _myLine;
    [SerializeField]
    private SpriteRenderer          _myYarnBall;
    [Space]
    [Header("MODEL VARs")]
    [SerializeField]
    private GameObject              _model;
    [SerializeField]
    private MeshFilter              _modelMeshFilter;
    [SerializeField]
    private Texture2D               _modelTex;
    [SerializeField]
    private Material                _material;
    [SerializeField]
    private ModelFilledUV           _modelUVFilled;
    
    private List<ColorAndPercent>   _levelColors = new List<ColorAndPercent>();
    private List<Vector3>           _modelMappedPoints = new List<Vector3>();
    [SerializeField]
    private List<Vector2>           _uvFilledPoints = new List<Vector2>();
    private Texture2D               _myTex;
    private int                     _progessCounter;
    private float                   _pixelsCount;
    private ColorAndPercent         _currentColor;
    [SerializeField] private List<Vector3>           _mappedPoints = new List<Vector3>();
    private int                     _mappedPointsCounter, _mappedPointsMax;
    //private bool                    _gotProgressStar;
    #endregion
    #region MonoBehavior Callbacks

    private void OnDestroy()
    {
        _levelProgression.Color_Changed -= OnColorChanging;
    }
    #endregion

    #region Public Methods
    public void ListenToReference(LevelReference currentLevelReference)
    {
        _levelProgression.Color_Changed += OnColorChanging;
        foreach (var item in _levelReference.RefColorsWithPercents)
            _levelColors.Add(new ColorAndPercent(item));

        _levelProgression.ColorAndPercents.Clear();
        _levelProgression.StarsCount = -1;
        _levelProgression.Progress = 0;
        _levelProgression.IsPainting = false;
        OnColorChanging(new ColorAndPercent(_levelReference.RefColorsWithPercents[0]));

        if (_levelColors.Count != _levelButtons.Count)
        {
            Debug.LogError("Match Level Colors and Level Buttons count to begin game, D-Head");
            return;
        }

        foreach (ColorAndPercent item in _levelColors)
        {
            item.color.a = 1f;
            item.percent = 0f;
            item.pixelsCounter = 0f;
            item.compared = false;
            _levelProgression.ColorAndPercents.Add(item);
        }

        int counter = _levelColors.Count;
        for (int i = 0; i < counter; i++)
        {
            AssociatedColor associated = _levelButtons[i].gameObject.GetComponent<AssociatedColor>();

            if (associated == null)
                _levelButtons[i].gameObject.AddComponent<AssociatedColor>().Color = _levelColors[i].color;
            else
                associated.Color = _levelColors[i].color;
        }

        _myTex = TransferAlpha(_modelTex);
        //_uvFilledPoints = _modelUVFilled.UVs;
        //_mappedPoints = _modelUVFilled.MappedPoints;
        _mappedPoints = GetModelWorldPositionPointsBasedOnKnittingSpeed(_modelMeshFilter,_myTex, _knittingStep);

        _material.mainTexture = _myTex;
        ManipulateAlpha(_myTex, 0f);
    }
    #endregion
    #region Private Methods

    private void OnColorChanging(ColorAndPercent color)
    {
        _currentColor = color;
        _myLine.startColor = color.color;
        _myLine.endColor = color.color;
        _myYarnBall.color = color.color;
    }
    private List<Vector3> GetModelWorldPositionPointsBasedOnKnittingSpeed(MeshFilter modelMesh, Texture2D texture2D, int knittingSpeed)
    {
        List<Vector3> worldPoints = new List<Vector3>();
        Vector3 lastPos = new Vector3(0f,0f,0f);

        int textureWidth = texture2D.width,
            textureHeight = texture2D.height;

        if (textureHeight != textureWidth)
            return null;

        for (int i = 0; i < textureWidth; i += knittingSpeed)
        {
            for (int j = 0; j < textureHeight; j += knittingSpeed)
            {
                Vector3[] points = modelMesh.mesh.GetMappedPoints(new Vector2(j * 1.0f / textureWidth, i * 1.0f / textureHeight));

                if (points.Length <= 0)
                    worldPoints.Add(lastPos);
                else
                {
                    worldPoints.Add(points[0]);
                    lastPos = points[0];
                }
            }
        }
        worldPoints.RemoveAt(0);
        return worldPoints;
    }
    private void UpdateStars()
    {
        _levelProgression.StarsCount += _levelReference.MatchingReferenceColors(_levelProgression.ColorAndPercents);

        //if (_levelProgression.Progress >= 0.5f && !_gotProgressStar)
        //{
        //    _levelProgression.StarsCount++;
        //    _gotProgressStar = true;
        //}
    }
    private void ManipulateAlpha(Texture2D A, float alpha)
    {
        Color _pixA = new Color(0f, 1f, 0f);
        int texWidth = A.width, texHeight = A.height;

        for (int i = 0; i < texWidth; i++)
            for (int j = 0; j < texHeight; j++)
            {
                _pixA = A.GetPixel(i, j);
                _pixA.a = Mathf.Clamp01(alpha);
                A.SetPixel(i, j, _pixA);
            }
        A.Apply();
        StartCoroutine(ChangeMyAlpha(_myTex, Mathf.Abs(1 - alpha)));
    }
    private Texture2D TransferAlpha(Texture2D A, Texture2D B = null)
    {
        if (B == null)
            B = new Texture2D(A.width, A.height, TextureFormat.ARGB32, false);

        Color pixA;
        int aWidth = A.width, aHeight = A.height;
        for (int i = 0; i < aWidth; i++)
        {
            for (int j = 0; j < aHeight; j++)
            {
                pixA = A.GetPixel(i, j);
                B.SetPixel(i, j, pixA);
            }
        }
        B.Apply();
        return B;
    }
    private Color[] GetPixels(Texture2D texture2D, int startIndex, int range)
    {
        List<Color> colors = new List<Color>();

        int width = texture2D.width;
        for (int i = 0; i < range; i++)
            for (int j = 0; j < range; j++)
                colors.Add(texture2D.GetPixel(Mathf.RoundToInt(_uvFilledPoints[i + startIndex].x * width), Mathf.RoundToInt(_uvFilledPoints[j + startIndex].y * width)));
        
        return colors.ToArray();
    }
    private void SetPixels(Texture2D texture2D, int startIndex, int range, Color[] colors)
    {
        int width = texture2D.width, counter = 0;
        for (int i = 0; i < range; i++)
        {
            for (int j = 0; j < range; j++)
            {
                texture2D.SetPixel(Mathf.RoundToInt(_uvFilledPoints[i + startIndex].x * width), Mathf.RoundToInt(_uvFilledPoints[j + startIndex].y * width), colors[counter]);
                counter++;
            }
        }
        texture2D.Apply();
    }
    #endregion
    #region Coroutines
    private IEnumerator ChangeMyAlphaSecondMethod(Texture2D A, float alpha)
    {
        Color[] pixA;
        int _newMax = _uvFilledPoints.Count, _newCounter = 0;
        
        while(_newCounter < _newMax)
        {
            if(_levelProgression.IsPainting)
            {
                pixA = GetPixels(_modelTex, _newCounter, _knittingStep);
                _progessCounter += _knittingStep;

                _currentColor.pixelsCounter += _knittingStep;
                _currentColor.percent = (_currentColor.pixelsCounter / _newMax) * 100f;

                CURRENT_PIXEL_POSITION = _mappedPoints[_newCounter];

                for (int i = 0; i < _knittingStep; i++)
                {
                    pixA[i].r *= _currentColor.color.r;
                    pixA[i].g *= _currentColor.color.g;
                    pixA[i].b *= _currentColor.color.b;
                    pixA[i].a  = Mathf.Clamp01(alpha);
                }
                SetPixels(A, _newCounter, _knittingStep, pixA);
                yield return new WaitForFixedUpdate();
                UpdateStars();
                _newCounter += _knittingStep;
                
                _levelProgression.Progress = _progessCounter * 1.0f / _newMax;
            }
            else
            {
                yield return new WaitForFixedUpdate();
                yield return null;
            }
        }
    }
    private IEnumerator ChangeMyAlpha(Texture2D A, float alpha)
    {
        Color[] pixA;
        float texWidth = A.width, texHeight = A.height, heightIterator = 0, widthIterator;
        int _pixelsBatch = _knittingStep * _knittingStep;
        _pixelsCount = texHeight * texWidth;

        if ((int)texWidth != (int)texHeight || (int)texWidth % _knittingStep != 0)
            yield return null;

        while (heightIterator < texHeight)
        {
            widthIterator = 0;
            pixA = null;
            while (widthIterator < texWidth)
            {
                if (_levelProgression.IsPainting)
                {
                    pixA = _modelTex.GetPixels((int)widthIterator, (int)heightIterator, _knittingStep, _knittingStep);

                    _progessCounter += _pixelsBatch;

                    _currentColor.pixelsCounter += _pixelsBatch;
                    _currentColor.percent = (_currentColor.pixelsCounter / _pixelsCount) * 100f;

                    CURRENT_PIXEL_POSITION = _mappedPoints[_mappedPointsCounter];


                    for (int i = 0; i < _pixelsBatch; i++)
                    {
                        pixA[i].r *= _currentColor.color.r;
                        pixA[i].g *= _currentColor.color.g;
                        pixA[i].b *= _currentColor.color.b;
                        pixA[i].a = Mathf.Clamp01(alpha);
                    }

                    A.SetPixels((int)widthIterator, (int)heightIterator, _knittingStep, _knittingStep, pixA);
                    A.Apply();
                    yield return new WaitForFixedUpdate();
                    UpdateStars();
                    widthIterator += _knittingStep;
                    if (_mappedPointsCounter < _mappedPointsMax - 1)
                        _mappedPointsCounter++;
                    _levelProgression.Progress = _progessCounter / _pixelsCount;
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                    yield return null;
                }
            }
            heightIterator += _knittingStep;
        }
    }
    #endregion
}
