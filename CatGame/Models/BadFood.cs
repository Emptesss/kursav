namespace CatGame.Models
{
    public class BadFood : Food
    {
        public int Penalty { get; set; } = 1; // Штраф за сбор плохой еды
    }
}
