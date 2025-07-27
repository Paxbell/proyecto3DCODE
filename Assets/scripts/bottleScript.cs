using System.Collections.Generic;
using UnityEngine;

public class bottleScript : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

    [Header("Sonidos de rotura")]
    public AudioClip[] sonidosRotura;
    public void shatter()
    {

        if (TryGetComponent<MeshRenderer>(out var mesh))
            mesh.enabled = false;

        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;


        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;
            part.gameObject.SetActive(true);
        }


        if (sonidosRotura != null && sonidosRotura.Length > 0)
        {
            int index = Random.Range(0, sonidosRotura.Length);
            AudioClip clipSeleccionado = sonidosRotura[index];

            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clipSeleccionado;
            audio.spatialBlend = 1f;
            audio.minDistance = 1f;
            audio.maxDistance = 15f;
            audio.rolloffMode = AudioRolloffMode.Linear;
            audio.Play();

            Destroy(audio, clipSeleccionado.length + 0.5f);
        }
    }
}
