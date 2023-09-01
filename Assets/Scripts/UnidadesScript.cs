using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    [SerializeField] float lerpDuration;

    [Header("UI")]
    [SerializeField] string unitName;

    private Vector3 lookForward= Vector3.zero;

    public void MoveTo(Stack<TileScript> path)
    {
        movedThisTurn = true;

        StartCoroutine(MoveUnitCoroutine(path));
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
    IEnumerator MoveUnitCoroutine(Stack<TileScript> path)
    {
        //Debug.Log("entra a la corrutina de movimiento");
        while (path.Count > 0)
        {
            //Saca el primer elemento del Stack
            TileScript tile = path.Peek();
            Vector3 startPosition = transform.position;
            Vector3 nextPosition = tile.transform.position - new Vector3(0, tile.transform.position.y - startPosition.y, 0);

            float timeElapsed = 0;
            while (timeElapsed < lerpDuration)
            {
                LookForward(nextPosition, startPosition);
                transform.position = Vector3.Lerp(startPosition, nextPosition, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            transform.position = nextPosition;
            path.Pop();
            yield return null;
        }
        GameManager.Instance.GMBusy(false);
        GameManager.Instance.CleanAllowedTiles();
    }
}
