using System.Collections.Generic;
using  UnityEngine;
public class RightScreen : Screen
{
    private class Ticker
    {
        public int value;
        public float period, t;

        public Ticker(int value)
        {
            this.value = value;
            period = 10f / value;
        }
    }

    private List<Ticker> _tickers = new List<Ticker>();
    protected override void OnClick()
    {
        tinter.Blink(new Color(0.12f, 0.12f, 0.12f));
        AudioSource.Play();
        MainScreen.Instance.SpawnDot();
    }

    protected override void Update()
    {
        base.Update();
        foreach (var ticker in _tickers)
        {
            ticker.t -= Time.deltaTime;
            if (ticker.t < 0)
            {
                OnClick();
                ticker.t = ticker.period;
            }
        }
    }

    public override void AddDot(CollectionDot dot)
    {
        dot.transform.SetParent(transform);
        _tickers.Add(new Ticker(dot.value));
    }

    public override void DetachDot(CollectionDot dot)
    {
        for (var i = 0; i < _tickers.Count; i++)
        {
            if (_tickers[i].value == dot.value)
            {
                _tickers.RemoveAt(i);
                break;
            }
        }
    }
}