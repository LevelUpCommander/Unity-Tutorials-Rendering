using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TransformationGrid类用于管理一个三维网格，其中包含多个变换点。
// 它通过一个三维数组来存储网格点，每个点是一个Transform对象。
public class TransformationGrid : MonoBehaviour
{
    // 预制件，用于创建网格点的模板。
    public Transform prefab;

    // 网格的分辨率，即每个维度上的网格点数量。
    public int gridResolution = 10;

    // 存储网格点的数组。
    private Transform[] grid;

    private List<Transformation> transformations;

    private Matrix4x4 transformation;

    // 在游戏对象被初始化时，Awake方法会被调用，用于初始化网格。
    private void Awake()
    {
        // 根据网格分辨率初始化网格数组。
        grid = new Transform[gridResolution * gridResolution * gridResolution];
        // 使用三重循环遍历所有网格点的位置，并创建它们。
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    // 调用CreateGridPoint方法创建网格点。
                    grid[i] = CreateGridPoint(x, y, z);
                }
            }
        }

        transformations = new List<Transformation>();
    }

    void Update()
    {
        UpdateTransformation();

        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    grid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }
    }

    void UpdateTransformation()
    {
        GetComponents<Transformation>(transformations);
        if (transformations.Count > 0)
        {
            transformation = transformations[0].Matrix;
            
            for (int i = 1; i < transformations.Count; i++)
            {
                transformation = transformations[i].Matrix * transformation;
            }
        }
    }

    Vector3 TransformPoint(int x, int y, int z)
    {
        Vector3 coordinates = GetCoordinates(x, y, z);
        // for (int i = 0; i < transformations.Count; i++)
        // {
        //     coordinates = transformations[i].Apply(coordinates);
        // }
        // return coordinates;
        return transformation.MultiplyPoint(coordinates);
    }

    // 创建一个点，实际上就是实例化预制件，确定其坐标并为其赋予独特的颜色。
    Transform CreateGridPoint(int x, int y, int z)
    {
        // 实例化预制件以创建新的网格点。
        Transform point = Instantiate<Transform>(prefab);
        // 设置网格点的名称，以便于识别其在网格中的位置。
        point.name = $"point_{x}_{y}_{z}";
        // 设置网格点的局部位置。
        point.localPosition = GetCoordinates(x, y, z);
        // 为网格点赋予独特的颜色，颜色根据其在网格中的位置决定。
        point.GetComponent<MeshRenderer>().material.color = new Color(
            (float)x / gridResolution,
            (float)y / gridResolution,
            (float)z / gridResolution
        );
        // 返回新创建的网格点。
        return point;
    }

    // 计算网格点的坐标，使其以原点为中心，因此变换（尤其是旋转和缩放）相对于网格立方体的中点。
    Vector3 GetCoordinates(int x, int y, int z)
    {
        // 返回网格点的坐标，通过减去一半的网格长度，使网格中心对齐。
        return new Vector3(
            x - (gridResolution - 0) * 0.5f,
            y - (gridResolution - 0) * 0.5f,
            z - (gridResolution - 0) * 0.5f
        );
    }
}