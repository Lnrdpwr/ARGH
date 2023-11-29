using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Transform _checkpoint;
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private float _resurrectionTime;

    public void DoDamage()
    {
        Instantiate(_deathEffect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        StartCoroutine(Resurrection());
    }

    IEnumerator Resurrection()
    {
        yield return new WaitForSeconds(_resurrectionTime);
        transform.position = _checkpoint.position;
        gameObject.SetActive(true);
    }
}