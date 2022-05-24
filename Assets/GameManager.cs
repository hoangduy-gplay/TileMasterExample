using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Intance;
    public int[,] array = new int[5, 5];
    public List<CountIdTile> lsCoutnTile;
    public List<Title> lstitle;
    public List<GameObject> lsPoint;
    public List<Title> lsTilesComplete;
    public GameObject bottom;
    public Title go;
    public Transform canvas;
    public int space;
    public GameObject map;
    private int spaceCell = 100;
    Vector3 pos = new Vector3(350, 0, 0);

    private void Start()
    {
        Intance = this;
        SetUp();
    }
    private void SetUp()
    {
        #region Array
        /*
                array = new int[5, 5];
                for (int i = 0; i < array.GetLength(0); i ++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        array[0, j] = 1;
                        array[i, 0] = 1;
                        array[array.GetLength(0) - 1, j] = 1;
                        array[i, array.GetLength(1) - 1] = 1;

                    }
                }
        */
        #endregion

        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                var tile = Instantiate(go, new Vector3(i + 2.5f, j + 10, 0) * space, Quaternion.identity).GetComponent<Title>();
                tile.transform.SetParent(map.transform);
                tile.id = array[i, j];
                tile.gameObject.name = "Tile " + tile.id;


            }
        }

    }
    public CountIdTile CountTileId(int id)
    {
        foreach (CountIdTile item in lsCoutnTile)
        {
            if (id == item.id)
            {
                return item;
            }
        }
        return null;

    }

    public void CheckAndDeleteListTileBottom(int param)
    {
        var temp = -1;
        for (int i = lsTilesComplete.Count - 1; i >= 0; i--)
        {
            if (lsTilesComplete[i].id == param)
            {
                if (CountTileId(lsTilesComplete[i].id).coutn == 3)
                {
                    temp = lsTilesComplete[i].id;
                    //  lsTilesComplete[i].willbeDelete = true;
                    Destroy(lstitle[i].gameObject);
                    lstitle.Remove(lstitle[i]);
                }
            }
        }
        if (temp != -1)
        {
            CountTileId(temp).coutn = 0;
        }

    }

    public void MoveTileInListComplete(Action action)
    {
        bool temp = false;
        for (int i = 0; i < lstitle.Count; i++)
        {

            lstitle[i].transform.DOMove(lsPoint[i].transform.position, 1).OnComplete(delegate
            {
                if (!temp)
                {
                    action?.Invoke();
                    temp = true;
                }


            });

        }
    }
    public void CheckDeleteTest()
    {
        for (int i = 0; i < lstitle.Count; i++)
        {
            if (lstitle[i].willbeDelete == true)
            {
                Destroy(lstitle[i].gameObject);
                lstitle.Remove(lstitle[i]);
                //   MoveTile();
            }
        }
    }
    public void AddTile(Title title)
    {
        lsTilesComplete.Add(title);
        CountTileId(title.id).coutn += 1;
        if (CountTileId(title.id).coutn == 3)
        {
            for (int i = lsTilesComplete.Count - 1; i >= 0; i--)
            {
                if (lsTilesComplete[i].id == title.id)
                {
                    //Destroy(lsTilesComplete[i].gameObject);
                    lsTilesComplete[i].willbeDelete = true;
                    lsTilesComplete.Remove(lsTilesComplete[i]);

                }
            }
            for (int i = lstitle.Count - 1; i >= 0; i--)
            {
                if (lstitle[i].willbeDelete)
                {
                    Destroy(lstitle[i].gameObject);
                    lstitle.Remove(lstitle[i]);
                    CountTileId(title.id).coutn = 0;
                    MoveTileInListComplete(delegate { });
                }
            }
        }
    }

}


[Serializable]
public class CountIdTile
{
    public int id;
    public int coutn;

}
