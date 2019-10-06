using UnityEngine;

public class DotsGroup : MonoBehaviour
{
    public static DotsGroup Instance;

    private void Awake()
    {
        Instance = this;
    }
}