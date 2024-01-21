namespace Muldis.Data_Library;

public readonly struct MDV_After_All_Others
    : MDV_Excuse, MDV_Orderable<MDV_After_All_Others>
{
    private static readonly MDV_After_All_Others __only
        = new MDV_After_All_Others();

    public override Int32 GetHashCode()
    {
        return 0;
    }

    public override String ToString()
    {
        return Internal_Preview.After_All_Others();
    }

    public override Boolean Equals(Object? obj)
    {
        return obj is MDV_After_All_Others;
    }

    public static MDV_After_All_Others only()
    {
        return MDV_After_All_Others.__only;
    }

    public static MDV_Boolean in_order(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        return MDV_Boolean.@true();
    }

    public static MDV_Boolean before(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        throw new NotImplementedException();
    }

    public static MDV_Boolean after(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        throw new NotImplementedException();
    }

    public static MDV_Boolean before_or_same(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        throw new NotImplementedException();
    }

    public static MDV_Boolean after_or_same(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        throw new NotImplementedException();
    }

    public static MDV_After_All_Others min(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        throw new NotImplementedException();
    }

    public static MDV_After_All_Others max(MDV_After_All_Others topic_0, MDV_After_All_Others topic_1)
    {
        throw new NotImplementedException();
    }
}
