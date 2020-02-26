using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    #region Private Fields
    [SerializeField] private LevelProgression levelProgressionSO;
    private int _filledStarsCount;
    #endregion
    #region Public Fields
    public Image[] filledStars;
    public GameObject doneButton;
    public GameObject[] colorButtons;
    public Image progressBar;
    #endregion
    #region MonoBehavior Callbacks
    private void Start()
    {
        levelProgressionSO.Stars_Changed += StarsChanged;
    }
    private void Update()
    {
        progressBar.fillAmount = levelProgressionSO.Progress;
    }
    #endregion
    #region Private Methods
    private void StarsChanged(int stars)
    {
        if (stars > (filledStars.Length - 1))
            return;

        for (int i = 0; i <= stars; i++)
            filledStars[i].enabled = true;
    }
    private void AddStar()
    {
        filledStars[_filledStarsCount].enabled = true;
        _filledStarsCount++;
    }
    #endregion
    #region Public Methods
    public void ActivateDoneButton()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            colorButtons[i].SetActive(false);
        }
        doneButton.SetActive(true);
    }
    #endregion
}
