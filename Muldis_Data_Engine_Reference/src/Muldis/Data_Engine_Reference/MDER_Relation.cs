namespace Muldis.Data_Engine_Reference;

public sealed class MDER_Relation : MDER_Any
{
    // The MDER_Machine VM that this MDER_Any "value" lives in.
    private readonly MDER_Machine __machine;

    // Surrogate identity for this MDER_Any with a simpler representation.
    private String? __cached_identity_as_String;

    internal readonly MDER_Heading heading;
    internal readonly MDER_Set body;

    internal MDER_Relation(MDER_Machine machine, MDER_Heading heading, MDER_Set body)
    {
        this.__machine = machine;
        this.heading = heading;
        this.body = body;
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

    internal override String _as_MUON_Plain_Text_artifact(String indent)
    {
        return "(Relation:("
            + ((MDER_Heading)this.heading)._as_MUON_Plain_Text_artifact(indent)
            + " : "
            + this.body._as_MUON_Plain_Text_artifact(indent)
            + "))";
    }

    internal Boolean _MDER_Relation__same(MDER_Relation topic_1)
    {
        MDER_Relation topic_0 = this;
        return topic_0.heading._MDER_Any__same(topic_1.heading)
            && topic_0.body._MDER_Any__same(topic_1.body);
    }
}
