using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject Enemy;
    [SerializeField]
    GameObject SetChar;

    private void Start()
    {
        StartCoroutine(EnemyInst());
    }

    IEnumerator EnemyInst()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            GameObject EnemyOb = Instantiate(Enemy);
            EnemyOb.transform.position = transform.position;

            EnemyOb.GetComponent<EnemyTest>().targetCharacter = SetChar;
            EnemyOb.GetComponent<EnemyTest>().targetTransform = SetChar.transform;
        }
    }
}
