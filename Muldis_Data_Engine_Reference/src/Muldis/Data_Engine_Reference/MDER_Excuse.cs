namespace Muldis.Data_Engine_Reference;

public sealed class MDER_Excuse : MDER_Any
{
    // The virtual machine that this "value" lives in.
    private readonly MDER_Machine __machine;

    // The Muldis Data Language value that is the "label" of this MDER_Excuse value.
    // TODO: Change this to a MDER_Nesting.
    internal readonly MDER_Any label;

    // The Muldis Data Language value that is the "attributes" of this MDER_Excuse value.
    internal readonly MDER_Tuple attrs;

    internal MDER_Excuse(MDER_Machine machine, MDER_Any label, MDER_Tuple attrs)
    {
        this.__machine = machine;
        this.label = label;
        this.attrs = attrs;
    }

    public override MDER_Machine machine()
    {
        return this.__machine;
    }

    internal String _as_MUON_Excuse_artifact(String indent)
    {
        // TODO: Change Excuse so represented as Nesting+Kit pair.
        return "(Excuse:("
            + this.label._as_MUON_Any_artifact(indent)
            + " : "
            + this.attrs._as_MUON_Any_artifact(indent)
            + "))";
    }

    internal Boolean _MDER_Excuse__same(MDER_Excuse topic_1)
    {
        MDER_Excuse topic_0 = this;
        return topic_0.label._MDER_Any__same(topic_1.label)
            && topic_0.attrs._MDER_Any__same(topic_1.attrs);
    }
}
