using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int _laneOffset = 3;
    [SerializeField] private List<MapConfig> _mapConfigs;

    private const int _maxMapCount = 10;
    private const int _mapSpace = 20;

    public enum TrackPos
    {
        Left = -1,
        Mid = 0,
        Right = 1
    }

    public static MapGenerator instance;

    private List<GameObject> _maps = new List<GameObject>();

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (RoadGenerator.instance.speed == 0) return;

        foreach (GameObject map in _maps)
        {
            map.transform.Translate(0, 0, -RoadGenerator.instance.speed * Time.deltaTime);
        }

        if (_maps[0].transform.position.z < -20)
        {
            Destroy(_maps[0]);
            _maps.RemoveAt(0);
            CreateRandomMap();
        }
    }

    public void ResetMaps()
    {
        while (_maps.Count > 0)
        {
            Destroy(_maps[0]);
            _maps.RemoveAt(0);
        }
        for (var i = 0; i < _maxMapCount; i++)
        {
            CreateRandomMap();
        }
    }

    private void CreateRandomMap()
    {
        var config = _mapConfigs[UnityEngine.Random.Range(0, _mapConfigs.Count)];
        if (config != null)
        {
            _maps.Add(MakeMap(config));
        }
    }
    private GameObject MakeMap(MapConfig mapConfig)
    {
        var result = new GameObject($"Map_{mapConfig.MapId}");
        result.transform.SetParent(transform);

        var position = Vector3.zero;

        if (_maps.Count > 0)
        {
            position = _maps.Last().transform.position + new Vector3(0, 0, _mapSpace);
        }

        for (var i = 0; i < mapConfig.MapItemConfigs.Count; i++)
        {

            var mapItemConfig = mapConfig.MapItemConfigs.FirstOrDefault(item => item.ItemId == i);
            if (mapItemConfig != null)
            {
                var obstaclePos = new Vector3((int)mapItemConfig.TrackPos * _laneOffset, 0, 0);

                var go = Instantiate(mapItemConfig.ItemPrefab, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        result.transform.position = position;
        return result;
    }

    [Serializable]
    public class MapConfig
    {
        [SerializeField] private int _mapId;
        [SerializeField] private List<MapItemConfig> _mapItemConfigs;

        public int MapId => _mapId;
        public List<MapItemConfig> MapItemConfigs => _mapItemConfigs;
    }

    [Serializable]
    public class MapItemConfig
    {
        [SerializeField] private int _itemId;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private TrackPos _trackPos;

        public int ItemId => _itemId;
        public GameObject ItemPrefab => _itemPrefab;
        public TrackPos TrackPos => _trackPos;
    }
}
