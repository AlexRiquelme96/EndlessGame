using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Selection")]
    [SerializeField] UnidadesScript playerSelected;
    [SerializeField] bool playerIsSelected = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (TryFindPlayer(Input.mousePosition))
            {
                GameManager.Instance.FindWalkableTiles(playerSelected);
            }
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
                playerSelected = mayPlayer;
                playerIsSelected = true;
            }
        }
        return playerIsSelected;
    }
}
