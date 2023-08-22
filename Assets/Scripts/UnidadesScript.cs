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
    [SerializeField] bool moved = false ;

    [Header("UI")]
    [SerializeField] string unitName;



    public void MoveTo(Stack<TileScript> path)
    {
        moved = true;

        StartCoroutine(MoveUnitInTime());

        //IEnumerator MoveUnitInTime()
        //{
        //    while (path.Count > 0)
        //    {
        //        TileScript tile = path.Peek();
        //        Vector3 nextPosition = tile.transform.position;
//
        //        float moveDistance = Vector3.Distance(transform.position, nextPosition);
        //        if (moveDistance >= 0.05f)
        //        {
        //            
        //        }
//
        //    }
        //}
    }

    private string MoveUnitInTime()
    {
        throw new NotImplementedException();
    }
}
