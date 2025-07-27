using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;

    public bulletScript bulletScript;
    private bool iswin;
    public int objetivosRestantes = 5;

    [Header("Scene Configuration")]
    [Tooltip("Número de la escena a cargar al presionar Enter")]
    public int sceneIndex = 0;
    public GameObject textoGanar;
    private TextMeshProUGUI mensajeTextoContador;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {

        GameObject counter = GameObject.FindGameObjectWithTag("counter");
        if (counter != null)
        {
            mensajeTextoContador = counter.GetComponent<TextMeshProUGUI>();
            mensajeTextoContador.text = "BOTELLAS RESTANTES: " + objetivosRestantes;

        }

        if (textoGanar != null)
        {
            textoGanar.SetActive(false);
        }
        ActualizarTexto();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && iswin)
        {
            LoadSceneByIndex();
        }
    }
    public void ReducirObjetivos()
    {
        objetivosRestantes--;
        ActualizarTexto();

        if (objetivosRestantes <= 0)
        {

            if (textoGanar != null)
            {
                textoGanar.SetActive(true);
                iswin = true;
            }
            Debug.Log("¡Todos los objetivos eliminados!");
        }
    }

    public void LoadSceneByIndex()
    {
        // Verifica que el índice esté dentro del rango
        if (sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"SceneLoader: El índice de escena {sceneIndex} no es válido.");
            return;
        }

        SceneManager.LoadScene(sceneIndex);
    }

    void ActualizarTexto()
    {
        mensajeTextoContador.text = "BOTELLAS RESTANTES: " + objetivosRestantes;
    }
}
