using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class User
{
    public int AttackLevel = 1;
    public int HPLevel = 1;
    public int AGILevel = 1;
    public bool Item1 = false;
    public bool Item2 = false;
    public bool Item3 = false;
    public bool Item4 = false;
}

public class GameManager : MonoSingleton<GameManager>
{
    public User userData;



    public void SaveToJson()
    {
        string ToJsonData = JsonUtility.ToJson(userData);
        string path = Application.dataPath + "/SaveData";
        string filepath = Application.dataPath;

        Debug.Log(ToJsonData);

        if (!File.Exists(path))
        {
            Directory.CreateDirectory(filepath+"/SaveData");
        }
        File.WriteAllText(path+"/SaveData.Json", ToJsonData);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        userData.AttackLevel = 2;
        SaveToJson();
    }

    private void Start()
    {
        SaveToJson();
        StartCoroutine(Wait());
    }

    public void LoadToJson()
    {
        string path = Application.dataPath + "/SaveData";
        string ToJsonData = JsonUtility.ToJson(userData);

        if (File.Exists(path))
        {
            string FromJsonData = File.ReadAllText(path);
            userData = JsonUtility.FromJson<User>(FromJsonData);
            Debug.Log(userData);
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
