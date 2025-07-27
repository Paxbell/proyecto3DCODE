using TMPro;
using UnityEngine;

public class ammoManager : MonoBehaviour
{
    public static ammoManager instance { get; set; }

        public TextMeshProUGUI ammoDisplay;
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
