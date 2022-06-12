using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    public static User userData;

    string ToJsonData = JsonUtility.ToJson(userData);
    string path = Application.dataPath + "/SaveData";
    
    public void SaveToJson()
    {
        File.WriteAllText(path, ToJsonData);
    }

    public void LoadToJson()
    {
        if (File.Exists(path))
        {
            string FromJsonData = File.ReadAllText(path);
            userData = JsonUtility.FromJson<User>(FromJsonData);
        }
    }

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
