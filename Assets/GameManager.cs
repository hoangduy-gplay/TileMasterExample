using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager Intance;
    public int[,] array = new int[5, 5];
    public List<CountIdTile> lsCoutnTile;
    public List<TitleData> lstitle;
    public List<GameObject> lsPoint;
    public List<Title> lsTilesComplete;
    public GameObject bottom;
    public Title go;
    public Transform canvas;
    public int space;
    public GameObject map;
    private int spaceCell = 100;
    Vector3 pos = new Vector3(350, 0, 0);

    Coroutine test = null;
    private bool listIsMoving;
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
    public bool HasSame3(int id)
    {
        int numSame = 0;
        foreach (var item in lstitle)
        {
            if (item.titleObject.isCome && id == item.id)
            {
                numSame++;
            }
        }
        if (numSame >= 3)
            return true;

        return false;

    }

    public void MoveTileInListComplete(Action action)
    {
        listIsMoving = true;
        int countLstitle = lstitle.Count;

        for (int i = 0; i < countLstitle; i++)
        {
            lstitle[i].isSorting = false;
        }

        for (int i = 0; i < countLstitle; i++)
        {
            int index = i;

            //if (index == indexMove)
            //    continue;
            if (lstitle[index].titleObject.isCome)
            {
                //Sap xep cac title o duoi
                if (Vector2.Distance(lstitle[index].titleObject.transform.position,
                      lsPoint[index].transform.position) > 0.1f)
                {
                    lstitle[index].isSorting = true;
                    lstitle[index].titleObject.transform.DOKill();
                    lstitle[index].titleObject.transform.DOMove(lsPoint[index].transform.position, 0.25f).OnComplete(delegate
                    {
                        lstitle[index].isSorting = false;
                    });
                }

            }
        }

        if (m_WaitForUtil != null)
            StopCoroutine(m_WaitForUtil);

        m_WaitForUtil = WaitForUtil(() =>
        {
            action?.Invoke();
        }, () => IsAllTitleSortDone() == true);
        StartCoroutine(m_WaitForUtil);
    }

    /// <summary>
    /// Cac title ow duoi da sorting xong het chua
    /// </summary>
    /// <returns></returns>
    private bool IsAllTitleSortDone()
    {
        for (int i = 0; i < lstitle.Count; i++)
        {
            int index = i;
            if (lstitle[index].isSorting)
                return false;
        }

        return true;
    }
    private IEnumerator m_WaitForUtil;
    private IEnumerator WaitForUtil(UnityAction action, System.Func<bool> condition)
    {
        yield return new WaitUntil(condition);
        action?.Invoke();
    }

    public void AddTile(Title title)
    {
        lsTilesComplete.Add(title);
        if (HasSame3(title.id))
        {
            List<Title> deleteLst = new List<Title>();
            for (int i = lsTilesComplete.Count - 1; i >= 0; i--)
            {
                int index = i;
                if (lsTilesComplete[index].id == title.id)
                {
                    lsTilesComplete[index].willbeDelete = true;
                    deleteLst.Add(lsTilesComplete[index]);
                }
            }
            lsTilesComplete.RemoveAll(x => x.id == title.id);
            for (int i = 0; i < deleteLst.Count; i++)
                Destroy(deleteLst[i].gameObject);


            lstitle.RemoveAll(x => x.titleObject.willbeDelete == true);
            MoveTileInListComplete(delegate { });
        }
    }


}


[Serializable]
public class CountIdTile
{
    public int id;
    public int coutn;

}
