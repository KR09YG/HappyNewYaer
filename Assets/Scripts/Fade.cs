using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    [SerializeField] private FadeObjType _fadeObjType;
    public enum FadeObjType
    {
        Image,
        TMPro,
        Button
    }

    
    public void FadeOut()
    {
        switch (_fadeObjType)
        {
            case FadeObjType.Image:
                GetComponent<Image>().DOFade(0, 1f);
                break;
            case FadeObjType.TMPro:
                GetComponent<TMPro.TextMeshProUGUI>().DOFade(0, 1f);
                break;
            case FadeObjType.Button:
                GetComponent<Button>().image.DOFade(0, 1f);
                GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(0, 1f);
                break;
        }
    }

    public void FadeIn()
    {
        Debug.Log(_fadeObjType);
        switch (_fadeObjType)
        {
            case FadeObjType.Image:
                Debug.Log("FadeIn Image");
                GetComponent<Image>().DOFade(1, 1f);
                break;
            case FadeObjType.TMPro:
                GetComponent<TMPro.TextMeshProUGUI>().DOFade(1, 1f);
                break;
            case FadeObjType.Button:
                GetComponent<Button>().image.DOFade(1, 1f);
                GetComponentInChildren<TMPro.TextMeshProUGUI>().DOFade(1, 1f);
                break;
        }
    }
}
