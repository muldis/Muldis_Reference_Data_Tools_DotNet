namespace Muldis.Data_Library;

public interface MDV_Bicessable<Specific_T>
    : MDV_Orderable<Specific_T>, MDV_Successable<Specific_T>
{
    // pred

    public static abstract Specific_T pred(Specific_T topic);
}
