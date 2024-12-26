using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class naturebotton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject OB;//切り替えるオブジェクト

    private float now;
    //[SerializeField] private bool onOff = false;//ブール関数

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OB.activeSelf == false)
        {
            OB.SetActive(true);
            now = Time.timeScale;
            //レポート押すと消えるバグあり、無視する
        }
        else
        {
            OB.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
