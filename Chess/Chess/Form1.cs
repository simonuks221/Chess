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
        ChessBoardNode[,] chessBoardNodeArray;

        int selectedX = -1; //Seected chess piece coords
        int selectedY = -1;

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
                    ShowValidMoves();
                }
            }
            else //Pressed after seleceting a piece, move a piece
            {
                if(availableMoveNodes.Contains(chessBoardNodeArray[locX, locY]) || availableAttackNodes.Contains(chessBoardNodeArray[locX, locY]))
                {
                    ChangePieceLocation(selectedX, selectedY, locX, locY);
                }

                chessBoardNodeArray[selectedX, selectedY].ChangeColor(ChessBoardNodeColor.None);

                selectedX = -1;
                selectedY = -1;

                foreach (ChessBoardNode n in availableMoveNodes)
                {
                    n.ChangeColor(ChessBoardNodeColor.None);
                }
                availableMoveNodes.Clear();

                foreach (ChessBoardNode n in availableAttackNodes)
                {
                    n.ChangeColor(ChessBoardNodeColor.None);
                }
                availableAttackNodes.Clear();

            }
        }

        void ShowValidMoves() //Show valid selected piece moves
        {
            switch (chessBoardNodeArray[selectedX, selectedY].chessPiece)
            {
                case ChessPiece.Bishop:
                    for(int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY + i)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY - i)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY - i)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY + i)) break;
                    }

                    break;
                case ChessPiece.King:
                    AddAvailableMoves(selectedX, selectedY + 1);
                    AddAvailableMoves(selectedX, selectedY - 1);
                    AddAvailableMoves(selectedX + 1, selectedY);
                    AddAvailableMoves(selectedX + 1, selectedY + 1);
                    AddAvailableMoves(selectedX + 1, selectedY - 1);
                    AddAvailableMoves(selectedX - 1, selectedY);
                    AddAvailableMoves(selectedX - 1, selectedY + 1);
                    AddAvailableMoves(selectedX - 1, selectedY - 1);
                    break;
                case ChessPiece.Knight:
                    AddAvailableMoves(selectedX + 1, selectedY + 2);
                    AddAvailableMoves(selectedX + 1, selectedY - 2);
                    AddAvailableMoves(selectedX - 1, selectedY + 2);
                    AddAvailableMoves(selectedX - 1, selectedY - 2);
                    AddAvailableMoves(selectedX + 2, selectedY + 1);
                    AddAvailableMoves(selectedX + 2, selectedY - 1);
                    AddAvailableMoves(selectedX - 2, selectedY + 1);
                    AddAvailableMoves(selectedX - 2, selectedY - 1);
                    break;
                case ChessPiece.Pawn:
                    if(chessBoardNodeArray[selectedX, selectedY].chessPieceColor == ChessPieceColor.White)
                    { //White
                        if(selectedY == 1) //Can go 2 times
                        {
                            AddAvailableMoves(selectedX, selectedY + 1, false);
                            AddAvailableMoves(selectedX, selectedY + 2, false);
                        }
                        else
                        {
                            AddAvailableMoves(selectedX, selectedY + 1, false);
                        }

                        AddAvailableOnlyAttacks(selectedX + 1, selectedY + 1);
                        AddAvailableOnlyAttacks(selectedX - 1, selectedY + 1);

                    }
                    else //Black
                    {
                        if(selectedY == 6) //Can go 2
                        {
                            AddAvailableMoves(selectedX, selectedY - 1, false);
                            AddAvailableMoves(selectedX, selectedY - 2, false);
                        }
                        else
                        {
                            AddAvailableMoves(selectedX, selectedY - 1, false);
                        }

                        AddAvailableOnlyAttacks(selectedX + 1, selectedY - 1);
                        AddAvailableOnlyAttacks(selectedX - 1, selectedY - 1);
                    }


                    break;
                case ChessPiece.Queen:
                    for (int x = selectedX + 1; x < 8; x++)
                    {
                        if (!AddAvailableMoves(x, selectedY)) break;
                    }
                    for (int x = selectedX - 1; x >= 0; x--)
                    {
                        if (!AddAvailableMoves(x, selectedY)) break;
                    }
                    for (int y = selectedY + 1; y < 8; y++)
                    {
                        if (!AddAvailableMoves(selectedX, y)) break;
                    }

                    for (int y = selectedY - 1; y >= 0; y--)
                    {
                        if (!AddAvailableMoves(selectedX, y)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY + i)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY - i)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX + i, selectedY - i)) break;
                    }
                    for (int i = 1; i < 8; i++)
                    {
                        if (!AddAvailableMoves(selectedX - i, selectedY + i)) break;
                    }
                    break;
                case ChessPiece.Rook:
                    for(int x = selectedX + 1; x < 8; x++)
                    {
                        if (!AddAvailableMoves(x, selectedY)) break;
                    }
                    for(int x = selectedX - 1; x >= 0; x--)
                    {
                        if (!AddAvailableMoves(x, selectedY)) break;
                    }

                    for (int y = selectedY + 1; y < 8; y++)
                    {
                        if (!AddAvailableMoves(selectedX, y)) break;
                    }

                    for (int y = selectedY - 1; y >= 0; y--)
                    {
                        if (!AddAvailableMoves(selectedX, y)) break;
                    }
                    break;
            }
        }

        bool AddAvailableMoves(int x, int y, bool canAttack = true) //Return if can move here
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
            {
                if (chessBoardNodeArray[x, y].chessPieceColor == ChessPieceColor.None)
                {
                    availableMoveNodes.Add(chessBoardNodeArray[x, y]);
                    chessBoardNodeArray[x, y].ChangeColor(ChessBoardNodeColor.Selected);
                    return true;
                }
                else if (chessBoardNodeArray[x, y].chessPieceColor != chessBoardNodeArray[selectedX, selectedY].chessPieceColor && canAttack)
                {
                    availableMoveNodes.Add(chessBoardNodeArray[x, y]);
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

        bool AddAvailableOnlyAttacks(int x, int y) //Return if can attack here
        {
            if (x < 8 && x >= 0 && y < 8 && y >= 0)
            {
                if (chessBoardNodeArray[x, y].chessPieceColor == ChessPieceColor.None)
                {
                    return false;
                }
                else if (chessBoardNodeArray[x, y].chessPieceColor != chessBoardNodeArray[selectedX, selectedY].chessPieceColor)
                {
                    availableAttackNodes.Add(chessBoardNodeArray[x, y]);
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

        void ChangePieceLocation(int fromX, int fromY, int toX, int toY) //Move piece
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
        }

        private void ResetButton_Click(object sender, EventArgs e) //Reset board on button press
        {
            ResetBoard();
        }
    }
}
