using Luminous.Enchantment;
using Luminous.Util;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Luminous.Relics;

[LuminousPool<SharedRelicPool>]
public sealed class BottledTornado : RelicModel {
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    public override bool HasUponPickupEffect => true;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new StringVar("Enchantment", ModelDb.Enchantment<Bottled>().Title.GetFormattedText())
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromEnchantment<Bottled>();
    public override async Task AfterObtained() {
        Bottled bottle = ModelDb.Enchantment<Bottled>();
        List<CardModel> list = PileType.Deck.GetPile(base.Owner).Cards.Where((CardModel c) => bottle.CusCanEnchant(c, this)).ToList();
        CardModel cardModel = (await CardSelectCmd.FromDeckForEnchantment(prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), cards: list.UnstableShuffle(base.Owner.RunState.Rng.Niche).ToList(), enchantment: bottle, amount: 1)).FirstOrDefault();
        if (cardModel != null) {
            CardCmd.Enchant<Bottled>(cardModel, 1m);
            NCardEnchantVfx nCardEnchantVfx = NCardEnchantVfx.Create(cardModel);
            if (nCardEnchantVfx != null) {
                NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
            }
        }
    }
}
