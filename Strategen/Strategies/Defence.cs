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

        gameboard.DeployUnit(UnitType.BARRICADE, 0, 0);
        gameboard.DeployUnit(UnitType.BARRICADE, 1, 1);
        gameboard.DeployUnit(UnitType.BARRICADE, 2, 2);
        if (turnNumber == 60) { 
            gameboard.DeployUnit(UnitType.DRAGON, 10, 5);
        }
    }
}
