using System;
using  UnityEngine;
using UnityEngine.UI;

public class LeftScreen : Screen
{
    public static LeftScreen Instance;
    public RectTransform innerBar;
    private float _progress = 0f, _progressVisual = 0f;
    private float _progressPerDot = 0.015f, _losePerSecond = 0.2f;
    private bool _nextLevel = false;
    public RawImage rawImage;
    public Text endText;
    public Canvas gameCanvas, endCanvas;

    public AudioClip click, explosion1, explosion2;

    private void Awake()
    {
        Instance = this;
    }
    
    public void Collect()
    {
        tinter.Blink(new Color(0.1f, 0.1f, 0.02f));
        _progress += _progressPerDot;
    }

    protected override void OnClick()
    {
        base.OnClick();
        _progress += _progressPerDot;
    }

    protected override void Update()
    {
        base.Update();
        _progressVisual += (_progress - _progressVisual) * Time.deltaTime * 2;
        innerBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400 * _progressVisual);
        if (_progress > 0)
        {
            _progress -= Time.deltaTime * _losePerSecond;
        }

        _progress = Math.Max(0f, _progress);
        if (_progress > 1f)
        {
            _progress = 0f;
            _progressVisual = 0f;
            OnFilled();
        }

        if (_nextLevel)
        {
            _losePerSecond = _progress > 0.6f ? 1f : 0.2f;
        }
    }

    private void OnFilled()
    {
        var _as = CameraScript.Instance.AudioSource;
        if (_nextLevel)
        {
            gameCanvas.gameObject.SetActive(false);
            endCanvas.gameObject.SetActive(true);
            Utils.InvokeDelayed(() => { endText.text += "Start with nothing\n"; }, 1f);
            Utils.InvokeDelayed(() => { endText.text += "End with everything\n\n"; }, 4f);
            Utils.InvokeDelayed(() => { endText.text += "but not really."; }, 9f);
            _as.clip = explosion2;
            _as.Play();
            return;
        } 
        _as.clip = explosion1;
        _as.Play();
        gameObject.SetActive(false);
        Utils.InvokeDelayed(() =>
        {
            gameObject.SetActive(true);
        }, 2f);
        tinter.Initial = Color.black;
        rawImage.color = Color.black;
        Utils.Animate(Color.black, new Color(1f, 0.56f, 0f) * 0.4f, 2f, (v) => { tinter.Initial = v; }, null, true, 2f);
        _nextLevel = true;
        _progressPerDot = 1f / 250;
        _losePerSecond = 0.2f;
    }

    public override void AddDot(CollectionDot dot)
    {
        Destroy(dot.gameObject);
        tinter.Blink(new Color(0.3f, 0.3f, 0.1f));
        _progress += _progressPerDot * dot.value;
    }
}