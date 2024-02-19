using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    protected int tick = 0;
    protected bool team;
    protected UnitType type;
    protected double health;
    protected double maxHealth;
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
    }
    public virtual bool IsMoving() {
        return false;
    }

    public void Update() {
        tick++;
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
        this.x = x;
        this.y = y;
        this.damage = damage;
        this.damageType = damageType;
        this.team = team;
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
    }
}
public class Knight : Unit {
    public Knight(bool team) {
        this.team = team;
        type = UnitType.KNIGHT;
        health = 100;
        maxHealth = 100;
    }
    public override void Damage(double damage, DamageType damageType) {
        health -= damageType == DamageType.MAGIC ? damage * 2 : damage;
        if (health > maxHealth) {
            health = maxHealth;
        }
    }
    public override List<TileDamage> GetDamages() {
        List<TileDamage> damages = new List<TileDamage>() { //The damage to inflict
            new TileDamage(0, team ? 1 : -1, 15, DamageType.PHYSICAL, team) 
        };


        return damages;
    }
    public override bool IsMoving() {
        return tick % 3 == 0;
    }
}
