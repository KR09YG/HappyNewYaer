using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Judge : MonoBehaviour
{
    private const string YAHOO_CLIENT_ID = "dj00aiZpPVU3Mk1LaUZIOEx0TiZzPWNvbnN1bWVyc2VjcmV0Jng9NDY-";
    private const string YAHOO_API_URL = "https://jlp.yahooapis.jp/FuriganaService/V2/furigana";

    private const string WIKI_API_URL = "https://ja.wikipedia.org/w/api.php";

    private string _furiganaResult = string.Empty;
    private const int CHAR_OFFSET = 96; // 'あ'のUnicode値から1を引いた値

    public int FuriganaLength => _furiganaResult.Length;

    public bool IsCorrect { get; private set; } = false;

    [System.Serializable]
    private class YahooResponse
    {
        public Result result;
    }

    [System.Serializable]
    private class Result
    {
        public Word[] word;
    }

    [System.Serializable]
    private class Word
    {
        public string surface;
        public string furigana;
    }

    private void Start()
    {
        StartCoroutine(GetFurigana("明けまして"));
    }

    public IEnumerator JudgeText(string s,char c)
    {
        IsCorrect = false;
        yield return GetFurigana(s);

        if (_furiganaResult.Length == 0)
        {
            Debug.Log("読み仮名の取得に失敗");
            yield break;
        }

        // 読み仮名の最初の文字がカタカナだった時
        if (_furiganaResult[0] >= 'ア' && _furiganaResult[0] <= 'ン')
        {
            Debug.Log((char)(_furiganaResult[0] - CHAR_OFFSET));
            if ((char)c == _furiganaResult[0] - CHAR_OFFSET)
            {
                IsCorrect = true;
                Debug.Log("正解");
                yield break;
            }
            else
            {
                Debug.Log("不正解");
                yield break;
            }
        }

        // 読み仮名の最初の文字がひらがなだった時
        if (c == _furiganaResult[0])
        {
            IsCorrect = true;
            Debug.Log("正解");
        } 
    }

    // 読み仮名を取得
    public IEnumerator GetFurigana(string text)
    {
        string json = "{\"id\":\"1\",\"jsonrpc\":\"2.0\",\"method\":\"jlp.furiganaservice.furigana\",\"params\":{\"q\":\""
                      + text + "\",\"grade\":1}}";

        string url = $"{YAHOO_API_URL}?appid={YAHOO_CLIENT_ID}";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("User-Agent", "UnityWebRequest");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string furigana = ParseFurigana(request.downloadHandler.text);
                Debug.Log($"{text} → {furigana}");
                _furiganaResult = furigana;
            }
            else
            {
                Debug.LogError($"Error: {request.error}");
            }
        }
    }

    private string ParseFurigana(string jsonResponse)
    {
        var response = JsonUtility.FromJson<YahooResponse>(jsonResponse);
        StringBuilder result = new StringBuilder();

        if (response == null || response.result == null || response.result.word == null)
        {
            return string.Empty;
        }

        foreach (var word in response.result.word)
        {
            result.Append(word.furigana ?? word.surface);
        }

        return result.ToString();
    }

    public IEnumerator WikiTryGetPage(string text, Action<bool> action)
    {
        string url = WIKI_API_URL + "?action=query" +
            "&titles=" + UnityWebRequest.EscapeURL(text) +
            "&format=json";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }

            string json = request.downloadHandler.text;

            // "missing" が含まれていなければページが存在
            bool exists = !json.Contains("\"missing\"");
            action(exists);
        }
    }
}