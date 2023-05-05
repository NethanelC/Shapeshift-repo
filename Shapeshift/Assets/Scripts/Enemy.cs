using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _transformPlayer, _transformGun;
    [SerializeField] private Rigidbody2D _rbGun, _rbBullet;
    [SerializeField] private GameObject _healthBar, _projRotate;
    [SerializeField] private float _speedBullet, _timeBtwBullets;
    [SerializeField] private float _moveSpeed, _moveBackSpeed;
    [SerializeField] private float _minRadius, _maxRadius;
    private Vector2 _positionStart;
    private bool _shooting, _triggered;
    private void Start()
    {
        _positionStart = transform.position;
    }
    private void FixedUpdate()
    {
        _transformGun.position = transform.position;
        Vector2 lookDirection = _transformPlayer.position - _transformGun.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        _rbGun.rotation = lookAngle;
        if (_projRotate != null)
        {
            _projRotate.transform.RotateAround(gameObject.transform.position, Vector3.forward, 125 * Time.fixedDeltaTime);
        }
        if (Vector2.Distance(transform.position, _transformPlayer.position) < _minRadius)
        {
            _triggered = true;
        }
        else if (Vector2.Distance(transform.position, _transformPlayer.position) > _maxRadius)
        {
            _triggered = false;
        }
        if (_triggered && _transformPlayer.gameObject.activeSelf)
        { 
            transform.position = Vector2.MoveTowards(transform.position, _transformPlayer.position, _moveSpeed * Time.fixedDeltaTime);
            _healthBar.SetActive(true);
            StartCoroutine(Shoot());
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _positionStart, _moveBackSpeed * Time.fixedDeltaTime);
            _healthBar.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collObject = collision.gameObject;
        if (collObject.layer == 6)
        {
            _triggered = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collObject = collision.gameObject;
        if (collObject.layer == 9)
        {
            _triggered = false;
        }
    }
    private IEnumerator Shoot()
    {
        if (_shooting || !_transformPlayer.gameObject.activeSelf)
        {
            yield break;
        }
        _shooting = true;    
        Instantiate(_rbBullet, _transformGun.position, _transformGun.rotation).AddForce(_transformGun.up * _speedBullet, ForceMode2D.Impulse);
        yield return new WaitForSeconds(_timeBtwBullets);
        _shooting = false;
    }
}
