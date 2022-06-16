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
        Debug.Log("BlueSword Active");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("Damage", PlayerT.attack);
        }
    }
}
