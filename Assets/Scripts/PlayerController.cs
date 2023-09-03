using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum States
    {
        Move,
        Atack,
        Waiting
    }

    [Header("Selection")]
    [SerializeField] UnidadesScript selectedPlayer;
    [SerializeField] bool playerIsSelected = false;
    [SerializeField] bool readyToMove = false;

    InputController inputController;


    States actualState = States.Waiting;




    // Start is called before the first frame update
    void Start()
    {
        actualState = States.Move;
    }

    // Update is called once per frame
    void Update()
    {
        switch (actualState)
        {
            case States.Move:
                PlayerMoves();
                break;
        }

    }

    //Funcion de control de Movimiento
    private void PlayerMoves()
    {
        if (GameManager.Instance.Busy())
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if (TryFindPlayer(Input.mousePosition))
            {
                GameManager.Instance.FindWalkableTiles(selectedPlayer);
            }
            else
            {
                if (selectedPlayer != null && selectedPlayer.CanMove())
                {
                    readyToMove = TryGetDestination(Input.mousePosition);
                }
            }
        }
        if (readyToMove && Input.GetMouseButton(0))
        {
            TryMoveToDestination();
            readyToMove = false;
        }
    }

    // Funcion para intentar seleccionar un personaje
    private bool TryFindPlayer(Vector3 selectionPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(selectionPosition);

        LayerMask selectionMaskPlayer = LayerMask.GetMask("Unidad");
       
        playerIsSelected = false;
        //Condicional para revisar que es lo que se clickeo
        if (Physics.Raycast(ray, out hit, selectionMaskPlayer))
        {
            UnidadesScript mayPlayer = hit.collider.GetComponent<UnidadesScript>();
            if (mayPlayer != null)
            {
                selectedPlayer = mayPlayer;
                playerIsSelected = true;
            }
        }
        return playerIsSelected;
    }

    private bool TryGetDestination(Vector3 selectionPosition)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(selectionPosition);

        LayerMask selectionMaskTile = LayerMask.GetMask("Tiles");

        bool destinationSelected = false;
        TileScript destinationTile = null;

        //Condicional para revisar que es lo que se clickeo
        if (Physics.Raycast(ray, out hit, selectionMaskTile))
        {
            destinationTile = hit.collider.GetComponent<TileScript>();
            
        }
        if (destinationTile != null && destinationTile.Selectable 
            && selectedPlayer.GetActiveTile() != destinationTile)
        {
            destinationSelected = true;
        }
        return destinationSelected;
    }

    private void TryMoveToDestination()
    {
        GameManager.Instance.MoveUnitInCalculatedPath(selectedPlayer);
    }
}
