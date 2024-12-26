using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Com_Botton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject OB;//オンオフを切り替えるオブジェクト

    private float now;//今の状態
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
            Time.timeScale = 0;
        }
        else
        {
            if (now != 0)
            {
                OB.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                OB.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
