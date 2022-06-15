using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart1 : MonoBehaviour
{
    public void StageStart()
    {
        GameManager.Instance.nowState = GameManager.GameState.Gaming;
        SceneManager.LoadScene("SampleScene");
    }
}
