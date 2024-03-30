using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour, IResetable
{
    private Rigidbody2D _myRigidbody = null;
    private AudioSource _myAudioSource = null;
    private PlayerScript _player = null;

    [BoxGroup("Movement")]
    [SerializeField]
    private float _velocity = 0f;
    [BoxGroup("Movement")]
    [SerializeField]
    private float _sinuousFrequency = 0f;
    [BoxGroup("Movement")]
    [SerializeField]
    private float _sinuousMagnitude = 0f;
    private bool _rocketStarted = false;

    [BoxGroup("Rotation")]
    [SerializeField]
    private Transform _rocketPoint = null;
    [BoxGroup("Rotation")]
    [SerializeField]
    private float _rotateSpeed = 100f;

    [BoxGroup("Effect")]
    [SerializeField]
    private float _scaleAmount = 3f;
    [BoxGroup("Effect")]
    [SerializeField]
    private float _scaleTime = 3f;

    [BoxGroup("SFX")]
    [SerializeField]
    private AudioClip _doorSfx = null;
    [BoxGroup("SFX")]
    [SerializeField]
    private AudioClip _engineSfx = null;

    [BoxGroup("Components")]
    [SerializeField]
    private SpriteRenderer _mySpriteRenderer = null;
    [BoxGroup("Components")]
    [SerializeField]
    private Collider2D _myCollider = null;

    //------------------------------------------------------------------

    private void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _myAudioSource = GetComponent<AudioSource>();
        _player = FindAnyObjectByType<PlayerScript>();

        GameManager.AddResetableObect(this);
    }

    private void Update()
    {
        RotateTowardsTarget();
    }

    private void FixedUpdate()
    {
        SinuousMovement();
    }

    //------------------------------------------------------------------

    private void SinuousMovement()
    {
        if (_rocketStarted)
        {
            _myRigidbody.MovePosition(_myRigidbody.position + ((Vector2)transform.up * Mathf.Sin(Time.time * _sinuousFrequency) * _sinuousMagnitude) +
                ((Vector2)transform.right * _velocity * Time.fixedDeltaTime));
        }

    }

    private void RotateTowardsTarget()
    {
        if (_rocketStarted)
        {
            float angle = Mathf.Atan2(_rocketPoint.position.y - transform.position.y, _rocketPoint.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotateSpeed * Time.deltaTime);
        }
    }

    public void StartRocket()
    {
        _myAudioSource.clip = _doorSfx;
        _myAudioSource.Play();
        _player.gameObject.SetActive(false);

        StartCoroutine(FlyAway());
    }

    private IEnumerator ScaleOverSeconds(GameObject objectToScale, Vector3 scaleTo, float seconds)
    {

        float elapsedTime = 0;
        Vector3 startingScale = objectToScale.transform.localScale;
        while (elapsedTime < seconds)
        {
            objectToScale.transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToScale.transform.localScale = scaleTo;
    }

    private IEnumerator FlyAway()
    {
        yield return new WaitForSeconds(0.75f);
        _mySpriteRenderer.sortingLayerName = "Player";
        _myAudioSource.clip = _engineSfx;
        _myAudioSource.Play();

        _rocketStarted = true;

        StartCoroutine(ScaleOverSeconds(gameObject, new Vector3(_scaleAmount, _scaleAmount, 1), _scaleTime));

        yield return new WaitForSeconds(_scaleTime);

        GameManager.GameOver();
    }

    public void SetCollider(bool State)
    {
        _myCollider.enabled = State;
    }

    public void ResetObject()
    {
        _myCollider.enabled = false;
    }
}
