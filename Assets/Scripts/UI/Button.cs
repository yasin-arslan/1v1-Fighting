using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Button Properties")]
    [SerializeField] private string _title;
    [SerializeField] private string _nextScene;
    [Header("Font Colors")]
    [SerializeField] private Color _defaultTextColor = Color.black;
    [SerializeField] private Color _hoverTextColor = Color.black;
    [SerializeField] private Color _clickedTextColor = Color.black;
    [Header("Background Colors")]
    [SerializeField] private Color _defaultBackgroundColor = Color.white;
    [SerializeField] private Color _hoverBackgroundColor = Color.white;
    [SerializeField] private Color _clickedBackgroundColor = Color.white;
    [Header("Animation Settings")]
    [SerializeField] private float _defaultAnimationDuration = .5f;
    [SerializeField] private float _hoverAnimationDuration = .5f;
    [SerializeField] private float _clickAnimationDuration = .5f;
    [Header("Controls")]
    [SerializeField] private TextMeshProUGUI _titleControl;
    [SerializeField] private Image _defaultBackgroundImage;
    [SerializeField] private Image _hoverBackgroundImage;
    [SerializeField] private CanvasGroup _defaultCanvasGroup;
    [SerializeField] private CanvasGroup _hoverCanvasGroup;
    [SerializeField] private LoadingManager loadingManager;

    private UnityAction _callbackAction;
    void OnValidate()
    {
        _titleControl.SetText(_title);
        _titleControl.color = _defaultTextColor;
        if (_defaultBackgroundImage == null)
        {
            Debug.LogFormat("{0} isn't initialized!", _defaultBackgroundImage);
        }
        if (_hoverBackgroundImage == null)
        {
            Debug.LogFormat("{0} isn't initialized!", _hoverBackgroundImage);
        }
        _defaultBackgroundImage.color = _defaultBackgroundColor;
        _hoverBackgroundImage.color = _hoverBackgroundColor;

    }
    public void RegisterCallback(UnityAction callback)
    {
        _callbackAction = callback;
    }
    /// <summary>
    ///Bu fonksiyon mouse pointerı buton alanına girdiğinde invoke ediliyor.
    ///Default image hover image ile değiştiriliyor, fade animasyonu bu değişim sırasında kullanılıyor.
    ///</summary>
    /// <param name="eventData">Mouse pointerı butonun alanına girdiğinde callback fonksiyonundan dönen parametre.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Sequence enterSequence = DOTween.Sequence();
        
        enterSequence.Append(
            DOTween.To(() => _titleControl.color, x => _titleControl.color = x, _hoverTextColor, _hoverAnimationDuration))
            .Join(_defaultCanvasGroup.DOFade(0, _hoverAnimationDuration))
            .Join(_hoverCanvasGroup.DOFade(1, _hoverAnimationDuration))
            .Join(DOTween.To(() => _defaultBackgroundImage.fillAmount, x => _defaultBackgroundImage.fillAmount = x, 0, _hoverAnimationDuration))
            .Join(DOTween.To(() => +_hoverBackgroundImage.fillAmount, x => _hoverBackgroundImage.fillAmount = x, 1, _hoverAnimationDuration)
        );
    }
    /// <summary>
    ///Bu fonksiyon mouse pointerı buton alanından çıktığında invoke ediliyor.
    ///Hover image default image ile değiştiriliyor, fade animasyonu bu değişim sırasında kullanılıyor.
    ///</summary>
    /// <param name="eventData">Mouse pointerı butonun alanından çıktığında callback fonksiyonundan dönen parametre.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Sequence exitSequence = DOTween.Sequence();
        exitSequence.Append(
            DOTween.To(() => _titleControl.color, x => _titleControl.color = x, _defaultTextColor, _defaultAnimationDuration))
            .Join(_defaultCanvasGroup.DOFade(1, _defaultAnimationDuration))
            .Join(_hoverCanvasGroup.DOFade(0, _defaultAnimationDuration))
            .Join(DOTween.To(() => _defaultBackgroundImage.fillAmount, x => _defaultBackgroundImage.fillAmount = x, 1, _defaultAnimationDuration))
            .Join(DOTween.To(() => _hoverBackgroundImage.fillAmount, x => _hoverBackgroundImage.fillAmount = x, 1, _defaultAnimationDuration)
        );
    }
    /// <summary>
    ///Bu fonksiyon mouse pointerı butona tıkladığında invoke ediliyor.
    ///Hover image color click background color ile değiştiriliyor, daha sonra defaulta dönülüp tekrar hovera dönülüyor fade animasyonu ile
    /// beraber farklı bir animasyon elde ediliyor.
    ///</summary>
    /// <param name="eventData">Mouse pointerı butonun alanına tıkladığında callback fonksiyonundan dönen parametre.</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Input.GetMouseButtonDown(0)) return;
        _callbackAction?.Invoke();
        Sequence clickSequence = DOTween.Sequence();
        clickSequence.Append(
            DOTween.To(() => _titleControl.color, x => _titleControl.color = x, _clickedTextColor, _clickAnimationDuration))
            .Join(DOTween.To(() => _hoverBackgroundImage.color, x => _hoverBackgroundImage.color = x, _clickedBackgroundColor, _clickAnimationDuration).OnComplete(
                () => { DOTween.To(() => _hoverBackgroundImage.color, x => _hoverBackgroundImage.color = x, _hoverBackgroundColor, _clickAnimationDuration); }
            ))
            .Append(DOTween.To(() => _titleControl.color, x => _titleControl.color = x, _defaultTextColor, _defaultAnimationDuration))
            .Join(_defaultCanvasGroup.DOFade(1, _defaultAnimationDuration))
            .Join(_hoverCanvasGroup.DOFade(0, _hoverAnimationDuration))
            .Join(DOTween.To(() => _defaultBackgroundImage.fillAmount, x => _defaultBackgroundImage.fillAmount = x, 1, _defaultAnimationDuration))
            .Join(DOTween.To(() => _hoverBackgroundImage.fillAmount, x => _hoverBackgroundImage.fillAmount = x, 0, _hoverAnimationDuration));
        //Eğer butonun mevcut sahnesinden sonraki sahnenin adı exit ise oyunu kapatıyor!
        if (_nextScene == "Exit")
        {
            loadingManager.exitGame();
        }
        else
        {
            loadingManager.loadLevel(_nextScene);
        }

    }
}
