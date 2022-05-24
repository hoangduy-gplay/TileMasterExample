using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class Title : MonoBehaviour
{
    public int id;
    public Text tvId;
    public Button btn;
    public bool wasFind = false;
    public bool willbeDelete;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        tvId.text = id.ToString();
        btn.onClick.AddListener(HandleOnClick);
    }

    private void HandleOnClick()
    {

        SortTitle();
    }
    private void SortTitle()
    {
        var GameManager1 = GameManager.Intance;

        for (int i = GameManager1.lstitle.Count - 1; i >= 0; i--)
        {
            if (GameManager1.lstitle[i].id == this.id)
            {

                GameManager1.lstitle.Insert(i + 1, this);
                index = GameManager1.lstitle.IndexOf(this);

                //GameManager1.CountTileId(this.id).coutn += 1;
                //  GameManager1.CheckAndDeleteListTileBottom(this.id);
                wasFind = true;
                break;
            }
        }
        if (!wasFind)
        {
            //if (GameManager1.lstitle.Count < 7)
            //{
            GameManager1.lstitle.Add(this);
            index = GameManager1.lstitle.IndexOf(this);
            //GameManager1.CountTileId(this.id).coutn += 1;
            //    GameManager1.CheckAndDeleteListTileBottom(this.id);
            //}
            //else return;

        }
        GameManager1.MoveTileInListComplete(delegate { GameManager1.AddTile(this); });
        //MoveTile();

    }
    public void MoveTile()
    {
        var GameManager1 = GameManager.Intance;
        this.transform.DOMove(GameManager1.lsPoint[this.index].transform.position, 1).OnComplete(delegate
        {

            //GameManager1.CountTileId(this.id).coutn += 1;
            //GameManager1.CheckAndDeleteListTileBottom(this.id);
            //   GameManager1.MoveTileInListComplete(delegate { });
            //
        });
    }
}
