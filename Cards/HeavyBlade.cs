using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Luminous.Cards;
public class HeavyBlade : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<IroncladCardPool>();
    public HeavyBlade() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move), new EnergyVar(3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        StrengthPower? strength = this.Owner.Creature.GetPower<StrengthPower>();
        decimal damage = this.DynamicVars.Damage.BaseValue + this.DynamicVars.Energy.BaseValue * (strength?.Amount - 1 ?? 0);
        await DamageCmd.Attack(damage).Targeting(cardPlay.Target!).FromCard((CardModel)this).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
    protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(2);
}
