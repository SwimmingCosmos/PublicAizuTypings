using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using My_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Music_Manager music;
    [SerializeField] private TextMeshProUGUI improve;//足される単位
    [SerializeField] private TextMeshProUGUI sums;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRect;
    public ulong _unitSum;//ulongは符号なし64ビット整数,　数値の合計
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void SilderDisplayCome()
    {
        //transform.DOLocalMoveY(-171.159f, 1f);
    }
    public void SilderDisplayBay()
    {
       // transform.DOLocalMoveY(-258f, 1f);
    }
    public void UnitIncrease(ulong increases)
    {
        if (increases == 0)
        {
            return;
        }
         Vector2 mpoint;
        improve.text = "+"+increases+"単位";
        _unitSum += (ulong) increases;
        var ups = Instantiate(improve,transform.position,Quaternion.identity,canvas.transform);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect,Input.mousePosition,canvas.worldCamera,out mpoint);
        sums.text = _unitSum + "単位";
        ups.transform.localPosition = new Vector2(Random.Range(mpoint.x-10,mpoint.x+10),Random.Range(mpoint.y+18,mpoint.y+25));
        ups.transform.DOLocalMoveY(30f, 1f).SetRelative();
        Destroy(ups.gameObject,0.7f);
        
    }

    
}
