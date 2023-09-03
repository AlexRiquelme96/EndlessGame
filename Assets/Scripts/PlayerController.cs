using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    InputAction clickAction;


    States actualState = States.Waiting;

    private void Awake()
    {
        inputController = new InputController();
    }




    // Start is called before the first frame update
    void Start()
    {
        actualState = States.Move;
    }

    private void OnEnable()
    {
        clickAction = inputController.UI.Click;
        inputController.UI.Enable();
    }

    private void OnDisable()
    {
        inputController.UI.Disable();
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
        if (clickAction.ReadValue<float>() ==1)
        {
            if (TryFindPlayer(Mouse.current.position.ReadValue()))
            {
                GameManager.Instance.FindWalkableTiles(selectedPlayer);
            }
            else
            {
                if (selectedPlayer != null && selectedPlayer.CanMove())
                {
                    readyToMove = TryGetDestination((Mouse.current.position.ReadValue()));
                }
            }
        }
        if (readyToMove)
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
