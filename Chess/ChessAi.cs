using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class MinmaxGameState
    {
        public ChessBoardNode[,] chessBoardNodeArray;
        public int blackScore;
        public int whiteScore;

        public int value;

        public int fromX;
        public int fromY;

        public int toX;
        public int toY;

        public MinmaxGameState parentMinmax;

        public List<MinmaxGameState> branches;

        public MinmaxGameState(ChessBoardNode[,] _array, int _fromX, int _fromY, int _toX, int _toY, MinmaxGameState _parentMinmax)
        {
            chessBoardNodeArray = _array;
            whiteScore = 0;
            blackScore = 0;
            branches = new List<MinmaxGameState>();

            value = 0;

            fromX = _fromX;
            fromY = _fromY;
            toX = _toX;
            toY = _toY;

            parentMinmax = _parentMinmax;

            calculateScore();
        }

        void calculateScore()
        {
            whiteScore = 0;
            blackScore = 0;
            foreach (ChessBoardNode n in chessBoardNodeArray)
            {
                int score = getChessPieceScore(n.chessPiece);
                if (n.chessPieceColor == ChessPieceColor.White)
                {
                    whiteScore += score;
                }
                else
                {
                    blackScore += score;
                }
            }
        }

        int getChessPieceScore(ChessPiece p)
        {
            switch (p)
            {
                case ChessPiece.Bishop:
                    return 3;
                case ChessPiece.King:
                    return 30;
                case ChessPiece.Knight:
                    return 3;
                case ChessPiece.Pawn:
                    return 1;
                case ChessPiece.Queen:
                    return 9;
                case ChessPiece.Rook:
                    return 5;
            }
            return 0;
        }
    }

    class MyBinaryTree
    {
        public MinmaxGameState root;
        public int amountOfLayers;

        public MyBinaryTree(MinmaxGameState _root, int _amountOfLayers)
        {
            root = _root;
            amountOfLayers = _amountOfLayers;
        }
    }

    class ChessAi
    {
        Form1 form;

        public ChessAi(Form1 _form)
        {
            form = _form;
        }

        List<ChessBoardNode> getAllColorPieces(ChessPieceColor _colorToFind)
        {
            List<ChessBoardNode> array = new List<ChessBoardNode>();
            foreach (ChessBoardNode n in form.chessBoardNodeArray)
            {
                if (n.chessPieceColor == _colorToFind)
                {
                    array.Add(n);
                }
            }
            return array;
        }

        public void MoveAi() //Suposed to use MinMax, however...
        {
            MinmaxGameState currentState = new MinmaxGameState(copyChessBoardArray(form.chessBoardNodeArray), 0, 0, 0, 0, null);
            MyBinaryTree tree = new MyBinaryTree(currentState, 3);
            copyChessBoardArray(form.chessBoardNodeArray);

            tree.root.branches = populateBranch(getAllColorPieces(ChessPieceColor.Black), form.chessBoardNodeArray);
            /*
            //foreach (MinmaxGameState layer1 in tree.root.branches)
            for(int i1 = 0; i1 < tree.root.branches.Count; i1++)
            {
                tree.root.branches[i1].branches = populateBranch(getAllColorPieces(ChessPieceColor.White),tree.root.chessBoardNodeArray);
                for (int i2 = 0; i2 < tree.root.branches[i1].branches.Count; i2++)
                {
                    tree.root.branches[i1].branches[i2].branches = populateBranch(getAllColorPieces(ChessPieceColor.Black), tree.root.chessBoardNodeArray);
                }
            }


            //Find best

            /*
            for(int i1 = 0; i1 < tree.root.branches.Count; i1++)
            {
                for (int i2 = 0; i2 < tree.root.branches[i1].branches.Count; i2++)
                {
                    tree.root.branches[i1].branches[i2].value = FindBestInBranch(tree.root.branches[i1].branches[i2].branches, ChessPieceColor.Black);
                }

                tree.root.branches[i1].value = FindWorstInBranch(tree.root.branches[i1].branches);
            }
            */

            int bestIndex = 0;
            int bestValue = 999;
            for(int i = 0; i < tree.root.branches.Count; i++)
            {
                if(tree.root.branches[i].whiteScore < bestValue)
                {
                    bestIndex = i;
                    bestValue = tree.root.branches[i].whiteScore;
                }
            }
            //form.chessBoardNodeArray = tree.root.branches[0].chessBoardNodeArray;
            form.ChangePieceLocation(tree.root.branches[bestIndex].fromX, tree.root.branches[bestIndex].fromY, tree.root.branches[bestIndex].toX, tree.root.branches[bestIndex].toY);
        }

        int FindWorstInBranch(List<MinmaxGameState> _branch) //Failed minmax
        {
            int worstValue = 999;
            foreach (MinmaxGameState state in _branch)
            {
                if (state.value < worstValue) worstValue = state.value;
            }
            return worstValue;
        }

        int FindBestInBranch(List<MinmaxGameState> _branch, ChessPieceColor _ofColor) //Failed minmax
        {
            int bestValue = 0;
            foreach(MinmaxGameState state in _branch)
            {
                switch (_ofColor)
                {
                    case ChessPieceColor.Black:
                        if (state.blackScore > bestValue) bestValue = state.blackScore;
                        break;
                    case ChessPieceColor.White:
                        if (state.whiteScore > bestValue) bestValue = state.whiteScore;
                        break;
                    case ChessPieceColor.None:
                        if (state.value > bestValue) bestValue = state.value;
                        break;
                }
            }

            return bestValue;
        }

        List<MinmaxGameState> populateBranch(List<ChessBoardNode> _nodesFrom, ChessBoardNode[,] _nodeGrid)
        {
            List<MinmaxGameState> array = new List<MinmaxGameState>();
            foreach (ChessBoardNode n in _nodesFrom)
            {
                form.selectedX = n.locationX;
                form.selectedY = n.locationY;
                List<ChessBoardNode> availableAttacks = new List<ChessBoardNode>();
                List<ChessBoardNode> availableMoves = form.ShowValidMoves(_nodeGrid, out availableAttacks);

                foreach (ChessBoardNode n2 in availableMoves) //Populates all available black moves on board
                {
                    int fromX, fromY, toX, toY;
                    fromX = n.locationX;
                    fromY = n.locationY;
                    toX = n2.locationX;
                    toY = n2.locationY;

                    ChessBoardNode[,] newStateArray = copyChessBoardArray(_nodeGrid);
                    newStateArray[n2.locationX, n2.locationY] = new ChessBoardNode(n2.locationX, n2.locationY);
                    newStateArray[n2.locationX, n2.locationY].ChangePiece(n.chessPiece, n.chessPieceColor);

                    newStateArray[n.locationX, n.locationY] = new ChessBoardNode(n.locationX, n.locationY);
                    newStateArray[n.locationX, n.locationY].ChangePiece(ChessPiece.None, ChessPieceColor.None);
                    MinmaxGameState newState = new MinmaxGameState(newStateArray, fromX, fromY, toX, toY, null);
                    array.Add(newState);

                }

                foreach (ChessBoardNode n2 in availableAttacks) //Populates all available black moves on board
                {
                    int fromX, fromY, toX, toY;
                    fromX = n.locationX;
                    fromY = n.locationY;
                    toX = n2.locationX;
                    toY = n2.locationY;

                    ChessBoardNode[,] newStateArray = copyChessBoardArray(_nodeGrid);
                    newStateArray[n2.locationX, n2.locationY] = new ChessBoardNode(n2.locationX, n2.locationY);
                    newStateArray[n2.locationX, n2.locationY].ChangePiece(n.chessPiece, n.chessPieceColor);

                    newStateArray[n.locationX, n.locationY] = new ChessBoardNode(n.locationX, n.locationY);
                    newStateArray[n.locationX, n.locationY].ChangePiece(ChessPiece.None, ChessPieceColor.None);
                    MinmaxGameState newState = new MinmaxGameState(newStateArray, fromX, fromY, toX, toY, null);
                    array.Add(newState);

                }
            }
            return array;
        }

        ChessBoardNode[,] copyChessBoardArray(ChessBoardNode[,] _arrayFrom) //Copies array to another
        {
            ChessBoardNode[,] copyArray = new ChessBoardNode[8, 8];

            for (int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    copyArray[x, y] = new ChessBoardNode(_arrayFrom[x, y].locationX, _arrayFrom[x, y].locationY);
                    copyArray[x, y].chessPieceColor = _arrayFrom[x, y].chessPieceColor;
                    copyArray[x, y].chessPiece = _arrayFrom[x, y].chessPiece;
                }
            }
            return copyArray;
        }
    }
}
