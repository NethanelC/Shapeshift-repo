using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerLoot : MonoBehaviour
{
    [SerializeField] private int _timeGiven;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.AddTime(_timeGiven);
        Destroy(gameObject);
    }
}
