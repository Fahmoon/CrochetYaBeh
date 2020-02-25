using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public  class ColorAndPercent
{
    public Color color;
    public float percent;
    [HideInInspector]
    public float pixelsCounter;

    public ColorAndPercent(Color col, float per)
    {
        this.color = col;
        this.percent = per;
    }
}
public class DeleteMeGameManager : MonoBehaviour
{
    [SerializeField, Range(0, 0.5f)]
    private float           _knittingDelay;
    [SerializeField, Range(1, 100)]
    private int             _knittingStep;
    [Space]
    [SerializeField]
    private Material        _mat;
    [SerializeField]
    private GameObject      _model;
    [SerializeField]
    private Texture2D       _modelTex;
    [SerializeField]
    private MeshFilter      _modelMeshFilter;
    [SerializeField]
    private ColorAndPercent[] _levelColors;
    [SerializeField]
    private List<Image>     _levelButtons;
    //[SerializeField]
    public Slider          _levelProgress;
    [SerializeField] MeshRenderer objMesh;
     [SerializeField] Transform anchor;
    private Texture2D       _myTex;
    private int             _progessCounter, _colorOnePercent, _colorTwoPercent, _colorThreePercent;
    private float           _pixelsCount;
    

    #region MonoBehavior Callbacks
    void Start()
    {
        TransferAlpha(_modelTex);

        _mat.mainTexture = _myTex;

     

        ManipulateAlpha(_myTex, 0f);


        foreach (ColorAndPercent item in _levelColors)
        {
            item.percent = 0f;
            item.pixelsCounter = 0f;
        }
        
        int counter = _levelColors.Length;
        for (int i = 0; i < counter; i++)
            _levelButtons[i].color = _levelColors[i].color;

       // Vector3 worldPos = UvTo3D(_modelMeshFilter.mesh.uv[2], _modelMeshFilter);
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//        cube.transform.position = worldPos;
      //  cube.transform.localScale = Vector3.one * 0.01f;
    }
    #endregion

    #region Private Fields
    private void GetAllTextures(GameObject obj)
    {
        List<Texture> allTexture = new List<Texture>();

        Material sharedMaterial = obj.GetComponent<Renderer>().sharedMaterial;
        Shader shader = sharedMaterial.shader;

        for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
        {
            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                Texture texture = sharedMaterial.GetTexture(ShaderUtil.GetPropertyName(shader, i));
                allTexture.Add(texture);
            }
        }

        Debug.Log("All Textures Count: " + allTexture.Count);
    }
    private IEnumerator ChangeMyAlpha(Texture2D A, float alpha)
    {
        Color[] pixA;
        float texWidth = A.width,
                texHeight = A.height,
                heightIterator = 0,
                widthIterator;
        _pixelsCount = texHeight * texWidth;

        if (texWidth != texHeight || texWidth % _knittingStep != 0)
            yield return null;

        while (heightIterator < texHeight)
        {
            widthIterator = 0;
            pixA = null;
            while (widthIterator < texWidth)
            {
                if ((Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
                {
                    pixA = _modelTex.GetPixels((int)widthIterator, (int)heightIterator, _knittingStep, _knittingStep);

                    int _pixelsCounter = pixA.Length;
                    Color currentColor = _levelColors[0].color;
                    _progessCounter += _pixelsCounter;

                    if (Input.GetKey(KeyCode.J))
                    {
                        currentColor = _levelColors[0].color;
                        _levelColors[0].pixelsCounter += _pixelsCounter;
                        _levelColors[0].percent =(_levelColors[0].pixelsCounter/ _pixelsCount) * 100f;
                    }
                    if (Input.GetKey(KeyCode.K))
                    {
                        currentColor = _levelColors[1].color;
                        _levelColors[1].pixelsCounter += _pixelsCounter;
                        _levelColors[1].percent = (_levelColors[1].pixelsCounter / _pixelsCount) * 100f;
                    }
                    if (Input.GetKey(KeyCode.L))
                    {
                        currentColor = _levelColors[2].color;
                        _levelColors[2].pixelsCounter += _pixelsCounter;
                        _levelColors[2].percent = (_levelColors[2].pixelsCounter / _pixelsCount) * 100f;
                    }


                    for (int i = 0; i < _pixelsCounter; i++)
                    {
                        pixA[i].r *= currentColor.r;
                        pixA[i].g *= currentColor.g;
                        pixA[i].b *= currentColor.b;
                        pixA[i].a = Mathf.Clamp01(alpha);
                    }

                    A.SetPixels((int)widthIterator, (int)heightIterator, _knittingStep, _knittingStep, pixA);
                    A.Apply();
                    yield return new WaitForFixedUpdate();

                    widthIterator += _knittingStep;
                    _levelProgress.value = _progessCounter / _pixelsCount;
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
    private void TransferAlpha(Texture2D A)
    {
        _myTex = new Texture2D(A.width, A.height, TextureFormat.ARGB32, false);

        Color pixA;

        for (int i = 0; i < A.width; i++)
        {
            for (int j = 0; j < A.height; j++)
            {
                pixA = A.GetPixel(i, j);
                _myTex.SetPixel(i, j, pixA);
            }
        }
        _myTex.Apply();
    }
    #endregion
}
