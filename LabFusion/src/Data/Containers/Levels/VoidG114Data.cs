﻿using UnityEngine;

using LabFusion.UI;
using LabFusion.SDK.Points;

namespace LabFusion.Data;

public class VoidG114Data : LevelDataHandler
{
    public override string LevelTitle => "15 - Void G114";

    // Point shop setup
    public static readonly Vector3 PointShopPosition = new(25.4318f, -1.1765f, 1.8907f);
    public static readonly Quaternion PointShopRotation = Quaternion.Euler(0f, 90f, 0f);

    public static readonly Vector3 SupportCubePosition = new(25.5273f, -1.6745f, 2.1946f);
    public static readonly Quaternion SupportCubeRotation = Quaternion.Euler(0f, 0f, 0f);
    public static readonly Vector3 SupportCubeScale = new(2.04f, 1f, 2.74f);

    // Info box setup
    public static readonly Vector3 InfoBoxPosition = new(29.0255f, -2.01f, 11.9655f);
    public static readonly Quaternion InfoBoxRotation = Quaternion.Euler(0f, 180f, 0f);

    // Cup board setup
    public static readonly Vector3 CupBoardPosition = new(26.5255f, -2.01f, 11.9655f);
    public static readonly Quaternion CupBoardRotation = Quaternion.Euler(0f, 180f, 0f);

    protected override void MainSceneInitialized()
    {
        CreateSupportCube();

        // Point shop
        PointShopHelper.SpawnBitMart(PointShopPosition, PointShopRotation);

        // Info box
        InfoBoxHelper.SpawnInfoBoard(InfoBoxPosition, InfoBoxRotation);

        // Cup board
        CupBoardHelper.SpawnAchievementBoard(CupBoardPosition, CupBoardRotation);
    }

    private static void CreateSupportCube()
    {
        GameObject supportCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        supportCube.name = "BitMart Support Cube";
        supportCube.transform.position = SupportCubePosition;
        supportCube.transform.rotation = SupportCubeRotation;
        supportCube.transform.localScale = SupportCubeScale;

        var meshRenderer = supportCube.GetComponentInChildren<MeshRenderer>();
        var planeRenderer = GameObject.Find("plane_1x6").GetComponentInChildren<MeshRenderer>();

        meshRenderer.material = planeRenderer.material;
    }
}