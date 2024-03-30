using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnTime : MonoBehaviour
{
    [SerializeField]
    private float _time = 1f;
    private float _timeStamp = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _timeStamp = _time + Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeStamp <= Time.time)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _timeStamp = _time + Time.time;
    }
}
