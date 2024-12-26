using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using My_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Question//1問ごとに問題と背景を設定する
{
    public string japanese;
    public string roman;
    public Sprite background;
}

public class TypingManager : MonoBehaviour
{
    public int score  = 0;
    public float countTime = 0.0f;//タイムを数える
    [SerializeField] private Question[] questions;
    
    [SerializeField] private TextMeshProUGUI textScore; //スコア表示用件Text
    [SerializeField] private TextMeshProUGUI textTime;//タイム表示ようText

    [SerializeField] private SpriteRenderer le;//左の扉のレンダー、今回は不使用
    [SerializeField] private SpriteRenderer ri;//右の扉、同上
    //[SerializeField] private Sprite oni;
    //[SerializeField] private Sprite yoi;

    [SerializeField] private OnOff on;
    [SerializeField] private Sprite _noImage;
    [SerializeField] private Image BackScreen; // ここにそれぞれのイメージ画像をアタッチする。
    [SerializeField] private TextMeshProUGUI textJapanese; // ここに日本語表示のTextMeshProをアタッチする。
    [SerializeField] private TextMeshProUGUI textRoman; // ここにローマ字表示のTextMeshProをアタッチする。
    
    [SerializeField] private RectTransform left; 
    [SerializeField] private RectTransform right;
    private readonly List<char> _roman = new List<char>();
    private bool _isWindows; //追加する
    private bool _isMac; //追加する

    private int _romanIndex;

    private void Start()
    {
        if(SystemInfo.operatingSystem.Contains("Windows"))//Windowsを区別する
        {
            _isWindows = true;　
        }

        if(SystemInfo.operatingSystem.Contains("Mac"))//Windowsを区別する
        {
            _isMac = true;
        }
        
        InitializeQuestion();
    }

    private void Update()//カウント測る
    {
        //カウントアップを表示、15点以上で終了の設定にする
        textTime.text = String.Format("{0:00.00}", countTime);
        //カウントアップ
        countTime += Time.deltaTime;
        if (score >= 15)
        {
            StartCoroutine(DoorsClose());
            on.otoman(5);
            End();
        }
    }

    private void End()//エンディング
    {
        on.SecondOut();
    }
    
    public IEnumerator DoorsClose()//ドアが閉まる演出
    {
        left.DOLocalMoveX(-254, 0.5f);
        right.DOLocalMoveX(254, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DoorsOpen());
    }

    public void EarlyClose()//ドアがリセットなどで閉まる時
    {
        left.DOLocalMoveX(-254, 0.5f);
        right.DOLocalMoveX(254, 0.5f);
    }
    
