using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopCanvasManager : MonoSingleton<ShopCanvasManager>
{
    public int ShopMemberShipLevel = 0;
    [SerializeField]
    GameObject ScrollIn;
    [SerializeField]
    ItemDataBase ItemData;
    [SerializeField]
    GameObject Scroll;
    [SerializeField]
    GameObject GameBase;
    [SerializeField]
    Image gameImage;
    [SerializeField]
    Button OffButton;

    public bool isShop = false;
    public bool isFirst = true;

    Transform back;

    [SerializeField]
    private GameObject[] gameObjectPrefabArr;

    public List<GameObject> ItemMemeber;

    private void Start()
    {
        GameManager.Instance.userData.Item[0] = true;
        GameManager.Instance.SaveToJson();
        GameManager.Instance.userData.isUse[0] = true;
        for(int i = 1; i < GameManager.Instance.userData.isUse.Length; i++)
        {
            if (GameManager.Instance.userData.isUse[i] == true)
            {
                GameManager.Instance.userData.isUse[0] = false;
            }
        }
        back = transform;
    }

    public void SetShop()
    {
        isShop = true;
        GameBase.SetActive(true);
        if(isFirst == true)
        {
            isFirst = false;
            objectSet();
        }

        GameBase.GetComponent<Image>().DOFade(1, 1f);
        Scroll.GetComponent<Image>().DOFade(0.3f, 1f);
        gameImage.DOFade(0.3f, 1f);
        OffButton.image.DOFade(1f, 1f);
    }

    public void OffShop()
    {
        isShop = false;
        GameBase.SetActive(false);
        GameBase.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        gameImage.color = new Color(1, 1, 1, 0);
        Scroll.GetComponent<Image>().color= new Color(1, 1, 1, 0);
        OffButton.image.color = new Color(1, 1, 1, 0);
    }

    public void objectSet()
    {
        for(int i = 0; i<gameObjectPrefabArr.Length; i++)
        {
            int temp = i;
            ItemMemeber.Add(Instantiate(gameObjectPrefabArr[i],ScrollIn.transform));
            ItemMemeber[i].GetComponentsInChildren<Image>()[1].sprite = ItemData.setItem[i].ItemForm;
            ItemMemeber[temp].GetComponentsInChildren<Button>()[1].onClick.AddListener(()=> ButtonClick(ItemData.setItem[temp], ItemMemeber[temp].GetComponentsInChildren<Button>()[1]));
            if(GameManager.Instance.userData.Item[i] == true)
            {
                ItemData.setItem[i].isBuy = true;
                ItemMemeber[temp].GetComponentsInChildren<Button>()[1].image.color = Color.red;
            }
            if (GameManager.Instance.userData.isUse[i] == true)
            {
                ItemData.setItem[i].isUse = true;
                ItemMemeber[temp].GetComponentsInChildren<Button>()[1].image.color = Color.blue;
            }
        }
    }

    public void ButtonClick(ItemSet item,Button BuyButton)
    {
        if (GameManager.Instance.userData.Item[item.ItemID] == true)
        {
            if (GameManager.Instance.userData.isUse[item.ItemID] == false)
            {
                for (int i = 0; i < ItemData.setItem.Length; i++)
                {
                    ItemData.setItem[i].isUse = false;
                    GameManager.Instance.userData.isUse[i] = false;
                }
                item.isUse = true;
                GameManager.Instance.userData.isUse[item.ItemID] = true;
                GameManager.Instance.SaveToJson();
                for(int i = 0; i<ItemMemeber.Count; i++)
                {
                    ItemMemeber[i].GetComponentsInChildren<Button>()[1].image.color = Color.red;
                }
                BuyButton.image.color = Color.blue;
            }
            
        }
        else
        {
            if (item.ItemSell < GameManager.Instance.userData.coin)
            {
                item.isBuy = true;
                GameManager.Instance.userData.coin -= item.ItemSell;
                Debug.Log("아이템을 구매 했습니다.");
                GameManager.Instance.userData.Item[item.ItemID] = true;
                GameManager.Instance.SaveToJson();
                BuyButton.image.color = Color.red;
            }
            else
            {
                Debug.Log("아이템을 구매 하기 위한 코인이 모자랍니다.");
            }
        }
       
        
        
    }
}
