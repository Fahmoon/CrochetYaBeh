using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class StateChanged : UnityEvent<GameStates> { }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameStates currentState;
    public StateChanged checkGameState=new StateChanged();
    private void Awake()
    {
        Instance = this;
    }

}
