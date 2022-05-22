using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public static RoadGenerator instance;
    public float speed = 0;

    private List<GameObject> _tiles = new List<GameObject>();
    private float _maxSpeed = 10f;
    private int _maxTileCount = 5;

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
            CreateTile();
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
        for (var i = 0; i < _maxTileCount; i++)
        {
            CreateTile();
        }

        MapGenerator.instance.ResetMaps();
    }

    private void CreateTile()
    {
        Vector3 pos = Vector3.zero;

        if (_tiles.Count > 0)
        {
            pos = _tiles.Last().transform.position + new Vector3(0, 0, 20);
        }

        GameObject newTile = Instantiate(tilePrefab, pos, Quaternion.identity);
        _tiles.Add(newTile);
    }
}
