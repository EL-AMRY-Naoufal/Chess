using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sergent : ChessMan
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        int i, j;
        ChessMan c;
        //white team
        if(isWhite)
        {
            if (CurrentY == 1)
            {
                c = boardmanager.Instance.ChessMans[CurrentX, CurrentY + 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY + 2] = true;
                }
            }
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
        }
        else // black team
        {
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

            if (CurrentY == 6)
            {
                c = boardmanager.Instance.ChessMans[CurrentX, CurrentY - 1];
                if (c == null)
                {
                    r[CurrentX, CurrentY - 2] = true;
                }
            }

        }

        
        return r;
    }
}
