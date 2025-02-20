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
        // Инициализация пула
        cubePool = new ObjectPool<GameObject>(
            () => Instantiate(cubePrefab), // Функция создания объекта
            cube => cube.SetActive(true),  // Функция активации объекта
            cube => cube.SetActive(false), // Функция деактивации объекта
            cube => Destroy(cube),         // Функция уничтожения объекта
            false,                         // Проверка на доступность объекта
            initialPoolSize,              // Начальный размер пула
            initialPoolSize * 2            // Максимальный размер пула
        );

        // Запуск спавна кубиков
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
        // Изменение цвета
        cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

        // Ожидание 2 секунды
        yield return new WaitForSeconds(2f);

        // Возвращение в пул
        cubePool.Release(cube);
    }
}
