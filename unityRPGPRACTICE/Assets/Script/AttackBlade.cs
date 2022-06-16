using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBlade : MonoBehaviour
{
    Test PlayerT;

    private void Start()
    {
        PlayerT = GetComponentInParent<Test>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Skill1();
        }
    }

    protected virtual void Skill1()
    {
        Debug.Log("½ºÅ³1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("Damage", PlayerT.attack);
        }
    }

}
