using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : ChessMan
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        ChessMan c;
        int i, j;

        //top side
        i = CurrentX - 1;
        j = CurrentY + 1;
        if (CurrentY != 7)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8)
                {
                    c = boardmanager.Instance.ChessMans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }
                i++;
            }
        }

        //down side
        i = CurrentX - 1;
        j = CurrentY - 1;
        if (CurrentY != 0)
        {
            for (int k = 0; k < 3; k++)
            {
                if (i >= 0 && i < 8)
                {
                    c = boardmanager.Instance.ChessMans[i, j];
                    if (c == null)
                        r[i, j] = true;
                    else if (isWhite != c.isWhite)
                        r[i, j] = true;
                }
                i++;
            }
        }

        //midlle left
        if (CurrentX != 0)
        {
            c = boardmanager.Instance.ChessMans[CurrentX - 1, CurrentY];
            if (c == null)
                r[CurrentX - 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r[CurrentX - 1, CurrentY] = true;
        }

        //midlle right
        if (CurrentX != 7)
        {
            c = boardmanager.Instance.ChessMans[CurrentX + 1, CurrentY];
            if (c == null)
                r[CurrentX + 1, CurrentY] = true;
            else if (isWhite != c.isWhite)
                r[CurrentX + 1, CurrentY] = true;
        }

        //topLeft
        i = CurrentX;
        j = CurrentY;
        while (true)
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
                if (c.isWhite != isWhite)
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
            if (i >= 8 || j < 0)
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
