using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{

    public enum ChessPieceColor { None, White, Black };
    public abstract class CPClass
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

        public abstract void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves);

        protected bool AddMove(ChessBoardNode[,] board, int newX, int newY, List<ChessBoardNode> moveList)
        {
            if(newX < 8 && newX >= 0 && newY < 8 && newY >= 0)
            {
                if (board[newX, newY].chessPiece == null)
                {
                    moveList.Add(board[newX, newY]);
                    return true;
                }
                else
                {
                    if (board[newX, newY].chessPiece.pieceColor == pieceColor)
                    {
                        return false;
                    }
                    else
                    {
                        moveList.Add(board[newX, newY]);
                        return false;
                    }
                }
            }
            else
            {
                //Console.WriteLine(newX + " " + newY);
                return false;
            }
        }
    }

    public class CPPawn : CPClass
    {
        public CPPawn(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Pawn";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves)
        {
            moves = new List<ChessBoardNode>();

            if(pieceColor == ChessPieceColor.White)
            {
                if(AddMove(board, x, y + 1, moves))
                {
                    if (y == 1)
                    {
                        AddMove(board, x, y + 2, moves);
                    }
                }
            }
            else
            {
                if (AddMove(board, x, y - 1, moves))
                {
                    if (y == 6)
                    {
                        AddMove(board, x, y - 2, moves);
                    }
                }
            }
        }
    }

    public class CPRook : CPClass
    {
        public CPRook(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Rook";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves)
        {
            moves = new List<ChessBoardNode>();

            for (int i = 1; i < 8; i++) 
            { 
                if(!AddMove(board, x + i, y, moves))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x - i, y, moves))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x, y + i, moves))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x, y - i, moves))
                {
                    break;
                }
            }
        }
    }

    public class CPBishop : CPClass
    {
        public CPBishop(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Bishop";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves)
        {
            moves = new List<ChessBoardNode>();
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x - i, y - i, moves))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x + i, y + i, moves))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x + i, y - i, moves))
                {
                    break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x - i, y + i, moves))
                {
                    break;
                }
            }
        }
    }

    public class CPKnight : CPClass
    {
        public CPKnight(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Knight";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves)
        {
            moves = new List<ChessBoardNode>();
            moves.AddRange(new List<ChessBoardNode> { board[x + 1, y + 2], board[x - 1, y + 2], board[x + 1, y - 2], board[x - 1, y - 2], board[x + 2, y + 1], board[x + 2, y - 1], board[x - 2, y + 1], board[x - 2, y - 1]});
        }


    }

    public class CPQueen : CPClass
    {
        public CPQueen(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Queen";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves)
        {
            moves = new List<ChessBoardNode>();
        }
    }

    public class CPKing : CPClass
    {
        public CPKing(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "King";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<ChessBoardNode> moves)
        {
            moves = new List<ChessBoardNode>();
            for(int xx = -1; xx < 2; xx++)
            {
                for(int yy = -1; yy < 2; yy++)
                {
                    //if(xx != 0 & yy != 0)
                    //{
                        AddMove(board, x + xx, y + yy, moves);
                    //}
                }
            }
        }
    }
}
