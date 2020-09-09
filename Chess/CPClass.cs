using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{

    public enum ChessPieceColor { None, White, Black };
    public class CPClass
    {
        public int x;
        public int y;

        public ChessPieceColor pieceColor;

        public string CPName = "NULL";

        public CPClass(int _x, int _y, ChessPieceColor _pieceColor)
        {
            x = _x;
            y = _y;
            pieceColor = _pieceColor;
        }
    }

    public class CPPawn : CPClass
    {
        public CPPawn(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Pawn";
        }
    }

    public class CPRook : CPClass
    {
        public CPRook(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Rook";
        }
    }

    public class CPBishop : CPClass
    {
        public CPBishop(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Bishop";
        }
    }

    public class CPKnight : CPClass
    {
        public CPKnight(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Knight";
        }
    }

    public class CPQueen : CPClass
    {
        public CPQueen(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Queen";
        }
    }

    public class CPKing : CPClass
    {
        public CPKing(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "King";
        }
    }
}
