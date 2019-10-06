using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Dot : MonoBehaviour
{
    public float speed = 1f;
    public RectTransform rect;
    public Collider boxCollider;

    private void Update()
    {
        var position = transform.position;
        position.x -= speed * Time.deltaTime;
        // ReSharper disable once Unity.InefficientPropertyAccess
        transform.position = position;
        if (rect.anchoredPosition.x - rect.rect.width < 0)
        {
            Despawn();
        }
    }

    private bool _collecting;
    public void StartCollecting()
    {
        if (_collecting) return;
        var over = Random.Range(0.4f, 0.8f);
        Utils.Animate(transform.position, BottomScreen.Instance.transform.position - new Vector3(-over * speed, 0), over, (v) =>
            {
                transform.position += v;
            }, this, false, 0f, InterpolationType.InvSquare);
        Utils.InvokeDelayed(Collect, over, this);
        GetComponent<RawImage>().color = new Color(0.3f, 0.9f, 0.3f);
        _collecting = true;
        boxCollider.enabled = false;
    }

    private void Despawn()
    {
        Destroy(gameObject);
        LeftScreen.Instance.Collect();
    }

    private void Collect()
    {
        Destroy(gameObject);
        BottomScreen.Instance.CollectDot();
    }
}