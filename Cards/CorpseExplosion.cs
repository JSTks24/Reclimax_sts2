using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luminous.Cards;

[LuminousPool<SilentCardPool>]
public sealed class CorpseExplosion : CardModel {
    public CorpseExplosion() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(6)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await PowerCmd.Apply<PoisonPower>(cardPlay.Target, base.DynamicVars["PoisonPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<CorpseExplosionPower>(cardPlay.Target, base.DynamicVars["PoisonPower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars["PoisonPower"].UpgradeValueBy(3);
}

public sealed class CorpseExplosionPower : PowerModel {
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength) {
        if (Owner == creature) {
            Flash();
            VfxCmd.PlayOnCreatureCenters(Applier.CombatState.HittableEnemies, "vfx/vfx_attack_slash");
            await CreatureCmd.Damage(choiceContext, Applier.CombatState.HittableEnemies, Owner.MaxHp, ValueProp.Unpowered, null, null);
        }
    }
}