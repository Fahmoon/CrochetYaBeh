using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField] LevelProgression levelProgressionSO;
    public Image[] filledStars;
    int filledStarsCount;
    public GameObject doneButton;
    public GameObject[] colorButtons;
    public Image progressBar;
    public void ActivateDoneButton()
    {
        for (int i = 0; i < colorButtons.Length; i++)
        {
            colorButtons[i].SetActive(false);
        }
        doneButton.SetActive(true);
    }
    void AddStar()
    {
        filledStars[filledStarsCount].enabled = true;
        filledStarsCount++;
    }
    private void Update()
    {
        progressBar.fillAmount = levelProgressionSO.Progress;
    }
}
