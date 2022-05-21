using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int _itemSpace = 15;
    private int _itemCountInMap = 5;
    private int _maxMapCount = 4;
    private int _mapSpace = 30;

    public int laneOffset = 3;

    enum TrackPos { Left = -1, Mid = 0, Right = 1};

    public GameObject obstacleTopPrefab;
    public GameObject obstacleBottomPrefab;
    public GameObject obstacleFullPrefab;
    public GameObject obstacleRampPrefab;

    public static MapGenerator instance;

    public List<GameObject> maps = new List<GameObject>();

    struct MapItem
    {
        public void SetValue(GameObject obstacle, TrackPos trackPos)
        {
            this.obstacle = obstacle;
            this.trackPos = trackPos;
        }

        public GameObject obstacle;
        public TrackPos trackPos;
    }

    private void Start()
    {
        instance = this;
        for(int i = 0; i < _maxMapCount; i++)
        {
            int rnd = UnityEngine.Random.Range(1, 4);

            if (rnd == 1)
            {
                maps.Add(MakeMap1());
            }
            if (rnd == 2)
            {
                maps.Add(MakeMap2());
            }
            if (rnd == 3)
            {
                maps.Add(MakeMap3());
            }
        }
    }

    private void Update()
    {
        if(RoadGenerator.instance.speed != 0)
        {
            foreach (GameObject map in maps)
            {
                map.transform.Translate(0, 0, -RoadGenerator.instance.speed * Time.deltaTime);
            }

            if (maps[0].transform.position.z < -100)
            {
                Destroy(maps[0]);
                maps.RemoveAt(0);

                int rnd = UnityEngine.Random.Range(1, 4);

                if (rnd == 1)
                {
                    maps.Add(MakeMap1());
                }
                if (rnd == 2)
                {
                    maps.Add(MakeMap2());
                }
                if (rnd == 3)
                {
                    maps.Add(MakeMap3());
                }
            }
        }
    }

    private GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);

        Vector3 pos = Vector3.zero;

        if (maps.Count > 0)
        {
            pos = maps[maps.Count - 1].transform.position + new Vector3(0, 0, _mapSpace);
        }

        MapItem item = new MapItem();

        for(int i = 0; i < _itemCountInMap; i++)
        {
            item.SetValue(null, TrackPos.Mid);

            if(i == 2) { item.SetValue(obstacleRampPrefab, TrackPos.Left); }
            if(i == 3) { item.SetValue(obstacleBottomPrefab, TrackPos.Right); }
            if(i == 4) { item.SetValue(obstacleBottomPrefab, TrackPos.Right); }

            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0, i * _itemSpace);

            if(item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        result.transform.position = pos;
        return result;
    }

    private GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);

        Vector3 pos = Vector3.zero;

        if (maps.Count > 0)
        {
            pos = maps[maps.Count - 1].transform.position + new Vector3(0, 0, _mapSpace);
        }

        MapItem item = new MapItem();

        for (int i = 0; i < _itemCountInMap; i++)
        {
            item.SetValue(null, TrackPos.Mid);

            if (i == 2) { item.SetValue(obstacleRampPrefab, TrackPos.Mid); }
            if (i == 3) { item.SetValue(obstacleTopPrefab, TrackPos.Left); }
            if (i == 4) { item.SetValue(obstacleBottomPrefab, TrackPos.Left); }

            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0, i * _itemSpace);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        result.transform.position = pos;
        return result;
    }

    private GameObject MakeMap3()
    {
        GameObject result = new GameObject("Map2");
        result.transform.SetParent(transform);

        Vector3 pos = Vector3.zero;

        if (maps.Count > 0)
        {
            pos = maps[maps.Count - 1].transform.position + new Vector3(0, 0, _mapSpace);
        }

        MapItem item = new MapItem();

        for (int i = 0; i < _itemCountInMap; i++)
        {
            item.SetValue(null, TrackPos.Mid);

            if (i == 1) { item.SetValue(obstacleTopPrefab, TrackPos.Mid); }
            if (i == 3) { item.SetValue(obstacleBottomPrefab, TrackPos.Left); }

            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0, i * _itemSpace);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }

        result.transform.position = pos;
        return result;
    }
}
