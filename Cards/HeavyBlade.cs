using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luminous.Cards;

[LuminousPool<IroncladCardPool>]
public class HeavyBlade : CardModel {
    public HeavyBlade() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move), new EnergyVar(3)];
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource) {
        if (Owner.Creature != dealer || cardSource != this || !(props.HasFlag(ValueProp.Move) && !props.HasFlag(ValueProp.Unpowered))) {
            return 0m;
        }
        StrengthPower? strength = Owner.Creature.GetPower<StrengthPower>();
        return (DynamicVars.Energy.BaseValue - 1) * (strength?.Amount ?? 0);
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).Targeting(cardPlay.Target!).FromCard((CardModel)this).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
    protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(2);
}
