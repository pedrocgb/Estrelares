using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private PlayerRocket _myRocket = null;
    private void Start()
    {
        _myRocket = FindAnyObjectByType<PlayerRocket>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            GameManager.ResetGame();
        }

        if (collision.CompareTag("Rocket"))
        {
            _myRocket.StartRocket();
        }

        if (collision.CompareTag("Teleporter"))
        {
            Teleporter tel = collision.GetComponent<Teleporter>();
            tel.TeleportPlayer();
        }

        if (collision.CompareTag("Star"))
        {
            Collectable c = collision.GetComponent<Collectable>();
            c.Collect(false);
        }

        if (collision.CompareTag("Master Collectable"))
        {
            Collectable c = collision.GetComponent<Collectable>();
            c.Collect(true);
        }
    }
}
