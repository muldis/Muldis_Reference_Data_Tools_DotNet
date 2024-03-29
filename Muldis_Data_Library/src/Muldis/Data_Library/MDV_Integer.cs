using System.Numerics;

namespace Muldis.Data_Library;

public readonly struct MDV_Integer
    : MDV_Orderable<MDV_Integer>, MDV_Numerical<MDV_Integer>
{
    private static readonly MDV_Integer __negative_one
        = new MDV_Integer(BigInteger.MinusOne);
    private static readonly MDV_Integer __zero
        = new MDV_Integer(BigInteger.Zero);
    private static readonly MDV_Integer __positive_one
        = new MDV_Integer(BigInteger.One);

    // A value of the .NET structure type BigInteger is immutable.
    // It should be safe to pass around without cloning.
    private readonly BigInteger __as_BigInteger;

    private MDV_Integer(BigInteger as_BigInteger)
    {
        this.__as_BigInteger = as_BigInteger;
    }

    public override Int32 GetHashCode()
    {
        return this.__as_BigInteger.GetHashCode();
    }

    public override String ToString()
    {
        return Internal_Identity.Integer(this);
    }

    public override Boolean Equals(Object? obj)
    {
        return (obj is MDV_Integer specific_obj)
            && this._same(specific_obj);
    }

    public static MDV_Integer from(BigInteger as_BigInteger)
    {
        return as_BigInteger.IsZero ? MDV_Integer.__zero
            : as_BigInteger.IsOne ? MDV_Integer.__positive_one
            : as_BigInteger.Equals(BigInteger.MinusOne) ? MDV_Integer.__negative_one
            : new MDV_Integer(as_BigInteger);
    }

    public static MDV_Integer from(Int64 as_Int64)
    {
        return MDV_Integer.from((BigInteger) as_Int64);
    }

    public static MDV_Integer from(Int32 as_Int32)
    {
        return MDV_Integer.from((BigInteger) as_Int32);
    }

    public static MDV_Integer negative_one()
    {
        return MDV_Integer.__negative_one;
    }

    public static MDV_Integer zero_()
    {
        return MDV_Integer.__zero;
    }

    public static MDV_Integer positive_one()
    {
        return MDV_Integer.__positive_one;
    }

    public void Deconstruct(out BigInteger as_BigInteger)
    {
        as_BigInteger = this.__as_BigInteger;
    }

    public void Deconstruct(out Int64 as_Int64)
    {
        // This will throw a .NET OverflowException if the value doesn't fit.
        as_Int64 = (Int64) this.__as_BigInteger;
    }

    public void Deconstruct(out Int32 as_Int32)
    {
        // This will throw a .NET OverflowException if the value doesn't fit.
        as_Int32 = (Int32) this.__as_BigInteger;
    }

    public BigInteger as_BigInteger()
    {
        return this.__as_BigInteger;
    }

    public Int64 as_Int64()
    {
        // This will throw a .NET OverflowException if the value doesn't fit.
        return (Int64) this.__as_BigInteger;
    }

    public Int32 as_Int32()
    {
        // This will throw a .NET OverflowException if the value doesn't fit.
        return (Int32) this.__as_BigInteger;
    }

    public MDV_Boolean same(MDV_Any topic_1)
    {
        MDV_Integer topic_0 = this;
        if (topic_1 is null)
        {
            throw new ArgumentNullException();
        }
        return (topic_1 is MDV_Integer specific_topic_1)
            ? MDV_Boolean.from(topic_0._same(specific_topic_1))
            : MDV_Boolean.@false();
    }

    private Boolean _same(MDV_Integer topic_1)
    {
        MDV_Integer topic_0 = this;
        return topic_0.__as_BigInteger.Equals(topic_1.__as_BigInteger);
    }

    public MDV_Boolean in_order(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Integer topic_0 = this;
        if (topic_1 is null)
        {
            throw new ArgumentNullException();
        }
        return MDV_Boolean.from(topic_0._in_order((MDV_Integer) topic_1));
    }

    private Boolean _in_order(MDV_Integer topic_1)
    {
        MDV_Integer topic_0 = this;
        return topic_0.__as_BigInteger <= topic_1.__as_BigInteger;
    }

    public MDV_Boolean so_zero()
    {
        MDV_Integer topic = this;
        return MDV_Boolean.from(topic.__as_BigInteger.IsZero);
    }

    public MDV_Numerical<MDV_Integer> zero()
    {
        return MDV_Integer.__zero;
    }

    public MDV_Integer greatest_common_divisor(MDV_Integer topic_1)
    {
        MDV_Integer topic_0 = this;
        return MDV_Integer.from(MDV_Integer._greatest_common_divisor(
            topic_0.__as_BigInteger, topic_1.__as_BigInteger));
    }

    internal static BigInteger _greatest_common_divisor(
        BigInteger topic_0, BigInteger topic_1)
    {
        // Note that greatest common divisor always has a non-negative result.
        return BigInteger.GreatestCommonDivisor(topic_0, topic_1);
    }

    public MDV_Integer least_common_multiple(MDV_Integer topic_1)
    {
        MDV_Integer topic_0 = this;
        return MDV_Integer.from(MDV_Integer._least_common_multiple(
            topic_0.__as_BigInteger, topic_1.__as_BigInteger));
    }

    internal static BigInteger _least_common_multiple(
        BigInteger topic_0, BigInteger topic_1)
    {
        // Note that least common multiple always has a non-negative result.
        if (topic_0.IsZero || topic_1.IsZero)
        {
            return topic_0;
        }
        return (BigInteger.Abs(topic_0) * (
            BigInteger.Abs(topic_1)
                / BigInteger.GreatestCommonDivisor(topic_0, topic_1)
        ));
    }

    // Given that .NET is deficient in contrast with other languages
    // by not automatically exposing "default" methods of interfaces in
    // classes that compose them, every affected composing class has
    // duplicate wrapper declarations of those methods, below this statement.
    // For brevity, input checks of the wrapped methods are not duplicated.

    public MDV_Boolean not_same(MDV_Any topic_1)
    {
        MDV_Any topic_0 = this;
        return ((MDV_Any) topic_0).not_same(topic_1);
    }

    public MDV_Any coalesce(MDV_Any topic_1)
    {
        MDV_Any topic_0 = this;
        return ((MDV_Any) topic_0).coalesce(topic_1);
    }

    public MDV_Any anticoalesce(MDV_Any topic_1)
    {
        MDV_Any topic_0 = this;
        return ((MDV_Any) topic_0).anticoalesce(topic_1);
    }

    public MDV_Boolean before(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Orderable<MDV_Integer> topic_0 = this;
        return ((MDV_Orderable<MDV_Integer>) topic_0).before(topic_1);
    }

    public MDV_Boolean after(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Orderable<MDV_Integer> topic_0 = this;
        return ((MDV_Orderable<MDV_Integer>) topic_0).after(topic_1);
    }

    public MDV_Boolean before_or_same(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Orderable<MDV_Integer> topic_0 = this;
        return ((MDV_Orderable<MDV_Integer>) topic_0).before_or_same(topic_1);
    }

    public MDV_Boolean after_or_same(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Orderable<MDV_Integer> topic_0 = this;
        return ((MDV_Orderable<MDV_Integer>) topic_0).after_or_same(topic_1);
    }

    public MDV_Orderable<MDV_Integer> min(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Orderable<MDV_Integer> topic_0 = this;
        return ((MDV_Orderable<MDV_Integer>) topic_0).min(topic_1);
    }

    public MDV_Orderable<MDV_Integer> max(MDV_Orderable<MDV_Integer> topic_1)
    {
        MDV_Orderable<MDV_Integer> topic_0 = this;
        return ((MDV_Orderable<MDV_Integer>) topic_0).max(topic_1);
    }

    public MDV_Boolean not_zero()
    {
        MDV_Numerical<MDV_Integer> topic = this;
        return ((MDV_Numerical<MDV_Integer>) topic).not_zero();
    }
}
