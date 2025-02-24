using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, INotifier
{
    private bool _hasCollided = false;
    private Renderer _renderer;
    private Color _initialColor = Color.blue;
    private float _minTime = 2.0f;
    private float _maxTime = 5.0f;

    public event Action<INotifier> CubeEndedLife;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Init()
    {
        _hasCollided = false;
        _renderer.material.color = _initialColor;
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            _hasCollided = true;
            _renderer.material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            float delay = UnityEngine.Random.Range(_minTime, _maxTime);
            StartCoroutine(Delay(delay));            
        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);

        CubeEndedLife?.Invoke(this);
    }
}

