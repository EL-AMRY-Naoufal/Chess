using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bichop : ChessMan
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        ChessMan c;
        int i,j;
        //topLeft
        i = CurrentX;
        j = CurrentY;
        while(true)
        {
            i--;
            j++;
            if (i < 0 || j >= 8)
                break;

            c = boardmanager.Instance.ChessMans[i, j];
            if (c == null)
                r[i, j] = true;
            else 
            {
                if(c.isWhite!=isWhite)
                    r[i, j] = true;

                break;
            }
        }

        //topRight
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i >= 8 || j >= 8)
                break;

            c = boardmanager.Instance.ChessMans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;

                break;
            }
        }

        //Downleft
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;

            c = boardmanager.Instance.ChessMans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;

                break;
            }
        }

        //downRight
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i >=8 || j < 0)
                break;

            c = boardmanager.Instance.ChessMans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;

                break;
            }
        }


        return r;
    }
}
