using System;
using System.Collections;
using System.Collections.Generic;
using My_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using unityroom.Api;

public class OnOff : MonoBehaviour  // 物を隠したり表示したりする
{
    [SerializeField] private GameObject[] titles;
    [SerializeField] private GameObject[] games;
    [SerializeField] private GameObject[] results;
    [SerializeField] private AudioClip[] bgms;

    [SerializeField] private GameObject owari;
    [SerializeField] private TypingManager tyMg;

    [SerializeField] private GameObject kekka;
    [SerializeField] private TextMeshProUGUI xxdan;

    [SerializeField] private GameObject xdan;

    [SerializeField] private GameObject result;

    [SerializeField] private GameObject don;
    [SerializeField] private GameObject good;
    [SerializeField] private TextMeshProUGUI dondo;
    public  Music_Manager music;

    /*[SerializeField] private AudioSource meoto;
    [SerializeField] private AudioSource cat;
    [SerializeField] private AudioSource over;
    [SerializeField] private Slider ss;*/
    // Start is called before the first frame update
    void Start()
    { /*float basicCat = cat.volume;
        float basicmeoto = meoto.volume;
        float basicover = over.volume;*/
    }

    public void allMusic()//使わない関数
    {
       /* meoto.volume =ss.value;
        cat.volume = 0.1f*ss.value;
        over.volume = ss.value;*/
    }
    public void otoman(int o)//音を出す
    {
        music.SeExport(o);
    }

    public void FirstClick()//ゲーム始める用の関数
    {
      //  meoto.Stop();
        foreach (GameObject i in titles)//タイトル全部消す
        {
            i.gameObject.SetActive(false);
        }
        
        
        foreach (GameObject i in games)//ゲーム始める
        {
            i.gameObject.SetActive(true);
        }
        music.BgmExport(1);
        tyMg.InitializeQuestion();
        StartCoroutine(tyMg.DoorsOpen());
    }
    public void SecondOut()//規定数打ち終わったら呼び出す
    {
        //cat.Stop();
        foreach (GameObject i in games)//ゲーム始める
        {
            i.gameObject.SetActive(false);
        }
       /* foreach (GameObject i in results)//ゲーム始める
        {
            i.gameObject.SetActive(true);
        }*/

        xxdan.text = tyMg.countTime.ToString("F2")+"秒";
        UnityroomApiClient.Instance.SendScore(1, tyMg.countTime, ScoreboardWriteMode.HighScoreAsc);//unityroomのAPIを叩いてランキング登録
        if (tyMg.score <= 1)
        {
            dondo.text = "Thank you for playing my game!";
        }
        else
        {
            good.SetActive(true);
        }
        tyMg.countTime = 0.0f;
        StartCoroutine(Moves());
    }

    public void Retry() //やり直す時に実行する、全てを初期化
    {
        int a= 0;
        music.BgmExport(0);//meoto
        tyMg.EarlyClose();
        foreach (GameObject i in games)//最後の部分全部消す
        {
            i.gameObject.SetActive(false);
        }
        foreach (GameObject i in results)//最後の部分全部消す
        {
            i.gameObject.SetActive(false);
        }
        foreach (GameObject i in titles)//最後の部分全部戻す
        {
            i.gameObject.SetActive(true);
            a++;
            if (a == 4)
            {
                i.gameObject.SetActive(false); 
            }
        }
        tyMg.countTime = 0.0f;
        tyMg.score = 0;
        tyMg.InitializeQuestion();
    }
    

    private IEnumerator Moves()//結果発表のコルーチン、Unitaskにしたい
    {
       owari.SetActive(true);
       otoman(1);
       yield return new WaitForSeconds(3f);
       owari.SetActive(false);
       yield return new WaitForSeconds(0.5f);
       kekka.SetActive(true);
       otoman(0);
       yield return new WaitForSeconds(0.8f);
       xdan.SetActive(true);
       otoman(0);
       yield return new WaitForSeconds(0.8f);
       result.SetActive(true);
       otoman(0);
       yield return new WaitForSeconds(0.8f);
       don.SetActive(true);
       otoman(0);
       music.BgmExport(2);//gameover
    }
    // Update is called once per frame
    void OnGUI()//スペースでリトライ
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Retry();
        }
    }
}
