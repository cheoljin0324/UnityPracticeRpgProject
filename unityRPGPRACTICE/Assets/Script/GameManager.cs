using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class User
{
    public int AttackLevel = 1;
    public int HPLevel = 1;
    public int AGILevel = 1;
    public int coin = 100;
    public bool[] Item = new bool[10];
    public bool[] isUse = new bool[10];
}

public class GameManager : MonoSingleton<GameManager>
{
    ShopCanvasManager shopManager;

    public User userData;

    public int NowStage = 1;
    public int MobAmount;

    public static GameManager Instance;
    public List<GameObject> EnemyObList;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetRemoveEnemy(GameObject Enemyobject)
    {
        EnemyObList.Remove(Enemyobject);
    }

    public void Loaded()
    {
        SceneManager.LoadScene("Shop");
    }

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

    public void SetMobAmount()
    {
        MobAmount = Random.RandomRange(NowStage + 1, NowStage + 4);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        SaveToJson();
    }

    private void Start()
    {
        LoadToJson();
    }

    public void LoadToJson()
    {
        string path = Application.dataPath + "/SaveData/SaveData.Json";

        if (File.Exists(path))
        {
            string FromJsonData = File.ReadAllText(path);
            userData = JsonUtility.FromJson<User>(FromJsonData);
            Debug.Log(userData);
        }
    }

    public enum GameState {Default ,StartGame , Gaming , EndGame, freshTime}
    public GameState nowState = GameState.Default;

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
        NowStage++;
    }

    public void freshTime()
    {
        nowState = GameState.freshTime;
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopCanvasManager>();
        shopManager.SetShop();
    }

}
