using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BlankStrategy : StrategyBase {

    public override void OnStart() {
        name = "Name";
        author = "Author";
    }

    public override void Update(int turnNumber) {

    }
}
