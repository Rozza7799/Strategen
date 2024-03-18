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
            gameboard.DeployUnit(UnitType.KNIGHT, 5, 5);
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
