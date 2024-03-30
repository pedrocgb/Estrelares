using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerScript : MonoBehaviour, IResetable
{
    [FoldoutGroup("Movement")]
    [SerializeField]
    private float _moveSpeed = 2f;
    private float _currentSpeed = 0f;
    private bool _isMoving = false;
    private Vector3 _direction = Vector3.back;
    private Vector3 _startPosition = Vector3.zero;
    private Quaternion _startRotation;

    [FoldoutGroup("Jump")]
    [SerializeField]
    private float _jumpPower = 10f;
    [FoldoutGroup("Jump")]
    [SerializeField]
    private float _jumpDuration = 1f;
    [FoldoutGroup("Jump")]
    [SerializeField]
    private float _fallDuration = 1f;
    private float _jumpTimeStamp = 0f;
    private float _fallTimeStamp = 0f;
    private bool _isGrounded = true;
    private bool _isJumping = false;

    [FoldoutGroup("Invert")]
    [SerializeField]
    private float _invertY = 0.75f;
    private float _currentInvertY = 0f;
    private bool _inverted = false;

    [FoldoutGroup("World")]
    [SerializeField]
    private Transform _currentWorld = null;
    private Transform _starterWorld = null;

    [FoldoutGroup("SFX")]
    [SerializeField]
    private AudioClip _jumpAudio = null;

    [FoldoutGroup("Components")]
    [SerializeField]
    private Animator _myAnimator = null;
    [FoldoutGroup("Components")]
    [SerializeField]
    private SpriteRenderer _mySpriteRenderer = null;
    [FoldoutGroup("Components")]
    [SerializeField]
    private AudioSource _myAudioSource = null;

    [FoldoutGroup("Components/Animators")]
    [SerializeField]
    private RuntimeAnimatorController _boyAnimator = null;
    [FoldoutGroup("Components/Animators")]
    [SerializeField]
    private RuntimeAnimatorController _girlAnimator = null;

    //------------------------------------------------------------------

    private void Start()
    {
        GameManager.AddResetableObect(this);
        _currentSpeed = _moveSpeed;
        _currentInvertY = _invertY;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _starterWorld = _currentWorld;

        if (Settings.SelectedCharacter == Character.Boy)
            _myAnimator.runtimeAnimatorController = _boyAnimator;
        else if (Settings.SelectedCharacter == Character.Girl)
            _myAnimator.runtimeAnimatorController = _girlAnimator;
    }

    private void Update()
    {
        InputHandler();
        Jump();
        Falling();
    }

    void FixedUpdate()
    {
        Move();
    }

    //------------------------------------------------------------------

    private void InputHandler()
    {
        // Change direction
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = Vector3.forward;
            FlipSprite(false);
        }
        if (Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = Vector3.back;
            FlipSprite(true);
        }

        // Move Player
        if (Input.GetKey(KeyCode.A) && !_isMoving ||
            Input.GetKey(KeyCode.LeftArrow) && !_isMoving ||
            Input.GetKey(KeyCode.D) && !_isMoving ||
            Input.GetKey(KeyCode.RightArrow) && !_isMoving)
        {
            _isMoving = true;
            _myAnimator.SetBool("Walking", _isMoving);
        }
        else if (Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.RightArrow))
        {
            _isMoving = false;
            _myAnimator.SetBool("Walking", _isMoving);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            StartJump();

        // Invert
        if (Input.GetKeyDown(KeyCode.W) && _inverted && _isGrounded ||
            Input.GetKeyDown(KeyCode.UpArrow) && _inverted && _isGrounded)
            InvertPosition(false);
        else if (Input.GetKeyDown(KeyCode.S) && !_inverted && _isGrounded ||
            Input.GetKeyDown(KeyCode.DownArrow) && !_inverted && _isGrounded)
            InvertPosition(true);
    }

    #region Movement Methods
    private void Move()
    {
        if (_isMoving)
        {
            transform.RotateAround(_currentWorld.transform.localPosition, _direction, Time.fixedDeltaTime * _currentSpeed);
        }
    }

    private void FlipSprite(bool Flip)
    {
        _mySpriteRenderer.flipX = Flip;
    }

    private void InvertPosition(bool Invert)
    {
        _inverted = Invert;
        
        if (_inverted)
        {
            _mySpriteRenderer.transform.localPosition = new Vector3(0, _currentInvertY, 0);
        }
        else
        {
            _mySpriteRenderer.transform.localPosition = new Vector3(0, 0, 0);
        }

        _mySpriteRenderer.flipY = _inverted;
    }
    #endregion

    #region Jump Methods
    private void StartJump()
    {
        _isJumping = true;
        _isGrounded = false;
        _myAnimator.SetBool("Jumping", true);

        _myAudioSource.clip = _jumpAudio;
        _myAudioSource.Play();

        _jumpTimeStamp = Time.time + _jumpDuration;
    }

    private void Jump()
    {
        if (_isJumping)
        {
            if (_jumpTimeStamp <= Time.time)
            {
                _isJumping = false;
                _fallTimeStamp = Time.time + _fallDuration;
            }
            
            if (!_inverted)
                transform.Translate(new Vector3(0, _jumpPower * Time.deltaTime, 0), Space.Self);
            else if (_inverted)
                transform.Translate(new Vector3(0, -_jumpPower * Time.deltaTime, 0), Space.Self);
        }
    }

    private void Falling()
    {
        if (!_isJumping && !_isGrounded)
        {
            if (_fallTimeStamp <= Time.time)
            {
                _isGrounded = true;
                _myAnimator.SetBool("Jumping", false);
            }


            if (!_inverted)
                transform.Translate(new Vector3(0, -_jumpPower * Time.deltaTime, 0), Space.Self);
            else if (_inverted)
                transform.Translate(new Vector3(0, _jumpPower * Time.deltaTime, 0), Space.Self);
        }
    }


    #endregion

    public void ChangeWorld(Vector3 NewPosition, float NewZRot, Transform NewWorld, float SpeedMultiplier, float NewInverterY)
    {
        transform.position = NewPosition;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, NewZRot);
        _currentWorld = NewWorld;
        _currentSpeed *= SpeedMultiplier;
        _currentInvertY = NewInverterY;
    }

    public void ResetObject()
    {
        InvertPosition(false);
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _currentSpeed = _moveSpeed;
        _currentInvertY = _invertY;
        _currentWorld = _starterWorld;

        _isMoving = false;
        _isGrounded = true;
        _isJumping = false;

        _myAnimator.SetBool("Jumping", false);
        _myAnimator.SetBool("Walking", false);
    }
}
