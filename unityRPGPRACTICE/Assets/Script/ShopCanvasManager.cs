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
    Button GameButton;
    [SerializeField]
    GameObject Scroll;

    public bool isShop = false;
    public bool isFirst = true;

    Transform back;

    [SerializeField]
    private GameObject[] gameObjectPrefabArr;

    public List<GameObject> ItemMemeber;

    public void LoadSample()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void Start()
    {
        back = transform;
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

        gameObject.GetComponent<Image>().DOFade(1, 1f);
        GameButton.GetComponent<Image>().DOFade(1, 1f);
        Scroll.GetComponent<Image>().DOFade(0.3f, 1f);
        
    }

    public void OffShop()
    {
        isShop = false;
        gameObject.SetActive(false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        GameButton.image.color = new Color(1, 1, 1, 0);
        Scroll.GetComponent<Image>().color= new Color(1, 1, 1, 0);
    }

    public void objectSet()
    {
        for(int i = 0; i<gameObjectPrefabArr.Length; i++)
        {
            ItemMemeber.Add(Instantiate(gameObjectPrefabArr[i],ScrollIn.transform));
        }
    }
}
