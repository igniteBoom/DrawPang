using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureItem : MonoBehaviour
{
    public Sprite[] _gestureSprite;
    private Image _myImage;
    
    // Start is called before the first frame update
    void Start()
    {
        _myImage = this.GetComponent<Image>(); 
        if (null == _myImage) Debug.Log("Sprite Component null"); 
        
        _myImage.sprite = _gestureSprite[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
