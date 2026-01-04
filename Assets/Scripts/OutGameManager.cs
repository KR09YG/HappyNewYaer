using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class OutGameManager : MonoBehaviour
{
    [SerializeField] private StartButtonEvent _startButtonEvent;
    [SerializeField] private Fade _textBox;
    [SerializeField] private Fade _image;
    private const int FADE_DURSTION = 1;

    public void OnStartButton()
    {
        Debug.Log("OnStartButton");
        StartCoroutine(WaitEvent());
    }

    public IEnumerator WaitEvent()
    {
        yield return new WaitForSeconds(FADE_DURSTION);
        Debug.Log("StartEvent");
        _textBox.FadeIn();
        _image.FadeIn();
        _startButtonEvent.NotifyListeners();
    }
}
