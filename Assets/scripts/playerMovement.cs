using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float velocidad = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocidad_salto;
    bool isGrounded;
    private Vector3 lastPosition;

    [Header("Sonidos")]
    public AudioClip caminarSound;
    public AudioClip saltoSound;

    private AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = gameObject.AddComponent<AudioSource>();
        lastPosition = transform.position;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocidad_salto.y < 0)
        {
            velocidad_salto.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * velocidad * Time.deltaTime);

        // Reproducir sonido de caminar
        bool seMueve = move.magnitude > 0.1f;
        if (isGrounded && seMueve && !audioSource.isPlaying)
        {
            audioSource.clip = caminarSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if ((!seMueve || !isGrounded) && audioSource.clip == caminarSound)
        {
            audioSource.Stop();
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocidad_salto.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (saltoSound != null)
            {
                audioSource.PlayOneShot(saltoSound);
            }
        }

        velocidad_salto.y += gravity * Time.deltaTime;
        controller.Move(velocidad_salto * Time.deltaTime);
    }
}
