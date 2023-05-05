using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shapedoor : MonoBehaviour
{
    [SerializeField] private GameObject _doorUI;
    [SerializeField] private Collider2D _doorCollider;
    [SerializeField] private Image _panelColor, _shapeImage;
    [SerializeField] private GameObject _particles;
    private static Collider2D _doorColliderS;
    private static Image _panelColorS, _shapeImageS;
    private static GameObject _particlesS;
    private void Start()
    {
        _doorColliderS = _doorCollider;
        _panelColorS = _panelColor;
        _shapeImageS = _shapeImage;
        _particlesS = _particles;
    }
    public static void ShiftSprite(Sprite sprite)
    {
        if (sprite == _shapeImageS.sprite)
        {
            _panelColorS.color = Color.green;
            _doorColliderS.enabled = false;
            _particlesS.SetActive(false);
        }
        else
        {
            _panelColorS.color = Color.red;
            _doorColliderS.enabled = true;
            _particlesS.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _doorUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _doorUI.SetActive(false);
        }
    }
}
