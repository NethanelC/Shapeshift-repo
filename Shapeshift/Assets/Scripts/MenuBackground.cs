using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _chasePrefab;
    private float _cooldown, _timer, _speed = 3,_screenHeight = 4.5f, _screenWidth = 8.5f;
    private Rigidbody2D _instantiatedPrefab;
    private void Start()
    {
        _instantiatedPrefab = Instantiate(_chasePrefab, new Vector3(100, 100, 100), Quaternion.identity);
    }
    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        if (_timer >= _cooldown)
        {
            int sideSpawn = Random.Range(0, 4);
            float x, y;
            Vector2 forceDirection;
            Quaternion rotation;
            //top to bot 
            if (sideSpawn == 0)
            {
                x = Random.Range(-_screenWidth, _screenWidth);
                y = _screenHeight + 1;
                forceDirection = Vector2.down;
                rotation = Quaternion.identity;
                _cooldown = 5;
            }
            //bot to top
            else if (sideSpawn == 1)
            {
                x = Random.Range(-_screenWidth, _screenWidth);
                y = -_screenHeight - 1;
                forceDirection = Vector2.up;
                rotation = Quaternion.Euler(0, 0, 180);
                _cooldown = 5;
            }
            //left to right
            else if (sideSpawn == 2)
            {
                x = -_screenWidth - 1;
                y = Random.Range(-_screenHeight, _screenHeight);
                forceDirection = Vector2.right;
                rotation = Quaternion.Euler(0, 0, 90);
                _cooldown = 7.5f;
            }
            //right to left
            else
            {
                x = _screenWidth + 1;
                y = Random.Range(-_screenHeight, _screenHeight);
                forceDirection = Vector2.left;
                rotation = Quaternion.Euler(0, 0, -90);
                _cooldown = 7.5f;
            }
            Vector2 randomLocation = new(x, y);
            _instantiatedPrefab.transform.SetPositionAndRotation(randomLocation, rotation);
            _instantiatedPrefab.velocity = Vector2.zero;
            _instantiatedPrefab.AddForce(forceDirection * _speed, ForceMode2D.Impulse);
            _timer = 0;
        }
    }
}
