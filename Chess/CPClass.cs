using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public abstract void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves);

        protected bool AddMove(ChessBoardNode[,] board, int newX, int newY, List<AvailableMove> moveList, MoveType moveType = MoveType.MoveAndAttack)
        {
            
            if (newX < 8 && newX >= 0 && newY < 8 && newY >= 0)
            {
                AvailableMove newMove = new AvailableMove(board[newX, newY], moveType);
                if (board[newX, newY].chessPiece == null)
                {
                    if (moveType == MoveType.OnlyAttack) return false;
                    else
                    {
                        moveList.Add(newMove);
                        return true;
                    }
                }
                else
                {
                    if (board[newX, newY].chessPiece.pieceColor == pieceColor)
                    {
                        return false;
                    }
                    else if(moveType != MoveType.OnlyMove)
                    {
                        moveList.Add(newMove);
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }

    public class CPPawn : CPClass
    {

        public bool movedDouble = false;
        public CPPawn(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Pawn";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves)
        {
            moves = new List<AvailableMove>();


            

            if(pieceColor == ChessPieceColor.White)
            {
                //Attack forward white
                AddMove(board, x + 1, y + 1, moves, MoveType.OnlyAttack);
                AddMove(board, x - 1, y + 1, moves, MoveType.OnlyAttack);

                //Movement forward white
                if (AddMove(board, x, y + 1, moves, MoveType.OnlyMove))
                {
                    if (y == 1)
                    {
                        AddMove(board, x, y + 2, moves, MoveType.OnlyMove);
                    }
                }

                //En passant white
                /*
                if (board[x + 1, y].chessPiece != null)
                {
                    if (board[x + 1, y].chessPiece.GetType() == typeof(CPPawn))
                    {
                        CPPawn p = (CPPawn)board[x + 1, y].chessPiece;
                        if (y == 4 && p.movedDouble)
                        {
                            AddMove(board, x + 1, y + 1, moves);
                        }
                    }
                }
                if (board[x - 1, y].chessPiece != null)
                {
                    if (board[x - 1, y].chessPiece.GetType() == typeof(CPPawn))
                    {
                        CPPawn p = (CPPawn)board[x - 1, y].chessPiece;
                        if(y == 4 && p.movedDouble)
                        {
                            AddMove(board, x - 1, y + 1, moves);
                        }
                    }
                }
                */

            }
            else
            {
                //Attack forward black
                AddMove(board, x + 1, y - 1, moves, MoveType.OnlyAttack);
                AddMove(board, x - 1, y - 1, moves, MoveType.OnlyAttack);

                //Movement forward black
                if (AddMove(board, x, y - 1, moves, MoveType.OnlyMove))
                {
                    if (y == 6)
                    {
                        AddMove(board, x, y - 2, moves, MoveType.OnlyMove);
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

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves)
        {
            moves = new List<AvailableMove>();

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

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves)
        {
            moves = new List<AvailableMove>();
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

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves)
        {
            moves = new List<AvailableMove>();
            AddMove(board, x + 2, y + 1, moves);
            AddMove(board, x + 2, y - 1, moves);

            AddMove(board, x - 2, y + 1, moves);
            AddMove(board, x - 2, y + 1, moves);

            AddMove(board, x - 1, y - 2, moves);
            AddMove(board, x + 1, y - 2, moves);

            AddMove(board, x - 1, y + 2, moves);
            AddMove(board, x + 1, y + 2, moves);
        }


    }

    public class CPQueen : CPClass
    {
        public CPQueen(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "Queen";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves)
        {
            moves = new List<AvailableMove>();
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
            for (int i = 1; i < 8; i++)
            {
                if (!AddMove(board, x + i, y, moves))
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

    public class CPKing : CPClass
    {
        public CPKing(int _x, int _y, ChessPieceColor _pieceColor) : base(_x, _y, _pieceColor)
        {
            CPName = "King";
        }

        public override void GetAvailableMoves(ChessBoardNode[,] board, out List<AvailableMove> moves)
        {
            moves = new List<AvailableMove>();
            for(int xx = -1; xx < 2; xx++)
            {
                for(int yy = -1; yy < 2; yy++)
                {
                     AddMove(board, x + xx, y + yy, moves);
                }
            }
        }
    }
}
