using Luminous.Util;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Luminous.Cards;

[LuminousPool<IroncladCardPool>]
public sealed class Berserk : CardModel {
    public Berserk() : base(0, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Binbong", 2), new EnergyVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await PowerCmd.Apply<VulnerablePower>(Owner.Creature, DynamicVars["Binbong"].BaseValue, Owner.Creature, this);
        Owner.Creature.GetPower<VulnerablePower>()!.SkipNextDurationTick = false;
        await PowerCmd.Apply<BerserkBuff>(Owner.Creature, DynamicVars.Energy.BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => DynamicVars["Binbong"].UpgradeValueBy(-1);
}
public sealed class BerserkBuff : PowerModel {
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.ForEnergy(this)];

    public override decimal ModifyMaxEnergy(Player player, decimal amount) {
        if (player != base.Owner.Player) {
            return amount;
        }
        return amount + (decimal)base.Amount;
    }
}