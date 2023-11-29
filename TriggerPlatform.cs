using UnityEngine;

public class TriggerPlatform : MonoBehaviour
{
    [Header("Параметры платформы")]
    [SerializeField] private AnimationCurve _slideCurve;
    [SerializeField] private Transform _destination;
    [SerializeField] private float _moveSpeed;

    private Vector2 _startPosition;
    private bool _waiting = true;
    private int _moveTime;

    void Start()
    {
        _startPosition = transform.position;
        _moveTime = (_startPosition - _destination.position).magnitude / _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && _waiting)
        {
            StartCoroutine(Move());
            collider.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            collider.transform.parent = null;
    }

    IEnumerator Move(Transform player)
    {
        _waiting = false;

        for(float i = 0; i <= _moveTime; i += Time.deltaTime)
        {
            transform.position = Vector2.Lerp(_startPosition, _destination.position, _slideCurve.Evaluate(i / _moveTime));

            yield return new WaitForEndOfFrame();
        }

        for (float i = _moveTime; i > 0; i -= Time.deltaTime)
        {
            transform.position = Vector2.Lerp(_startPosition, _destination.position, _slideCurve.Evaluate(i / _moveTime));

            yield return new WaitForEndOfFrame();
        }

        _waiting = true;
    }
}