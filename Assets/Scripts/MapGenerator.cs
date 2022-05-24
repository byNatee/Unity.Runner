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

    [Serializable]
    public struct MapItem
    {
        [SerializeField] private GameObject _obstacle;
        [SerializeField] private TrackPos _trackPos;

        public GameObject Obstacle => _obstacle;
        public TrackPos TrackPos => _trackPos;

        public void SetValue(GameObject obstacle, TrackPos trackPos)
        {
            _obstacle = obstacle;
            _trackPos = trackPos;
        }
    }

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
        var config = _mapConfigs.FirstOrDefault(map => map.MapId == UnityEngine.Random.Range(0, _mapConfigs.Count));
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

        var mapItem = new MapItem();


        for (var i = 0; i < mapConfig.MapItemConfigs.Count; i++)
        {
            mapItem.SetValue(null, TrackPos.Mid);

            var mapItemConfig = mapConfig.MapItemConfigs.FirstOrDefault(item => item.ItemId == i);
            if (mapItemConfig != null)
            {
                mapItem.SetValue(mapItemConfig.MapItem.Obstacle, mapItemConfig.MapItem.TrackPos);

                var obstaclePos = new Vector3((int)mapItem.TrackPos * _laneOffset, 0, 0);

                var go = Instantiate(mapItem.Obstacle, obstaclePos, Quaternion.identity);
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
        [SerializeField] private MapItem _mapItem;

        public int ItemId => _itemId;
        public MapItem MapItem => _mapItem;
    }
}
