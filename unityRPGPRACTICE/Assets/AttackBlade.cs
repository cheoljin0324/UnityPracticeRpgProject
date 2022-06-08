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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("Damage", PlayerT.attack);
        }
    }

}
