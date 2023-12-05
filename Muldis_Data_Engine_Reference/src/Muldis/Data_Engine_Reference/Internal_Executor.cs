using System.Collections;
using System.Numerics;

namespace Muldis.Data_Engine_Reference;

internal class Executor
{
    internal readonly Memory memory;

    internal readonly Standard_Generator standard_generator;

    internal Executor(Memory memory)
    {
        this.memory = memory;

        this.standard_generator = new Standard_Generator();
    }

    internal MDL_Any Evaluates(MDL_Any function, MDL_Any args = null)
    {
        Memory m = this.memory;
        if (args is null)
        {
            args = m.MDL_Tuple_D0;
        }
        if (args.WKBT != Well_Known_Base_Type.MDL_Tuple)
        {
            throw new ArgumentException(m.Simple_MDL_Excuse(
                "X_Args_Not_Tuple").ToString());
        }
        MDL_Tuple args_as_Tuple = (MDL_Tuple)args;
        if (Is_Absolute_Name(function))
        {
            if (Object.ReferenceEquals(
                Array__maybe_at((MDL_Array)function, 0), m.MDL_Attr_Name("foundation")))
            {
                return evaluate_foundation_function(
                    (MDL_Text)Array__maybe_at((MDL_Array)function, 1), args_as_Tuple);
            }
            throw new NotImplementedException();
        }
        if (Is_Function_Call(function))
        {
            throw new NotImplementedException();
        }
        if (Is_Function(function))
        {
            throw new NotImplementedException();
        }
        if (Is_Package(function))
        {
            throw new NotImplementedException();
        }
        throw new ArgumentException(m.Simple_MDL_Excuse(
            "X_Malformed_Function_Call").ToString());
    }

    internal void Performs(MDL_Any procedure, MDL_Any args = null)
    {
        Memory m = this.memory;
        if (args is null)
        {
            args = m.MDL_Tuple_D0;
        }
        if (args.WKBT != Well_Known_Base_Type.MDL_Tuple)
        {
            throw new ArgumentException(m.Simple_MDL_Excuse("X_Args_Not_Tuple").ToString());
        }
        MDL_Tuple args_as_Tuple = (MDL_Tuple)args;
        if (Is_Absolute_Name(procedure))
        {
            throw new NotImplementedException();
        }
        if (Is_Procedure_Call(procedure))
        {
            throw new NotImplementedException();
        }
        if (Is_Procedure(procedure))
        {
            throw new NotImplementedException();
        }
        if (Is_Package(procedure))
        {
            throw new NotImplementedException();
        }
        throw new ArgumentException(m.Simple_MDL_Excuse(
            "X_Malformed_Procedure_Call").ToString());
    }

