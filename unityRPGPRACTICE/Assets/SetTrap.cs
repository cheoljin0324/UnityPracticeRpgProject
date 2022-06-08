using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SetTrap : MonoBehaviour
{
    [SerializeField]
    Transform ComPos;

    void ShotTrap()
    {
        gameObject.transform.DOMove(ComPos.position,0.5f, false);
    }
}
