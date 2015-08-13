using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stones.Model;
using System.Drawing;

namespace Stones {
    public abstract class Logic {
        public readonly static Point Draw = new Point(-1, -1);

        Board board;
        int gameSize;        

        public Logic(Board board, int gameSize) {
            if(board == null)
                throw new ArgumentNullException();
            if(gameSize < 2)
                throw new ArgumentException();
            this.board = board;
            this.gameSize = gameSize;            
        }

        public Board Board {
            get { return board; }
        }

        public int GameSize {
            get { return gameSize; }
        }

        public bool HaveVictoryAt(int x, int y, CellState color) {
            return HaveVictoryAt(new Point(x, y), color);
        }

        public bool HaveVictoryAt(Point location, CellState color) {
            return Board.GetStone(location) == color && CalcScore(location, color) >= GameSize - 1;
        }

        public abstract Point SelectBestCell(CellState color);

        public CellState GetOpponentColor(CellState myColor) {
            if(myColor == CellState.White)
                return CellState.Black;
            if(myColor == CellState.Black)
                return CellState.White;
            throw new ArgumentException();
        }

        int CountStonesInDirection(Point start, int dx, int dy, CellState color) {
            int result = 0;

            for(int i = 1; i < GameSize; i++) {
                Point current = start + new Size(i * dx, i * dy);
                if(Board.GetStone(current) != color)
                    break;
                result++;
            }

            return result;
        }

        protected int CalcScore(Point location, CellState color) {
            int[] counts = new int[] { 
                CountStonesInDirection(location, -1, 0, color) + CountStonesInDirection(location, 1, 0, color),
                CountStonesInDirection(location, 0, -1, color) + CountStonesInDirection(location, 0, 1, color),
                CountStonesInDirection(location, -1, -1, color) + CountStonesInDirection(location, 1, 1, color),
                CountStonesInDirection(location, -1, 1, color) + CountStonesInDirection(location, 1, -1, color)
            };

            int result = 0;
            for(int i = 0; i < counts.Length; i++) {
                result = Math.Max(result, counts[i]);
            }
            return result;
        }
    }
}
