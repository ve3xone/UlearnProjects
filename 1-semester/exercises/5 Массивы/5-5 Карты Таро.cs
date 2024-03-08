private static string GetSuit(Suits? suit)
{
        return suit.Value switch
        {
            Suits.Wands => "жезлов",
            Suits.Coins => "монет",
            Suits.Cups => "кубков",
            Suits.Swords => "мечей"
        };
}