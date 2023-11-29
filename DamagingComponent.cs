using UnityEngine;

public class DamagingComponent : MonoBehaviour
{
    [Header("Параметры урона")]
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PlayerHealth player))
            player.DoDamage(_damage);
    }
}