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
    Button GameButton;
    [SerializeField]
    GameObject Scroll;
    [SerializeField]
    GameObject GameBase;

    public bool isShop = false;
    public bool isFirst = true;

    Transform back;

    [SerializeField]
    private GameObject[] gameObjectPrefabArr;

    public List<GameObject> ItemMemeber;

    private void Start()
    {
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
        GameButton.GetComponent<Image>().DOFade(1, 1f);
        Scroll.GetComponent<Image>().DOFade(0.3f, 1f);
        
    }

    public void OffShop()
    {
        isShop = false;
        GameBase.SetActive(false);
        GameBase.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameButton.image.color = new Color(1, 1, 1, 0);
        Scroll.GetComponent<Image>().color= new Color(1, 1, 1, 0);
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
        if (item.isBuy == true)
        {
            if (item.isUse == false)
            {
                for (int i = 0; i < ItemData.setItem.Length; i++)
                {
                    ItemData.setItem[i].isUse = false;
                }
                item.isUse = true;
                GameManager.Instance.userData.isUse[item.ItemID] = true;
                GameManager.Instance.SaveToJson();
            }
            
        }
        else
        {
            if (item.ItemSell < GameManager.Instance.userData.coin)
            {
                item.isBuy = true;
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
