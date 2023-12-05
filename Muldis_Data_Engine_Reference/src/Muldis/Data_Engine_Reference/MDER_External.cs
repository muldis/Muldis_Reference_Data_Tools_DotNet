namespace Muldis.Data_Engine_Reference;

internal class MDER_External : MDER_Any
{
    internal readonly Object external_value;

    internal MDER_External(Memory memory, Object external_value)
        : base(memory, Well_Known_Base_Type.MDER_External)
    {
        this.external_value = external_value;
    }
}
