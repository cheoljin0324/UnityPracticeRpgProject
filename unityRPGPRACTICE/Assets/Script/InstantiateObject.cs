using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public GameObject Enemy;
    [SerializeField]
    GameObject SetChar;
    public bool EndElement = false;

    
    public List<GameObject> EnemyList;

    private void Start()
    {
        EnemyInst();
    }

    private void Update()
    {
        if(EndElement==true && EnemyList.Count == 0)
        {
            Debug.Log("스테이지 클리어");
            GameManager.Instance.EndState();
            GameManager.Instance.freshTime();
            EndElement = false;
        }
    }

    public void EnemyInst()
    {
        GameManager.Instance.SetMobAmount();
       for (int i = 0; i<GameManager.Instance.MobAmount; i++)
        {
            GameObject EnemyOb = Instantiate(Enemy);
            EnemyOb.transform.position = transform.position;
            EnemyList.Add(EnemyOb);

            EnemyOb.GetComponent<EnemyTest>().targetCharacter = SetChar;
            EnemyOb.GetComponent<EnemyTest>().targetTransform = SetChar.transform;
            EnemyOb.GetComponent<EnemyTest>().InstOb = gameObject;
            EnemyOb.GetComponent<EnemyTest>().ID = EnemyList.Count;
            EnemyOb.transform.position = new Vector3(Random.RandomRange(transform.position.x - 10, transform.position.x + 10), transform.position.y, Random.RandomRange(transform.position.z - 10, transform.position.z + 10));

            Debug.Log(EnemyOb.GetComponent<EnemyTest>().ID);
            GameManager.Instance.EnemyObList.Add(EnemyOb);
        }
        EndElement = true;
    }

    void SetRemoveList(GameObject EnemyOb)
    {
        EnemyList.Remove(EnemyOb);
    }
}
