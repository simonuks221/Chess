using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public int selectedX = -1; //Seected chess piece coords
        public int selectedY = -1;

        List<ChessBoardNode> availableMoveNodes = new List<ChessBoardNode>();
        List<ChessBoardNode> availableAttackNodes = new List<ChessBoardNode>();

        ChessPieceColor moveColor;

        ChessAi chessAi;

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
                chessBoardNodeArray[x, 1].ChangePiece(ChessPiece.Pawn, ChessPieceColor.White);
                chessBoardNodeArray[x, 6].ChangePiece(ChessPiece.Pawn, ChessPieceColor.Black);
            }

            chessBoardNodeArray[0, 0].ChangePiece(ChessPiece.Rook, ChessPieceColor.White);
            chessBoardNodeArray[7, 0].ChangePiece(ChessPiece.Rook, ChessPieceColor.White);
            chessBoardNodeArray[0, 7].ChangePiece(ChessPiece.Rook, ChessPieceColor.Black);
            chessBoardNodeArray[7, 7].ChangePiece(ChessPiece.Rook, ChessPieceColor.Black);

            chessBoardNodeArray[1, 0].ChangePiece(ChessPiece.Knight, ChessPieceColor.White);
            chessBoardNodeArray[6, 0].ChangePiece(ChessPiece.Knight, ChessPieceColor.White);
            chessBoardNodeArray[1, 7].ChangePiece(ChessPiece.Knight, ChessPieceColor.Black);
            chessBoardNodeArray[6, 7].ChangePiece(ChessPiece.Knight, ChessPieceColor.Black);

            chessBoardNodeArray[2, 0].ChangePiece(ChessPiece.Bishop, ChessPieceColor.White);
            chessBoardNodeArray[5, 0].ChangePiece(ChessPiece.Bishop, ChessPieceColor.White);
            chessBoardNodeArray[2, 7].ChangePiece(ChessPiece.Bishop, ChessPieceColor.Black);
            chessBoardNodeArray[5, 7].ChangePiece(ChessPiece.Bishop, ChessPieceColor.Black);

            chessBoardNodeArray[3, 0].ChangePiece(ChessPiece.Queen, ChessPieceColor.White);
            chessBoardNodeArray[3, 7].ChangePiece(ChessPiece.Queen, ChessPieceColor.Black);

            chessBoardNodeArray[4, 0].ChangePiece(ChessPiece.King, ChessPieceColor.White);
            chessBoardNodeArray[4, 7].ChangePiece(ChessPiece.King, ChessPieceColor.Black);
        }

        public void onButtonPressed(object sender, EventArgs e) //Chess board button pressed
        {
            Button buttonClicked = (Button)sender;

            int locX = (int)Math.Floor(buttonClicked.Location.X / 50f); //Get pressed button x and y coords
            int locY = (int)Math.Floor(buttonClicked.Location.Y / 50f);

            if(selectedX == -1 && selectedY == -1) //First selection
            {
                if (chessBoardNodeArray[locX, locY].chessPiece != ChessPiece.None && chessBoardNodeArray[locX, locY].chessPieceColor == moveColor)
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
            switch (_arrayFrom[selectedX, selectedY].chessPiece)
            {
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
                case ChessPiece.King:
                    AddAvailableMoves(selectedX, selectedY + 1, validMoves);
                    AddAvailableMoves(selectedX, selectedY - 1, validMoves);
                    AddAvailableMoves(selectedX + 1, selectedY, validMoves);
                    AddAvailableMoves(selectedX + 1, selectedY + 1, validMoves);
                    AddAvailableMoves(selectedX + 1, selectedY - 1, validMoves);
                    AddAvailableMoves(selectedX - 1, selectedY, validMoves);
                    AddAvailableMoves(selectedX - 1, selectedY + 1, validMoves);
                    AddAvailableMoves(selectedX - 1, selectedY - 1, validMoves);
                    break;
                case ChessPiece.Knight:
                    AddAvailableMoves(selectedX + 1, selectedY + 2, validMoves);
                    AddAvailableMoves(selectedX + 1, selectedY - 2, validMoves);
                    AddAvailableMoves(selectedX - 1, selectedY + 2, validMoves);
                    AddAvailableMoves(selectedX - 1, selectedY - 2, validMoves);
                    AddAvailableMoves(selectedX + 2, selectedY + 1, validMoves);
                    AddAvailableMoves(selectedX + 2, selectedY - 1, validMoves);
                    AddAvailableMoves(selectedX - 2, selectedY + 1, validMoves);
                    AddAvailableMoves(selectedX - 2, selectedY - 1, validMoves);
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
            }
            validOnlyAttackMoves = validAttackMoves;
            return validMoves;
        }

        public bool AddAvailableMoves(int x, int y, List<ChessBoardNode> arrayToAdd, bool canAttack = true) //Return if can move here
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
            {
                if (chessBoardNodeArray[x, y].chessPieceColor == ChessPieceColor.None)
                {
                    arrayToAdd.Add(chessBoardNodeArray[x, y]);
                    chessBoardNodeArray[x, y].ChangeColor(ChessBoardNodeColor.Selected);
                    return true;
                }
                else if (chessBoardNodeArray[x, y].chessPieceColor != chessBoardNodeArray[selectedX, selectedY].chessPieceColor && canAttack)
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

        bool AddAvailableOnlyAttacks(int x, int y,ChessBoardNode[,] _arrayFrom, List<ChessBoardNode> _arrayToAdd) //Return if can attack here
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
            {
                if (_arrayFrom[x, y].chessPieceColor == ChessPieceColor.None)
                {
                    return false;
                }
                else if (_arrayFrom[x, y].chessPieceColor != _arrayFrom[selectedX, selectedY].chessPieceColor)
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
            if(chessBoardNodeArray[toX, toY].chessPiece == ChessPiece.King) //Check win conditions
            {
                moveColor = ChessPieceColor.None;
                MoveLabel.Text = chessBoardNodeArray[fromX, fromY].chessPieceColor + " won";
            }
            else
            {
                chessBoardNodeArray[toX, toY].ChangePiece(chessBoardNodeArray[fromX, fromY].chessPiece, chessBoardNodeArray[fromX, fromY].chessPieceColor);
                chessBoardNodeArray[fromX, fromY].ChangePiece(ChessPiece.None, ChessPieceColor.None);

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
                    if (chessAi == null)
                    {
                        chessAi = new ChessAi(this);
                    }
                    chessAi.MoveAi();
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e) //Reset board on button press
        {
            ResetBoard();
        }
    }
}
