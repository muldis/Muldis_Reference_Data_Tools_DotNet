namespace Muldis.Data_Engine_Reference;

public sealed class MDER_Article : MDER_Any
{
    // The virtual machine that this "value" lives in.
    private readonly MDER_Machine __machine;

    // The Muldis Data Language value that is the "label" of this MDER_Article value.
    // TODO: Change this to a MDER_Nesting.
    internal readonly MDER_Any label;

    // The Muldis Data Language value that is the "attributes" of this MDER_Article value.
    internal readonly MDER_Tuple attrs;

    internal MDER_Article(MDER_Machine machine, MDER_Any label, MDER_Tuple attrs)
    {
        this.__machine = machine;
        this.label = label;
        this.attrs = attrs;
    }

    public override MDER_Machine machine()
    {
        return this.__machine;
    }

    internal override String _as_MUON_Plain_Text_artifact(String indent)
    {
        // TODO: Change Article so represented as Nesting+Kit pair.
        return "(Article:("
            + this.label._as_MUON_Plain_Text_artifact(indent)
            + " : "
            + this.attrs._as_MUON_Plain_Text_artifact(indent)
            + "))";
    }

    internal Boolean _MDER_Article__same(MDER_Article topic_1)
    {
        MDER_Article topic_0 = this;
        return topic_0.label._MDER_Any__same(topic_1.label)
            && topic_0.attrs._MDER_Any__same(topic_1.attrs);
    }
}
