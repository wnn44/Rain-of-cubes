using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] int initialPoolSize = 10;

    private ObjectPool<GameObject> cubePool;

    private void Start()
    {
        // ������������� ����
        cubePool = new ObjectPool<GameObject>(
            () => Instantiate(cubePrefab), // ������� �������� �������
            cube => cube.SetActive(true),  // ������� ��������� �������
            cube => cube.SetActive(false), // ������� ����������� �������
            cube => Destroy(cube),         // ������� ����������� �������
            false,                         // �������� �� ����������� �������
            initialPoolSize,              // ��������� ������ ����
            initialPoolSize * 2            // ������������ ������ ����
        );

        // ������ ������ �������
        InvokeRepeating(nameof(SpawnCube), 0f, 0.5f);
    }

    private void SpawnCube()
    {
        GameObject cube = cubePool.Get();
        cube.transform.position = new Vector3(Random.Range(-5f, 5f), 10f, 0f);
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            StartCoroutine(ReturnCubeToPool(collision.gameObject));
        }
    }

    private IEnumerator ReturnCubeToPool(GameObject cube)
    {
        // ��������� �����
        cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

        // �������� 2 �������
        yield return new WaitForSeconds(2f);

        // ����������� � ���
        cubePool.Release(cube);
    }
}
