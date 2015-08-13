using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Stones.Model {
    public class Board {
        int size;
        CellState[,] stones;

        public Board(int size) {
            if(size < 1)
                throw new ArgumentException();

            this.size = size;
            Clear();
        }

        public int Size {
            get { return size; }
        }

        public void PutStone(CellState color, Point location) {
            PutStone(color, location.X, location.Y);
        }

        public void PutStone(CellState color, int x, int y) {
            if(IsOutside(x, y))
                throw new ArgumentException();
            this.stones[x, y] = color;
        }

        public CellState GetStone(Point location) {
            return GetStone(location.X, location.Y);
        }

        public CellState GetStone(int x, int y) {
            if(IsOutside(x, y))
                return CellState.None;
            return this.stones[x, y];
        }

        public bool IsOutside(int x, int y) {
            return x < 0 || x > Size - 1 || y < 0 || y > Size - 1;
        }

        public IEnumerable<Point> EnumerateCells() {
            for(int y = 0; y < Size; y++) {
                for(int x = 0; x < Size; x++) {
                    yield return new Point(x, y);
                }
            }
        }

        public void Clear() {
            this.stones = new CellState[size, size];
        }
    }
}
