using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletHit;
    [SerializeField] private int Damage;
    private float _lifeTime = 2;
    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == 7 || coll.gameObject.layer == 8) 
        {
            coll.gameObject.GetComponent<Health>().TakeDamage(Damage); 
        }
        Instantiate(_bulletHit, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
