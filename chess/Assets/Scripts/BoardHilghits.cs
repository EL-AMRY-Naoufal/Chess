using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHilghits : MonoBehaviour
{
    public static BoardHilghits Instance { set; get; }

    public GameObject HighlitPrefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject GethighlitObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);
        if (go == null)
        {
            go = Instantiate(HighlitPrefab);
            highlights.Add(go);
        }
        return go;
    }

    public void HighlitAllowedMoves(bool[,] moves)
    {
        GameObject go;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    go = GethighlitObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }

    public void HideHighlits()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }

}