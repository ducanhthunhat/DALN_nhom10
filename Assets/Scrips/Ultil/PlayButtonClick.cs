using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(Button))]
public class PlayButtonClick : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickClip));

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayButtonHover();
    }
}
