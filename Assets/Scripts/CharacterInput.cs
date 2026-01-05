using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _selectedCharText;
    [SerializeField] private TextMeshProUGUI _attentionText;
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Judge _judge;
    [SerializeField] private float _attentionTime = 3f;
    [SerializeField] private GameClearAction _gameClearAction;

    private Action<bool> _wikiAction;
    private const string AKEOME = "あけおめ";
    private char _selectedChar = ' ';
    private int _currentIndex = 0;
    private string _currentInput = string.Empty;
    private List<string> _usedWords = new List<string>(4);
    private List<int> _scores = new List<int>(4);

    private void Awake()
    {
        _titleText.text = AKEOME;
        SetSelectChar(_currentIndex);
        _input.onSubmit.AddListener(OnSubmit);
        _wikiAction = WikiResult;
        AudioManager.Instance.PlayBGM(SoundType.BGM_Game);
    }

    private void SetSelectChar(int index)
    {
        if (index < AKEOME.Length)
        {
            _selectedChar = AKEOME[index];
            _selectedCharText.text = _selectedChar.ToString();
        }
        else
        {
            _gameClearAction.NotifyListeners(_usedWords, _scores);
        }
    }

    public void OnSubmit(string text)
    {
        Debug.Log(text);
        _input.text = string.Empty;
        _currentInput = text;
        StartCoroutine(Judge(text));
    }

    private IEnumerator Judge(string text)
    {
        // 頭文字が指定の文字から始まっているか確認
        yield return JudgeCurrentChar(text);

        // 頭文字が指定文字以外の場合注意文を表示し再度打ち直し
        if (!_judge.IsCorrect)
        {
            Debug.Log("attention");
            AudioManager.Instance.PlaySE(SoundType.SE_Wrong);
            yield return AttentionText($"頭文字を「{_selectedChar}」から始めてください");
            yield break;
        }

        yield return _judge.WikiTryGetPage(text, _wikiAction);
    }

    private void WikiResult(bool isSuccess)
    {
        if (isSuccess)
        {
            SuccessGetWikiPage();
        }
        else
        {
            AudioManager.Instance.PlaySE(SoundType.SE_Wrong);
            StartCoroutine(AttentionText("この単語はウィキペディアに掲載されていません"));
        }
    }

    private void SuccessGetWikiPage()
    {
        AudioManager.Instance.PlaySE(SoundType.SE_ButtonClick);
        _usedWords.Add(_currentInput);
        _currentInput = string.Empty;
        _input.text = string.Empty;
        _scores.Add(_judge.FuriganaLength);
        _currentIndex++;
        SetSelectChar(_currentIndex);
    }

    private IEnumerator JudgeCurrentChar(string text)
    {
        Debug.Log(_selectedChar);
        yield return _judge.JudgeText(text, _selectedChar);
    }

    private IEnumerator AttentionText(string text)
    {
        _attentionText.text = text;
        yield return new WaitForSeconds(_attentionTime);
        _attentionText.text = string.Empty;
    }
}
