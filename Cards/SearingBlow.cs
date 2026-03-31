using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luminous.Cards;
public sealed class SearingBlow : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<IroncladCardPool>();
    public SearingBlow() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move)];
    public override int MaxUpgradeLevel => int.MaxValue;
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target!).FromCard((CardModel)this).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(4 + CurrentUpgradeLevel);
}