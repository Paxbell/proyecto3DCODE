using TMPro;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
   

    public float damage = 25f;  // Daño que hará la bala, ajustable



    private void OnCollisionEnter(Collision HittedObject)
    {
        if (HittedObject.gameObject.CompareTag("Target") ||
            HittedObject.gameObject.CompareTag("Wall"))
        {
            print("contacto en " + HittedObject.gameObject.name);
            CreateBulletImpactEffect(HittedObject);
            Destroy(gameObject);
        }

        else if (HittedObject.gameObject.CompareTag("Bottle"))
        {
            print("contacto en botella!");
            HittedObject.gameObject.GetComponent<bottleScript>().shatter();
            gameManager.Instance.ReducirObjetivos();

            Destroy(gameObject);
        }

        else if (HittedObject.gameObject.CompareTag("Enemy"))  // Aquí el tag de tus zombies
        {
            print("Impacto en enemigo: " + HittedObject.gameObject.name);

            // Intenta obtener componente Zombie o BossZombie
            var zombie = HittedObject.gameObject.GetComponent<zombieScript>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
            else
            {
                var boss = HittedObject.gameObject.GetComponent<bossScript>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                }
            }

            CreateBulletImpactEffect(HittedObject);
            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision HittedObject)
    {
        ContactPoint contact = HittedObject.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(HittedObject.gameObject.transform);
    }
}
