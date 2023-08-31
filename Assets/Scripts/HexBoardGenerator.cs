using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexBoardGenerator : MonoBehaviour
{
    static int width = 10;
    static int length = 13;
    static float cellSize = 2f;
    static float VERTICAL_HEX_OFFSET = .75f;

    [MenuItem("Custom/CreateBoard")]


    static void CreateBoard()
    {
        GameObject tilePrefab = GameObject.FindGameObjectWithTag("TilePrefab");
        GameObject tiles = GameObject.FindGameObjectWithTag("Tiles");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                GameObject go = Instantiate(tilePrefab, tiles.transform);
                go.name = $"{i}, {j}";
                if (j % 2 == 1)
                {
                    go.transform.position = new Vector3((i * cellSize) + (cellSize / 2f), 0f, j * cellSize * VERTICAL_HEX_OFFSET);
                }
                else
                {
                    go.transform.position = new Vector3(i * cellSize, 0f, j * cellSize * VERTICAL_HEX_OFFSET);
                }

            }
        }
    }

    [MenuItem("Custom/DeleteBoard")]

    static void DeleteBoard()
    {
        GameObject[] casillasEliminar = GameObject.FindGameObjectsWithTag("TilePrefab");

        foreach (GameObject go in casillasEliminar)
        {
            if (go.transform.parent.name == "Tiles")
                DestroyImmediate(go);
        }
    }
}