    internal Boolean Is_Excuse(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Fraction(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Bits(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Blob(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Text(MDL_Any value)
    {
        return value.WKBT == Well_Known_Base_Type.MDL_Text;
    }

    internal Boolean Is_String(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Set(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Tuple_Array(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Relation(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Tuple_Bag(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Interval(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Package(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Function(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Procedure(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Function_Call(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Procedure_Call(MDL_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Heading(MDL_Any value)
    {
        return value.WKBT == Well_Known_Base_Type.MDL_Heading;
    }

    internal Boolean Is_Attr_Name_List(MDL_Any value)
    {
        Memory m = this.memory;
        if (value.WKBT != Well_Known_Base_Type.MDL_Array)
        {
            return false;
        }
        // TODO: Replace this loop with an "All" higher order function call.
        Int64 count = Array__count((MDL_Array)value);
        for (Int64 i = 0; i < count; i++)
        {
            if (!Is_Text(Array__maybe_at((MDL_Array)value, i)))
            {
                return false;
            }
        }
        return true;
    }

    internal Boolean Is_Local_Name(MDL_Any value)
    {
        if (!Is_Attr_Name_List(value))
        {
            return false;
        }
        Int64 count = Array__count((MDL_Array)value);
        if (count == 0)
        {
            return false;
        }
        MDL_Any first = Array__maybe_at((MDL_Array)value, 0);
        if ((Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("foundation")) && count == 2)
            || (Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("used")) && count >= 2)
            || (Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("package")) && count >= 1)
            || (Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("folder")) && count >= 1)
            || (Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("material")) && count == 1)
            || (Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("floating")) && count >= 2))
        {
            return true;
        }
        return false;
    }

    internal Boolean Is_Absolute_Name(MDL_Any value)
    {
        if (!Is_Local_Name(value))
        {
            return false;
        }
        MDL_Any first = Array__maybe_at((MDL_Array)value, 0);
        if (Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("foundation"))
            || Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("used"))
            || Object.ReferenceEquals(first, this.memory.MDL_Attr_Name("package")))
        {
            return true;
        }
        return false;
    }

    internal MDL_Any evaluate_foundation_function(MDL_Text func_name, MDL_Tuple args)
    {
        Memory m = this.memory;
        String func_name_s = func_name.code_point_members;

        // TYPE DEFINERS

        if (Constants.Strings__Foundation_Type_Definer_Function_Names().Contains(func_name_s))
        {
            if (!m.Tuple__Has_Heading(args, m.MDL_Heading_0))
            {
                throw new ArgumentException(m.Simple_MDL_Excuse(
                    "X_Type_Definer_Function_Args_Not_MDL_Heading_0").ToString());
            }
            MDL_Any v = args.attrs["\u0000"];
            switch (func_name_s)
            {
                case "Any":
                    return m.MDL_True();
                case "None":
                    return m.MDL_False();

                // FOUNDATION BASE TYPE DEFINERS

                case "Ignorance":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Ignorance
                    );
                case "Boolean":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_False
                            || v.WKBT == Well_Known_Base_Type.MDL_True
                    );
                case "Integer":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Integer
                    );
                case "Array":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Array
                    );
                case "Bag":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Bag
                    );
                case "Tuple":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Tuple
                    );
                case "Article":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Article
                    );
                case "Handle":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Variable
                            || v.WKBT == Well_Known_Base_Type.MDL_Process
                            || v.WKBT == Well_Known_Base_Type.MDL_Stream
                            || v.WKBT == Well_Known_Base_Type.MDL_External
                    );

                // HANDLE SUBTYPE DEFINERS

                case "Variable":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Variable
                    );
                case "Process":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Process
                    );
                case "Stream":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Stream
                    );
                case "External":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_External
                    );

                // ARRAY SUBTYPE DEFINERS

                // TUPLE SUBTYPE DEFINERS

                // ARTICLE SUBTYPE DEFINERS

                case "Blob":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Blob
                    );

                case "Text":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Text
                    );

                case "Excuse":
                    return m.MDL_Boolean(
                        v.WKBT == Well_Known_Base_Type.MDL_Excuse
                    );

                default:
                    throw new NotImplementedException();
            }
        }

        // NON TYPE DEFINER UNARY FUNCTIONS

        if (Constants.Strings__Foundation_NTD_Unary_Function_Names().Contains(func_name_s))
        {
            if (!m.Tuple__Has_Heading(args, m.MDL_Heading_0))
            {
                throw new ArgumentException(m.Simple_MDL_Excuse(
                    "X_Non_Type_Definer_Unary_Function_Args_Not_MDL_Heading_0").ToString());
            }
            MDL_Any v = args.attrs["\u0000"];
            switch (func_name_s)
            {
                case "Integer_opposite":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Integer)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Integer_opposite_Arg_0_Not_Integer").ToString());
                    }
                    return m.MDL_Integer(-((MDL_Integer)v).as_BigInteger);
                case "Integer_modulus":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Integer)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Integer_modulus_Arg_0_Not_Integer").ToString());
                    }
                    return m.MDL_Integer(BigInteger.Abs(((MDL_Integer)v).as_BigInteger));
                case "Integer_factorial":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Integer
                        || ((MDL_Integer)v).as_BigInteger < 0)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Integer_factorial_Arg_0_Not_Integer_NN").ToString());
                    }
                    // Note that System.Numerics.BigInteger doesn't come
                    // with a Factorial(n) so we have to do it ourselves.
                    return m.MDL_Integer(Integer__factorial(((MDL_Integer)v).as_BigInteger));
                case "Array_count":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Array)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Array_count_Arg_0_Not_Array").ToString());
                    }
                    return m.MDL_Integer(Array__count((MDL_Array)v));
                case "Bag_unique":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Bag)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Bag_unique_Arg_0_Not_Bag").ToString());
                    }
                    return new MDL_Bag(m,
                        new MDL_Bag_Struct {
                            local_symbolic_type = Symbolic_Bag_Type.Unique,
                            members = ((MDL_Bag)v).tree_root_node,
                            cached_members_meta = new Cached_Members_Meta {
                                tree_all_unique = true,
                            },
                        }
                    );
                case "Tuple_degree":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Tuple)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Tuple_degree_Arg_0_Not_Tuple").ToString());
                    }
                    return m.MDL_Integer(((MDL_Tuple)v).attrs.Count);
                case "Tuple_heading":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Tuple)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Tuple_heading_Arg_0_Not_Tuple").ToString());
                    }
                    return m.Tuple__Heading((MDL_Tuple)v);
                case "Text_from_UTF_8_Blob":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Blob)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Text_from_UTF_8_Blob_Arg_0_Not_Blob").ToString());
                    }
                    return m.MDL_Text_from_UTF_8_MDL_Blob((MDL_Blob)v);
                case "MDPT_Parsing_Unit_Text_to_Any":
                    if (v.WKBT != Well_Known_Base_Type.MDL_Text)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_MDPT_Parsing_Unit_Text_to_Any_Arg_0_Not_Text").ToString());
                    }
                    // TODO: Everything.
                    return m.MDL_Ignorance();
                default:
                    throw new NotImplementedException();
            }
        }

        // BINARY FUNCTIONS

        if (Constants.Strings__Foundation_Binary_Function_Names().Contains(func_name_s))
        {
            if (!m.Tuple__Has_Heading(args, m.MDL_Heading_0_1))
            {
                throw new ArgumentException(m.Simple_MDL_Excuse(
                    "X_Binary_Function_Args_Not_MDL_Heading_0_1").ToString());
            }
            MDL_Any a0 = args.attrs["\u0000"];
            MDL_Any a1 = args.attrs["\u0001"];
            switch (func_name_s)
            {
                case "same":
                    return m.MDL_Boolean(Any__same(a0, a1));
                case "Integer_plus":
                    if (a0.WKBT != Well_Known_Base_Type.MDL_Integer)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Integer_plus_Arg_0_Not_Integer").ToString());
                    }
                    if (a1.WKBT != Well_Known_Base_Type.MDL_Integer)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Integer_plus_Arg_1_Not_Integer").ToString());
                    }
                    return m.MDL_Integer(((MDL_Integer)a0).as_BigInteger + ((MDL_Integer)a1).as_BigInteger);
                case "Array_at":
                    if (a0.WKBT != Well_Known_Base_Type.MDL_Array)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Array_at_Arg_0_Not_Array").ToString());
                    }
                    if (a1.WKBT != Well_Known_Base_Type.MDL_Integer
                        || ((MDL_Integer)a1).as_BigInteger < 0)
                    {
                        throw new ArgumentException(m.Simple_MDL_Excuse(
                            "X_Array_at_Arg_1_Not_Integer_NN").ToString());
                    }
                    MDL_Any maybe_member = Array__maybe_at((MDL_Array)a0, (Int64)((MDL_Integer)a1).as_BigInteger);
                    if (maybe_member is null)
                    {
                        throw new ArgumentException(
                            m.well_known_excuses["No_Such_Ord_Pos"].ToString());
                    }
                    return maybe_member;
                default:
                    throw new NotImplementedException();
            }
        }

        // TERNARY FUNCTIONS

        if (Constants.Strings__Foundation_Ternary_Function_Names().Contains(func_name_s))
        {
            if (!m.Tuple__Has_Heading(args, m.MDL_Heading_0_1_2))
            {
                throw new ArgumentException(m.Simple_MDL_Excuse(
                    "X_Ternary_Function_Args_Not_MDL_Heading_0_1_2").ToString());
            }
            MDL_Any v = args.attrs["\u0000"];
            switch (func_name_s)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        throw new NotImplementedException();
    }

    internal Boolean Any__same(MDL_Any a0, MDL_Any a1)
    {
        if (Object.ReferenceEquals(a0, a1))
        {
            // We should always get here for any singleton well known base types:
            // MDL_Ignorance, MDL_False, MDL_True.
            return true;
        }
        if (a0.WKBT != a1.WKBT)
        {
            return false;
        }
        if (a0.cached_MDL_Any_identity is not null
            && a1.cached_MDL_Any_identity is not null)
        {
            if (String.Equals(a0.cached_MDL_Any_identity, a1.cached_MDL_Any_identity))
            {
                merge_two_same(a0, a1);
                return true;
            }
            return false;
        }
        Boolean result;
        switch (a0.WKBT)
        {
            case Well_Known_Base_Type.MDL_Ignorance:
            case Well_Known_Base_Type.MDL_False:
            case Well_Known_Base_Type.MDL_True:
                // We should never get here.
                throw new InvalidOperationException();
            case Well_Known_Base_Type.MDL_Integer:
                result = (((MDL_Integer)a0).as_BigInteger == ((MDL_Integer)a1).as_BigInteger);
                break;
            case Well_Known_Base_Type.MDL_Fraction:
                MDL_Fraction fa0 = (MDL_Fraction)a0;
                MDL_Fraction fa1 = (MDL_Fraction)a1;
                if (fa0.maybe_as_Decimal() is not null && fa1.maybe_as_Decimal() is not null)
                {
                    result = (fa0.maybe_as_Decimal() == fa1.maybe_as_Decimal());
                    break;
                }
                if (fa0.denominator() == fa1.denominator())
                {
                    result = (fa0.numerator() == fa1.numerator());
                    break;
                }
                fa0.ensure_coprime();
                fa1.ensure_coprime();
                result = (fa0.denominator() == fa1.denominator()
                    && fa0.numerator() == fa1.numerator());
                break;
            case Well_Known_Base_Type.MDL_Bits:
                result = Enumerable.SequenceEqual(
                    BitArray_to_List(((MDL_Bits)a0).bit_members),
                    BitArray_to_List(((MDL_Bits)a1).bit_members));
                break;
            case Well_Known_Base_Type.MDL_Blob:
                result = Enumerable.SequenceEqual(
                    ((MDL_Blob)a0).octet_members,
                    ((MDL_Blob)a1).octet_members);
                break;
            case Well_Known_Base_Type.MDL_Text:
                result = String.Equals(((MDL_Text)a0).code_point_members,
                    ((MDL_Text)a1).code_point_members);
                break;
            case Well_Known_Base_Type.MDL_Array:
                this.memory.Array__Collapse((MDL_Array)a0);
                this.memory.Array__Collapse((MDL_Array)a1);
                result = this.collapsed_Array_Struct_same(
                    ((MDL_Array)a0).tree_root_node, ((MDL_Array)a0).tree_root_node);
                break;
            case Well_Known_Base_Type.MDL_Set:
                // MDL_Set and MDL_Bag have the same internal representation.
                this.memory.Set__Collapse(set: ((MDL_Set)a0), want_indexed: true);
                this.memory.Set__Collapse(set: ((MDL_Set)a1), want_indexed: true);
                result = this.collapsed_Bag_Struct_same(
                    ((MDL_Set)a0).tree_root_node, ((MDL_Set)a0).tree_root_node);
                break;
            case Well_Known_Base_Type.MDL_Bag:
                // MDL_Set and MDL_Bag have the same internal representation.
                this.memory.Bag__Collapse(bag: ((MDL_Bag)a0), want_indexed: true);
                this.memory.Bag__Collapse(bag: ((MDL_Bag)a1), want_indexed: true);
                result = this.collapsed_Bag_Struct_same(
                    ((MDL_Set)a0).tree_root_node, ((MDL_Set)a0).tree_root_node);
                break;
            case Well_Known_Base_Type.MDL_Heading:
                HashSet<String> atnms0 = ((MDL_Heading)a0).attr_names;
                HashSet<String> atnms1 = ((MDL_Heading)a1).attr_names;
                return (atnms0.Count == atnms1.Count)
                    && Enumerable.All(atnms0, atnm => atnms1.Contains(atnm));
            case Well_Known_Base_Type.MDL_Tuple:
                Dictionary<String, MDL_Any> attrs0 = ((MDL_Tuple)a0).attrs;
                Dictionary<String, MDL_Any> attrs1 = ((MDL_Tuple)a1).attrs;
                // First test just that the Tuple headings are the same,
                // and only if they are, compare the attribute values.
                return (attrs0.Count == attrs1.Count)
                    && Enumerable.All(attrs0, attr => attrs1.ContainsKey(attr.Key))
                    && Enumerable.All(attrs0, attr => Any__same(attr.Value, attrs1[attr.Key]));
            case Well_Known_Base_Type.MDL_Tuple_Array:
                result = Any__same(((MDL_Tuple_Array)a0).heading, ((MDL_Tuple_Array)a1).heading)
                      && Any__same(((MDL_Tuple_Array)a0).body, ((MDL_Tuple_Array)a1).body);
                break;
            case Well_Known_Base_Type.MDL_Relation:
                result = Any__same(((MDL_Relation)a0).heading, ((MDL_Relation)a1).heading)
                      && Any__same(((MDL_Relation)a0).body, ((MDL_Relation)a1).body);
                break;
            case Well_Known_Base_Type.MDL_Tuple_Bag:
                result = Any__same(((MDL_Tuple_Bag)a0).heading, ((MDL_Tuple_Bag)a1).heading)
                      && Any__same(((MDL_Tuple_Bag)a0).body, ((MDL_Tuple_Bag)a1).body);
                break;
            case Well_Known_Base_Type.MDL_Article:
                result = Any__same(((MDL_Article)a0).label, ((MDL_Article)a1).label)
                      && Any__same(((MDL_Article)a0).attrs, ((MDL_Article)a1).attrs);
                break;
            case Well_Known_Base_Type.MDL_Excuse:
                result = Any__same(((MDL_Excuse)a0).label, ((MDL_Excuse)a1).label)
                      && Any__same(((MDL_Excuse)a0).attrs, ((MDL_Excuse)a1).attrs);
                break;
            case Well_Known_Base_Type.MDL_Variable:
            case Well_Known_Base_Type.MDL_Process:
            case Well_Known_Base_Type.MDL_Stream:
            case Well_Known_Base_Type.MDL_External:
                // Every Muldis Data Language Handle object is always distinct from every other one.
                return false;
            default:
                throw new NotImplementedException();
        }
        if (result)
        {
            merge_two_same(a0, a1);
        }
        return result;
    }

    private List<Boolean> BitArray_to_List(BitArray value)
    {
        System.Collections.IEnumerator e = value.GetEnumerator();
        List<Boolean> list = new List<Boolean>();
        while (e.MoveNext())
        {
            list.Add((Boolean)e.Current);
        }
        return list;
    }

    private Boolean collapsed_Array_Struct_same(
        MDL_Array_Struct n0, MDL_Array_Struct n1)
    {
        if (Object.ReferenceEquals(n0, n1))
        {
            return true;
        }
        if (n0.local_symbolic_type == Symbolic_Array_Type.None
            && n1.local_symbolic_type == Symbolic_Array_Type.None)
        {
            // In theory we should never get here assuming that
            // the empty Array is optimized to return a constant
            // at selection time, but we will check anyway.
            return true;
        }
        if (n0.local_symbolic_type == Symbolic_Array_Type.Singular
            && n1.local_symbolic_type == Symbolic_Array_Type.Singular)
        {
            return ((n0.Local_Singular_Members().multiplicity
                    == n1.Local_Singular_Members().multiplicity)
                && Any__same(n0.Local_Singular_Members().member,
                    n1.Local_Singular_Members().member));
        }
        if (n0.local_symbolic_type == Symbolic_Array_Type.Arrayed
            && n1.local_symbolic_type == Symbolic_Array_Type.Arrayed)
        {
            // This works because MDL_Any Equals() calls Any__Same().
            return Enumerable.SequenceEqual(
                n0.Local_Arrayed_Members(),
                n1.Local_Arrayed_Members());
        }
        MDL_Array_Struct n0_ = n0;
        MDL_Array_Struct n1_ = n1;
        if (n0_.local_symbolic_type == Symbolic_Array_Type.Arrayed
            && n1_.local_symbolic_type == Symbolic_Array_Type.Singular)
        {
             n1_ = n0;
             n0_ = n1;
        }
        if (n0_.local_symbolic_type == Symbolic_Array_Type.Singular
            && n1_.local_symbolic_type == Symbolic_Array_Type.Arrayed)
        {
            Multiplied_Member sm = n0_.Local_Singular_Members();
            List<MDL_Any> am = n1_.Local_Arrayed_Members();
            return sm.multiplicity == am.Count
                && Enumerable.All(am, m => Any__same(m, sm.member));
        }
        // We should never get here.
        throw new NotImplementedException();
    }

    private Boolean collapsed_Bag_Struct_same(
        MDL_Bag_Struct n0, MDL_Bag_Struct n1)
    {
        if (Object.ReferenceEquals(n0, n1))
        {
            return true;
        }
        if (n0.local_symbolic_type == Symbolic_Bag_Type.None
            && n1.local_symbolic_type == Symbolic_Bag_Type.None)
        {
            // In theory we should never get here assuming that
            // the empty Array is optimized to return a constant
            // at selection time, but we will check anyway.
            return true;
        }
        if (n0.local_symbolic_type == Symbolic_Bag_Type.Indexed
            && n1.local_symbolic_type == Symbolic_Bag_Type.Indexed)
        {
            Dictionary<MDL_Any, Multiplied_Member> im0
                = n0.Local_Indexed_Members();
            Dictionary<MDL_Any, Multiplied_Member> im1
                = n1.Local_Indexed_Members();
            return im0.Count == im1.Count
                && Enumerable.All(
                    im0.Values,
                    m => im1.ContainsKey(m.member)
                        && im1[m.member].multiplicity == m.multiplicity
                );
        }
        // We should never get here.
        throw new NotImplementedException();
    }

    internal void merge_two_same(MDL_Any a0, MDL_Any a1)
    {
        // This may be run on 2 MDL_Any only if it was already
        // determined they represent the same Muldis Data Language value; typically
        // it is invoked by Any__same() when it would result in true.
            // TODO: Make a more educated decision on which one to keep.
            // It should also have logic dispatching on WKBT and
            // do struct mergers as applicable eg copying identity info.
    }

    private BigInteger Integer__factorial(BigInteger value)
    {
        // Note that System.Numerics.BigInteger doesn't come
        // with a Factorial(n) so we have to do it ourselves.
        // Note we calculate this by iteration rather than recursion in
        // order to scale better ... unless the C# compiler is smart
        // enough to optimize tail recursion anyway.
        BigInteger result = 1;
        for (Int32 i = 2; i <= value; i++)
        {
            result *= i;
        }
        return result;
    }

    internal Int64 Bits__count(MDL_Bits bits)
    {
        return bits.bit_members.Length;
    }

    internal Int64 Blob__count(MDL_Blob blob)
    {
        return blob.octet_members.Length;
    }

    internal Int64 Text__count(MDL_Text node)
    {
        if (node.cached_member_count is null)
        {
            if (node.has_any_non_BMP)
            {
                String s = node.code_point_members;
                Int32 count = 0;
                for (Int32 i = 0; i < s.Length; i++)
                {
                    count++;
                    if ((i+1) < s.Length
                        && Char.IsSurrogatePair(s[i], s[i+1]))
                    {
                        i++;
                    }
                }
                node.cached_member_count = count;
            }
            else
            {
                node.cached_member_count = node.code_point_members.Length;
            }
        }
        return (Int64)node.cached_member_count;
    }

    internal Int64 Array__count(MDL_Array array)
    {
        return Array__node__tree_member_count(array.tree_root_node);
    }

    private Int64 Array__node__tree_member_count(MDL_Array_Struct node)
    {
        if (node.cached_members_meta.tree_member_count is null)
        {
            switch (node.local_symbolic_type)
            {
                case Symbolic_Array_Type.None:
                    node.cached_members_meta.tree_member_count = 0;
                    break;
                case Symbolic_Array_Type.Singular:
                    node.cached_members_meta.tree_member_count
                        = node.Local_Singular_Members().multiplicity;
                    break;
                case Symbolic_Array_Type.Arrayed:
                    node.cached_members_meta.tree_member_count
                        = node.Local_Arrayed_Members().Count;
                    break;
                case Symbolic_Array_Type.Catenated:
                    node.cached_members_meta.tree_member_count
                        = Array__node__tree_member_count(node.Tree_Catenated_Members().a0)
                        + Array__node__tree_member_count(node.Tree_Catenated_Members().a1);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        return (Int64)node.cached_members_meta.tree_member_count;
    }

    private MDL_Integer Bits__maybe_at(MDL_Bits bits, Int64 ord_pos)
    {
        return this.memory.MDL_Integer(bits.bit_members[(Int32)ord_pos] ? 1 : 0);
    }

    private MDL_Integer Blob__maybe_at(MDL_Blob blob, Int64 ord_pos)
    {
        return this.memory.MDL_Integer(blob.octet_members[(Int32)ord_pos]);
    }

    private MDL_Integer Text__maybe_at(MDL_Text node, Int64 ord_pos)
    {
        if (node.has_any_non_BMP)
        {
            String s = node.code_point_members;
            Int64 logical_i = 0;
            for (Int32 i = 0; i < s.Length; i++)
            {
                if (logical_i == ord_pos)
                {
                    if ((i+1) < s.Length
                        && Char.IsSurrogatePair(s[i], s[i+1]))
                    {
                        return this.memory.MDL_Integer(
                            Char.ConvertToUtf32(s[i], s[i+1]));
                    }
                    return this.memory.MDL_Integer(s[i]);
                }
                logical_i++;
                if ((i+1) < s.Length
                    && Char.IsSurrogatePair(s[i], s[i+1]))
                {
                    i++;
                }
            }
        }
        return this.memory.MDL_Integer(
            node.code_point_members[(Int32)ord_pos]);
    }

    private MDL_Any Array__maybe_at(MDL_Array array, Int64 ord_pos)
    {
        return Array__node__maybe_at(array.tree_root_node, ord_pos);
    }

    private MDL_Any Array__node__maybe_at(MDL_Array_Struct node, Int64 ord_pos)
    {
        if (node.cached_members_meta.tree_member_count == 0)
        {
            return null;
        }
        Int64 maybe_last_ord_pos_seen = -1;
        switch (node.local_symbolic_type)
        {
            case Symbolic_Array_Type.None:
                return null;
            case Symbolic_Array_Type.Singular:
                if (ord_pos <= (maybe_last_ord_pos_seen + node.Local_Singular_Members().multiplicity))
                {
                    return node.Local_Singular_Members().member;
                }
                return null;
            case Symbolic_Array_Type.Arrayed:
                Int64 local_member_count = Array__node__tree_member_count(node);
                if (ord_pos <= (maybe_last_ord_pos_seen + local_member_count))
                {
                    Int64 ord_pos_within_local = ord_pos - (1 + maybe_last_ord_pos_seen);
                    return node.Local_Arrayed_Members()[(Int32)ord_pos_within_local];
                }
                return null;
            case Symbolic_Array_Type.Catenated:
                MDL_Any maybe_member = Array__node__maybe_at(node.Tree_Catenated_Members().a0, ord_pos);
                if (maybe_member is not null)
                {
                    return maybe_member;
                }
                maybe_last_ord_pos_seen += Array__node__tree_member_count(node.Tree_Catenated_Members().a0);
                if (ord_pos <= maybe_last_ord_pos_seen)
                {
                    return null;
                }
                Int64 ord_pos_within_succ = ord_pos - (1 + maybe_last_ord_pos_seen);
                return Array__node__maybe_at(node.Tree_Catenated_Members().a1, ord_pos_within_succ);
            default:
                throw new NotImplementedException();
        }
    }
}