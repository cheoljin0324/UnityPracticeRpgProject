using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject Enemy;
    [SerializeField]
    GameObject SetChar;
    public bool EndElement = false;

    int nowStageMob = 5;
    public List<GameObject> EnemyList;

    private void Start()
    {
        StartCoroutine(EnemyInst());
    }

    private void Update()
    {
        if(EndElement==true && EnemyList.Count == 0)
        {
            Debug.Log("스테이지 클리어");
        }
    }

    IEnumerator EnemyInst()
    {
        while (EnemyList.Count!=nowStageMob)
        {
            yield return new WaitForSeconds(10f);
            GameObject EnemyOb = Instantiate(Enemy);
            EnemyOb.transform.position = transform.position;
            EnemyList.Add(EnemyOb);

            EnemyOb.GetComponent<EnemyTest>().targetCharacter = SetChar;
            EnemyOb.GetComponent<EnemyTest>().targetTransform = SetChar.transform;
            EnemyOb.GetComponent<EnemyTest>().InstOb = gameObject;
            EnemyOb.GetComponent<EnemyTest>().ID = EnemyList.Count;

            Debug.Log(EnemyOb.GetComponent<EnemyTest>().ID);
        }
        EndElement = true;
    }

    void SetRemoveList(int i)
    {
        EnemyList.RemoveAt(i-1);
    }
}
