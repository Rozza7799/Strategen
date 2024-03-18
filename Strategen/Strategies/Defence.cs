using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Defence : StrategyBase {

    public override void OnStart() {
        name = "DEFENCE";
        author = "Tutorial";
    }

    public override void Update(int turnNumber) {
        
        if (turnNumber == 4) { 
            gameboard.DeployUnit(UnitType.BARRICADE, 10, 5);
        }
        /*
        gameboard.DeployUnit(UnitType.KNIGHT, 6, 6);
        gameboard.DeployUnit(UnitType.KNIGHT, 7, 5);
        gameboard.DeployUnit(UnitType.KNIGHT, 8, 6);
        gameboard.DeployUnit(UnitType.KNIGHT, 9, 5);
        gameboard.DeployUnit(UnitType.KNIGHT, 10, 6);
        gameboard.DeployUnit(UnitType.KNIGHT, 11, 5);
        gameboard.DeployUnit(UnitType.KNIGHT, 12, 6);
        gameboard.DeployUnit(UnitType.KNIGHT, 13, 5);
        gameboard.DeployUnit(UnitType.KNIGHT, 14, 6);*/
        
        
    }
}
