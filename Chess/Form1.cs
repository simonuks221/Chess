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
            if (chessBoardNodeArray != null) //Clear all buttons from previos play
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

        public void SpawnChessPiece(int x, int y, ChessPieceColor color, Type _CPClass)
        {
            Object newObject = Activator.CreateInstance(_CPClass, new Object[] { x, y, color });
            CPClass newClass = (CPClass)newObject;
            chessPieces.Add(newClass);
            chessBoardNodeArray[x, y].ChangePiece(newClass);
        }

        public void onButtonPressed(object sender, EventArgs e) //Chess board button pressed
        {
            Button buttonClicked = (Button)sender;

            int locX = (int)Math.Floor(buttonClicked.Location.X / 50f); //Get pressed button x and y coords
            int locY = (int)Math.Floor(buttonClicked.Location.Y / 50f);

            if (selectedX == -1 && selectedY == -1) //First selection
            {
                if (chessBoardNodeArray[locX, locY].chessPiece != null && chessBoardNodeArray[locX, locY].chessPiece.pieceColor == moveColor)
                {
                    availableMoveNodes.Clear();

                    selectedX = locX;
                    selectedY = locY;

                    chessBoardNodeArray[locX, locY].ChangeColor(ChessBoardNodeColor.Selected);
                    availableMoveNodes = GetMoves();
                    ShowMoves(availableMoveNodes);
                }
            }
            else //Pressed after seleceting a piece, move a piece
            {
                if (availableMoveNodes.Contains(chessBoardNodeArray[locX, locY]) || availableAttackNodes.Contains(chessBoardNodeArray[locX, locY]))
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


                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        chessBoardNodeArray[x, y].ChangeColor(ChessBoardNodeColor.None);
                    }
                }
                availableMoveNodes.Clear();
                availableAttackNodes.Clear();
            }
        }

        public void ShowMoves(List<ChessBoardNode> moves) //Return if can move here
        {
            foreach (ChessBoardNode n in moves)
            {
                Console.WriteLine(n.locationX + " " + n.locationY);
                if (n.chessPiece == null)
                {
                    n.ChangeColor(ChessBoardNodeColor.Selected);
                }
                else
                {
                    n.ChangeColor(ChessBoardNodeColor.Kill);
                }
            }
        }

        public List<ChessBoardNode> GetMoves() //Show valid selected piece moves
        {
            List<ChessBoardNode> newMoves = new List<ChessBoardNode>();

            CPClass chessPiece = chessBoardNodeArray[selectedX, selectedY].chessPiece;

            chessPiece.GetAvailableMoves(chessBoardNodeArray, out newMoves);

            return newMoves;
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
                chessBoardNodeArray[fromX, fromY].chessPiece.x = toX;
                chessBoardNodeArray[fromX, fromY].chessPiece.y = toY;

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
