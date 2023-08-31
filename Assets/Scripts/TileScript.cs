using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [Header("Cualities")]
    [SerializeField] bool walkable = true;
    [SerializeField] bool selectable = false;

    [Header("Functionalities")]
    [SerializeField] LayerMask layerFound;
    [SerializeField] bool visited = false;
    [SerializeField] bool actual = false;
    [SerializeField] bool destination = false;
    [SerializeField] bool calculatedPath = false;

    [Header("Tiles")]
    [SerializeField] List<TileScript> adjacent = new List<TileScript>();
    [SerializeField] int distance = 0;
    [SerializeField] TileScript father;

    Color previousColor;

    //Seccion de Setter and Getter

    public bool Visited { get { return visited; } set {  visited = value; } }
    public bool Selectable { get { return selectable; } set {  selectable = value; } }
    public bool Actual { get { return actual; } set { actual = value; } }
    public bool Destination { get { return destination; } set { destination = value; } }
    public bool CalculatedPath { get { return calculatedPath; } set { calculatedPath = value; } }
    public int Distance { get { return distance; } set { distance = value; } }
    public List<TileScript>  Adjacent { get { return adjacent; } set { adjacent = value; } }
    public TileScript Father { get { return father; } set {  father = value; } }


    //Cambiar color de Las Casillas
    private void Update()
    {
        if (actual)
        {
            Color x = Color.magenta;
            x.a = 0.25f;
            GetComponent<Renderer>().material.color = Color.Lerp(previousColor, x, 2);
            previousColor = x;
        }
        else if (destination)
        {
            Color x = Color.yellow;
            x.a = 0.25f;
            GetComponent<Renderer>().material.color = Color.Lerp(previousColor, x, 2);
            previousColor = x;
        }
        else if (selectable)
        {
            Color x = Color.green;
            x.a = 0.25f;
            GetComponent<Renderer>().material.color = Color.Lerp(previousColor, x, 2);
            previousColor = x;
        }
        else
        {
            Color x = Color.gray;
            x.a = 0.25f;
            GetComponent<Renderer>().material.color = Color.Lerp(previousColor, x, 2);
            previousColor = x;
        }
        if (calculatedPath)
        {
            Color x = Color.cyan;
            x.a = 0.25f;
            GetComponent<Renderer>().material.color = Color.Lerp(previousColor, x, 2);
            previousColor = x;
        }
    }


    //Funcion para Resetear Valores de Casillas
    public void InitializeTiles()
    {
        //Limpia la Lista, por si acaso
        adjacent.Clear();
        selectable = false;
        walkable = true;
        visited = false;
        distance = 0;
        actual = false;
        destination = false;
        calculatedPath = false;
        father = null;
    }

    //Funcion para encontrar las Casillas Adyacentes
    public void FindTilesArround(float checkDistance)
    {
        //Resetea los Valores de cada Casilla
        InitializeTiles();

        //Se crea un Vector 3 para cada direccion
        Vector3 topRight    = new Vector3(checkDistance/2, 0, checkDistance * .75f);
        Vector3 topLeft     = new Vector3(-checkDistance / 2, 0, checkDistance * .75f);
        Vector3 botRight    = new Vector3(checkDistance / 2, 0, -checkDistance * .75f);
        Vector3 botLeft     = new Vector3(-checkDistance / 2, 0, -checkDistance * .75f);
        Vector3 right       = new Vector3(checkDistance, 0, 0);
        Vector3 left        = new Vector3(- checkDistance, 0, 0);

        //Se llama la Funcion para Revisar la Disponibilidad de las Casillas Adyacentes
        CheckTileArround(topRight);
        CheckTileArround(topLeft);
        CheckTileArround(botRight);
        CheckTileArround(botLeft);
        CheckTileArround(right);
        CheckTileArround(left);
    }

    //Funcion para Revisar la Disponibilidad de las Casillas Adyacentes
    private void CheckTileArround(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 6, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider collider in colliders)
        {
            TileScript tile = collider.GetComponent<TileScript>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position - new Vector3(0, 1, 0), Vector3.up, out hit, 2, layerFound))
                {
                    adjacent.Add(tile);
                }
                else
                {
                    tile.walkable = false;
                    tile.selectable = false;

                    if (hit.collider.GetComponent<UnidadesScript>()  != null)
                    {
                        tile.walkable= true;
                        adjacent.Add(tile);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Arriba Derecha
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + new Vector3(1 , 0, 2 * .75f), new Vector3(0.25f, 6f, 0.25f));

        //Arriba Izquierda
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + new Vector3(-1 , 0, 2 * .75f), new Vector3(0.25f, 6f, 0.25f));

        //Abajo Derecha
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position + new Vector3(1, 0, - 2 * .75f), new Vector3(0.25f, 6f, 0.25f));

        //Abajo Izquierda
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position + new Vector3(-1, 0, -2 * .75f), new Vector3(0.25f, 6f, 0.25f));

        //Derecha
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(2, 0, 0), new Vector3(0.25f, 6f, 0.25f));

        //Izquierda
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(-2, 0, 0), new Vector3(0.25f, 6f, 0.25f));

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(transform.position - new Vector3(0, 1, 0), Vector3.up, out hit, 10, layerFound);

        if (hitSomething)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
            Gizmos.DrawRay(transform.position, Vector3.up * hit.distance);
        }
        else
        {

        }

    }
}
