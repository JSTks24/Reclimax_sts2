using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Luminous.Cards;

public sealed class Heatsink : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<DefectCardPool>();
    public Heatsink() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        await PowerCmd.Apply<HeatsinkBuff>(this.Owner.Creature, this.DynamicVars.Cards.BaseValue, this.Owner.Creature, this);
    }
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1);
}

class HeatsinkBuff : PowerModel {
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay) {
        if (cardPlay.Card.Owner.Creature == this.Owner && CombatManager.Instance.IsInProgress && cardPlay.Card.Type == CardType.Power) {
            Flash();
            await CardPileCmd.Draw(context, Amount, this.Owner.Player!);
        }
    }
}