using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IResetable
{
    private PlayerRocket _rocket = null;
    [SerializeField]
    private bool _rotate;
    [SerializeField]
    private float _rotateSpeed = 0f;

    private void Start()
    {
        _rocket = FindAnyObjectByType<PlayerRocket>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _rotateSpeed) * Time.deltaTime);
    }

    public void Collect(bool MasterCollectable)
    {
        if (!MasterCollectable)
        {
            Settings.IncrementStars();
        }
        else if (MasterCollectable)
            _rocket.SetCollider(true);

        gameObject.SetActive(false);
    }

    public void ResetObject()
    {

    }
}