    public IEnumerator DoorsOpen()//ドアが演出で開く時
    {
        
        left.DOLocalMoveX(-636, 0.5f);
        right.DOLocalMoveX(642, 0.5f);
        on.otoman(2);
        yield return new WaitForSeconds(0.6f);
        //le.sprite = oni;
        //ri.sprite = yoi;
        
    }
    private IEnumerator DoorsBadClose()//使わない
    {
        //le.sprite = oni;
        //ri.sprite = oni;
        left.DOLocalMoveX(-238, 0.5f);
        right.DOLocalMoveX(232, 0.5f);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DoorsOpen());
    }
    

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            switch (InputKey(GetCharFromKeyCode(Event.current.keyCode)))
            {
                case 1: // 正解タイプ時
                    _romanIndex++;
                    if (_roman[_romanIndex] == '@') // 「@」がタイピングの終わりの判定となる。
                    {
                        score++;
                        on.otoman(6);
                        StartCoroutine(DoorsClose());
                        InitializeQuestion();
                    }
                    else
                    {
                        textRoman.text = GenerateTextRoman();
                        on.otoman(4);
                    }
                    break;
                case 2://ミスったとき
                    if (Input.GetKey(KeyCode.Space))
                    {
                        break;
                    }
                    //score = 1;
                    //StartCoroutine(DoorsBadClose());
                    //InitializeQuestion();
                    textRoman.text = GenerateFailTextRoman();
                    on.otoman(3);
                    // ミスタイプ時
                    break;
            }
        }
    }

    public void InitializeQuestion()
    {


        Question question = questions[UnityEngine.Random.Range(0, questions.Length)];//出すクイズを決める
        BackScreen.sprite = question.background;//背景設定
        

        _roman.Clear();

        _romanIndex = 0;

        char[] characters = question.roman.ToCharArray();

        foreach (char character in characters)
        {
            _roman.Add(character);
        }

        _roman.Add('@');

        textJapanese.text = question.japanese;
        textRoman.text = GenerateTextRoman();
        textScore.text = "残り"+(15-score).ToString()+"個";
    }
    string GenerateFailTextRoman()
    {
        string text = "<style=typed>";
        for (int i = 0; i < _roman.Count; i++)
        {
            if (_roman[i] == '@')
            {
                break;
            }
            if (i == _romanIndex)
            {
                text += "</style><style=Failed>";
            }

            if (i == _romanIndex+1)
            {
                text += "</style><style=untyped>";
            }

            text += _roman[i];
        }

        text += "</style>";

        return text;
    }

    string GenerateTextRoman()
    {
        string text = "<style=typed>";
        for (int i = 0; i < _roman.Count; i++)
        {
            if (_roman[i] == '@')
            {
                break;
            }

            if (i == _romanIndex)
            {
                text += "</style><style=untyped>";
            }

            text += _roman[i];
        }

        text += "</style>";

        return text;
    }
    
    //ここからはアキオさんのサイトを参考
    //https://qiita.com/AkioMabuchi/items/a7afa292b9e47123b222
    int InputKey(char inputChar)
{
    char prevChar3 = _romanIndex >= 3 ? _roman[_romanIndex - 3] : '\0';
    char prevChar2 = _romanIndex >= 2 ? _roman[_romanIndex - 2] : '\0';
    char prevChar = _romanIndex >= 1 ? _roman[_romanIndex - 1] : '\0';
    char currentChar = _roman[_romanIndex];
    char nextChar = _roman[_romanIndex + 1];
    char nextChar2 = nextChar == '@' ? '@' : _roman[_romanIndex + 2];

    //前後の文字で入力する文字を変えたい。
    if (inputChar == '\0')
    {
        return 0;
    }

    if (inputChar == currentChar)
    {
        return 1;
    }

    //「い」の柔軟な入力（Windowsのみ）
    if (_isWindows && inputChar == 'y' && currentChar == 'i' &&
        (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' || prevChar == 'u' || prevChar == 'e' ||
         prevChar == 'o'))
    {
        _roman.Insert(_romanIndex, 'y');
        return 1;
    }

    if (_isWindows && inputChar == 'y' && currentChar == 'i' && prevChar == 'n' && prevChar2 == 'n' &&
        prevChar3 != 'n')
    {
        _roman.Insert(_romanIndex, 'y');
        return 1;
    }

    if (_isWindows && inputChar == 'y' && currentChar == 'i' && prevChar == 'n' && prevChar2 == 'x')
    {
        _roman.Insert(_romanIndex, 'y');
        return 1;
    }

    //「う」の柔軟な入力（「whu」はWindowsのみ）
    if (inputChar == 'w' && currentChar == 'u' && (prevChar == '\0' || prevChar == 'a' || prevChar == 'i' ||
                                                   prevChar == 'u' || prevChar == 'e' || prevChar == 'o'))
    {
        _roman.Insert(_romanIndex, 'w');
        return 1;
    }

    if (inputChar == 'w' && currentChar == 'u' && prevChar == 'n' && prevChar2 == 'n' && prevChar3 != 'n')
    {
        _roman.Insert(_romanIndex, 'w');
        return 1;
    }

    if (inputChar == 'w' && currentChar == 'u' && prevChar == 'n' && prevChar2 == 'x')
    {
        _roman.Insert(_romanIndex, 'w');
        return 1;
    }

    if (_isWindows && inputChar == 'h' && prevChar2 != 't' && prevChar2 != 'd' && prevChar == 'w' &&
        currentChar == 'u')
    {
        _roman.Insert(_romanIndex, 'h');
        return 1;
    }

    //「か」「く」「こ」の柔軟な入力（Windowsのみ）
    if (_isWindows && inputChar == 'c' && prevChar != 'k' &&
        currentChar == 'k' && (nextChar == 'a' || nextChar == 'u' || nextChar == 'o'))
    {
        _roman[_romanIndex] = 'c';
        return 1;
    }

    //「く」の柔軟な入力（Windowsのみ）
    if (_isWindows && inputChar == 'q' && prevChar != 'k' && currentChar == 'k' && nextChar == 'u')
    {
        _roman[_romanIndex] = 'q';
        return 1;
    }

    //「し」の柔軟な入力
    if (inputChar == 'h' && prevChar == 's' && currentChar == 'i')
    {
        _roman.Insert(_romanIndex, 'h');
        return 1;
    }

    //「じ」の柔軟な入力
    if (inputChar == 'j' && currentChar == 'z' && nextChar == 'i')
    {
        _roman[_romanIndex] = 'j';
        return 1;
    }

    //「しゃ」「しゅ」「しぇ」「しょ」の柔軟な入力
    if (inputChar == 'h' && prevChar == 's' && currentChar == 'y')
    {
        _roman[_romanIndex] = 'h';
        return 1;
    }

    //「じゃ」「じゅ」「じぇ」「じょ」の柔軟な入力
    if (inputChar == 'z' && prevChar != 'j' && currentChar == 'j' &&
        (nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
    {
        _roman[_romanIndex] = 'z';
        _roman.Insert(_romanIndex + 1, 'y');
        return 1;
    }

    if (inputChar == 'y' && prevChar == 'j' &&
        (currentChar == 'a' || currentChar == 'u' || currentChar == 'e' || currentChar == 'o'))
    {
        _roman.Insert(_romanIndex, 'y');
        return 1;
    }

    //「し」「せ」の柔軟な入力（Windowsのみ）
    if (_isWindows && inputChar == 'c' && prevChar != 's' && currentChar == 's' &&
        (nextChar == 'i' || nextChar == 'e'))
    {
        _roman[_romanIndex] = 'c';
        return 1;
    }

    //「ち」の柔軟な入力
    if (inputChar == 'c' && prevChar != 't' && currentChar == 't' && nextChar == 'i')
    {
        _roman[_romanIndex] = 'c';
        _roman.Insert(_romanIndex + 1, 'h');
        return 1;
    }

    //「ちゃ」「ちゅ」「ちぇ」「ちょ」の柔軟な入力
    if (inputChar == 'c' && prevChar != 't' && currentChar == 't' && nextChar == 'y')
    {
        _roman[_romanIndex] = 'c';
        return 1;
    }

    //「cya」=>「cha」
    if (inputChar == 'h' && prevChar == 'c' && currentChar == 'y')
    {
        _roman[_romanIndex] = 'h';
        return 1;
    }

    //「つ」の柔軟な入力
    if (inputChar == 's' && prevChar == 't' && currentChar == 'u')
    {
        _roman.Insert(_romanIndex, 's');
        return 1;
    }

    //「つぁ」「つぃ」「つぇ」「つぉ」の柔軟な入力
    if (inputChar == 'u' && prevChar == 't' && currentChar == 's' &&
        (nextChar == 'a' || nextChar == 'i' || nextChar == 'e' || nextChar == 'o'))
    {
        _roman[_romanIndex] = 'u';
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    if (inputChar == 'u' && prevChar2 == 't' && prevChar == 's' &&
        (currentChar == 'a' || currentChar == 'i' || currentChar == 'e' || currentChar == 'o'))
    {
        _roman.Insert(_romanIndex, 'u');
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    //「てぃ」の柔軟な入力
    if (inputChar == 'e' && prevChar == 't' && currentChar == 'h' && nextChar == 'i')
    {
        _roman[_romanIndex] = 'e';
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    //「でぃ」の柔軟な入力
    if (inputChar == 'e' && prevChar == 'd' && currentChar == 'h' && nextChar == 'i')
    {
        _roman[_romanIndex] = 'e';
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    //「でゅ」の柔軟な入力
    if (inputChar == 'e' && prevChar == 'd' && currentChar == 'h' && nextChar == 'u')
    {
        _roman[_romanIndex] = 'e';
        _roman.Insert(_romanIndex + 1, 'x');
        _roman.Insert(_romanIndex + 2, 'y');
        return 1;
    }

    //「とぅ」の柔軟な入力
    if (inputChar == 'o' && prevChar == 't' && currentChar == 'w' && nextChar == 'u')
    {
        _roman[_romanIndex] = 'o';
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }
    //「どぅ」の柔軟な入力
    if (inputChar == 'o' && prevChar == 'd' && currentChar == 'w' && nextChar == 'u')
    {
        _roman[_romanIndex] = 'o';
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    //「ふ」の柔軟な入力
    if (inputChar == 'f' && currentChar == 'h' && nextChar == 'u')
    {
        _roman[_romanIndex] = 'f';
        return 1;
    }

    //「ふぁ」「ふぃ」「ふぇ」「ふぉ」の柔軟な入力（一部Macのみ）
    if (inputChar == 'w' && prevChar == 'f' &&
        (currentChar == 'a' || currentChar == 'i' || currentChar == 'e' || currentChar == 'o'))
    {
        _roman.Insert(_romanIndex, 'w');
        return 1;
    }

    if (inputChar == 'y' && prevChar == 'f' && (currentChar == 'i' || currentChar == 'e'))
    {
        _roman.Insert(_romanIndex, 'y');
        return 1;
    }

    if (inputChar == 'h' && prevChar != 'f' && currentChar == 'f' &&
        (nextChar == 'a' || nextChar == 'i' || nextChar == 'e' || nextChar == 'o'))
    {
        if (_isMac)
        {
            _roman[_romanIndex] = 'h';
            _roman.Insert(_romanIndex + 1, 'w');
        }
        else
        {
            _roman[_romanIndex] = 'h';
            _roman.Insert(_romanIndex + 1, 'u');
            _roman.Insert(_romanIndex + 2, 'x');
        }
        return 1;
    }

    if (inputChar == 'u' && prevChar == 'f' &&
        (currentChar == 'a' || currentChar == 'i' || currentChar == 'e' || currentChar == 'o'))
    {
        _roman.Insert(_romanIndex, 'u');
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    if (_isMac && inputChar == 'u' && prevChar == 'h' && currentChar == 'w' &&
        (nextChar == 'a' || nextChar == 'i' || nextChar == 'e' || nextChar == 'o'))
    {
        _roman[_romanIndex] = 'u';
        _roman.Insert(_romanIndex + 1, 'x');
        return 1;
    }

    //「ん」の柔軟な入力（「n'」には未対応）
    if (inputChar == 'n' && prevChar2 != 'n' && prevChar == 'n' && currentChar != 'a' && currentChar != 'i' &&
        currentChar != 'u' && currentChar != 'e' && currentChar != 'o' && currentChar != 'y')
    {
        _roman.Insert(_romanIndex, 'n');
        return 1;
    }

    if (inputChar == 'x' && prevChar != 'n' && currentChar == 'n' && nextChar != 'a' && nextChar != 'i' &&
        nextChar != 'u' && nextChar != 'e' && nextChar != 'o' && nextChar != 'y')
    {
        if (nextChar == 'n')
        {
            _roman[_romanIndex] = 'x';
        }
        else
        {
            _roman.Insert(_romanIndex, 'x');
        }
        return 1;
    }

    //「うぃ」「うぇ」「うぉ」を分解する
    if (inputChar == 'u' && currentChar == 'w' && nextChar == 'h' && (nextChar2 == 'a' || nextChar2 == 'i' || nextChar2 == 'e' || nextChar2 == 'o' ))
    {
        _roman[_romanIndex] = 'u';
        _roman[_romanIndex] = 'x';
    }

    //「きゃ」「にゃ」などを分解する
    if (inputChar == 'i' && currentChar == 'y' &&
        (prevChar == 'k' || prevChar == 's' || prevChar == 't' || prevChar == 'n' || prevChar == 'h' ||
         prevChar == 'm' || prevChar == 'r' || prevChar == 'g' || prevChar == 'z' || prevChar == 'd' ||
         prevChar == 'b' || prevChar == 'p') &&
        (nextChar == 'a' || nextChar == 'u' || nextChar == 'e' || nextChar == 'o'))
    {
        if (nextChar == 'e')
        {
            _roman[_romanIndex] = 'i';
            _roman.Insert(_romanIndex + 1, 'x');
        }
        else
        {
            _roman.Insert(_romanIndex, 'i');
            _roman.Insert(_romanIndex + 1, 'x');
        }
        return 1;
    }

    //「しゃ」「ちゃ」などを分解する
    if (inputChar == 'i' &&
        (currentChar == 'a' || currentChar == 'u' || currentChar == 'e' || currentChar == 'o') &&
        (prevChar2 == 's' || prevChar2 == 'c') && prevChar == 'h')
    {
        if (nextChar == 'e')
        {
            _roman.Insert(_romanIndex, 'i');
            _roman.Insert(_romanIndex + 1, 'x');
        }
        else
        {
            _roman.Insert(_romanIndex, 'i');
            _roman.Insert(_romanIndex + 1, 'x');
            _roman.Insert(_romanIndex + 2, 'y');
        }
        return 1;
    }

    //「しゃ」を「c」で分解する（Windows限定）
    if (_isWindows && inputChar == 'c' && currentChar == 's' && prevChar != 's' && nextChar == 'y' &&
        (nextChar2 == 'a' || nextChar2 == 'u' || nextChar2 == 'e' || nextChar2 == 'o'))
    {
        if (nextChar2 == 'e')
        {
            _roman[_romanIndex] = 'c';
            _roman[_romanIndex + 1] = 'i';
            _roman.Insert(_romanIndex + 1, 'x');
        }
        else
        {
            _roman[_romanIndex] = 'c';
            _roman.Insert(_romanIndex + 1, 'i');
            _roman.Insert(_romanIndex + 2, 'x');
        }
        return 1;
    }

    //「っ」の柔軟な入力
    if ((inputChar == 'x' || inputChar == 'l') &&
        (currentChar == 'k' && nextChar == 'k' || currentChar == 's' && nextChar == 's' ||
         currentChar == 't' && nextChar == 't' || currentChar == 'g' && nextChar == 'g' ||
         currentChar == 'z' && nextChar == 'z' || currentChar == 'j' && nextChar == 'j' ||
         currentChar == 'd' && nextChar == 'd' || currentChar == 'b' && nextChar == 'b' ||
         currentChar == 'p' && nextChar == 'p'))
    {
        _roman[_romanIndex] = inputChar;
        _roman.Insert(_romanIndex + 1, 't');
        _roman.Insert(_romanIndex + 2, 'u');
        return 1;
    }

    //「っか」「っく」「っこ」の柔軟な入力（Windows限定）
    if (_isWindows && inputChar == 'c' && currentChar == 'k' && nextChar == 'k' &&
        (nextChar2 == 'a' || nextChar2 == 'u' || nextChar2 == 'o'))
    {
        _roman[_romanIndex] = 'c';
        _roman[_romanIndex + 1] = 'c';
        return 1;
    }

    //「っく」の柔軟な入力（Windows限定）
    if (_isWindows && inputChar == 'q' && currentChar == 'k' && nextChar == 'k' && nextChar2 == 'u')
    {
        _roman[_romanIndex] = 'q';
        _roman[_romanIndex + 1] = 'q';
        return 1;
    }

    //「っし」「っせ」の柔軟な入力（Windows限定）
    if (_isWindows && inputChar == 'c' && currentChar == 's' && nextChar == 's' &&
        (nextChar2 == 'i' || nextChar2 == 'e'))
    {
        _roman[_romanIndex] = 'c';
        _roman[_romanIndex + 1] = 'c';
        return 1;
    }

    //「っちゃ」「っちゅ」「っちぇ」「っちょ」の柔軟な入力
    if (inputChar == 'c' && currentChar == 't' && nextChar == 't' && nextChar2 == 'y')
    {
        _roman[_romanIndex] = 'c';
        _roman[_romanIndex + 1] = 'c';
        return 1;
    }

    //「っち」の柔軟な入力
    if (inputChar == 'c' && currentChar == 't' && nextChar == 't' && nextChar2 == 'i')
    {
        _roman[_romanIndex] = 'c';
        _roman[_romanIndex + 1] = 'c';
        _roman.Insert(_romanIndex + 2, 'h');
        return 1;
    }

    //「l」と「x」の完全互換性
    if (inputChar == 'x' && currentChar == 'l')
    {
        _roman[_romanIndex] = 'x';
        return 1;
    }

    if (inputChar == 'l' && currentChar == 'x')
    {
        _roman[_romanIndex] = 'l';
        return 1;
    }

    return 2;
}

    char GetCharFromKeyCode(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A:
                return 'a';
            case KeyCode.B:
                return 'b';
            case KeyCode.C:
                return 'c';
            case KeyCode.D:
                return 'd';
            case KeyCode.E:
                return 'e';
            case KeyCode.F:
                return 'f';
            case KeyCode.G:
                return 'g';
            case KeyCode.H:
                return 'h';
            case KeyCode.I:
                return 'i';
            case KeyCode.J:
                return 'j';
            case KeyCode.K:
                return 'k';
            case KeyCode.L:
                return 'l';
            case KeyCode.M:
                return 'm';
            case KeyCode.N:
                return 'n';
            case KeyCode.O:
                return 'o';
            case KeyCode.P:
                return 'p';
            case KeyCode.Q:
                return 'q';
            case KeyCode.R:
                return 'r';
            case KeyCode.S:
                return 's';
            case KeyCode.T:
                return 't';
            case KeyCode.U:
                return 'u';
            case KeyCode.V:
                return 'v';
            case KeyCode.W:
                return 'w';
            case KeyCode.X:
                return 'x';
            case KeyCode.Y:
                return 'y';
            case KeyCode.Z:
                return 'z';
            case KeyCode.Alpha0:
                return '0';
            case KeyCode.Alpha1:
                return '1';
            case KeyCode.Alpha2:
                return '2';
            case KeyCode.Alpha3:
                return '3';
            case KeyCode.Alpha4:
                return '4';
            case KeyCode.Alpha5:
                return '5';
            case KeyCode.Alpha6:
                return '6';
            case KeyCode.Alpha7:
                return '7';
            case KeyCode.Alpha8:
                return '8';
            case KeyCode.Alpha9:
                return '9';
            case KeyCode.Minus:
                return '-';
            case KeyCode.Caret:
                return '^';
            case KeyCode.Backslash:
                return '\\';
            case KeyCode.At:
                return '@';
            case KeyCode.LeftBracket:
                return '[';
            case KeyCode.Semicolon:
                return ';';
            case KeyCode.Colon:
                return ':';
            case KeyCode.RightBracket:
                return ']';
            case KeyCode.Comma:
                return ',';
            case KeyCode.Period:
                return '.';
            case KeyCode.Slash:
                return '/';
            case KeyCode.Underscore:
                return '_';
            case KeyCode.Backspace:
                return '\b';
            case KeyCode.Return:
                return '\r';
            case KeyCode.Space:
                return ' ';
            default: //上記以外のキーが押された場合は「null文字」を返す。
                return '\0';
        }
    }
}