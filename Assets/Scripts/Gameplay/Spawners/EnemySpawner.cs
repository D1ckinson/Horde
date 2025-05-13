using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Collider _gameArea;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private int _maxEnemyCount = 50;
    [SerializeField] private float _spawnDelay = 0.6f;
    [SerializeField] private Transform _player;

    private Pool<Enemy> _pool;
    private Transform _transform;
    private Coroutine _spawnProcess;

    private void OnValidate()
    {
        _gameArea.ThrowIfNull();
        _prefab.ThrowIfNull();
        _player.ThrowIfNull();
    }

    private void Awake()
    {
        _pool = new(() => Instantiate(_prefab));
        _transform = transform;
        Run();
    }

    private void OnDrawGizmos()
    {
        CustomGizmos.DrawCircle(transform.position, _radius);
    }

    public void Run()
    {
        if (_spawnProcess != null)
            throw new System.InvalidOperationException();

        _spawnProcess = StartCoroutine(SpawnProcess());
    }

    public void Stop()
    {
        if (_spawnProcess == null)
            throw new System.InvalidOperationException();

        StopCoroutine(_spawnProcess);
    }

    private IEnumerator SpawnProcess()
    {
        WaitForSeconds wait = new(_spawnDelay);

        while (true)
        {
            Spawn();

            yield return wait;
        }
    }

    private void Spawn()
    {
        if (_pool.ReleaseCount >= _maxEnemyCount)
            return;

        Enemy enemy = _pool.Get();
        enemy.SetPosition(GenerateRandomPoint());
        enemy.Follow(_player);
    }

    public Vector3 GenerateRandomPoint()
    {
        float randomAngle = Random.Range(Constants.Zero, Constants.FullCircleDegrees) * Mathf.Deg2Rad;

        Vector3 direction = new(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
        Vector3 point = _transform.position + direction * _radius;

        if (_gameArea.ClosestPoint(point) == point)
        {
            return point;
        }

        return _transform.position - direction * _radius;
    }
}
