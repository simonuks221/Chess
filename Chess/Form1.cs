using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        public ChessBoardNode[,] chessBoardNodeArray;
        List<CPClass> chessPieces = new List<CPClass>();

        public int selectedX = -1; //Seected chess piece coords
        public int selectedY = -1;

        List<ChessBoardNode> availableMoveNodes = new List<ChessBoardNode>();
        List<ChessBoardNode> availableAttackNodes = new List<ChessBoardNode>();

        ChessPieceColor moveColor;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //Reset board on start
        {
            ResetBoard();
        }

        void ResetBoard()
        {
            if(chessBoardNodeArray != null) //Clear all buttons from previos play
            {
                List<Button> buttons = ChessBoard.Controls.OfType<Button>().ToList();
                foreach (Button b in buttons)
                {
                    ChessBoard.Controls.Remove(b);
                    b.Dispose();
                }
            }

            chessBoardNodeArray = new ChessBoardNode[8, 8];

            moveColor = ChessPieceColor.White;
            MoveLabel.Text = moveColor.ToString();

            for (int x = 0; x < 8; x++) //Spawn ned buttons and set them up
            {
                for (int y = 0; y < 8; y++)
                {
                    Button newChessButton = new Button();
                    ChessBoard.Controls.Add(newChessButton);
                    newChessButton.Location = new Point(x * 50, y * 50);
                    newChessButton.Width = 50;
                    newChessButton.Height = 50;

                    chessBoardNodeArray[x, y] = new ChessBoardNode(x, y, newChessButton);
                    newChessButton.Click += new EventHandler(onButtonPressed);
                }
            }


            //Set all starting pieces
            for (int x = 0; x < 8; x++) //Set pawns
            {

                SpawnChessPiece(x, 1, ChessPieceColor.White, typeof(CPPawn));
                SpawnChessPiece(x, 6, ChessPieceColor.Black, typeof(CPPawn));
            }

            
            SpawnChessPiece(0, 0, ChessPieceColor.White, typeof(CPRook));
            SpawnChessPiece(7, 0, ChessPieceColor.White, typeof(CPRook));
            SpawnChessPiece(0, 7, ChessPieceColor.Black, typeof(CPRook));
            SpawnChessPiece(7, 7, ChessPieceColor.Black, typeof(CPRook));

            SpawnChessPiece(1, 0, ChessPieceColor.White, typeof(CPKnight));
            SpawnChessPiece(6, 0, ChessPieceColor.White, typeof(CPKnight));
            SpawnChessPiece(1, 7, ChessPieceColor.Black, typeof(CPKnight));
            SpawnChessPiece(6, 7, ChessPieceColor.Black, typeof(CPKnight));

            SpawnChessPiece(2, 0, ChessPieceColor.White, typeof(CPBishop));
            SpawnChessPiece(5, 0, ChessPieceColor.White, typeof(CPBishop));
            SpawnChessPiece(2, 7, ChessPieceColor.Black, typeof(CPBishop));
            SpawnChessPiece(5, 7, ChessPieceColor.Black, typeof(CPBishop));

            SpawnChessPiece(3, 0, ChessPieceColor.White, typeof(CPQueen));
            SpawnChessPiece(3, 7, ChessPieceColor.Black, typeof(CPQueen));

            SpawnChessPiece(4, 0, ChessPieceColor.White, typeof(CPKing));
            SpawnChessPiece(4, 7, ChessPieceColor.Black, typeof(CPKing));
            
        }

        public void SpawnChessPiece(int x, int y, ChessPieceColor color, Type _CPClass )
        {
            Object newObject = Activator.CreateInstance(_CPClass, new Object[] {x, y, color });
            CPClass newClass = (CPClass)newObject;
            chessPieces.Add(newClass);
            chessBoardNodeArray[x, y].ChangePiece(newClass);
        }

        public void onButtonPressed(object sender, EventArgs e) //Chess board button pressed
        {
            Button buttonClicked = (Button)sender;

            int locX = (int)Math.Floor(buttonClicked.Location.X / 50f); //Get pressed button x and y coords
            int locY = (int)Math.Floor(buttonClicked.Location.Y / 50f);

            if(selectedX == -1 && selectedY == -1) //First selection
            {
                if (chessBoardNodeArray[locX, locY].chessPiece != null && chessBoardNodeArray[locX, locY].chessPiece.pieceColor == moveColor)
                {
                    availableMoveNodes.Clear();

                    selectedX = locX;
                    selectedY = locY;

                    chessBoardNodeArray[locX, locY].ChangeColor(ChessBoardNodeColor.Selected);
                    availableMoveNodes = ShowValidMoves(chessBoardNodeArray, out availableAttackNodes);
                }
            }
            else //Pressed after seleceting a piece, move a piece
            {
                if(availableMoveNodes.Contains(chessBoardNodeArray[locX, locY]) || availableAttackNodes.Contains(chessBoardNodeArray[locX, locY]))
                {
                    ChangePieceLocation(selectedX, selectedY, locX, locY);
                }

                SoftClearBoard();
            }
        }

        public void SoftClearBoard() //Clears all highlighted tiles nad other stuff.
        {
            if (selectedX != -1)
            {
                chessBoardNodeArray[selectedX, selectedY].ChangeColor(ChessBoardNodeColor.None);

                selectedX = -1;
                selectedY = -1;


                for(int x = 0; x < 8; x++)
                {
                    for(int y = 0; y < 8; y++)
                    {
                        chessBoardNodeArray[x, y].ChangeColor(ChessBoardNodeColor.None);
                    }
                }
                availableMoveNodes.Clear();
                availableAttackNodes.Clear();
            }
        }

        public List<ChessBoardNode> ShowValidMoves(ChessBoardNode[,] _arrayFrom, out List<ChessBoardNode> validOnlyAttackMoves) //Show valid selected piece moves
        {
            List<ChessBoardNode> validMoves = new List<ChessBoardNode>();
            List<ChessBoardNode> validAttackMoves = new List<ChessBoardNode>();

            CPClass chessPiece = chessBoardNodeArray[selectedX, selectedY].chessPiece;

            foreach(Point p in chessPiece.moveOffsets)
            {
                AddAvailableMoves(selectedX + p.X, selectedY + p.Y, validMoves, false); ;
            }
            /*
           
                case ChessPiece.Bishop:
                    for(int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY + i, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY - i, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY - i, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY + i, validMoves)) break;
                    }

                    break;
                case ChessPiece.Pawn:
                    if(_arrayFrom[selectedX, selectedY].chessPieceColor == ChessPieceColor.White)
                    { //White
                        if(selectedY == 1) //Can go 2 times
                        {
                            AddAvailableMoves(selectedX, selectedY + 1, validMoves, false);
                            AddAvailableMoves(selectedX, selectedY + 2, validMoves, false);
                        }
                        else
                        {
                            AddAvailableMoves(selectedX, selectedY + 1, validMoves, false);
                        }

                        AddAvailableOnlyAttacks(selectedX + 1, selectedY + 1, _arrayFrom, validAttackMoves);
                        AddAvailableOnlyAttacks(selectedX - 1, selectedY + 1, _arrayFrom, validAttackMoves);

                    }
                    else //Black
                    {
                        if(selectedY == 6) //Can go 2
                        {
                            AddAvailableMoves(selectedX, selectedY - 1, validMoves, false);
                            AddAvailableMoves(selectedX, selectedY - 2, validMoves, false);
                        }
                        else
                        {
                            AddAvailableMoves(selectedX, selectedY - 1, validMoves, false);
                        }

                        AddAvailableOnlyAttacks(selectedX + 1, selectedY - 1, _arrayFrom, validAttackMoves);
                        AddAvailableOnlyAttacks(selectedX - 1, selectedY - 1, _arrayFrom, validAttackMoves);
                    }


                    break;
                case ChessPiece.Queen:
                    for (int x = selectedX + 1; x < 8; x++)
                    {
                        if (!AddAvailableMoves(x, selectedY, validMoves)) break;
                    }
                    for (int x = selectedX - 1; x >= 0; x--)
                    {
                        if (!AddAvailableMoves(x, selectedY, validMoves)) break;
                    }
                    for (int y = selectedY + 1; y < 8; y++)
                    {
                        if (!AddAvailableMoves(selectedX, y, validMoves)) break;
                    }

                    for (int y = selectedY - 1; y >= 0; y--)
                    {
                        if (!AddAvailableMoves(selectedX, y, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY + i, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY - i, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY - i, validMoves)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY + i, validMoves)) break;
                    }
                    break;
                case ChessPiece.Rook:
                    for(int x = selectedX + 1; x < 8; x++)
                    {
                        if (!AddAvailableMoves(x, selectedY, validMoves)) break;
                    }
                    for(int x = selectedX - 1; x >= 0; x--)
                    {
                        if (!AddAvailableMoves(x, selectedY, validMoves)) break;
                    }

                    for (int y = selectedY + 1; y < 8; y++)
                    {
                        if (!AddAvailableMoves(selectedX, y, validMoves)) break;
                    }

                    for (int y = selectedY - 1; y >= 0; y--)
                    {
                        if (!AddAvailableMoves(selectedX, y, validMoves)) break;
                    }
                    break;
            
            */
            validOnlyAttackMoves = validAttackMoves;
            return validMoves;
        }

        public bool AddAvailableMoves(int x, int y, List<ChessBoardNode> arrayToAdd, bool canAttack = true) //Return if can move here
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
            {
                if (chessBoardNodeArray[x, y].chessPiece == null)
                {
                    arrayToAdd.Add(chessBoardNodeArray[x, y]);
                    chessBoardNodeArray[x, y].ChangeColor(ChessBoardNodeColor.Selected);
                    return true;
                }
                else if (chessBoardNodeArray[x, y].chessPiece.pieceColor != chessBoardNodeArray[selectedX, selectedY].chessPiece.pieceColor && canAttack)
                {
                    arrayToAdd.Add(chessBoardNodeArray[x, y]);
                    chessBoardNodeArray[x, y].ChangeColor(ChessBoardNodeColor.Kill);
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        bool AddAvailableOnlyAttacks(int x, int y, ChessBoardNode[,] _arrayFrom, List<ChessBoardNode> _arrayToAdd) //Return if can attack here
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
            {
                if (_arrayFrom[x, y].chessPiece.pieceColor == ChessPieceColor.None)
                {
                    return false;
                }
                else if (_arrayFrom[x, y].chessPiece.pieceColor != _arrayFrom[selectedX, selectedY].chessPiece.pieceColor)
                {
                    _arrayToAdd.Add(chessBoardNodeArray[x, y]);
                    _arrayFrom[x, y].ChangeColor(ChessBoardNodeColor.Kill);
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ChangePieceLocation(int fromX, int fromY, int toX, int toY) //Move piece
        {
            
            if((CPKing)chessBoardNodeArray[toX, toY].chessPiece != null) //Check win conditions
            {
                moveColor = ChessPieceColor.None;
                MoveLabel.Text = chessBoardNodeArray[fromX, fromY].chessPiece.pieceColor + " won";
            }
            else
            {
                chessBoardNodeArray[toX, toY].ChangePiece(chessBoardNodeArray[fromX, fromY].chessPiece);
                chessBoardNodeArray[fromX, fromY].ChangePiece(null);

                //Change current moving team color
                if (moveColor == ChessPieceColor.White)
                {
                    moveColor = ChessPieceColor.Black;
                    
                }
                else
                {
                    moveColor = ChessPieceColor.White;
                }
                MoveLabel.Text = moveColor.ToString();
            }
            
            SoftClearBoard();

            if (moveColor == ChessPieceColor.Black)
            {
                if (UseAiCheckBox.Checked)
                {
                    
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e) //Reset board on button press
        {
            ResetBoard();
        }
    }
}
