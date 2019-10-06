using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CollectionDot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int value, lvl;
    public RawImage rawImage;
    public RectTransform radius;
    public Screen attachedTo;
    public float radiusValue;
    public RectTransform rect;
    public SphereCollider sphereCollider;

    private float _radiusSpeed = 4f;
    private float _curRadius, _initRadius;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(DotsGroup.Instance.transform);
        rawImage.raycastTarget = false;
        if (attachedTo)
        {
            attachedTo.DetachDot(this);
        }
    }

    private void Update()
    {
        if (!radius.gameObject.activeSelf) return;
        if (_curRadius < _initRadius)
        {
            _curRadius += Time.deltaTime * _radiusSpeed;
        }

        if (_curRadius > _initRadius) _curRadius = _initRadius;
        SetRadiusSize(_curRadius);
    }

    public void ShowRadius(bool value)
    {
        var v = rawImage.rectTransform.rect.width;
        radiusValue = v + 14 * lvl;
        radius.gameObject.SetActive(value);
        sphereCollider.enabled = value;
        _curRadius = radiusValue;
        _initRadius = _curRadius;
        _radiusSpeed = lvl * lvl * lvl * 5;
        SetRadiusSize(radiusValue);
        Debug.Log(_radiusSpeed);
    }

    private void SetRadiusSize(float value)
    {
        radius.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        radius.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,  value);
        sphereCollider.radius = value / 2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var v = CameraScript.Instance.Camera.ScreenToWorldPoint(eventData.position);
        v.z = 0;
        transform.position = v;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!radius.gameObject.activeSelf) return;
        var dot = other.GetComponent<Dot>();
        if (dot)
        {
            dot.StartCollecting();
            _curRadius = 0f;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(Screen.CurrentScreen);
        if (Screen.CurrentScreen != null)
        {
            Screen.CurrentScreen.AddDot(this);
        }
        attachedTo = Screen.CurrentScreen;
        rawImage.raycastTarget = true;
        var v = rect.anchoredPosition3D;
        v.z = 0;
        rect.anchoredPosition3D = v;
    }
}