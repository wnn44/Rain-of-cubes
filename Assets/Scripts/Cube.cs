using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(ColorModifier))]
public class Cube : MonoBehaviour
{
    private bool _hasCollided = false;
    private Renderer _renderer;
    private Color _initialColor = Color.blue;
    private float _minTime = 2.0f;
    private float _maxTime = 5.0f;
    private Rigidbody _rigidbody;
    private ColorModifier _colorModifier;

    public event Action<Cube> EndedLife;
    public event Action<Cube> CollisionCube;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _colorModifier = GetComponent<ColorModifier>();
    }

    public void Init()
    {
        _hasCollided = false;
        _renderer.material.color = _initialColor;
        _rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _hasCollided = true;

            _colorModifier.RColor(this);

            float delay = UnityEngine.Random.Range(_minTime, _maxTime);
            StartCoroutine(Delay(delay));
        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);

        EndedLife?.Invoke(this);
    }
}

