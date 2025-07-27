using UnityEngine;
using UnityEngine.AI;

public class zombieScript : MonoBehaviour
{
    public float health = 100f;
    public float speed = 3.5f;
    public float attackDistance = 2f;
    public float damagePerAttack = 10f;
    public float attackCooldown = 1.5f;

    protected Transform player;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float lastAttackTime;

    protected bool isDead = false;

    [Header("Sonidos")]
    public AudioClip sonidoIdle;
    public AudioClip sonidoAtaque;
    public AudioClip sonidoMuerte;

    private AudioSource audioSource;

    // Cambia Start a virtual y protected para poder sobreescribirlo
    protected virtual void Start()
    {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    agent = GetComponent<NavMeshAgent>();
    animator = GetComponent<Animator>();
    agent.speed = speed;

    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.loop = true;
    audioSource.spatialBlend = 1f;
    audioSource.minDistance = 1f;
    audioSource.maxDistance = 15f;
    audioSource.rolloffMode = AudioRolloffMode.Linear;

    if (sonidoIdle != null)
    {
        audioSource.clip = sonidoIdle;
        audioSource.Play();
    }
    }

    void Update()
    {
        if (isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackDistance)
        {
            agent.isStopped = true;
            animator.SetBool("walk", false);
            animator.SetTrigger("attack");

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                if (sonidoAtaque != null)
                    audioSource.PlayOneShot(sonidoAtaque);

                player.GetComponent<healthScript>().TakeDamage(damagePerAttack);
                lastAttackTime = Time.time;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("walk", true);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("die");

        if (sonidoMuerte != null)
        {
            audioSource.Stop(); // detener sonido idle si está
            audioSource.loop = false;
            audioSource.clip = sonidoMuerte;
            audioSource.Play();
        }

        Destroy(gameObject, 3.5f); // Opcional: se destruye después de 3 segundos
    }
}
