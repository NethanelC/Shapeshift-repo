using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoLoot : MonoBehaviour
{
    [SerializeField] private int _ammoGiven;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.AddAmmo(_ammoGiven);
        Destroy(gameObject);    
    }
}
