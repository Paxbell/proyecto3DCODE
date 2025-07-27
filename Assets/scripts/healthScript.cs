using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class healthScript : MonoBehaviour
{
    [Header("Valores de Vida")]
    public float vidaMaxima = 100f;
    private float vidaActual;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoPorcentajeVida;
    [SerializeField] private Image efectoSangre;
    [SerializeField] private TextMeshProUGUI textoMuerte;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoHerida;
    [SerializeField] private AudioClip sonidoCorazon;
    [SerializeField] private AudioClip sonidoMuerte;
    [SerializeField] private float alturaFinalCamara = 1f;

    [Header("Referencias de Jugador")]
    [SerializeField] private MonoBehaviour playerMovementScript;


    [SerializeField] private MonoBehaviour playerMouseMovement;

    [SerializeField] private GameObject Weapon;

    [SerializeField] private GameObject camaraJugador;
    [SerializeField] private float duracionCaidaCamara = 2f;
    private bool corazonActivo = false;
    private bool yaMurio = false;

    void Start()
    {
        vidaActual = vidaMaxima;

      
        if (textoPorcentajeVida != null)
            textoPorcentajeVida.text = "100%";

        if (efectoSangre != null)
            efectoSangre.canvasRenderer.SetAlpha(0f);

        if (textoMuerte != null)
            textoMuerte.gameObject.SetActive(false);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float cantidad)
    {
        if (yaMurio) return;

  
        vidaActual = Mathf.Clamp(vidaActual - cantidad, 0f, vidaMaxima);
        if (textoPorcentajeVida != null)
        {
            int pct = Mathf.CeilToInt((vidaActual / vidaMaxima) * 100f);
            textoPorcentajeVida.text = pct + "%";
        }

    
        if (sonidoHerida != null)
            audioSource.PlayOneShot(sonidoHerida);

     
        if (efectoSangre != null)
            StartCoroutine(FadeEfectoSangre());

 
        if (vidaActual <= 10f && !corazonActivo && sonidoCorazon != null)
        {
            corazonActivo = true;
            audioSource.PlayOneShot(sonidoCorazon);
        }


        if (vidaActual <= 0f)
            Morir();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && yaMurio)
        {
            SceneManager.LoadScene(1);
        }
    }
    private void Morir()
    {
        yaMurio = true;

       
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        if (playerMouseMovement != null)
            playerMouseMovement.enabled = false;

        if (Weapon != null)
            Weapon.SetActive(false);


        if (textoMuerte != null)
            textoMuerte.gameObject.SetActive(true);

      
        if (efectoSangre != null)
            efectoSangre.CrossFadeAlpha(0.8f, 0.5f, false);

    
        if (sonidoMuerte != null)
            audioSource.PlayOneShot(sonidoMuerte);

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Transform camT = mainCam.transform;
            camT.SetParent(null);
            StartCoroutine(RotarCamaraProgressiva(camT));
        }

    }

    private IEnumerator RotarCamaraProgressiva(Transform camT)
    {
        Quaternion rotInicial = camT.rotation;
        Vector3 posInicial = camT.position;
        Quaternion rotFinal = Quaternion.Euler(90f, rotInicial.eulerAngles.y, rotInicial.eulerAngles.z);
        Vector3 posFinal = new Vector3(posInicial.x, alturaFinalCamara, posInicial.z);

        float elapsed = 0f;
        while (elapsed < duracionCaidaCamara)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duracionCaidaCamara);
            camT.rotation = Quaternion.Slerp(rotInicial, rotFinal, t);
            camT.position = Vector3.Lerp(posInicial, posFinal, t);
            yield return null;
        }


        camT.rotation = rotFinal;
        camT.position = posFinal;
    }
    private IEnumerator FadeEfectoSangre()
    {

        efectoSangre.gameObject.SetActive(true);
        efectoSangre.canvasRenderer.SetAlpha(0f);
        efectoSangre.CrossFadeAlpha(0.6f, 0.1f, false);

        yield return new WaitForSeconds(0.4f);

        efectoSangre.CrossFadeAlpha(0f, 0.8f, false);
    }
}
