using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public enum UnitType {
    EMPTY,
    BARRICADE,
    KNIGHT,
    ARCHER,
    FARM,
    CATAPULT,
    DRAGON,
    BALLISTA
}

public enum DamageType {
    PHYSICAL,
    FIRE,
    MAGIC
}

public class Unit {
    protected int tick = 1;
    protected bool team;
    protected UnitType type;
    protected double health;
    protected double maxHealth;
    protected bool hasMoved; // true if unit has attempted a move this turn
    public Unit() {
        type = UnitType.EMPTY;
        health = 0;
    }

    public virtual List<TileDamage> GetDamages() {
        return new List<TileDamage>();
    }

    public virtual void Damage(double damage, DamageType damageType) {
        health -= damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (tick < 5 && damage > 0) {
            health -= damage;
        }
    }
    public virtual bool IsMoving() {
        return false;
    }

    public void Update() {
        tick++;
    }
    public void ResetMove() {
        hasMoved = false;
    }

    public void SetType(UnitType type) {
        this.type = type;

    }

    public double GetHealth() {
        return health;
    }
    public UnitType GetUnitType() {
        return type;
    }

    public bool getTeam() {
        return team;
    }
    public void SetTeam(bool team) {
        this.team = team;
    }

    public UnitDetails GenerateUnitDetails() {
        return new UnitDetails(type, health);
    }
}

public class TileDamage {
    public bool team;
    public int x;
    public int y;
    public float damage;
    public DamageType damageType;
    public TileDamage(int x, int y, float damage, DamageType damageType, bool team) {
        this.team = team;
        this.x = team ? x : -x;
        this.y = team ? y : -y;
        this.damage = damage;
        this.damageType = damageType;
    }
    public TileDamage(int x, int y, DamageType damageType,  float damage, bool team) { // A Different constructor that doesn't flip the sign
        this.team = team;
        this.x = x;
        this.y = y;
        this.damage = damage;
        this.damageType = damageType;
    }
}

public class UnitDetails {
    public UnitType type;
    public bool team;
    public double health;

    public UnitDetails(UnitType type, double health) {
        this.type = type;
        this.health = health;
    }
    public double GetHealth() { return health; }
    public UnitType GetUnitType() { return type; }

}

public class Barricade : Unit {
    public Barricade(bool team) {
        type = UnitType.BARRICADE;
        this.team = team;
        health = 100;
        maxHealth = health;
    }
    public override void Damage(double damage, DamageType damageType) {
        health -= damageType == DamageType.MAGIC ? damage / 2 : damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (tick < 5 && damage > 0) {
            health -= damage;
        }
    }
}
public class Knight : Unit {
    public Knight(bool team) {
        this.team = team;
        type = UnitType.KNIGHT;
        health = 100;
        maxHealth = health;
    }
    public override void Damage(double damage, DamageType damageType) {
        health -= damageType == DamageType.MAGIC ? damage * 2 : damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (tick < 5 && damage > 0) {
            health -= damage;
        }
    }
    public override List<TileDamage> GetDamages() {
        List<TileDamage> damages = new List<TileDamage>() { //The damage to inflict
            new TileDamage(0, 1, 15, DamageType.PHYSICAL, team)
        };
        return damages;
    }
    public override bool IsMoving() {
        if (tick % 3 == 2 && !hasMoved) {
            hasMoved = true;
            return true;
        } else {
            return false;
        }
    }
}

