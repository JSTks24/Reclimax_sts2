using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Luminous.Cards;

class Concentrate : CardModel {
    public override CardPoolModel Pool => ModelDb.CardPool<SilentCardPool>();
    public Concentrate() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars {
        get {
            return (IEnumerable<DynamicVar>)[new EnergyVar(2), new CardsVar(3)];
        }
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay) {
        int cardCount = base.DynamicVars.Cards.IntValue;
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, base.Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, cardCount), null, this));
        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, base.Owner);
    }
    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(-1);
}
