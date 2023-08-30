using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Instance;

    [Header("General Parameters")]
    [SerializeField] float checkDistance = 2f;
    [SerializeField] LayerMask tileLayer;

    [Header("Movement")]
    [SerializeField] TileScript destinationTile;
    [SerializeField] TileScript activePosition;

    GameObject[] tiles;

    [SerializeField] List<TileScript> allowedTile = new List<TileScript>();
    Stack<TileScript> calculatedPath = new Stack<TileScript>();

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        tiles = GameObject.FindGameObjectsWithTag("TilePrefab");
    }

    private void Update()
    {
        //Si se Apreta la Tecla L
        if (Input.GetKeyUp(KeyCode.L)) 
        {
            //Activa la Funcion de Buscar Casillas Caminables
            FindWalkableTiles(null);
        }

        if (allowedTile.Count > 0)
        {
            PathViewTo(Input.mousePosition);
        }
    }

    // Funcion de Buscar Casillas Caminables
    public void FindWalkableTiles(UnidadesScript unidad)
    {
        Debug.Log("aqui entra");
        //Limpia las casillas disponibles
        CleanAllowedTiles();

        //Activa la Funcion de Encontrar Todas las Casillas
        FindAllTiles(unidad);

        //Obtener Posicion de la Unidad Seleccionada
        GetActivePosition(unidad);

        //Recorrer Casillas Adyacentes y Encontrar Caminos Posibles

        Queue<TileScript> queue = new Queue<TileScript>();

        queue.Enqueue(activePosition);
        activePosition.Visited = true;

        while (queue.Count > 0)
        {
            TileScript analizedTile = queue.Dequeue();
            allowedTile.Add(analizedTile);
            analizedTile.Selectable = true;

            //if (analizedTile.Distance < unidad.moveDistance)
            if (analizedTile.Distance < 5)
            {
                foreach (TileScript adjacentTile in analizedTile.Adjacent)
                {
                    if (!adjacentTile.Visited)
                    {
                        adjacentTile.Father = analizedTile;
                        adjacentTile.Visited = true;
                        adjacentTile.Distance = 1 + analizedTile.Distance;
                        queue.Enqueue(adjacentTile);
                    }
                }
            }
        }

    }

    private void GetActivePosition(UnidadesScript unidad)
    {
        activePosition = unidad.GetActiveTile();
        activePosition.Actual = true;
    }

    private void CleanAllowedTiles()
    {
        foreach (TileScript c in allowedTile)
        {
            c.InitializeTiles();
        }
        allowedTile.Clear();


        //activePosition = null;
    }

    //Funcion de Buscar todas las Casillas
    private void FindAllTiles(UnidadesScript unidad)
    {
        //Envia la informacion al TileScript de la Distancia de Checkeo
        foreach (GameObject tile in tiles)
        {
            TileScript c = tile.GetComponent<TileScript>();
            c.FindTilesArround(checkDistance);
        }
    }

    internal void PathViewTo(Vector3 mousePosition)
    {
        GetDestinationTile(mousePosition);
        BuildDestinationPath();
    }

    //Obtiene las coordenadas del Tile Seleccionado
    private void GetDestinationTile(Vector3 mousePosition)
    {
        ClearDestinationTile();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, tileLayer))
        {
            TileScript tile = hit.collider.GetComponent<TileScript>();
            if (tile != null && tile.Selectable)
            {
                destinationTile = tile;
                destinationTile.Destination = true; 
            }
        }
    }

    //Limpia la casilla de destino que dejo de estar seleccionada
    private void ClearDestinationTile()
    {
        if (destinationTile != null)
        {
            destinationTile.Destination = false;
            destinationTile = null;
        }
    }

    //Calcular la Ruta Optima
    internal void BuildDestinationPath()
    {
        if (destinationTile == null) return;

        ClearDestinationPath();

        TileScript nextTile = destinationTile;

        while (nextTile != null)
        {
            if (nextTile == destinationTile)
                nextTile.Destination = true;
            else
                nextTile.CalculatedPath = true;
            calculatedPath.Push(nextTile);
            nextTile = nextTile.Father;
        }
    }

    private void ClearDestinationPath()
    {
        foreach (TileScript tile in calculatedPath)
        {
            tile.CalculatedPath = false;
        }
        calculatedPath.Clear();
    }

    internal void MoveUnitInCalculatedPath(UnidadesScript unidad)
    {
        //Si no hay camino calculado, no pasa nada
        if (calculatedPath.Count == 0) return;

        unidad.MoveTo(calculatedPath);

    }
}
