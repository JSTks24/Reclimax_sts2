using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Rooms;

namespace Luminous.Cards;
public sealed class SelfRepair : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<DefectCardPool>();
    public SelfRepair() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }
     protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(7)];
     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await PowerCmd.Apply<SelfRepairBuff>(Owner.Creature, DynamicVars.Heal.BaseValue, Owner.Creature, this);
     }
     protected override void OnUpgrade() => this.DynamicVars.Heal.UpgradeValueBy(3);
}
public class SelfRepairBuff : PowerModel {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterCombatEnd(CombatRoom room) {
        if (!base.Owner.IsDead) {
            Flash();
            await CreatureCmd.Heal(base.Owner, base.Amount);
        }
    }
}