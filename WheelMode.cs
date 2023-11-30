using UnityEngine;

public class WheelMode : MonoBeahviour
{
    [Header("Параметры колеса")]
    [SerializeField] private float _maxAcceleration;
    [SerializeField] private float _accelerationGain;
    private float _acceleration;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        _acceleration = Mathf.Clamp(-_maxAcceleration, _maxAcceleration, _acceleration + horizontalInput * Time.deltaTime * _accelerationGain);
        _rigidbody.velocity = new Vector2(_acceleration, _rigidbody.velocity.y);
    }
}