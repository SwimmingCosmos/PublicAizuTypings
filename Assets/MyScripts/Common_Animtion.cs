using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using My_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Common_Animtion : MonoBehaviour,
IPointerUpHandler,
IPointerDownHandler,
IPointerEnterHandler,
IPointerExitHandler
{
    private RectTransform myDelta;
    private CanvasGroup myImage;
//基本的にbottonUIは全てこれにしたい
    [SerializeField] private Music_Manager music;
    // Start is called before the first frame update
    void Start()
    {
        myDelta = this.GetComponent<RectTransform>();
        myImage = GetComponent<CanvasGroup>();
    }
// タップダウン  
    public void OnPointerDown(PointerEventData eventData)
    {
       
    }  
    
    // タップアップ  
    public void OnPointerUp(PointerEventData eventData)
    {
        
    }  
    //ポインターイン
    public  void OnPointerEnter(PointerEventData eventData)
    {
       // transform.DOScale(1.05f, 0.3f);
        //myDelta.DOSizeDelta(new Vector2(10, 10), 0.3f).SetRelative();
        music.SeExport(7);
        myImage.DOFade(1f, 0.24f);
        
    }
    //ポインターアウト
    public void OnPointerExit(PointerEventData eventData)
    {
        //transform.DOScale(1f, 0.3f);
        //myDelta.DOSizeDelta(new Vector2(-10, -10), 0.3f).SetRelative();
        myImage.DOFade(0.6f, 0.24f);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
