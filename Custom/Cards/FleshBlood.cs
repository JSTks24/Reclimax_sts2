#if ANGEL_COURTYARD
using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace Luminous.Custom.Cards;

[LuminousPool<NecrobinderCardPool>]
public sealed class FleshBlood : CardModel {
    private decimal _summon = 6;
    private decimal _increaseSummon = 0;

    [SavedProperty]
    public decimal Summon {
        get {
            return _summon;
        }
        set {
            _summon = value;
            DynamicVars["Aminous"].BaseValue = value;
        }
    }

    [SavedProperty]
    public decimal IncreaseSummon {
        get {
            return _increaseSummon;
        }
        set {
            _increaseSummon = value;
        }
    }
    public FleshBlood() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new SummonVar("Aminous", Summon), new SummonVar("Luminous", 2), new DynamicVar("Base", 6)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await OstyCmd.Summon(choiceContext, base.Owner, base.DynamicVars["Aminous"].BaseValue, this);
        var power = await PowerCmd.Apply<FleshBloodPower>(Owner.Creature, DynamicVars["Luminous"].BaseValue, Owner.Creature, this);
        power.CardSource = DeckVersion as FleshBlood;
    }
    protected override void OnUpgrade() {
        DynamicVars["Base"].UpgradeValueBy(1);
        DynamicVars["Luminous"].UpgradeValueBy(1);
        UpdateValue();
    }
    public void IncreaseUpdate(decimal amount) {
        IncreaseSummon += amount;
        UpdateValue();
    }
    private void UpdateValue() {
        Summon = DynamicVars["Base"].BaseValue + IncreaseSummon;
    }
}

public sealed class FleshBloodPower : PowerModel {
    public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerType Type => PowerType.Buff;
    public FleshBlood? CardSource { get; set; }
    public override async Task AfterAttack(AttackCommand command) {
        if (command.Results.Any((DamageResult r) => r.WasTargetKilled) && command.Attacker == CardSource?.Owner.Osty) {
            CardSource?.IncreaseUpdate(Amount);
        }
    }
}
#endif