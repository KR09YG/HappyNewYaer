using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Text;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour
{
    [SerializeField] private GameClearAction _gameClearAction;
    [SerializeField] private Fade[] _fades;
    [SerializeField] private Fade _resultPanel;
    [SerializeField] private Fade _text;
    [SerializeField] private Fade _button;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _sumText;
    [SerializeField] private TextMeshProUGUI _speakText;
    [SerializeField] private Image _image;
    [SerializeField] private List<ResultData> _resultDataList = new List<ResultData>();
    private const string AKEOME = "‚ ‚¯‚¨‚ß";
    private const int BONUS_PER_CHAR = 300;


    private void Awake()
    {
        _gameClearAction.RegisterListener(EndGame);
    }

    public void Title()
    {
        AudioManager.Instance.PlaySE(SoundType.SE_ButtonClick);
        SceneManager.LoadScene("Title");
    }

    private void EndGame(List<string> result, List<int> scores)
    {
        int resultMoney = scores.Sum() * BONUS_PER_CHAR;
        Result(resultMoney);

        AudioManager.Instance.PlayBGM(SoundType.BGM_Result);

        foreach (var fade in _fades)
        {
            fade.FadeIn();
        }

        _image.GetComponent<Fade>().FadeIn();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < result.Count; i++)
        {
            sb.AppendLine($"{AKEOME[i]} {result[i]}");
        }

        _resultText.text = sb.ToString();

        sb.Clear();

        for (int i = 0; i < scores.Count; i++)
        {
            string s = i == scores.Count - 1 ? $"{AKEOME[i]}:{scores[i]}•¶Žš = " : $"{AKEOME[i]}:{scores[i]}•¶Žš + ";
            sb.Append(s);
        }

        _scoreText.text = sb.ToString();
        _sumText.text = $"Œv: {resultMoney}‰~";
    }

    private void Result(int score)
    {
        int befor = _resultDataList[0].score;
        if (score < befor)
        {
            _resultText.text = _resultDataList[0].sentence;
            _image.sprite = _resultDataList[0].Sprite;
            _speakText.text = _resultDataList[0].sentence;
            return;
        }

        for (int i = 1; i < _resultDataList.Count; i++)
        {
            if (score >= befor && score < _resultDataList[i].score)
            {
                _resultText.text = _resultDataList[i - 1].sentence;
                _image.sprite = _resultDataList[i - 1].Sprite;
                _speakText.text = _resultDataList[i - 1].sentence;
                return;
            }

            if (i == _resultDataList.Count - 1)
            {
                _resultText.text = _resultDataList[i].sentence;
                _image.sprite = _resultDataList[i].Sprite;
                _speakText.text = _resultDataList[i].sentence;
            }
        }
    }

    private void OnDestroy()
    {
        _gameClearAction.UnregisterListener(EndGame);
    }
}
