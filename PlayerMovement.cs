using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBeahviout
{
    [Header("Передвижение")]
    [SerializeField] private float _speed;
    private float _horizontalInput;

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

    [Header("Скольжение по стенам")]
    [SerializeField] private LayerMask _walls;
    [SerializeField] private GameObject _slideEffect;
    [SerializeField] private float _slideSpeed;
    private bool _sliding;

    private Rigidbody2D _rigidbody;
    private bool _lockMovement = false;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_lockMovement) return;

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _rigidbody.velocity = new Vector2(_horizontalInput * _speed, _rigidbody.velocity.y);

        _onGround = Physics2D.OverlapCircle(_jumpChecker.position, 0.1f, _ground);
        _sliding = Physics2D.Raycast(transform.position, new Vector2(_horizontalInput, 0), 0.5f, _walls)
    }

    private void Update()
    {
        if (_lockMovement) return;

        if (Input.GetKeyDown(KeyCode.Space) && _onGround && _canJump)
        {
            Jump(new Vector2(_rigidbody.velocity.x, _jumpForce));
                if(_sliding)
                    StartCoroutine(WallJump(_horizontalInput * -1))
        }
            
        else if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash)
            StartCoroutine(Dash());
    }

    private void Jump(Vector2 dircetion)
    {
        _rigidbody.velocity = direction;
        StartCoroutine(BlockJump());
    }

    IEnumerator WallJump(int direction)
    {
        _lockMovement = true;

        for(float i = 0; i < 0.25f; i += Time.deltaTime)
        {   
            float xVelocity = Mathf.Lerp(_speed * direction, 0, i / 0.25f)
            _rigidbody.velocity = new Vector2(xVelocity, _rigidbody.velocity.y);
        }

        _lockMovement = false;
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










