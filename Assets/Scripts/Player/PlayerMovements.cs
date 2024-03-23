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

    private void HorizontalMovement()
    {
        _inputAxis = Input.GetAxis("Horizontal");
        _velocity.x = Mathf.MoveTowards(_velocity.x, _inputAxis * _moveSpeed, _moveSpeed * Time.deltaTime);
    }

    private void GroundedMovement()
    {
        Jumping = _velocity.y > 0;

        if (Input.GetButtonDown("Jump"))
        {
            _velocity.y = JumpForse;
            Jumping = true;
        }
    }
}
