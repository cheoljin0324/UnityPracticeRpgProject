using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    // Start is called before the first frame update
    public void sStart()
    {
        Debug.Log(GameManager.Instance.userData.AttackLevel);   
    }
}
