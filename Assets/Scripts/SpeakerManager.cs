using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

public class SpeakerManager : MonoBehaviour
{
    [SerializeField] private StartButtonEvent _startButtonEvent;
    [SerializeField] private TextMeshProUGUI _speakerText;
    [SerializeField] private Image _speakerImage;
    [SerializeField] private List<SpeakData> _sentences = new List<SpeakData>();
    [SerializeField] private float _speakDelay = 1;
    [SerializeField] private float _speakTimeout = 1;
    [SerializeField] private Fade _fade;
    private const float FADE_DURSTION = 1;
    private bool _isSpeaking = false;
    private int _currentSentenceIndex = -1;
    private string _currentSentence = string.Empty;
    private const string NEXT_SCENE_NAME = "MainGameScene";

    private void Awake()
    {
        _startButtonEvent.RegisterListener(SetUpSpeak);
    }

    private void SetUpSpeak() =>
        StartCoroutine(WaitSpeak());

    private IEnumerator WaitSpeak()
    {
        Debug.Log("Start Speaking");
        yield return new WaitForSeconds(FADE_DURSTION + _speakDelay);
        Debug.Log("Speak Now");
        _isSpeaking = true;
        Speak();
    }

    private void Speak()
    {
        Debug.Log(_currentSentenceIndex);
        if (_currentSentenceIndex + 1 >= _sentences.Count)
        {
            _isSpeaking = false;
            _fade.FadeIn();
            StartCoroutine(WaitSceneChange());
            return;
        }
        _currentSentenceIndex++;
        _speakerImage.sprite = _sentences[_currentSentenceIndex].Sprite;
        _speakerText.text = _sentences[_currentSentenceIndex].Sentence;
    }

    private IEnumerator WaitSceneChange()
    {
        yield return new WaitForSeconds(FADE_DURSTION);
        SceneManager.LoadScene(NEXT_SCENE_NAME);
    }

    private void Update()
    {
        if (!_isSpeaking) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Speak();
        }
    }

    private void OnDestroy()
    {
        _startButtonEvent.UnregisterListener(SetUpSpeak);
    }
}
