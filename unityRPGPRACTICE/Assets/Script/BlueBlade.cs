using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlade : AttackBlade
{
    Test PlayerT; 

    private void Start()
    {
        PlayerT = GetComponentInParent<Test>();
    }


    protected override void Skill1()
    {
        Debug.Log("블루 소드 스킬");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("Damage", PlayerT.attack);
        }
    }
}
