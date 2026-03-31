using Luminous.Relics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Luminous.Enchantment;
public sealed class Bottled : EnchantmentModel {
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Innate)];
    protected override void OnEnchant() => base.Card.AddKeyword(CardKeyword.Innate);
    public bool CusCanEnchant(CardModel card, RelicModel cls) {
        CardType type = card.Type;
        if ((uint)(type - 4) <= 2u) {
            return false;
        }
        CardPile? pile = card.Pile;
        if (pile != null && pile.Type == PileType.Deck && card.Keywords.Contains(CardKeyword.Unplayable)) {
            return false;
        }
        if (card.Enchantment != null && (!IsStackable || card.Enchantment.GetType() != GetType())) {
            return false;
        }
        if (cls is BottledFlame && card.Type == CardType.Attack)
            return true;
        else if (cls is BottledLightning && card.Type == CardType.Skill)
            return true;
        else if (cls is BottledTornado && card.Type == CardType.Power)
            return true;
        return false;
    }
}