public class Catapult : Unit {
    public Catapult(bool team) {
        this.team = team;
        type = UnitType.CATAPULT;
        health = 35;
        maxHealth = health;
    }
    public override void Damage(double damage, DamageType damageType) {
        health -= damageType == DamageType.FIRE ? damage * 2 : damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (tick < 5 && damage > 0) {
            health -= damage;
        }
    }
    public override List<TileDamage> GetDamages() {
        List<TileDamage> damages = new List<TileDamage>(); //The damage to inflict
        if (tick % 3 == 2) {
            damages.Add(new TileDamage(-1, 5, 25, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 5, 75, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(1, 5, 25, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 4, 25, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 6, 25, DamageType.PHYSICAL, team));
        };
        return damages;
    }
    public override bool IsMoving() {
        if (tick % 10 == 9 && !hasMoved) {
            hasMoved = true;
            return true;
        } else {
            return false;
        }
    }
}

public class Archer : Unit {
    public Archer(bool team) {
        this.team = team;
        type = UnitType.ARCHER;
        health = 70;
        maxHealth = health;
    }
    public override void Damage(double damage, DamageType damageType) {
        health -= damageType == DamageType.FIRE ? damage * 2 : damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (tick < 5 && damage > 0) {
            health -= damage;
        }
    }
    public override List<TileDamage> GetDamages() {
        List<TileDamage> damages = new List<TileDamage>(); //The damage to inflict
        if (tick % 3 == 2) {
            damages.Add(new TileDamage(0, 1, 20, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 2, 30, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 3, 15, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 4, 15, DamageType.PHYSICAL, team));
            damages.Add(new TileDamage(0, 5, 10, DamageType.PHYSICAL, team));
        };
        return damages;
    }
    public override bool IsMoving() {
        if (tick % 5 == 4 && !hasMoved) {
            hasMoved = true;
            return true;
        } else {
            return false;
        }
    }
}

public class Dragon : Unit {
    public Dragon(bool team) {
        this.team = team;
        type = UnitType.DRAGON;
        health = 150;
        maxHealth = health;
    }
    public override void Damage(double damage, DamageType damageType) {
        health -= damageType == DamageType.FIRE ? damage / 2 : damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (tick < 5 && damage > 0) {
            health -= damage;
        }
    }
    public override List<TileDamage> GetDamages() {
        List<TileDamage> damages = new List<TileDamage>(); //The damage to inflict
        if (tick % 3 == 2) {


            //Attack Pattern
            // D is dragon, X is attack

            //   X X
            // X X X X
            // D X X X
            // X X X X
            //   X X

            damages.Add(new TileDamage(0, 1, 50, DamageType.FIRE, team)); //Center Column
            damages.Add(new TileDamage(0, 2, 30, DamageType.FIRE, team));
            damages.Add(new TileDamage(0, 3, 20, DamageType.FIRE, team));
            damages.Add(new TileDamage(0, 4, 15, DamageType.FIRE, team));

            damages.Add(new TileDamage(1, 1, 45, DamageType.FIRE, team)); // One away
            damages.Add(new TileDamage(1, 2, 25, DamageType.FIRE, team));
            damages.Add(new TileDamage(1, 3, 15, DamageType.FIRE, team));
            damages.Add(new TileDamage(1, 4, 10, DamageType.FIRE, team));
            damages.Add(new TileDamage(-1, 1, 45, DamageType.FIRE, team));
            damages.Add(new TileDamage(-1, 2, 25, DamageType.FIRE, team));
            damages.Add(new TileDamage(-1, 3, 15, DamageType.FIRE, team));
            damages.Add(new TileDamage(-1, 4, 10, DamageType.FIRE, team));

            damages.Add(new TileDamage(2, 1, 30, DamageType.FIRE, team)); //Two away
            damages.Add(new TileDamage(2, 2, 15, DamageType.FIRE, team));
            damages.Add(new TileDamage(-2, 1, 30, DamageType.FIRE, team)); 
            damages.Add(new TileDamage(-2, 2, 15, DamageType.FIRE, team));

            damages.Add(new TileDamage(1, 0, 45, DamageType.FIRE, team)); //Next to
            damages.Add(new TileDamage(-1, 0, 45, DamageType.FIRE, team));

        };
        return damages;
    }
    public override bool IsMoving() {
        if (tick % 4 == 3 && !hasMoved) {
            hasMoved = true;
            return true;
        } else {
            return false;
        }
    }
}
