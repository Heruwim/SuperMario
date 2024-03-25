using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _maxJumpHeight = 5f;
    [SerializeField] private float _maxJumpTime = 1f;

    private Camera _camera;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;

    private float _inputAxis;

    public float JumpForse => (2f * _maxJumpHeight) / (_maxJumpTime / 2f);
    public float Gravity => (-2f * _maxJumpHeight) / Mathf.Pow(_maxJumpTime / 2f, 2);

    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }

    public bool Running => Mathf.Abs(_velocity.x) > 0.25 || Mathf.Abs(_inputAxis) > 0.25;
    public bool Sliding => (_inputAxis > 0 &&  _velocity.x < 0) || (_inputAxis < 0 && _velocity.x > 0);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void Update()
    {
        HorizontalMovement();

        Grounded = _rigidbody.Raycast(Vector2.down);
        if (Grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = _rigidbody.position;
        position += _velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = _camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = _camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        _rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if(transform.DotTest(collision.transform, Vector2.up))
            {
                _velocity.y = 0f;
            }
        }
    }

    private void HorizontalMovement()
    {
        _inputAxis = Input.GetAxis("Horizontal");
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * _moveSpeed, _moveSpeed * Time.deltaTime);

        if(_rigidbody.Raycast(Vector2.right * _velocity.x))
        {
            _velocity.x = 0f;
        }

        if(_velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(_velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    private void GroundedMovement()
    {
        _velocity.y = Mathf.Max(_velocity.y, 0f);
        Jumping = _velocity.y > 0;

        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y = JumpForse;
            Jumping = true;
        }
    }

    private void ApplyGravity()
    {
        bool falling = _velocity.y < 0 || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        _velocity.y += Gravity * multiplier * Time.deltaTime;
        _velocity.y = Mathf.Max(_velocity.y, Gravity / 2);
    }
}
