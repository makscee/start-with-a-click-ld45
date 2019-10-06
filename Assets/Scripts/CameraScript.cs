using UnityEngine;
public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    public Camera Camera;
    public AudioSource AudioSource;
    private void Awake()
    {
        Instance = this;
        Camera = GetComponent<Camera>();
    }
}