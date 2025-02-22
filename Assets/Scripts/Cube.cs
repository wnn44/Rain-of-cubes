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
    private string _tagObjCollision = "Platform";

    public event Action<INotifier> ReturnToPool;

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

    private Color GenerateRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollided == false && collision.gameObject.tag == _tagObjCollision)
        {
            _hasCollided = true;
            _renderer.material.color = GenerateRandomColor();

            StartCoroutine(ReturnToPoolAfterDelay(UnityEngine.Random.Range(_minTime, _maxTime)));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ReturnToPool?.Invoke(this);
    }
}

