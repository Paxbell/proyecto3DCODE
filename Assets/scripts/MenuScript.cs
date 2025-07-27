using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject playbutton; //boton de play
        public GameObject controlbutton; //boton de controles
    public GameObject controles; //texto de controles

    
    void Start()
    {
        controles.SetActive(false);
        playbutton.SetActive(true);
        controlbutton.SetActive(true);
    }


    public void Controles()
    {
        controles.SetActive(true);
    }

    public void Jugar()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
