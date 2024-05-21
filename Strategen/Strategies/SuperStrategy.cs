using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class SuperStrategy : StrategyBase {

    public override void OnStart() {
        name = "SuperStat";
        author = "Tutorial";
    }

    public override void Update(int turnNumber) {

        if (turnNumber == 8) {
            gameboard.DeployUnit(UnitType.BARRICADE, 0, 0);
            gameboard.DeployUnit(UnitType.BARRICADE, 5, 5);
            gameboard.DeployUnit(UnitType.BARRICADE, 6, 6);
            gameboard.DeployUnit(UnitType.BARRICADE, 7, 5);
            gameboard.DeployUnit(UnitType.BARRICADE, 8, 6);
            gameboard.DeployUnit(UnitType.BARRICADE, 9, 5);
            gameboard.DeployUnit(UnitType.BARRICADE, 10, 6);
            gameboard.DeployUnit(UnitType.BARRICADE, 11, 5);
            gameboard.DeployUnit(UnitType.BARRICADE, 12, 6);
            gameboard.DeployUnit(UnitType.BARRICADE, 13, 5);
            gameboard.DeployUnit(UnitType.BARRICADE, 14, 6);

        }

        gameboard.GetUnitDetails(0, 5);

        if (turnNumber > 30 && turnNumber < 100 && turnNumber % 15 == 1) {
            gameboard.DeployUnit(UnitType.KNIGHT, 4, 1);
            gameboard.DeployUnit(UnitType.KNIGHT, 3, 1);
            gameboard.DeployUnit(UnitType.KNIGHT, 4, 3);
            gameboard.DeployUnit(UnitType.KNIGHT, 3, 3);
            gameboard.DeployUnit(UnitType.KNIGHT, 4, 5);
            gameboard.DeployUnit(UnitType.KNIGHT, 3, 5);
            gameboard.DeployUnit(UnitType.KNIGHT, 4, 7);
            gameboard.DeployUnit(UnitType.KNIGHT, 3, 7);
        }
        if (turnNumber > 110) {
            gameboard.DeployUnit(UnitType.DRAGON, 8, 8);
        }
    }
}
