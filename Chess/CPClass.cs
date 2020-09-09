using System;
using System.Collections.Generic;
using System.Drawing;
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

        public List<Point> moveOffsets = new List<Point>();

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

            if (pieceColor == ChessPieceColor.White)
            {
                moveOffsets.Add(new Point(0, 1));
            }
            else
            {
                moveOffsets.Add(new Point(0, -1));
            }
        }
    }

    public class CPRook : CPClass
    {
        public CPRook(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Rook";

            for (int xx = -8; xx <=8; xx++)
            {
                moveOffsets.Add(new Point(xx, 0));
            }
            for (int yy = -8; yy <= 8; yy++)
            {
                moveOffsets.Add(new Point(0, yy));
            }
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

            moveOffsets.AddRange(new List<Point>() { new Point(1, 2), new Point(1, -2), new Point(-1, 2), new Point(-1, -2), new Point(2, 1), new Point(2, -1), new Point(-2, 1), new Point(-2, -1) });
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

            for(int xx = -1; xx <= 1; xx++)
            {
                for(int yy = -1; yy <= 1; yy++)
                {
                     moveOffsets.Add(new Point(xx, yy));
                }
            }
        }
    }
}
