using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    private SpriteRenderer _mySpriterenderer = null;
    
    void Start()
    {
        //float width = ScreenSize.GetScreenToWorldWidth;
        //transform.localScale = Vector3.one * width;

        _mySpriterenderer = GetComponent<SpriteRenderer>();
        if (_mySpriterenderer == null) return;

        transform.localScale = Vector3.one;

        float width = _mySpriterenderer.sprite.bounds.size.x;
        float height = _mySpriterenderer.sprite.bounds.size.y;

        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3((float)(worldScreenWidth / width), (float)(worldScreenHeight / height), 1);        
    }
}
