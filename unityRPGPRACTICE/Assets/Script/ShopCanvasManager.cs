using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShopCanvasManager : MonoSingleton<ShopCanvasManager>
{
    [SerializeField]
    private Transform ShopPos;
    public int ShopMemberShipLevel = 0;
    [SerializeField]
    GameObject ScrollIn;

    public bool isShop = false;
    public bool isFirst = true;

    Transform back;

    [SerializeField]
    private GameObject[] gameObjectPrefabArr;

    public List<GameObject> ItemMemeber;

    private void Start()
    {
        back = transform;
        SetShop();
    }

    public void SetShop()
    {
        isShop = true;
        gameObject.SetActive(true);
        if(isFirst == true)
        {
            isFirst = false;
            objectSet();
        }

        gameObject.transform.DOMove(ShopPos.position, 1f, false);
    }

    public void OffShop()
    {
        isShop = false;
        gameObject.SetActive(false);
        gameObject.transform.DOMove(back.position, 1f, false);
    }

    public void objectSet()
    {
        for(int i = 0; i<gameObjectPrefabArr.Length; i++)
        {
            ItemMemeber.Add(Instantiate(gameObjectPrefabArr[i],ScrollIn.transform));
        }
    }
}
