using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategen.Strategies {
    internal class SampleAlgorithm : StrategyBase {

        public override void Update(int turnNumber) {
            gameboard.DeployUnit(UnitType.WALL, 5, 5);
            gameboard.DeployUnit(UnitType.WALL, 6, 6);
            gameboard.DeployUnit(UnitType.WALL, 7, 5);
            gameboard.DeployUnit(UnitType.WALL, 8, 6);
            gameboard.DeployUnit(UnitType.WALL, 9, 5);
            gameboard.DeployUnit(UnitType.WALL, 10, 6);
            gameboard.DeployUnit(UnitType.WALL, 11, 5);
            gameboard.DeployUnit(UnitType.WALL, 12, 6);
            gameboard.DeployUnit(UnitType.WALL, 13, 5);
            gameboard.DeployUnit(UnitType.WALL, 14, 6);

        }
    }
}
