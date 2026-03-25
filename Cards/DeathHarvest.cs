using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luminous.Cards;

public sealed class DeathHarvest : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<NecrobinderCardPool>();
    public DeathHarvest()
        : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) {
    }

    public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords {
        get {
            return (IEnumerable<CardKeyword>) [CardKeyword.Exhaust];
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars {
        get {
            return (IEnumerable<DynamicVar>)[new DamageVar(4M, ValueProp.Move)];
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        DeathHarvest card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Attack", card.Owner.Character.AttackAnimDelay);
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel)card).TargetingAllOpponents(card.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        if (!base.Owner.Creature.IsDead)
            await CreatureCmd.Heal(card.Owner.Creature, (Decimal)attackCommand.Results.Sum<DamageResult>((Func<DamageResult, int>)(r => r.UnblockedDamage)));
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
