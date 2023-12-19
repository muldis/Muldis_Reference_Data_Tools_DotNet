namespace Muldis.Data_Engine_Reference;

public sealed class MDER_External : MDER_Any
{
    // The virtual machine that this "value" lives in.
    private readonly MDER_Machine __machine;

    internal readonly Object external_value;

    internal MDER_External(MDER_Machine machine, Object external_value)
    {
        this.__machine = machine;
        this.external_value = external_value;
    }

    public override MDER_Machine machine()
    {
        return this.__machine;
    }

    internal override String _as_MUON_Plain_Text_artifact(String indent)
    {
        // We display something useful for debugging purposes, but no
        // (transient) MDER_External can actually be rendered as MUON.
        return "`Some MDER_External value is here.`";
    }
}
