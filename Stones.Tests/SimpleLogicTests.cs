using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Stones.Model;
using System.Drawing;

namespace Stones.Tests {

    [TestFixture]
    public class SimpleLogicTests {

        [Test]
        public void EstimateForStageOne() {
            var board = new Board(3);
            var logic = new SimpleLogic(board, 3);

            board.PutStone(CellState.Black, 2, 0);
            board.PutStone(CellState.White, 0, 1);
            board.PutStone(CellState.White, 0, 2);

            Assert.AreEqual(3, logic.EstimateForStageOne(0, 0, CellState.Black));
            Assert.AreEqual(2, logic.EstimateForStageOne(1, 0, CellState.Black));
            Assert.AreEqual(2, logic.EstimateForStageOne(1, 1, CellState.Black));
            Assert.AreEqual(2, logic.EstimateForStageOne(2, 1, CellState.Black));
            Assert.AreEqual(2, logic.EstimateForStageOne(1, 2, CellState.Black));
            Assert.AreEqual(1, logic.EstimateForStageOne(2, 2, CellState.Black));

            Assert.AreEqual(int.MaxValue, logic.EstimateForStageOne(0, 0, CellState.White));            
        }

        [Test]
        public void FilterCellsStageOne() {
            var board = new Board(4);
            var logic = new SimpleLogic(board, 4);

            board.PutStone(CellState.White, 3, 0);
            board.PutStone(CellState.White, 0, 3);

            var filteredCells = logic.FilterCellsStageOne(CellState.White);

            Assert.AreEqual(6, filteredCells.Count);
            Assert.IsFalse(filteredCells.Contains(new Point(0, 0)));
            Assert.IsTrue(filteredCells.Contains(new Point(2, 0)));
            Assert.IsTrue(filteredCells.Contains(new Point(1, 2)));
            Assert.IsFalse(filteredCells.Contains(new Point(2, 3)));
        }

        [Test]
        public void FilterCellsStageOne_Victory() {
            var board = new Board(3);
            var logic = new SimpleLogic(board, 3);

            board.PutStone(CellState.White, 0, 1);
            board.PutStone(CellState.White, 0, 2);
            board.PutStone(CellState.Black, 2, 1);
            board.PutStone(CellState.Black, 2, 2);

            var bestWhite = logic.FilterCellsStageOne(CellState.White);
            var bestBlack = logic.FilterCellsStageOne(CellState.Black);

            Assert.AreEqual(1, bestWhite.Count);
            Assert.AreEqual(1, bestBlack.Count);
        }

        [Test]
        public void EstimateForStageTwo() {
            var board = new Board(3);
            var logic = new SimpleLogic(board, 3);

            board.PutStone(CellState.White, 0, 0);
            board.PutStone(CellState.White, 1, 2);
            board.PutStone(CellState.Black, 2, 0);

            Assert.AreEqual(3, logic.EstimateForStageTwo(1, 0, CellState.White));
            Assert.AreEqual(4, logic.EstimateForStageTwo(0, 1, CellState.White));
            Assert.AreEqual(5, logic.EstimateForStageTwo(1, 1, CellState.White));
            Assert.AreEqual(3, logic.EstimateForStageTwo(2, 1, CellState.White));
            Assert.AreEqual(2, logic.EstimateForStageTwo(0, 2, CellState.White));
            Assert.AreEqual(2, logic.EstimateForStageTwo(2, 2, CellState.White));
        }

        [Test]
        public void EstimateForStageThree() {
            var board = new Board(5);
            var logic = new SimpleLogic(board, 3);

            var estimateCenter = logic.EstimateForStageThree(2, 2, CellState.None);
            var estimate11 = logic.EstimateForStageThree(1, 1, CellState.None);
            var estimate00 = logic.EstimateForStageThree(0, 0, CellState.None);

            Assert.Less(estimate11, estimateCenter);
            Assert.Less(estimate00, estimate11);
        }
    }
}
