using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stones.Model;
using System.Drawing;

namespace Stones {
    class SimpleLogic : Logic {
        delegate IComparable EstimateFunction(Point location, CellState color);

        static readonly Random rnd = new Random();

        public SimpleLogic(Board board, int gameSize)
            : base(board, gameSize) {

        }

        public override Point SelectBestCell(CellState color) {
            var candidates = FilterCellsStageThree(color);

            if(candidates.Count == 0)
                return Draw;

            if(candidates.Count == 1)
                return candidates[0];

            int index = rnd.Next(0, candidates.Count - 1);
            return candidates[index];
        }


        internal List<Point> FilterCellsStageOne(CellState color) {
            return FilterCellsCore(Board.EnumerateCells(), EstimateForStageOne, color);
        }

        List<Point> FilterCellsStageTwo(CellState color) {
            return FilterCellsCore(FilterCellsStageOne(color), EstimateForStageTwo, color);
        }

        List<Point> FilterCellsStageThree(CellState color) {
            return FilterCellsCore(FilterCellsStageTwo(color), EstimateForStageThree, color);
        }

        List<Point> FilterCellsCore(IEnumerable<Point> source, EstimateFunction estimator, CellState color) {
            var result = new List<Point>();
            IComparable bestEstimate = null;

            foreach(Point location in source) {
                if(Board.GetStone(location) != CellState.None)
                    continue;
                var estimate = estimator(location, color);

                int compareResult = estimate.CompareTo(bestEstimate);
                if(compareResult < 0)
                    continue;
                if(compareResult > 0) {
                    result.Clear();
                    bestEstimate = estimate;
                }
                result.Add(location);
            }

            return result;
        }


        internal IComparable EstimateForStageOne(Point location, CellState color) {
            int selfScore = 1 + CalcScore(location, color);
            int opponentScore = 1 + CalcScore(location, GetOpponentColor(color));

            if(selfScore >= GameSize)
                selfScore = int.MaxValue;

            return Math.Max(selfScore, opponentScore);
        }

        internal IComparable EstimateForStageOne(int x, int y, CellState color) {
            return EstimateForStageOne(new Point(x, y), color);
        }

        internal IComparable EstimateForStageTwo(Point location, CellState color) {
            int cx=location.X;
            int cy=location.Y;

            int selfCount = 0;
            int opponentCount = 0;

            for(int x = cx - 1; x <= cx + 1; x++) {
                for(int y = cy - 1; y <= cy + 1; y++) {
                    if(Board.GetStone(x, y) == color)
                        selfCount++;
                    if(Board.GetStone(x, y) == GetOpponentColor(color))
                        opponentCount++;
                }
            }

            return 2 * selfCount + opponentCount;
        }

        internal IComparable EstimateForStageTwo(int x, int y, CellState color) {
            return EstimateForStageTwo(new Point(x, y), color);
        }

        internal IComparable EstimateForStageThree(Point location, CellState color) {
            var dx = location.X - Board.Size / 2;
            var dy = location.Y - Board.Size / 2;
            return -Math.Sqrt(dx * dx + dy * dy);
        }

        internal IComparable EstimateForStageThree(int x, int y, CellState color) {
            return EstimateForStageThree(new Point(x, y), color);
        }
    }
}
