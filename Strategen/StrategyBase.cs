using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StrategyBase {
    public BoardInterface gameboard;
    public string name = "StrategyBase";
    public string author = "Unknown";

    public void SetInterface(BoardInterface gameboard) {
        this.gameboard = gameboard;
    }

    public virtual void OnStart() {/* Is called before the first game turn, Only used for changing the name and author of the strategy */}

    public virtual void Update(int turnNumber) { }
}