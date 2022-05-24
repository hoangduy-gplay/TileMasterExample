using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class TitleData
{
    public int id;
    public Title titleObject;
    public bool isSorting;//DDang bay xuong hay k?
}

public class Title : MonoBehaviour
{
    public int id;
    public Text tvId;
    public Button btn;
    public bool wasFind = false;
    public bool willbeDelete;
    public int index;
    private bool isClicked;
    public bool isCome;
    // Start is called before the first frame update
    void Start()
    {
        tvId.text = id.ToString();
        btn.onClick.AddListener(HandleOnClick);
        isClicked = false;
        isCome = false;
    }

    private void HandleOnClick()
    {
        SortTitle();
    }
    private void SortTitle()
    {
        if (isClicked)
            return;

        var GameManager1 = GameManager.Intance;
        if (GameManager1.lstitle == null)
            GameManager1.lstitle = new List<TitleData>();
        TitleData dt = new TitleData();
        dt.id = this.id;
        dt.titleObject = this;

        for (int i = GameManager1.lstitle.Count - 1; i >= 0; i--)
        {
            if (GameManager1.lstitle[i].id == this.id)
            {
                GameManager1.lstitle.Insert(i + 1, dt);
                index = i + 1;
                wasFind = true;
                break;
            }
        }
        if (!wasFind)
        {
            GameManager1.lstitle.Add(dt);
            index = GameManager1.lstitle.Count - 1;
        }
        GameManager1.MoveTileInListComplete(delegate
        {
            this.transform.DOMove(GameManager1.lsPoint[index].transform.position, 1).OnComplete(() =>
            {
                isCome = true;
                GameManager1.AddTile(this);
            });
        });

        isClicked = true;
    }
}
