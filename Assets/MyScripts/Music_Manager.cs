using UnityEngine;
using UnityEngine.UI;

namespace My_Scripts
{
    public class Music_Manager : MonoBehaviour
    {
        [SerializeField] private AudioSource[] bgms; // bgm入れるところ(AudiioSouceで管理)
        [SerializeField] private AudioSource bgmSettings;
        [SerializeField] private float[] bVolume;//bgm音量調整
        [SerializeField] private new AudioClip[] ses;　// se入れるところ
        [SerializeField] private AudioSource seSettings;
        [SerializeField] private float[] sVolume;//se音量調整
        [SerializeField] private Slider master;
        [SerializeField] private Slider sesSlider;
        [SerializeField] private Slider bgmsliSlider;
       
        private float[] basicbgms = new float[10];//BGMごとに適切な音量をつけたい
        private float[] basicses = new float[30];//同上(BGMのSE版)
        // Start is called before the first frame update
        void Start() //音量の初期値を記録する
        {
            bgmSettings.volume = basicbgms[0];
            MasterLoadSlider();
            BgmLoadSlider();
            SeLoadSlider();
        }

        //リロードしても音量が変わらないようにセーブを実装
        void MasterLoadSlider()
        {
            master.value = PlayerPrefs.GetFloat("MasterValue",0.8f);
            ChangeVolumeMaster(0);
        }
        void BgmLoadSlider()
        {
            bgmsliSlider.value = PlayerPrefs.GetFloat("bgmValue",0.8f);
            ChangeVolumeBgm(0);
        }
        void SeLoadSlider()
        {
            sesSlider.value = PlayerPrefs.GetFloat("seValue",0.8f);
            ChangeVolumeSes();
        }
        public void ChangeVolumeMaster(int bi)//主音源が変わる時
        {
            int i = 0;
            foreach (var bgm in bVolume)
            {
                basicbgms[i] = bgmsliSlider.value*bgm*master.value;
                bgms[i].volume = basicbgms[i];
                i++;
            }
            i = 0;
            foreach (var se in sVolume)
            {
                basicses[i] = bgmsliSlider.value*se*master.value;
                i++;
            }
            PlayerPrefs.SetFloat("MasterValue", master.value);
            Debug.Log("マスター："+master.value);
            Debug.Log("マスターSE："+bgmsliSlider.value);
            bgmSettings.volume = basicbgms[bi];
        }
        public void ChangeVolumeBgm(int bi)//BGM音源が変わる時
        {
           
            int i = 0;
            foreach (var bgm in bVolume)
            {
                basicbgms[i] = bgmsliSlider.value * bgm * master.value;
                Debug.Log("bgm:"+bgm);
                bgms[i].volume = basicbgms[i];
                i++;
            }
            PlayerPrefs.SetFloat("bgmValue", bgmsliSlider.value);
        }
        public void ChangeVolumeSes()//SE音源が変わる時
        {
            Debug.Log("cc");
            int i = 0;
            foreach (var se in sVolume)
            {
                basicses[i] = sesSlider.value*se*master.value;
                i++;
            }
            PlayerPrefs.SetFloat("seValue", sesSlider.value);
            
        }
        public void BgmExport(int number)//BGM音源が変わる時
        {
            int tmp = 0;
            bgms[number].Play();
            foreach (var bgm in bgms)
            {
                if (tmp != number)
                {
                    bgm.Stop();
                    tmp++;
                }
            }
            
            if(bgms[2] == true  && number != 2){bgms[2].Stop();}//
            if(bgms[1] == true  && number != 1){bgms[1].Stop();}//
            if(bgms[0] == true  && number != 0){bgms[0].Stop();}//
            //foreach文がうまくうごかないためif分で対応、ちゃんと作るなら直したい。
            bgms[number].volume = basicbgms[number];
            
        }
        public void SeExport(int number)//SEを出す時、（ただしなるべくotoman関数で対応したい）
        {
            seSettings.volume = basicses[number];
            seSettings.PlayOneShot(ses[number]);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
