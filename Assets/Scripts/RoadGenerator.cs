using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject tilePrefab;

    private List<GameObject> _tiles = new List<GameObject>();
    private float _maxSpeed = 10f;
    public float speed = 0;
    private int _maxTileCount = 5;

    public static RoadGenerator instance;

    private void Start()
    {
        instance = this;
        ResetLevel();
    }

    private void Update()
    {
        if (speed == 0) return;

        foreach (GameObject tile in _tiles)
        {
            tile.transform.Translate(0, 0, -speed * Time.deltaTime);
        }

        if (_tiles[0].transform.position.z < -20)
        {
            Destroy(_tiles[0]);
            _tiles.RemoveAt(0);
            CreateNextTile();
        }
    }
    public void StartLevel()
    {
        speed = _maxSpeed;
    }

    public void ResetLevel()
    {
        speed = 0;
        while (_tiles.Count > 0)
        {
            Destroy(_tiles[0]);
            _tiles.RemoveAt(0);
        }
        for (int i = 0; i < _maxTileCount; i++)
        {
            CreateNextTile();
        }
    }

    private void CreateNextTile()
    {
        Vector3 pos = Vector3.zero;
        if (_tiles.Count > 0)
        {
            pos = _tiles[_tiles.Count - 1].transform.position + new Vector3(0, 0, 20);
        }
        GameObject newTile = Instantiate(tilePrefab) as GameObject;
        newTile.transform.position = pos;
        _tiles.Add(newTile);
    }
}
