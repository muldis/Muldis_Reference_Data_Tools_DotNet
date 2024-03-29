using System.Collections;

namespace Muldis.Data_Engine_Reference;

public sealed class MDER_Bits : MDER_Any
{
    // The MDER_Machine VM that this MDER_Any "value" lives in.
    private readonly MDER_Machine __machine;

    // Surrogate identity for this MDER_Any with a simpler representation.
    private String? __cached_identity_as_String;

    // A value of the .NET class BitArray is mutable.
    // It should be cloned as needed for protection of our internals.
    private readonly BitArray __bit_members_as_BitArray;

    internal MDER_Bits(MDER_Machine machine,
        BitArray bit_members_as_BitArray)
    {
        this.__machine = machine;
        this.__bit_members_as_BitArray = bit_members_as_BitArray;
    }

    public override MDER_Machine machine()
    {
        return this.__machine;
    }

    internal override String _identity_as_String()
    {
        if (this.__cached_identity_as_String is null)
        {
            this.__cached_identity_as_String
                = this._as_MUON_Plain_Text_artifact("");
        }
        return this.__cached_identity_as_String;
    }

    internal override String _preview_as_String()
    {
        return this._as_MUON_Plain_Text_artifact("");
    }

    public BitArray bit_members_as_BitArray()
    {
        // A BitArray is mutable so clone to protect our internals.
        return new BitArray(this.__bit_members_as_BitArray);
    }

    internal BitArray _bit_members_as_BitArray()
    {
        return this.__bit_members_as_BitArray;
    }

    internal override String _as_MUON_Plain_Text_artifact(String indent)
    {
        return "0bb" + String.Concat(Enumerable.Select(
            this._BitArray_to_List(this.__bit_members_as_BitArray),
            m => m ? "1" : "0"));
    }

    internal Boolean _MDER_Bits__same(MDER_Bits topic_1)
    {
        MDER_Bits topic_0 = this;
        return Enumerable.SequenceEqual(
            this._BitArray_to_List(topic_0.__bit_members_as_BitArray),
            this._BitArray_to_List(topic_1.__bit_members_as_BitArray));
    }

    private List<Boolean> _BitArray_to_List(BitArray topic)
    {
        System.Collections.IEnumerator e = topic.GetEnumerator();
        List<Boolean> list = new List<Boolean>();
        while (e.MoveNext())
        {
            list.Add((Boolean)e.Current);
        }
        return list;
    }

    internal Int32 _MDER_Bits__bit_count()
    {
        return this.__bit_members_as_BitArray.Length;
    }

    internal MDER_Integer? _MDER_Bits__bit_maybe_at(Int32 ord_pos)
    {
        return (ord_pos >= this.__bit_members_as_BitArray.Length) ? null
            : this.machine().MDER_Integer(
                this.__bit_members_as_BitArray[ord_pos] ? 1 : 0);
    }
}
