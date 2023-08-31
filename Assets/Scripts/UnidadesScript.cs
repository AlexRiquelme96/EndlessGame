using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnidadesScript : MonoBehaviour
{
    [Header("Atributes")]
    [SerializeField] int actualHP;
    [SerializeField] int maxHP;
    [SerializeField] int moveDistance;
    [SerializeField] int moveSpeed;


    [Header("Movement")]
    [SerializeField] bool movedThisTurn = false;
    [SerializeField] bool playerStuned = false;

    [Header("UI")]
    [SerializeField] string unitName;

    Vector3 lookForward = Vector3.zero;
    Vector3 speed = Vector3.zero;

    public void MoveTo(Stack<TileScript> path)
    {
        movedThisTurn = true;

        StartCoroutine(MoveUnitCoroutine());

        IEnumerator MoveUnitCoroutine()
        {
            //Debug.Log("entra a la corrutina de movimiento");
            while (path.Count > 0)
            {
                //Saca el primer elemento del Stack
                TileScript tile = path.Peek();

                Vector3 nextPosition = tile.transform.position - new Vector3 (0, tile.transform.position.y, 0);
                
                Vector3 playerPosition = transform.position - new Vector3 (0,transform.position.y,0);

                float moveDistance = Vector3.Distance(playerPosition, nextPosition);

                // Debug.Log(moveDistance);

                if (moveDistance >= 0.05f)
                {
                    LookForward(nextPosition, playerPosition);
                    HorizontalSpeedMovement();

                    //Que el personaje mire hacia delante
                    transform.forward = lookForward;
                    playerPosition += speed * Time.deltaTime; 
                }
                else
                {
                    playerPosition = nextPosition;
                    path.Pop();
                }

                yield return null;
            }
        }
    }

    private void HorizontalSpeedMovement()
    {
        Debug.Log("forward" + transform.forward);
        speed = transform.forward * moveSpeed;
        Debug.Log("velocidad " + speed);
    }

    internal TileScript GetActiveTile()
    {
        RaycastHit hit;
        TileScript activeTile = null;

        Vector3 InitialRayPosition = transform.position + new Vector3(0, 0.5f, 0);
        LayerMask tileMask = LayerMask.GetMask("Tiles");

        if (Physics.Raycast(InitialRayPosition, Vector3.down, out hit, 10, tileMask))
        {
            activeTile = hit.collider.GetComponent<TileScript>();
        }
        return activeTile;
    }

    // Calcular hacia donde mira el Personaje
    private void LookForward(Vector3 nextPosition, Vector3 playerPosition)
    {
        lookForward = nextPosition - playerPosition;
        lookForward.Normalize();
    }

    public bool CanMove()
    {
        return movedThisTurn || playerStuned ? false : true;
    }
}
