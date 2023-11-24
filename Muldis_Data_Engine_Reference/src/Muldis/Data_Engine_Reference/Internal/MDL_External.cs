using System;

namespace Muldis.Data_Engine_Reference.Internal;

internal class MDL_External : MDL_NQA
{
    internal readonly Object external_value;

    internal MDL_External(Memory memory, Object external_value)
        : base(memory, Well_Known_Base_Type.MDL_External)
    {
        this.external_value = external_value;
    }
}