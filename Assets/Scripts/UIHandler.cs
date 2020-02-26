using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    #region Private Fields
    private int _filledStarsCount;
    #endregion
    #region Public Fields
    public Image referenceImage;
    public Image[] filledStars;
    public GameObject doneButton;
    public GameObject[] colorButtons;
    public Image progressBar;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject gameOverUI;
    #endregion
    #region MonoBehavior Callbacks
    private void Start()
    {
        GameManager.Instance.levelProgressionSO.Stars_Changed += StarsChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.levelProgressionSO.Stars_Changed -= StarsChanged;
    }
    private void Update()
    {
        if (GameManager.Instance.levelProgressionSO.IsPainting)
            progressBar.fillAmount = GameManager.Instance.levelProgressionSO.Progress;
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

    #endregion
    #region Public Methods
    public void ListenToCurrentReference(LevelReference currentReference)
    {
        referenceImage.sprite = currentReference.RefSprite;
    }
    public void GameOverRoutineUI()
    {
        for (int i = 0; i < gameplayUI.transform.childCount; i++)
        {
            gameplayUI.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void EnableGameOverPanel()
    {
        gameOverUI.SetActive(true);
    }
    public void EnableDoneButton()
    {
        doneButton.SetActive(true);
    }
    #endregion
}
