using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessMan
{
    public override bool[,] PossibleMove()
    {

        bool[,] r = new bool[8, 8];
         ChessMan c, c2;
        int[] e = boardmanager.Instance.EnPassantMove;
        //White team moves
        if (isWhite)
         {
             //diagonale left
             if (CurrentX !=0 && CurrentY !=7)
             {
                 if(e[0]==CurrentX-1 && e[1]==CurrentY+1)
                    r[CurrentX - 1, CurrentY + 1] = true;
                 c = boardmanager.Instance.ChessMans[CurrentX - 1, CurrentY + 1];
                 if (c!=null && !c.isWhite)
                 {
                     r[CurrentX - 1, CurrentY + 1] = true;
                 }
             }

             //diagonal right
             if (CurrentX != 7 && CurrentY != 7)
             {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                    r[CurrentX + 1, CurrentY + 1] = true;
                c = boardmanager.Instance.ChessMans[CurrentX + 1, CurrentY + 1];
                 if (c != null && !c.isWhite)
                 {
                     r[CurrentX + 1, CurrentY + 1] = true;
                 }
             }
             //middle
             if(CurrentY!=7)
             {
                 c = boardmanager.Instance.ChessMans[CurrentX , CurrentY + 1];
                 if (c==null)
                 {
                     r[CurrentX, CurrentY + 1] = true;
                 }
             }

             //middle in first move
             if(CurrentY==1)
             {
                 c = boardmanager.Instance.ChessMans[CurrentX, CurrentY + 1];
                 c2 = boardmanager.Instance.ChessMans[CurrentX, CurrentY + 2];
                 if(c==null && c2 == null)
                 {
                     r[CurrentX, CurrentY + 2] = true;
                 }
             }
         }
         else //black team
         {
             //diagonale left
             if (CurrentX != 0 && CurrentY != 0)
             {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                    r[CurrentX - 1, CurrentY - 1] = true;
                c = boardmanager.Instance.ChessMans[CurrentX - 1, CurrentY - 1];
                 if (c != null && c.isWhite)
                 {
                     r[CurrentX - 1, CurrentY - 1] = true;
                 }
             }

             //diagonal right
             if (CurrentX != 7 && CurrentY != 0)
             {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                    r[CurrentX + 1, CurrentY - 1] = true;
                c = boardmanager.Instance.ChessMans[CurrentX + 1, CurrentY - 1];
                 if (c != null && c.isWhite)
                 {
                     r[CurrentX + 1, CurrentY - 1] = true;
                 }
             }
             //middle
             if (CurrentY != 0)
             {
                 c = boardmanager.Instance.ChessMans[CurrentX, CurrentY - 1];
                 if (c == null)
                 {
                     r[CurrentX, CurrentY - 1] = true;
                 }
             }

             //middle in first move
             if (CurrentY == 6)
             {
                 c = boardmanager.Instance.ChessMans[CurrentX, CurrentY - 1];
                 c2 = boardmanager.Instance.ChessMans[CurrentX, CurrentY - 2];
                 if (c == null && c2 == null)
                 {
                     r[CurrentX, CurrentY - 2] = true;
                 }
             }
         }
        return r;
    }
}
