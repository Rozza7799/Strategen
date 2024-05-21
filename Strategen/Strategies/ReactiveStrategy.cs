using Strategen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ReactiveStrategy : StrategyBase {

    public override void OnStart() {
        name = "Copycat";
        author = "Tutorial";
    }

    public override void Update(int turnNumber) {
        for (int x = 0; x < 15; x++) {
            for (int y = 8; y < 15; y++) {
                if (gameboard.GetUnitDetails(x, y).GetUnitType() == UnitType.BARRICADE) {
                    gameboard.DeployUnit(UnitType.BARRICADE, x, 7 - (y-8));
                }
            }
        }
        
    }
}
