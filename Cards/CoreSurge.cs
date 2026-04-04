using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Luminous.Util;


namespace Luminous.Cards;

[LuminousPool<DefectCardPool>]
public sealed class CoreSurge : CardModel {
    public CoreSurge(): base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars {
        get {
            return (IEnumerable<DynamicVar>)[new DamageVar(11M, ValueProp.Move), new PowerVar<ArtifactPower>(1M)];
        }
    }
    public override IEnumerable<CardKeyword> CanonicalKeywords {
        get {
            return (IEnumerable<CardKeyword>)[CardKeyword.Exhaust];
        }
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        CoreSurge card = this;
        await CreatureCmd.TriggerAnim(card.Owner.Creature, "Attack", card.Owner.Character.AttackAnimDelay);
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target!).FromCard((CardModel)card).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        await PowerCmd.Apply<ArtifactPower>(card.Owner.Creature, card.DynamicVars["ArtifactPower"].BaseValue, card.Owner.Creature, card);
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(4M);
}
