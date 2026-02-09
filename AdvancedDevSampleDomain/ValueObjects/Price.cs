using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Domain.ValueObjects
{
    // CORRECTION : On enlève "struct" pour en faire un Type Référence (class)
    // "public record Price" est équivalent à "public record class Price"
    public record Price
    {
        public decimal Value { get; init; }

        // Constructeur pour EF Core (parfois requis)
        protected Price() { }

        public Price(decimal value)
        {
            if (value < 0) // J'ai remis < 0 car 0 est souvent accepté, mais <= 0 marche aussi selon votre règle
            {
                throw new DomainException("Un prix ne peut pas être négatif.");
            }

            Value = value;
        }

        // Optionnel : Pour afficher joliment le prix
        public override string ToString() => Value.ToString("F2");
    }
}