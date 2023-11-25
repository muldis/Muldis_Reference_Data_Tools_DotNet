using System.Numerics;

namespace Muldis.Data_Engine_Reference.Internal;

internal class MDL_Fraction : MDL_NQA
{
    // The _maybe_as_Decimal field is optionally valued if the MDL_Fraction
    // value is known small enough to fit in the range it can represent
    // and it is primarily used when the MDL_Fraction value was input to
    // the system as a .NET Decimal in the first place or was output
    // from the system as such; a MDL_Fraction input first as a Decimal
    // is only copied to the as_pair field when needed;
    // if both said fields are valued at once, they are redundant.
    private Nullable<Decimal> _maybe_as_Decimal;

    // The as_pair field, comprising a numerator+denominator field
    // pair, can represent any
    // MDL_Fraction value at all, such that the Fraction's value is
    // defined as the fractional division of numerator by denominator.
    // as_pair might not be defined if as_Decimal is defined.
    private Fraction_As_Pair maybe_as_pair;

    internal MDL_Fraction(Memory memory, Decimal as_Decimal,
        BigInteger numerator, BigInteger denominator)
        : base(memory, Well_Known_Base_Type.MDL_Fraction)
    {
        this._maybe_as_Decimal = as_Decimal;
        this.maybe_as_pair = new Fraction_As_Pair(numerator, denominator);
    }

    internal MDL_Fraction(Memory memory, Decimal as_Decimal)
        : base(memory, Well_Known_Base_Type.MDL_Fraction)
    {
        this._maybe_as_Decimal = as_Decimal;
        this.maybe_as_pair = null;
    }

    internal MDL_Fraction(Memory memory,
        BigInteger numerator, BigInteger denominator)
        : base(memory, Well_Known_Base_Type.MDL_Fraction)
    {
        this._maybe_as_Decimal = null;
        this.maybe_as_pair = new Fraction_As_Pair(numerator, denominator);
    }

    internal Nullable<Decimal> maybe_as_Decimal()
    {
        return this._maybe_as_Decimal;
    }

    internal BigInteger numerator()
    {
        this.ensure_pair();
        return this.maybe_as_pair.numerator;
    }

    internal BigInteger denominator()
    {
        this.ensure_pair();
        return this.maybe_as_pair.denominator;
    }

    internal void ensure_coprime()
    {
        this.ensure_pair();
        if (this.maybe_as_pair.cached_is_coprime == true)
        {
            return;
        }
        // Note that GreatestCommonDivisor() always has a non-negative result.
        BigInteger gcd = BigInteger.GreatestCommonDivisor(
            this.maybe_as_pair.numerator, this.maybe_as_pair.denominator);
        if (gcd > 1)
        {
            // Make the numerator and denominator coprime.
            this.maybe_as_pair.numerator   = this.maybe_as_pair.numerator   / gcd;
            this.maybe_as_pair.denominator = this.maybe_as_pair.denominator / gcd;
        }
        this.maybe_as_pair.cached_is_coprime = true;
    }

    private void ensure_pair()
    {
        if (this.maybe_as_pair is not null)
        {
            return;
        }
        // If numerator+denominator are null, _maybe_as_Decimal must not be.
        Int32[] dec_bits = Decimal.GetBits((Decimal)this._maybe_as_Decimal);
        // https://msdn.microsoft.com/en-us/library/system.decimal.getbits(v=vs.110).aspx
        // The GetBits spec says that it returns 4 32-bit integers
        // representing the 128 bits of the Decimal itself; of these,
        // the first 3 integers' bits give the mantissa,
        // the 4th integer's bits give the sign and the scale factor.
        // "Bits 16 to 23 must contain an exponent between 0 and 28,
        // which indicates the power of 10 to divide the integer number."
        Int32 scale_factor_int = ((dec_bits[3] >> 16) & 0x7F);
        Decimal denominator_dec = 1M;
        // Decimal doesn't have exponentiation op so we do it manually.
        for (Int32 i = 1; i <= scale_factor_int; i++)
        {
            denominator_dec = denominator_dec * 10M;
        }
        Decimal numerator_dec = (Decimal)this._maybe_as_Decimal * denominator_dec;
        this.maybe_as_pair = new Fraction_As_Pair(
            new BigInteger(numerator_dec), new BigInteger(denominator_dec));
        this.maybe_as_pair.cached_is_terminating_decimal = true;
    }

    internal Boolean is_terminating_decimal()
    {
        if (this._maybe_as_Decimal is not null)
        {
            return true;
        }
        if (this.maybe_as_pair.cached_is_terminating_decimal is null)
        {
            this.ensure_coprime();
            Boolean found_all_2_factors = false;
            Boolean found_all_5_factors = false;
            BigInteger confirmed_quotient = this.maybe_as_pair.denominator;
            BigInteger attempt_quotient = 1;
            BigInteger attempt_remainder = 0;
            while (!found_all_2_factors)
            {
                attempt_quotient = BigInteger.DivRem(
                    confirmed_quotient, 2, out attempt_remainder);
                if (attempt_remainder > 0)
                {
                    found_all_2_factors = true;
                }
                else
                {
                    confirmed_quotient = attempt_quotient;
                }
            }
            while (!found_all_5_factors)
            {
                attempt_quotient = BigInteger.DivRem(
                    confirmed_quotient, 5, out attempt_remainder);
                if (attempt_remainder > 0)
                {
                    found_all_5_factors = true;
                }
                else
                {
                    confirmed_quotient = attempt_quotient;
                }
            }
            this.maybe_as_pair.cached_is_terminating_decimal = (confirmed_quotient == 1);
        }
        return (Boolean)this.maybe_as_pair.cached_is_terminating_decimal;
    }

    internal Int32 pair_decimal_denominator_scale()
    {
        if (this.maybe_as_pair is null || !is_terminating_decimal())
        {
            throw new InvalidOperationException();
        }
        this.ensure_coprime();
        for (Int32 dec_scale = 0; dec_scale <= Int32.MaxValue; dec_scale++)
        {
            // BigInteger.Pow() can only take an Int32 exponent anyway.
            if (BigInteger.Remainder(BigInteger.Pow(10,dec_scale), this.maybe_as_pair.denominator) == 0)
            {
                return dec_scale;
            }
        }
        // If somehow the denominator can be big enough that we'd actually get here.
        throw new NotImplementedException();
    }
}