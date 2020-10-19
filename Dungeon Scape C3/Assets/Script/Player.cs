using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private float _jumpForce = 5.0f;
    [SerializeField]
    private bool _grounded = false;
    private bool _onAir = false;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float move = Input.GetAxis("Horizontal");

        if (Input.GetMouseButtonDown(0) && _grounded && move == 0) _animator.SetTrigger("attack");

        if (Input.GetKeyDown(KeyCode.Space) && _grounded) 
        {
            _animator.SetBool("jump", true);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _grounded = false;
            _onAir = true;
            StartCoroutine(WaitAndUpdateAir());
        }

        var hitInfo = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, 1 << 8);
        Debug.DrawRay(transform.position, Vector2.down *  0.8f, Color.green);

        if (hitInfo.collider != null && !_onAir)
        {
            _grounded = true;
            _animator.SetBool("jump", false);
        }
        

        

        if (move > 0) _spriteRenderer.flipX = false;
        if (move < 0) _spriteRenderer.flipX = true;

        _rigidbody2D.velocity = new Vector2(move * 5, _rigidbody2D.velocity.y);

        _animator.SetFloat("move", Mathf.Abs(move));
    }
    private IEnumerator WaitAndUpdateAir()
    {
        yield return new WaitForSeconds(0.1f);
        _onAir = false;
        
    }

}
