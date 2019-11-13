using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.1f);

        Gizmos.color = Color.red;

        Vector3 offset = transform.position + transform.right * 0.5f;
        Gizmos.DrawLine(transform.position, offset);
        Gizmos.DrawLine(offset, transform.position + transform.right * 0.4f + transform.up * -0.1f);
        Gizmos.DrawLine(offset, transform.position + transform.right * 0.4f + transform.up * 0.1f);
    }
#endif
}
