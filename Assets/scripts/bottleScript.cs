using System.Collections.Generic;
using UnityEngine;

public class bottleScript : MonoBehaviour
{
    public List<Rigidbody> allParts = new List<Rigidbody>();

public void shatter()
{
    // Desactivar el mesh y collider del objeto original
    if (TryGetComponent<MeshRenderer>(out var mesh))
        mesh.enabled = false;

    if (TryGetComponent<Collider>(out var col))
        col.enabled = false;

    // Activar fragmentos
    foreach (Rigidbody part in allParts)
    {
        part.isKinematic = false;
        part.gameObject.SetActive(true);
    }
}
}
