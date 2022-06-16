using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("This is my Trap");
        gameObject.transform.parent.SendMessage("ShotTrap");
    }
}
