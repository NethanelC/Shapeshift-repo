using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletHit;
    [SerializeField] private float _lifeTime = 2;
    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Instantiate(_bulletHit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
