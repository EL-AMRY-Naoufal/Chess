using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quenn : ChessMan
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        ChessMan c;
        int i, j;
        //Right
        i = CurrentX;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = boardmanager.Instance.ChessMans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //Left
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = boardmanager.Instance.ChessMans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;

                break;
            }
        }

        //up
        i = CurrentY;
        while (true)
        {
            i++;
            if (i >= 8)
                break;

            c = boardmanager.Instance.ChessMans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;

                break;
            }
        }

        //Down
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = boardmanager.Instance.ChessMans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;

                break;
            }
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
