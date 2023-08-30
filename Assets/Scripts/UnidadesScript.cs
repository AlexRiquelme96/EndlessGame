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
    [SerializeField] bool movedThisTurn = false ;

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
            while (path.Count > 0)
            {
                //Saca el primer elemento del Stack
                TileScript tile = path.Peek();

                Vector3 nextPosition = tile.transform.position;

                float moveDistance = Vector3.Distance(transform.position, nextPosition);

                if (moveDistance >= 0.05f)
                {
                    LookForward(nextPosition);

                    //Que el personaje mire hacia delante
                    transform.forward = lookForward;
                    transform.position += speed * Time.deltaTime; 
                }
                else
                {
                    transform.position = nextPosition;
                    path.Pop();
                }

                yield return null;
            }
        }
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
    private void LookForward(Vector3 nextPosition)
    {
        lookForward = nextPosition - transform.position;
        lookForward.Normalize();
    }
}
