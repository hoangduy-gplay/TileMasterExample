using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Test : SerializedMonoBehaviour
{
    public int[,] array = new int[5, 5];
    public GameObject testOg;

    // Start is called before the first frame update
    void Start()
    {
        HandleSpwanTile();
    }

    private void HandleSpwanTile()
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[i, j] == 1)
                {
                    var tile = Instantiate(testOg, new Vector3(i, j), Quaternion.identity);
                }

            }
        }
    }
}
