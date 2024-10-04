using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessMan
{
   public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        //Upleft
        KnightMove(CurrentX - 1, CurrentY + 2, ref r);

        //Upright
        KnightMove(CurrentX + 1, CurrentY + 2, ref r);

        //RightUp
        KnightMove(CurrentX + 2, CurrentY + 1, ref r);

        //RightDown
        KnightMove(CurrentX + 2, CurrentY - 1, ref r);

        //Downleft
        KnightMove(CurrentX - 1, CurrentY - 2, ref r);

        //Downright
        KnightMove(CurrentX + 1, CurrentY - 2, ref r);

        //LefttUp
        KnightMove(CurrentX - 2, CurrentY + 1, ref r);

        //LefttDown
        KnightMove(CurrentX - 2, CurrentY - 1, ref r);
        return r;
    }

    public void KnightMove(int x, int y, ref bool[,] r)
    {
        ChessMan c;
        if(x>=0 && x<8 && y>=0 && y<8)
        {
            c = boardmanager.Instance.ChessMans[x, y];
            if (c == null)
                r[x, y] = true;
            else if (isWhite != c.isWhite)
                r[x, y] = true;
        }
    }

}
