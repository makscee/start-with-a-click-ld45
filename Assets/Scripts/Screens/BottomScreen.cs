using  UnityEngine;

public class BottomScreen : Screen
{
    public static BottomScreen Instance;
    public RectTransform innerBar;
    private float _progress = 0f, _progressVisual = 0f;
    private const float ProgressPerDot = 0.1f;
    public Collection collection;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Update()
    {
        base.Update();
        _progressVisual += (_progress - _progressVisual) * Time.deltaTime * 2;
        innerBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 600 * _progressVisual);
    }

    public void CollectDot()
    {
        _progress += ProgressPerDot;
        if (_progress >= 1f)
        {
            _progress = 0f;
            _progressVisual = 0f;
            collection.AddValue(1);
        }
        tinter.Blink(new Color(0.02f, 0.1f, 0.02f));
    }

    public override void DetachDot(CollectionDot dot)
    {
        Debug.Log(dot.value);
        collection.DetachDot(dot.gameObject);
        collection.AddValue(-dot.value);
    }

    protected override void OnClick()
    {
        base.OnClick();
//        collection.AddValue(256);
    }

    public override void AddDot(CollectionDot dot)
    {
        Destroy(dot.gameObject);
        collection.AddValue(dot.value);
    }
}