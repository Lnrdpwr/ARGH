using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour
{
    [Header("Крайние точки перемещения")]
    [SerializeField] private Transform[] _points;

    [Header("Параметры передвижения")]
    [SerializeField] private AnimationCurve _moveCurve;
    [SerializeField] private float _moveSpeed;

    void Start()
    {
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            foreach(Transform point in _points)
            {
                float moveTime = (transform.position - point.position).magnitude / _moveSpeed;
                Vector2 lastPosition = transform.position;

                for(float i = 0; i < moveTime; i += Time.deltaTime)
                {
                    transform.position = Vector2.Lerp(lastPosition, point.position, _moveCurve.Evaluate(i/moveTime))

                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}