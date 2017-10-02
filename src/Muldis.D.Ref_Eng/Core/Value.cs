using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Muldis.D.Ref_Eng.Core
{
    // Muldis.D.Ref_Eng.Core.MD_MSBT
    // Enumerates Muldis D base types that are considered well-known to
    // this Muldis D language implementation, and that in particular have
    // their own dedicated handling code or formats in the implementation.
    // Every one of these types is either disjoint from or a proper subtype
    // or proper supertype of each of the others of these types, and in
    // particular many of these are proper subtypes of Capsule; however,
    // every Muldis D value is considered to have a best fit into exactly
    // one of these types and that value is expected to be normalized
    // towards its best fit storage format.

    internal enum MD_Well_Known_Base_Type
    {
        MD_Boolean,
        MD_Integer,
        MD_Fraction,
        MD_Bits,
        MD_Blob,
        MD_Text,
        MD_Array,
        MD_Set,
        MD_Bag,
        MD_Tuple,
        MD_Capsule,
        MD_Variable,
        MD_Process,
        MD_Stream,
        MD_External,
        MD_Excuse,
    };

    // Muldis.D.Ref_Eng.Core.MD_Any
    // Represents a Muldis D "value", which is an individual constant that
    // is not fixed in time or space.  Every Muldis D value is unique,
    // eternal, and immutable; it has no address and can not be updated.
    // Several distinct MD_Any objects may denote the same Muldis D
    // "value"; however, any time that two MD_Any are discovered to
    // denote the same Muldis D value, any references to one may be safely
    // replaced by references to the other.
    // As a primary aid in implementing the "flyweight pattern", a MD_Any
    // object is actually a lightweight "value handle" pointing to a
    // separate "value struct" object with its components; this allows us
    // to get as close as easily possible to replacing references to one
    // MD_Any with another where the underlying virtual machine or
    // garbage collector doesn't natively provide that ability.
    // Similarly, proving equality of two "value" can often short-circuit.
    // While MD_Any are immutable from a user's perspective, their
    // components may in fact mutate for memory sharing or consolidating.
    // Iff a Muldis D "value" is a "Handle" then it references something
    // that possibly can mutate, such as a Muldis D "variable".

    internal class MD_Any
    {
        internal MD_Any_Struct AS { get; set; }

        internal Boolean Same(MD_Any value)
        {
            return AS.Memory.Executor.Any__same(this, value);
        }

        internal String MD_Any_Identity()
        {
            // This called function will test if AS.Cached_MD_Any_Identity
            // is null and assign it a value if so and use its value if not.
            return AS.Memory.Identity_Generator.MD_Any_to_Identity_String(this);
        }

        public override String ToString()
        {
            return AS.Memory.Preview_Generator.MD_Any_To_Preview_String(this);
        }
    }

    internal class MD_Any_Comparer : EqualityComparer<MD_Any>
    {
        public override Boolean Equals(MD_Any v1, MD_Any v2)
        {
            if (v1 == null && v2 == null)
            {
                // Would we ever get here?
                return true;
            }
            if (v1 == null || v2 == null)
            {
                return false;
            }
            return v1.Same(v2);
        }

        public override Int32 GetHashCode(MD_Any v)
        {
            if (v == null)
            {
                // Would we ever get here?
                return 0;
            }
            return v.MD_Any_Identity().GetHashCode();
        }
    }

    internal class MD_Any_Struct
    {
        // Memory pool this Muldis D "value" lives in.
        internal Memory Memory { get; set; }

        // Muldis D most specific well known base data type (MSBT) this
        // "value" is a member of.  Determines interpreting Details field.
        // Some of these types have their own subset of specialized
        // representation formats for the sake of optimization.
        internal MD_Well_Known_Base_Type MD_MSBT { get; set; }

        // Details of this Muldis D "value", in one of several possible
        // specialized representation formats depending on the data type.
        // Iff MSBT is MD_Boolean, this field holds a Nullable<Boolean>.
        // Iff MSBT is MD_Integer, this field holds a BigInteger.
            // While we conceptually could special case smaller integers with
            // additional fields for performance, we won't, mainly to keep
            // things simpler, and because BigInteger special-cases internally.
        // Iff MSBT is MD_Fraction, this field holds a MD_Fraction_Struct.
        // Iff MSBT is MD_Bits, this field holds a BitArray.
            // Consider a MD_Bits_Struct if we want symbolic like MD_Array.
        // Iff MSBT is MD_Blob, this field holds a Byte[].
            // Consider a MD_Blob_Struct if we want symbolic like MD_Array.
        // Iff MSBT is MD_Text, this field holds a MD_Text_Struct.
        // Iff MSBT is MD_Array, this field holds a MD_Array_Struct.
        // Iff MSBT is MD_Set, this field holds a MD_Bag_Struct (like MD_Bag).
        // Iff MSBT is MD_Bag, this field holds a MD_Bag_Struct (like MD_Set).
        // Iff MSBT is MD_Tuple, this field holds a MD_Tuple_Struct (like MD_Excuse).
        // Iff MSBT is MD_Capsule, this field holds a MD_Capsule_Struct.
        // Iff MSBT is MD_Variable, this field holds a MD_Any.
            // For a MD_Variable, Details holds its Current_Value.
            // This can become a MD_Variable_Struct if we want to store other things.
        // Iff MSBT is MD_Process, this field holds an Object.
            // TODO: Replace this with some other type when we know what that is.
        // Iff MSBT is MD_Stream, this field holds an Object.
            // TODO: Replace this with some other type when we know what that is.
        // Iff MSBT is MD_External, this field holds an Object.
            // The entity that is defined and managed externally to the Muldis
            // D language environment, which the MD_External value is an opaque
            // and transient reference to.
        // Iff MSBT is MD_Excuse, this field holds a MD_Tuple_Struct (like MD_Tuple).
        internal Object Details { get; set; }

        // Set of well-known Muldis D types that this value is known to be
        // a member of.  This is calculated semi-lazily as needed.
        // This set excludes on purpose the subset of well-known types that
        // should be trivial to test membership of by other means; in
        // particular it excludes {Any,None}, the MD_Well_Known_Base_Type;
        // types not excluded are more work to test.
        internal HashSet<MD_Well_Known_Type> Cached_WKT { get; set; }

        // Normalized serialization of the Muldis D "value" that its host
        // MD_Any_Struct represents.  This is calculated lazily if needed,
        // typically when the "value" is a member of an indexed collection.
        // The serialization format either is or resembles a Muldis D Plain Text
        // literal for selecting the value, in the form of character strings
        // whose character codepoints are typically in the 0..127 range.
        internal String Cached_MD_Any_Identity { get; set; }

        internal Nullable<Boolean> MD_Boolean()
        {
            return (Nullable<Boolean>)Details;
        }

        internal BigInteger MD_Integer()
        {
            return (BigInteger)Details;
        }

        internal MD_Fraction_Struct MD_Fraction()
        {
            return (MD_Fraction_Struct)Details;
        }

        internal BitArray MD_Bits()
        {
            return (BitArray)Details;
        }

        internal Byte[] MD_Blob()
        {
            return (Byte[])Details;
        }

        internal MD_Text_Struct MD_Text()
        {
            return (MD_Text_Struct)Details;
        }

        internal MD_Array_Struct MD_Array()
        {
            return (MD_Array_Struct)Details;
        }

        internal MD_Bag_Struct MD_Set()
        {
            return MD_Bag();
        }

        internal MD_Bag_Struct MD_Bag()
        {
            return (MD_Bag_Struct)Details;
        }

        internal MD_Tuple_Struct MD_Tuple()
        {
            return (MD_Tuple_Struct)Details;
        }

        internal MD_Capsule_Struct MD_Capsule()
        {
            return (MD_Capsule_Struct)Details;
        }

        internal MD_Any MD_Variable()
        {
            return (MD_Any)Details;
        }

        internal Object MD_Process()
        {
            return (Object)Details;
        }

        internal Object MD_Stream()
        {
            return (Object)Details;
        }

        internal Object MD_External()
        {
            return (Object)Details;
        }

        internal MD_Tuple_Struct MD_Excuse()
        {
            return (MD_Tuple_Struct)Details;
        }
    }

    // Muldis.D.Ref_Eng.Core.MD_Fraction_Struct
    // When a Muldis.D.Ref_Eng.Core.MD_Any is representing a MD_Fraction,
    // an MD_Fraction_Struct is used by it to hold the MD_Fraction-specific details.

    internal class MD_Fraction_Struct
    {
        // The As_Decimal field is optionally valued if the MD_Fraction
        // value is known small enough to fit in the range it can represent
        // and it is primarily used when the MD_Fraction value was input to
        // the system as a .Net Decimal in the first place or was output
        // from the system as such; a MD_Fraction input first as a Decimal
        // is only copied to the As_Pair field when needed;
        // if both said fields are valued at once, they are redundant.
        internal Nullable<Decimal> As_Decimal { get; set; }

        // The As_Pair field, comprising a Numerator+Denominator field
        // pair, can represent any
        // MD_Fraction value at all, such that the Fraction's value is
        // defined as the fractional division of Numerator by Denominator.
        // As_Pair might not be defined if As_Decimal is defined.
        internal MD_Fraction_Pair As_Pair { get; set; }

        internal void Ensure_Coprime()
        {
            Ensure_Pair();
            if (As_Pair.Cached_Is_Coprime == true)
            {
                return;
            }
            // Note that GreatestCommonDivisor() always has a non-negative result.
            BigInteger gcd = BigInteger.GreatestCommonDivisor(As_Pair.Numerator, As_Pair.Denominator);
            if (gcd > 1)
            {
                // Make the numerator and denominator coprime.
                As_Pair.Numerator   = As_Pair.Numerator   / gcd;
                As_Pair.Denominator = As_Pair.Denominator / gcd;
            }
            As_Pair.Cached_Is_Coprime = true;
        }

        internal void Ensure_Pair()
        {
            if (As_Pair != null)
            {
                return;
            }
            // If Numerator+Denominator are null, As_Decimal must not be.
            Int32[] dec_bits = Decimal.GetBits((Decimal)As_Decimal);
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
            Decimal numerator_dec = (Decimal)As_Decimal * denominator_dec;
            As_Pair = new MD_Fraction_Pair {
                Numerator = new BigInteger(numerator_dec),
                Denominator = new BigInteger(denominator_dec),
                Cached_Is_Terminating_Decimal = true,
            };
        }

        internal Boolean Is_Terminating_Decimal()
        {
            if (As_Decimal != null)
            {
                return true;
            }
            if (As_Pair.Cached_Is_Terminating_Decimal == null)
            {
                Ensure_Coprime();
                Boolean found_all_2_factors = false;
                Boolean found_all_5_factors = false;
                BigInteger confirmed_quotient = As_Pair.Denominator;
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
                As_Pair.Cached_Is_Terminating_Decimal = (confirmed_quotient == 1);
            }
            return (Boolean)As_Pair.Cached_Is_Terminating_Decimal;
        }

        internal Int32 Pair_Decimal_Denominator_Scale()
        {
            if (As_Pair == null || !Is_Terminating_Decimal())
            {
                throw new InvalidOperationException();
            }
            Ensure_Coprime();
            for (Int32 dec_scale = 0; dec_scale <= Int32.MaxValue; dec_scale++)
            {
                // BigInteger.Pow() can only take an Int32 exponent anyway.
                if (BigInteger.Remainder(BigInteger.Pow(10,dec_scale), As_Pair.Denominator) == 0)
                {
                    return dec_scale;
                }
            }
            // If somehow the Denominator can be big enough that we'd actually get here.
            throw new NotImplementedException();
        }
    }

    // Muldis.D.Ref_Eng.Core.MD_Fraction_Pair
    // Represents a numerator/denominator pair.

    internal class MD_Fraction_Pair
    {
        // The Numerator+Denominator field pair can represent any
        // MD_Fraction value at all, such that the Fraction's value is
        // defined as the fractional division of Numerator by Denominator.
        // Denominator may never be zero; otherwise, the pair must always
        // be normalized at least such that the Denominator is positive.
        internal BigInteger Numerator { get; set; }
        internal BigInteger Denominator { get; set; }

        // This is true iff we know that the Numerator and
        // Denominator are coprime (their greatest common divisor is 1);
        // this is false iff we know that they are not coprime.
        // While the pair typically need to be coprime in order to reliably
        // determine if 2 MD_Fraction represent the same Muldis D value,
        // we don't necessarily store them that way for efficiency sake.
        internal Nullable<Boolean> Cached_Is_Coprime { get; set; }

        // This is true iff we know that the MD_Fraction value can be
        // represented as a terminating decimal number, meaning that the
        // prime factorization of the MD_Fraction's coprime Denominator
        // consists only of 2s and 5s; this is false iff we know that the
        // MD_Fraction value would be an endlessly repeating decimal
        // number, meaning that the MD_Fraction's coprime Denominator has
        // at least 1 prime factor that is not a 2 or a 5.
        // The identity serialization of a MD_Fraction uses a single decimal
        // number iff it would terminate and a coprime integer pair otherwise.
        // This field may be true even if Cached_Is_Coprime isn't because
        // this MD_Fraction_Pair was derived from a Decimal.
        internal Nullable<Boolean> Cached_Is_Terminating_Decimal { get; set; }

        // Iff this field is defined, we ensure that both the current
        // MD_Fraction_Struct has a Denominator equal to it, and also that
        // any other MD_Fraction_Struct derived from it has the same
        // Denominator as well, iff the MD_Fraction value can be exactly
        // represented by such a MD_Fraction_Struct.
        // Having this field defined tends to suppress automatic efforts to
        // normalize the MD_Fraction_Struct to a coprime state.
        // The main purpose of this field is to aid in system performance
        // when a lot of math, particularly addition and subtraction, is
        // done with rationals having a common conceptual fixed precision,
        // so that the performance is then closer to integer math.
        internal Nullable<BigInteger> Denominator_Affinity { get; set; }
    }

    // Muldis.D.Ref_Eng.Core.MD_Text_Struct
    // When a Muldis.D.Ref_Eng.Core.MD_Any is representing a MD_Text,
    // an MD_Text_Struct is used by it to hold the MD_Text-specific details.

    internal class MD_Text_Struct
    {
        // Represents a Muldis D Text value where each member value is
        // a Muldis D Integer in the range {0..0xD7FF,0xE000..0x10FFFF}.
        // A .Net String is the simplest storage representation for that
        // type which doesn't internally use trees for sharing or multipliers.
        // This is the canonical storage type for a regular character string.
        // Each logical member represents a single Unicode standard character
        // codepoint from either the Basic Multilingual Plane (BMP), which
        // is those member values in the range {0..0xD7FF,0xE000..0xFFFF},
        // or from either of the 16 supplementary planes, which is those
        // member values in the range {0x10000..0x10FFFF}.
        // Codepoint_Members is represented using a standard .Net
        // String value for simplicity but a String has a different native
        // concept of components; it is formally an array of .Net Char
        // each of which is either a whole BMP codepoint or half of a
        // non-BMP codepoint; a non-BMP codepoint is represented by a pair
        // of consecutive Char with numeric values in {0xD800..0xDFFF};
        // therefore, the native "length" of a String only matches the
        // "length" of the Muldis D Text when all codepoints are in the BMP.
        // While it is possible for a .Net String to contain an isolated
        // "surrogate" Char outside of a proper "surrogate pair", both
        // Muldis.DBP and Muldis.D.Ref_Eng forbid such a malformed String
        // from either being used internally or being passed in by the API.
        internal String Codepoint_Members { get; set; }

        // Nullable Boolean
        // This is true iff we know that at least 1 Codepoint member is NOT
        // in the Basic Multilingual Plane (BMP); this is false iff we know
        // that there is no such Codepoint member.  That is, with respect
        // to a .Net String, this is true iff we know the String has at least
        // 1 "surrogate pair".  We cache this knowledge because a .Net String
        // with any non-BMP Char is more complicated to count the members
        // of or access members by ordinal position or do some other stuff.
        // This field is always defined as a side-effect of Muldis.D.Ref_Eng
        // forbidding a malformed String and so they are always tested at
        // the borders, that test also revealing if a String has non-BMP chars.
        internal Boolean Has_Any_Non_BMP { get; set; }

        // Cached count of codepoint members of the Muldis D Text.
        internal Nullable<Int64> Cached_Member_Count { get; set; }
    }

    // Muldis.D.Ref_Eng.Core.Symbolic_Array_Type
    // Enumerates the various ways that a MD_Array collection can be defined
    // symbolically in terms of other collections.
    // None means the collection simply has zero members.

    internal enum Symbolic_Array_Type
    {
        None,
        Arrayed,
        Catenated,
    }

    // Muldis.D.Ref_Eng.Core.MD_Array_Struct
    // TODO: Refactor MD_Array_Struct to be more like MD_Bag_Struct.
    // When a Muldis.D.Ref_Eng.Core.MD_Any is representing a MD_Array,
    // a MD_Array_Struct is used by it to hold the MD_Array-specific details.
    // It takes the form of a tree of its own kind to aid in reusability
    // of common substrings of members of distinct MD_Array values;
    // the actual members of the MD_Array value are, in order, any members
    // specified by Pred_Members, then any Local_*_Members, then
    // Succ_Members.  A MD_Array_Struct also uses run-length encoding to
    // optimize storage, such that the actual "local" members of a node are
    // defined as the sequence in Local_*_Members repeated the number of
    // times specified by Local_Multiplicity.
    // The "tree" is actually a uni-directional graph as multiple nodes can
    // cite the same other conceptually immutable nodes as their children.

    internal class MD_Array_Struct
    {
        // LST determines how to interpret most of the other fields.
        // Iff LST is None, this node is explicitly a leaf node defining zero members.
        // Iff LST is Arrayed, Local_Arrayed_Members, combined with
        // Local_Multiplicity, defines all of the Array members.
        // Iff LST is Catenated, this Array's members are defined as
        // the catenation of Pred_Members and Succ_Members.
        // TODO: Refactor MD_Array_Struct to be more like MD_Bag_Struct.
        internal Symbolic_Array_Type Local_Symbolic_Type { get; set; }

        // Iff this is zero, then there are zero "local" members;
        // iff this is 1, then the "local" members are as Local_*_Members
        // specifies with no repeats;
        // iff this is >1, then the local members have that many occurrances.
        internal Int64 Local_Multiplicity { get; set; }

        // This field is used iff LST is Arrayed.
        // A List<MD_Any> is the simplest storage representation for that
        // type which doesn't internally use trees for sharing or multipliers.
        internal List<MD_Any> Local_Arrayed_Members { get; set; }

        // This field is used iff LST is Catenated.
        internal MD_Array_Struct Pred_Members { get; set; }

        // This field is used iff LST is Catenated.
        internal MD_Array_Struct Succ_Members { get; set; }

        // A cache of calculations about this Bag's members.
        internal Cached_Members_Meta Cached_Members_Meta { get; set; }
    }

    // Muldis.D.Ref_Eng.Core.Symbolic_Bag_Type
    // Enumerates the various ways that a MD_Bag collection can be defined
    // symbolically in terms of other collections.
    // None means the collection simply has zero members.

    internal enum Symbolic_Bag_Type
    {
        None,
        Singular,
        Arrayed,
        Indexed,
        Unique,
        Insert_N,
        Remove_N,
        Member_Plus,
        Except,
        Intersect,
        Union,
        Exclusive,
    }

    // Muldis.D.Ref_Eng.Core.Multiplied_Member
    // Represents a multiset of 1..N members of a collection where every
    // member is the same Muldis D value.

    internal class Multiplied_Member
    {
        // The Muldis D value that every member of this multiset is.
        internal MD_Any Member { get; set; }

        // The count of members of this multiset.
        internal Int64 Multiplicity { get; set; }

        internal Multiplied_Member(MD_Any member, Int64 multiplicity = 1)
        {
            Member       = member;
            Multiplicity = multiplicity;
        }

        internal Multiplied_Member Clone()
        {
            return (Multiplied_Member)this.MemberwiseClone();
        }
    }

    // Muldis.D.Ref_Eng.Core.MD_Bag_Struct
    // When a Muldis.D.Ref_Eng.Core.MD_Any is representing a MD_Bag,
    // a MD_Bag_Struct is used by it to hold the MD_Bag-specific details.
    // Also used for MD_Set, so any MD_Bag reference generally should be
    // read as either MD_Set or MD_Bag.
    // The "tree" is actually a uni-directional graph as multiple nodes can
    // cite the same other conceptually immutable nodes as their children.
    // Note that MD_Bag is the most complicated Muldis D Foundation type to
    // implement while at the same time is the least critical to the
    // internals; it is mainly just used for user data.

    internal class MD_Bag_Struct
    {
        // LST determines how to interpret most of the other fields.
        // Iff LST is None, this node is explicitly a leaf node defining zero members.
        // Iff LST is Singular, Local_Singular_Members defines all of the Bag members.
        // Iff LST is Arrayed, Local_Arrayed_Members defines all of the Bag members.
        // Iff LST is Indexed, Local_Indexed_Members defines all of the Bag members.
        // Iff LST is Unique, this Bag's members are defined as
        // the unique members of Primary_Arg.
        // Iff LST is Insert_N, this Bag's members are defined as
        // the multiset sum of Primary_Arg and Local_Singular_Members.
        // Iff LST is Remove_N, this Bag's members are defined as
        // the multiset difference of Primary_Arg and Local_Singular_Members.
        // Iff LST is Member_Plus, this Bag's members are defined as
        // the multiset sum of Primary_Arg and Extra_Arg.
        // Iff LST is Except, this Bag's members are defined as
        // the multiset difference of Primary_Arg and Extra_Arg.
        // Iff LST is Intersect, this Bag's members are defined as
        // the multiset intersection of Primary_Arg and Extra_Arg.
        // Iff LST is Union, this Bag's members are defined as
        // the multiset union of Primary_Arg and Extra_Arg.
        // Iff LST is Exclusive, this Bag's members are defined as
        // the multiset symmetric difference of Primary_Arg and Extra_Arg.
        internal Symbolic_Bag_Type Local_Symbolic_Type { get; set; }

        // This field is used iff LST is one of {Singular, Insert_N, Remove_N}.
        internal Multiplied_Member Local_Singular_Members { get; set; }

        // This field is used iff LST is Arrayed.
        internal List<Multiplied_Member> Local_Arrayed_Members { get; set; }

        // This field is used iff LST is Indexed.
        // The Dictionary has one key-asset pair for each distinct Muldis D
        // "value", all of which are indexed by Cached_MD_Any_Identity.
        internal Dictionary<MD_Any,Multiplied_Member>
            Local_Indexed_Members { get; set; }

        // This field is used iff LST is one of {Unique, Insert_N, Remove_N,
        // Member_Plus, Except, Intersect, Union, Exclusive}.
        internal MD_Bag_Struct Primary_Arg { get; set; }

        // This field is used iff LST is one of
        // {Member_Plus, Except, Intersect, Union, Exclusive}.
        internal MD_Bag_Struct Extra_Arg { get; set; }

        // A cache of calculations about this Bag's members.
        internal Cached_Members_Meta Cached_Members_Meta { get; set; }
    }

    // Muldis.D.Ref_Eng.Core.Cached_Members_Meta
    // Represents cached metadata for the members of a Muldis D
    // "discrete homogeneous" collection such as an Array or Bag,
    // particularly a collection implemented as a tree of nodes,
    // where each node may define members locally or refer to child nodes.

    internal class Cached_Members_Meta
    {
        // Nullable Integer
        // Cached count of members of the Muldis D Array/Bag represented by
        // this tree node including those defined by it and child nodes.
        internal Nullable<Int64> Tree_Member_Count { get; set; }

        // Nullable Boolean
        // This is true iff we know that no 2 members of the Muldis D
        // Array/Bag represented by this tree node (including child nodes)
        // are the same value, and false iff we know that at least 2
        // members are the same value.
        internal Nullable<Boolean> Tree_All_Unique { get; set; }

        // Nullable Boolean
        // This is true iff we know that the Muldis D Array/Bag represented
        // by this tree node (as with Tree_All_Unique) has no member that
        // is not a Muldis D Tuple and has no 2 members that do not have
        // the same heading; this is false iff we know that any member is
        // not a Tuple or that any 2 members do not have the same heading.
        internal Nullable<Boolean> Tree_Relational { get; set; }

        // Nullable Integer
        // Cached count of members defined by the Local_*_Members fields as
        // they are defined in isolation.
        internal Nullable<Int64> Local_Member_Count { get; set; }

        // Nullable Boolean
        // This is true iff we know that no 2 members of the Muldis D Array/Bag
        // represented by this tree node (specifically the Local_*_Members
        // fields as they are defined in isolation) are the same value, and
        // false iff we know that at least 2 members are the same value.
        internal Nullable<Boolean> Local_All_Unique { get; set; }

        // Nullable Boolean
        // This is true iff we know that the Muldis D Array/Bag represented
        // by this tree node (as with Local_All_Unique) has no member that
        // is not a Muldis D Tuple and has no 2 members that do not have
        // the same heading; this is false iff we know that any member is
        // not a Tuple or that any 2 members do not have the same heading.
        internal Nullable<Boolean> Local_Relational { get; set; }
    }

    // Muldis.D.Ref_Eng.Core.MD_Tuple_Struct
    // When a Muldis.D.Ref_Eng.Core.MD_Any is representing a MD_Tuple,
    // a MD_Tuple_Struct is used by it to hold the MD_Tuple-specific details.
    // Also used for MD_Excuse.
    // For efficiency, a few most-commonly used Muldis D Tuple attribute
    // names have their own corresponding MD_Tuple_Struct fields, while the
    // long tail of remaining possible but less often used names don't.
    // For example, all routine argument lists of degree 0..3 with just
    // conceptually-ordered arguments are fully covered with said few,
    // including nearly all routines of the Muldis D Standard Library.

    internal class MD_Tuple_Struct
    {
        // Count of attributes of the Muldis D Tuple.
        // This can be calculated from other fields, but is always defined.
        internal Int32 Degree { get; set; }

        // Iff Muldis D Tuple has attr named [0], this field has its asset.
        // This is the canonical name of a first conceptually-ordered attr.
        internal MD_Any A0 { get; set; }

        // Iff Muldis D Tuple has attr named [1], this field has its asset.
        // This is the canonical name of a second conceptually-ordered attr.
        internal MD_Any A1 { get; set; }

        // Iff Muldis D Tuple has attr named [2], this field has its asset.
        // This is the canonical name of a third conceptually-ordered attr.
        internal MD_Any A2 { get; set; }

        // Iff Muldis D Tuple has exactly 1 attribute with some other name
        // than the [N] ones handled above, this other attr is represented
        // by this field as a of name-asset pair.
        internal Nullable<KeyValuePair<String,MD_Any>> Only_OA { get; set; }

        // Iff Muldis D Tuple has at least 2 attributes with some other name
        // than the [N] ones handled above, those other attrs are represented
        // by this field as a set of name-asset pairs.
        internal Dictionary<String,MD_Any> Multi_OA { get; set; }
    }

    // Muldis.D.Ref_Eng.Core.MD_Capsule_Struct
    // When a Muldis.D.Ref_Eng.Core.MD_Any is representing a MD_Capsule,
    // a MD_Capsule_Struct is used by it to hold the MD_Capsule-specific details.

    internal class MD_Capsule_Struct
    {
        // The Muldis D value that is the "label" of this MD_Capsule value.
        internal MD_Any Label { get; set; }

        // The Muldis D value that is the "attributes" of this MD_Capsule value.
        internal MD_Any Attrs { get; set; }
    }
}
