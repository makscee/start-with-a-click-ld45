using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class Screen : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public Button button;
    public Tinter tinter;
    private RawImage _rawImage;
    public static Screen CurrentScreen;
    public AudioSource AudioSource;

    private void OnEnable()
    {
        button.onClick.AddListener(OnClick);
        _rawImage = GetComponent<RawImage>();
        tinter = new Tinter(_rawImage.color);
    }

    protected virtual void Update()
    {
        var dt = Time.deltaTime;
        tinter.Update(dt);
        _rawImage.color = tinter.GetCurrent();
    }

    protected virtual void OnClick()
    {
        tinter.Blink(new Color(0.2f, 0.2f, 0.2f));
        AudioSource.Play();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        CurrentScreen = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CurrentScreen = null;
    }

    public virtual void DetachDot(CollectionDot dot)
    {
        
    }

    public abstract void AddDot(CollectionDot dot);
}