using UnityEngine;

public class WarpTunnel : MonoBehaviour
{
    [SerializeField] private WarpTunnel otherTunnel;

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        other.transform.position = otherTunnel.transform.position;
    }
}
