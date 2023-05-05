using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _gateTransformUp , _gateTransformDown;
    private int _currCollisions;
    private float StartYup, EndYup;
    private float StartYdown, EndYdown;
    private void Start()
    {
        StartYup = _gateTransformUp.position.y;
        EndYup = _gateTransformUp.position.y + 1; //magic number
        StartYdown = _gateTransformDown.position.y;
        EndYdown = _gateTransformDown.position.y -1; //magic number
        var particles = _particleSystem.main;
        particles.startColor = Color.red;
    }
    private void FixedUpdate()
    {
        if (_currCollisions > 0 && _gateTransformUp.position.y < EndYup)
        {
            _gateTransformUp.Translate(1.5f * Time.fixedDeltaTime * Vector2.up);
            _gateTransformDown.Translate(1.5f * Time.fixedDeltaTime * Vector2.down);
        }
        else if (_currCollisions < 1 && _gateTransformUp.position.y > StartYup)
        {
            _gateTransformUp.Translate(1.5f * Time.fixedDeltaTime * Vector2.down);
            _gateTransformDown.Translate(1.5f * Time.fixedDeltaTime * Vector2.up);
        }
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
            _currCollisions++;
            var particles = _particleSystem.main;
            particles.startColor = Color.green;
            _spriteRenderer.color = Color.green;
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        _currCollisions--;
        if(_currCollisions < 1)
        {
            var particles = _particleSystem.main;
            particles.startColor = Color.red;
            _spriteRenderer.color = Color.red; 
        }
    }
}
