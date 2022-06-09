using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public enum GameState {Default ,StartGame , Gaming , EndGame}
    GameState nowState = GameState.Default;

    public void NowGamingState()
    {
        nowState = GameState.Gaming;
    }

    public void StartState()
    {
        nowState = GameState.StartGame;
    }

    public void EndState()
    {
        nowState = GameState.EndGame;
    }

}
