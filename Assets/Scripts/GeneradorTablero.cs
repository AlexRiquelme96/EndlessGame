using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class NewBehaviourScript : MonoBehaviour
{
    static int width = 10;
    static int length = 10;

    static float tileOffsetX = 2;
    static float tileOffsetY = 2;

    [MenuItem("Custom/CreateBoard")]

    static void CreateBoard()
    {
        GameObject casillaPrefab = GameObject.FindGameObjectWithTag("TilePrefab");
        GameObject tiles = GameObject.FindGameObjectWithTag("Tiles");

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                GameObject go = Instantiate(casillaPrefab, tiles.transform);
                go.name = $"{i}, {j}";

                go.transform.position = new Vector3(i * tileOffsetX, 0.01f, j * tileOffsetY);

            }
        }

    }

    [MenuItem("Custom/DeleteBoard")]

    static void DeleteBoard()
    {
        GameObject[] casillasEliminar = GameObject.FindGameObjectsWithTag("TilePrefab");

        foreach(GameObject go in casillasEliminar)
        {
            if (go.transform.parent.name == "Tiles")
                DestroyImmediate(go);
        }
    }
}
