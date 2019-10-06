using System;
using System.Collections.Generic;
using  UnityEngine;
using Random = UnityEngine.Random;

public class MainScreen : Screen
{
    private struct Catcher
    {
        public Vector2 position;
        public float radius;
        public int value;
        public CollectionDot dot;
    }
    
    private List<Catcher> _catchers = new List<Catcher>();
    public static MainScreen Instance;
    public GameObject dotPrefab;
    private const int CollectLimit = 5;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDot()
    {
        var dot = Instantiate(dotPrefab, transform);
        var dotRectTransform = dot.GetComponent<RectTransform>(); 
        float yPos = Random.Range(-10, -390);
        dotRectTransform.anchoredPosition = new Vector2(dotRectTransform.anchoredPosition.x, yPos);
    }

    private void CollectDots()
    {
        var childCount = transform.childCount;
        var collected = 0;
        for (var i = 0; i < childCount; i++)
        {
            var dot = transform.GetChild(i).GetComponent<Dot>();
            if (dot)
            {
                dot.StartCollecting();
                if (++collected >= CollectLimit) break;
            }
        }
    }
    protected override void OnClick()
    {
        base.OnClick();
        CollectDots();
    }

    public override void DetachDot(CollectionDot dot)
    {
        dot.ShowRadius(false);
        for (var i = 0; i < _catchers.Count; i++)
        {
            if (_catchers[i].dot == dot)
            {
                _catchers.RemoveAt(i);
                break;
            }
        }
    }

    public override void AddDot(CollectionDot dot)
    {
        dot.transform.SetParent(transform);
        dot.ShowRadius(true);
        _catchers.Add(new Catcher()
        {
            position = dot.rect.anchoredPosition,
            radius = dot.radiusValue,
            value = dot.value,
            dot = dot,
        });
    }
}