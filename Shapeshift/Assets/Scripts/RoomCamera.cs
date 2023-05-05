using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    [SerializeField] private GameObject _cameraObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _cameraObject.SetActive(true); 
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _cameraObject.SetActive(false);
    }
}
