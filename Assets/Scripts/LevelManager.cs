using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _stonePref;
    [SerializeField] private GameObject _farmerPref;
    [SerializeField] private GameObject _dogPref;
    [SerializeField] private GameObject _player;

    private Transform _stones;
    private Transform _enemies;
    private readonly Vector3 _playerStartPos = new Vector3(-4.2f, 0f, -2f);

    public Transform _items;

    private void Awake()
    {
        _stones  = new GameObject("Stones").transform;
        _enemies = new GameObject("Enemies").transform;
        _items   = new GameObject("Items").transform;
    }

    public void LevelInit(int nFarmers = 2, int nDogs = 1)
    {
        CreateStones();
        CreateEnemies(nFarmers, nDogs);
    }

    public void LevelClear()
    {
        _player.transform.position = _playerStartPos;

        for (int i = 0; i < _stones.childCount; i++)
        {
            Destroy(_stones.GetChild(i).gameObject);
        }

        for (int i = 0; i < _enemies.childCount; i++)
        {
            Destroy(_enemies.GetChild(i).gameObject);
        }

        for (int i = 0; i < _items.childCount; i++)
        {
            Destroy(_items.GetChild(i).gameObject);
        }
    }

    private void CreateStones()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                float x = -7 + 2 * j - 0.2f * i;
                float y = 2.7f - 1.8f * i;
                float z = (-3.6f + y) / 7.2f;
                Instantiate(_stonePref, new Vector3(x, y, z), Quaternion.identity, _stones);
            }
        }
    }

    public void CreateEnemies(int nFarmers, int nDogs)
    {
        // Farmer spawn points
        // x: -0.1f ... 7.9f (step: 2f + 0.2f * rY)
        // y: -2.9f ... 3.9f (step: 1.8f)

        // Dog spawn points
        // x:  0.2f ... 8.2f (step: 2f + 0.2f * rY)
        // y: -3.8f ... 3.4f (step: 1.8f)

        var rand = new System.Random();
        int rX, rY;
        float x, y, z;

        for (int i = 0; i < nFarmers; i++)
        {
            rX = rand.Next(4);
            rY = rand.Next(5);

            x = 0.3f + rX * 2f - 0.2f * rY;
            y = 3.9f - rY * 1.8f;
            z = (-3.6f + y) / 7.2f;

            Instantiate(_farmerPref, new Vector3(x, y, z), Quaternion.identity, _enemies);
        }

        rX = rand.Next(4);
        rY = rand.Next(5);

        x = 0.2f + rX * 2f - 0.2f * rY;
        y = 3.4f - rY * 1.8f;
        z = (-3.6f + y) / 7.2f;

        Instantiate(_dogPref, new Vector3(x, y, z), Quaternion.identity, _enemies);
    }
}
