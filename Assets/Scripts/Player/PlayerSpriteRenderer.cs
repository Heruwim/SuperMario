using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    [SerializeField] private Sprite _idle;
    [SerializeField] private Sprite _jump;
    [SerializeField] private Sprite _slide;
    [SerializeField] private Sprite _run;

    private SpriteRenderer _spriteRenderer;
    private PlayerMovements _movements;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movements = GetComponentInParent<PlayerMovements>();
    }

    private void LateUpdate()
    {
        if (_movements.Jumping)
        {
            _spriteRenderer.sprite = _jump;
        }
        else if (_movements.Sliding)
        {
            _spriteRenderer.sprite = _slide;
        }
        else if (_movements.Running)
        {
            _spriteRenderer.sprite = _run;
        }
        else
        {
            _spriteRenderer.sprite = _idle;
        }
    }
}
