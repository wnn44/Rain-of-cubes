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

    public event Action<INotifier> ClubEndedLife;

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
            _renderer.material.color = platform.GenerateRandomColor();

            StartCoroutine(ReturnToPoolAfterDelay(UnityEngine.Random.Range(_minTime, _maxTime)));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ClubEndedLife?.Invoke(this);
    }
}

