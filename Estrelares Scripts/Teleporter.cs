using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private PlayerScript _player = null;
    private PlayerRocket _rocket = null;

    [BoxGroup("New World")]
    [SerializeField]
    private Transform _teleportWorld = null;
    [BoxGroup("New World")]
    [SerializeField]
    private Vector3 _teleportPosition = Vector3.zero;
    [BoxGroup("New World")]
    [SerializeField]
    private float _teleportRotation = 0f;
    [BoxGroup("New World")]
    [SerializeField]
    private float _speedMultiplier = 2f;
    [BoxGroup("New World")]
    [SerializeField]
    private float _inverterY = 0f;

    private void Start()
    {
        _player = FindAnyObjectByType<PlayerScript>();
        _rocket = FindAnyObjectByType<PlayerRocket>();
    }

    public void TeleportPlayer()
    {
        _player.ChangeWorld(_teleportPosition, _teleportRotation, _teleportWorld, _speedMultiplier, _inverterY);
    }
}
