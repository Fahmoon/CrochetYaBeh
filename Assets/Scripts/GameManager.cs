using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[System.Serializable]
public class ReferenceUpdated : UnityEvent<LevelReference> { }
public class GameManager : MonoBehaviour
{
    UIHandler uIHandler;
    public LevelReference[] levelReferencesSO;
    [HideInInspector] private LevelReference currentLevelReferenceSO;
    public LevelProgression levelProgressionSO;
    public static GameManager Instance;
    public GameStates currentState;
    public ReferenceUpdated referenceUpdated = new ReferenceUpdated();
    [SerializeField] GameObject yarnBall;
    [SerializeField] GameObject myRope;
    [SerializeField] GameObject myKnittedObject;
    [SerializeField] ParticleSystem winConfetti;
    private bool gameOverRoutineStarted;

    public LevelReference CurrentLevelReferenceSO
    {
        get => currentLevelReferenceSO;
        set
        {
            currentLevelReferenceSO = value;
            referenceUpdated.Invoke(currentLevelReferenceSO);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uIHandler = FindObjectOfType<UIHandler>();
        CurrentLevelReferenceSO = levelReferencesSO[Random.Range(0, levelReferencesSO.Length)];
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if (!gameOverRoutineStarted)
        {
            if (levelProgressionSO.Progress == 1)
            {
                StartGameOverRoutine();
            }
        }
    }
    void StartGameOverRoutine()
    {
        gameOverRoutineStarted = true;
        uIHandler.GameOverRoutineUI();
        StartCoroutine(GameOverRoutineNonUI());
    }
    IEnumerator GameOverRoutineNonUI()
    {
        yarnBall.SetActive(false);
        myRope.SetActive(false);
        ////
        float timeStamp = Time.time;
        float totalRotateDuration = 4;
        float slowDownDuration = 1.5f;
        float rotationSpeed = 200;
        float originalRotationSpeed = rotationSpeed;
        ////
        winConfetti.Play();
        while (Time.time - timeStamp < totalRotateDuration)
        {
            if (Time.time - timeStamp < totalRotateDuration - slowDownDuration)
            {
                myKnittedObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            else
            {
                rotationSpeed = Mathf.MoveTowards(rotationSpeed, 0, originalRotationSpeed * Time.deltaTime / slowDownDuration);
                myKnittedObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
        winConfetti.Play();
        uIHandler.EnableDoneButton();

    }
    public void StartCharacterEnterRoutine()
    {
        StartCoroutine(CharacterEntranceRoutine());
    }
    IEnumerator CharacterEntranceRoutine()
    {
        yield return null;
    }
}
