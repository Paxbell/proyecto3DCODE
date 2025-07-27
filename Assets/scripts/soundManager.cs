using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager instance { get; set; }

    public AudioSource shootingSoundM911;
    public AudioSource reloadSound;
    public AudioSource emptySound;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
