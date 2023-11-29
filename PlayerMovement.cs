using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBeahviout
{
    [Header("Передвижение")]
    [SerializeField] private float _speed;

    [Header("Прыжок")]
    [SerializeField] private Transform _jumpChecker;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _jumpForce;
    private bool _onGround;
    private bool _canJump = true;

    [Header("Дэш")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _dashReload;
    private bool _canDash = true;

    private Rigidbody2D _rigidbody;
    private bool _lockMovement = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_lockMovement) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        _rigidbody.velocity = new Vector2(horizontalInput * _speed, _rigidbody.velocity.y);

        _onGround = Physics2D.OverlapCircle(_jumpChecker.position, 0.1f, _ground);
    }

    void Update()
    {
        if (_lockMovement) return;

        if (Input.GetKeyDown(KeyCode.Space) && _onGround && _canJump)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            StartCoroutine(BlockJump());
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
            StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        _lockMovement = true;
        _canDash = false;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 dashDirection = new Vector2(horizontalInput, verticalInput).normalize * _dashSpeed;
        /* Esli ne rabotaet, to udali i napishi:
         * Vector2 dashDirection = new Vector2(horizontalInput, verticalInput);
         * dashDirection = dashDirection.normalized * _dashSpeed;
         */

        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = dashDirection;

        yield return new WaitForSeconds(_dashTime);

        _rigidbody.gravityScale = 1;
        _lockMovement = false;

        StartCoroutine(ReloadDash());
    }

    IEnumerator BlockJump()
    {
        _canJump = false;
        yield return new WaitForSeconds(0.1f);
        _canJump = true;
    }

    IEnumerator ReloadDash()
    {
        yield return new WaitForSeconds(_dashReload);
        yield return new WaitUntil(() => _onGround);
        _canDash = true;
    }
}










