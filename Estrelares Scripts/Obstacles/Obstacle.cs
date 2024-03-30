using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IResetable
{
    protected bool _finishedSpawn = false;

    [FoldoutGroup("Pop Up")]
    [SerializeField]
    private float _minRandomStart = 0f;
    [FoldoutGroup("Pop Up")]
    [SerializeField]
    private float _maxRandomStart = 1f;
    private float RandomStart
    {
        get { return Random.Range(_minRandomStart, _maxRandomStart); }
    }

    [FoldoutGroup("Pop Up")]
    [SerializeField]
    private float _popUpSize = 1.2f;

    [BoxGroup("Components")]
    [SerializeField]
    private Collider2D _myCollider = null;

    private AudioSource _myAudioSource = null;

    //------------------------------------------------------------------

    protected virtual void Start()
    {
        GameManager.AddResetableObect(this);
        _myAudioSource = GetComponent<AudioSource>();

        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(ScaleOverSeconds(gameObject, new Vector3(_popUpSize, _popUpSize, 1), RandomStart));
    }

    //------------------------------------------------------------------

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

        _finishedSpawn = true;
        _myCollider.enabled = true;
        _myAudioSource.Play();
    }

    public virtual void ResetObject()
    {
        _finishedSpawn = false;
        _myCollider.enabled = false;
        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(ScaleOverSeconds(gameObject, new Vector3(_popUpSize, _popUpSize, 1), RandomStart));
    }
}
