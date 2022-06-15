using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemDataBase : MonoBehaviour
{
    [SerializeField]
    public ItemSet[] setItem;

}

[System.Serializable]
public struct ItemSet
{
    public Sprite ItemForm;
    public string ItemName;
    public int ItemID;
    public bool isBuy;
    public bool isUse;
    public GameObject Item;
    public int ItemSell;
}
