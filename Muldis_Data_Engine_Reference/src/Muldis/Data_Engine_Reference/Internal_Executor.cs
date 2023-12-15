using System.Collections;
using System.Numerics;
using System.Text;

namespace Muldis.Data_Engine_Reference;

internal class Internal_Executor
{
    private readonly MDER_Machine __machine;

    internal Internal_Executor(MDER_Machine machine)
    {
        this.__machine = machine;
    }

    internal MDER_Machine machine()
    {
        return this.__machine;
    }

    internal MDER_Any Evaluates(MDER_Any function, MDER_Any? args = null)
    {
        if (args is null)
        {
            args = this.__machine.MDER_Tuple_D0;
        }
        if (args._WKBT() != Internal_Well_Known_Base_Type.MDER_Tuple)
        {
            throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                "X_Args_Not_Tuple").ToString());
        }
        MDER_Tuple args_as_Tuple = (MDER_Tuple)args;
        if (Is_Absolute_Name(function))
        {
            if (Object.ReferenceEquals(
                Array__maybe_at((MDER_Array)function, 0), this.__machine.MDER_Attr_Name("foundation")))
            {
                return evaluate_foundation_function(
                    (MDER_Text)Array__maybe_at((MDER_Array)function, 1)!, args_as_Tuple);
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
        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
            "X_Malformed_Function_Call").ToString());
    }

    internal void Performs(MDER_Any procedure, MDER_Any? args = null)
    {
        if (args is null)
        {
            args = this.__machine.MDER_Tuple_D0;
        }
        if (args._WKBT() != Internal_Well_Known_Base_Type.MDER_Tuple)
        {
            throw new ArgumentException(this.__machine.Simple_MDER_Excuse("X_Args_Not_Tuple").ToString());
        }
        MDER_Tuple args_as_Tuple = (MDER_Tuple)args;
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
        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
            "X_Malformed_Procedure_Call").ToString());
    }

    internal Boolean Is_Excuse(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Fraction(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Bits(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Blob(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Text(MDER_Any value)
    {
        return value._WKBT() == Internal_Well_Known_Base_Type.MDER_Text;
    }

    internal Boolean Is_String(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Set(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Tuple_Array(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Relation(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Tuple_Bag(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Interval(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Package(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Function(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Procedure(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Function_Call(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Procedure_Call(MDER_Any value)
    {
        throw new NotImplementedException();
    }

    internal Boolean Is_Heading(MDER_Any value)
    {
        return value._WKBT() == Internal_Well_Known_Base_Type.MDER_Heading;
    }

    internal Boolean Is_Attr_Name_List(MDER_Any value)
    {
        if (value._WKBT() != Internal_Well_Known_Base_Type.MDER_Array)
        {
            return false;
        }
        // TODO: Replace this loop with an "All" higher order function call.
        Int64 count = Array__count((MDER_Array)value);
        for (Int64 i = 0; i < count; i++)
        {
            if (!Is_Text(Array__maybe_at((MDER_Array)value, i)!))
            {
                return false;
            }
        }
        return true;
    }

    internal Boolean Is_Local_Name(MDER_Any value)
    {
        if (!Is_Attr_Name_List(value))
        {
            return false;
        }
        Int64 count = Array__count((MDER_Array)value);
        if (count == 0)
        {
            return false;
        }
        MDER_Any first = Array__maybe_at((MDER_Array)value, 0)!;
        if ((Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("foundation")) && count == 2)
            || (Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("used")) && count >= 2)
            || (Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("package")) && count >= 1)
            || (Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("folder")) && count >= 1)
            || (Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("material")) && count == 1)
            || (Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("floating")) && count >= 2))
        {
            return true;
        }
        return false;
    }

    internal Boolean Is_Absolute_Name(MDER_Any value)
    {
        if (!Is_Local_Name(value))
        {
            return false;
        }
        MDER_Any first = Array__maybe_at((MDER_Array)value, 0)!;
        if (Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("foundation"))
            || Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("used"))
            || Object.ReferenceEquals(first, this.__machine.MDER_Attr_Name("package")))
        {
            return true;
        }
        return false;
    }

    internal MDER_Any evaluate_foundation_function(MDER_Text func_name, MDER_Tuple args)
    {
        String func_name_s = func_name._code_point_members();

        // TYPE DEFINERS

        if (Internal_Constants.Strings__Foundation_Type_Definer_Function_Names().Contains(func_name_s))
        {
            if (!this.Tuple__Has_Heading(args, this.__machine.MDER_Heading_0))
            {
                throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                    "X_Type_Definer_Function_Args_Not_MDER_Heading_0").ToString());
            }
            MDER_Any v = args.attrs["\u0000"];
            switch (func_name_s)
            {
                case "Any":
                    return this.__machine.MDER_True();
                case "None":
                    return this.__machine.MDER_False();

                // FOUNDATION BASE TYPE DEFINERS

                case "Ignorance":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Ignorance
                    );
                case "Boolean":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_False
                            || v._WKBT() == Internal_Well_Known_Base_Type.MDER_True
                    );
                case "Integer":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Integer
                    );
                case "Array":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Array
                    );
                case "Bag":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Bag
                    );
                case "Tuple":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                    );
                case "Article":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Article
                    );
                case "Handle":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Variable
                            || v._WKBT() == Internal_Well_Known_Base_Type.MDER_Process
                            || v._WKBT() == Internal_Well_Known_Base_Type.MDER_Stream
                            || v._WKBT() == Internal_Well_Known_Base_Type.MDER_External
                    );

                // HANDLE SUBTYPE DEFINERS

                case "Variable":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Variable
                    );
                case "Process":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Process
                    );
                case "Stream":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Stream
                    );
                case "External":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_External
                    );

                // ARRAY SUBTYPE DEFINERS

                // TUPLE SUBTYPE DEFINERS

                // ARTICLE SUBTYPE DEFINERS

                case "Blob":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Blob
                    );

                case "Text":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Text
                    );

                case "Excuse":
                    return this.__machine.MDER_Boolean(
                        v._WKBT() == Internal_Well_Known_Base_Type.MDER_Excuse
                    );

                default:
                    throw new NotImplementedException();
            }
        }

        // NON TYPE DEFINER UNARY FUNCTIONS

        if (Internal_Constants.Strings__Foundation_NTD_Unary_Function_Names().Contains(func_name_s))
        {
            if (!this.Tuple__Has_Heading(args, this.__machine.MDER_Heading_0))
            {
                throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                    "X_Non_Type_Definer_Unary_Function_Args_Not_MDER_Heading_0").ToString());
            }
            MDER_Any v = args.attrs["\u0000"];
            switch (func_name_s)
            {
                case "Integer_opposite":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Integer)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Integer_opposite_Arg_0_Not_Integer").ToString());
                    }
                    return this.__machine.MDER_Integer(-((MDER_Integer)v).as_BigInteger());
                case "Integer_modulus":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Integer)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Integer_modulus_Arg_0_Not_Integer").ToString());
                    }
                    return this.__machine.MDER_Integer(BigInteger.Abs(((MDER_Integer)v).as_BigInteger()));
                case "Integer_factorial":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Integer
                        || ((MDER_Integer)v).as_BigInteger() < 0)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Integer_factorial_Arg_0_Not_Integer_NN").ToString());
                    }
                    // Note that System.Numerics.BigInteger doesn't come
                    // with a Factorial(n) so we have to do it ourselves.
                    return this.__machine.MDER_Integer(Integer__factorial(((MDER_Integer)v).as_BigInteger()));
                case "Array_count":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Array)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Array_count_Arg_0_Not_Array").ToString());
                    }
                    return this.__machine.MDER_Integer(Array__count((MDER_Array)v));
                case "Bag_unique":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Bag)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Bag_unique_Arg_0_Not_Bag").ToString());
                    }
                    return new MDER_Bag(this.__machine,
                        new Internal_MDER_Discrete_Struct {
                            local_symbolic_type = Internal_Symbolic_Discrete_Type.Unique,
                            members = ((MDER_Bag)v).tree_root_node,
                            cached_members_meta = new Internal_Cached_Members_Meta {
                                tree_all_unique = true,
                            },
                        }
                    );
                case "Tuple_degree":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Tuple)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Tuple_degree_Arg_0_Not_Tuple").ToString());
                    }
                    return this.__machine.MDER_Integer(((MDER_Tuple)v).attrs.Count);
                case "Tuple_heading":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Tuple)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Tuple_heading_Arg_0_Not_Tuple").ToString());
                    }
                    return this.Tuple__Heading((MDER_Tuple)v);
                case "Text_from_UTF_8_Blob":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Blob)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Text_from_UTF_8_Blob_Arg_0_Not_Blob").ToString());
                    }
                    return this.MDER_Text_from_UTF_8_MDER_Blob((MDER_Blob)v);
                case "MDPT_Parsing_Unit_Text_to_Any":
                    if (v._WKBT() != Internal_Well_Known_Base_Type.MDER_Text)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_MDPT_Parsing_Unit_Text_to_Any_Arg_0_Not_Text").ToString());
                    }
                    // TODO: Everything.
                    return this.__machine.MDER_Ignorance();
                default:
                    throw new NotImplementedException();
            }
        }

        // BINARY FUNCTIONS

        if (Internal_Constants.Strings__Foundation_Binary_Function_Names().Contains(func_name_s))
        {
            if (!this.Tuple__Has_Heading(args, this.__machine.MDER_Heading_0_1))
            {
                throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                    "X_Binary_Function_Args_Not_MDER_Heading_0_1").ToString());
            }
            MDER_Any a0 = args.attrs["\u0000"];
            MDER_Any a1 = args.attrs["\u0001"];
            switch (func_name_s)
            {
                case "same":
                    return this.__machine.MDER_Boolean(a0._MDER_Any__same(a1));
                case "Integer_plus":
                    if (a0._WKBT() != Internal_Well_Known_Base_Type.MDER_Integer)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Integer_plus_Arg_0_Not_Integer").ToString());
                    }
                    if (a1._WKBT() != Internal_Well_Known_Base_Type.MDER_Integer)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Integer_plus_Arg_1_Not_Integer").ToString());
                    }
                    return this.__machine.MDER_Integer(((MDER_Integer)a0).as_BigInteger() + ((MDER_Integer)a1).as_BigInteger());
                case "Array_at":
                    if (a0._WKBT() != Internal_Well_Known_Base_Type.MDER_Array)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Array_at_Arg_0_Not_Array").ToString());
                    }
                    if (a1._WKBT() != Internal_Well_Known_Base_Type.MDER_Integer
                        || ((MDER_Integer)a1).as_BigInteger() < 0)
                    {
                        throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                            "X_Array_at_Arg_1_Not_Integer_NN").ToString());
                    }
                    MDER_Any maybe_member = Array__maybe_at((MDER_Array)a0, (Int64)((MDER_Integer)a1).as_BigInteger())!;
                    if (maybe_member is null)
                    {
                        throw new ArgumentException(
                            this.__machine.well_known_excuses["No_Such_Ord_Pos"].ToString());
                    }
                    return maybe_member;
                default:
                    throw new NotImplementedException();
            }
        }

        // TERNARY FUNCTIONS

        if (Internal_Constants.Strings__Foundation_Ternary_Function_Names().Contains(func_name_s))
        {
            if (!this.Tuple__Has_Heading(args, this.__machine.MDER_Heading_0_1_2))
            {
                throw new ArgumentException(this.__machine.Simple_MDER_Excuse(
                    "X_Ternary_Function_Args_Not_MDER_Heading_0_1_2").ToString());
            }
            MDER_Any v = args.attrs["\u0000"];
            switch (func_name_s)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        throw new NotImplementedException();
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

    internal Int64 Bits__count(MDER_Bits bits)
    {
        return bits._bit_members().Length;
    }

    internal Int64 Blob__count(MDER_Blob blob)
    {
        return blob._octet_members().Length;
    }

    internal Int64 Text__count(MDER_Text node)
    {
        if (node.cached_member_count is null)
        {
            if (node.has_any_non_BMP)
            {
                String s = node._code_point_members();
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
                node.cached_member_count = node._code_point_members().Length;
            }
        }
        return (Int64)node.cached_member_count;
    }

    internal Int64 Array__count(MDER_Array array)
    {
        return Discrete__node__tree_member_count(array.tree_root_node);
    }

    private Int64 Discrete__node__tree_member_count(Internal_MDER_Discrete_Struct node)
    {
        if (node.cached_members_meta!.tree_member_count is null)
        {
            switch (node.local_symbolic_type)
            {
                case Internal_Symbolic_Discrete_Type.None:
                    node.cached_members_meta.tree_member_count = 0;
                    break;
                case Internal_Symbolic_Discrete_Type.Singular:
                    node.cached_members_meta.tree_member_count
                        = node.Local_Singular_Members().multiplicity;
                    break;
                case Internal_Symbolic_Discrete_Type.Arrayed:
                    node.cached_members_meta.tree_member_count
                        = node.Local_Arrayed_Members().Count;
                    break;
                case Internal_Symbolic_Discrete_Type.Catenated:
                    node.cached_members_meta.tree_member_count
                        = Discrete__node__tree_member_count(node.Tree_Catenated_Members().a0)
                        + Discrete__node__tree_member_count(node.Tree_Catenated_Members().a1);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        return (Int64)node.cached_members_meta.tree_member_count;
    }

    private MDER_Integer? Bits__maybe_at(MDER_Bits bits, Int64 ord_pos)
    {
        return this.__machine.MDER_Integer(bits._bit_members()[(Int32)ord_pos] ? 1 : 0);
    }

    private MDER_Integer? Blob__maybe_at(MDER_Blob blob, Int64 ord_pos)
    {
        return this.__machine.MDER_Integer(blob._octet_members()[(Int32)ord_pos]);
    }

    private MDER_Integer? Text__maybe_at(MDER_Text node, Int64 ord_pos)
    {
        if (node.has_any_non_BMP)
        {
            String s = node._code_point_members();
            Int64 logical_i = 0;
            for (Int32 i = 0; i < s.Length; i++)
            {
                if (logical_i == ord_pos)
                {
                    if ((i+1) < s.Length
                        && Char.IsSurrogatePair(s[i], s[i+1]))
                    {
                        return this.__machine.MDER_Integer(
                            Char.ConvertToUtf32(s[i], s[i+1]));
                    }
                    return this.__machine.MDER_Integer(s[i]);
                }
                logical_i++;
                if ((i+1) < s.Length
                    && Char.IsSurrogatePair(s[i], s[i+1]))
                {
                    i++;
                }
            }
        }
        return this.__machine.MDER_Integer(
            node._code_point_members()[(Int32)ord_pos]);
    }

    private MDER_Any? Array__maybe_at(MDER_Array array, Int64 ord_pos)
    {
        return Discrete__node__maybe_at(array.tree_root_node, ord_pos);
    }

    private MDER_Any? Discrete__node__maybe_at(Internal_MDER_Discrete_Struct node, Int64 ord_pos)
    {
        if (node.cached_members_meta!.tree_member_count == 0)
        {
            return null;
        }
        Int64 maybe_last_ord_pos_seen = -1;
        switch (node.local_symbolic_type)
        {
            case Internal_Symbolic_Discrete_Type.None:
                return null;
            case Internal_Symbolic_Discrete_Type.Singular:
                if (ord_pos <= (maybe_last_ord_pos_seen + node.Local_Singular_Members().multiplicity))
                {
                    return node.Local_Singular_Members().member;
                }
                return null;
            case Internal_Symbolic_Discrete_Type.Arrayed:
                Int64 local_member_count = Discrete__node__tree_member_count(node);
                if (ord_pos <= (maybe_last_ord_pos_seen + local_member_count))
                {
                    Int64 ord_pos_within_local = ord_pos - (1 + maybe_last_ord_pos_seen);
                    return node.Local_Arrayed_Members()[(Int32)ord_pos_within_local];
                }
                return null;
            case Internal_Symbolic_Discrete_Type.Catenated:
                MDER_Any maybe_member = Discrete__node__maybe_at(node.Tree_Catenated_Members().a0, ord_pos)!;
                if (maybe_member is not null)
                {
                    return maybe_member;
                }
                maybe_last_ord_pos_seen += Discrete__node__tree_member_count(node.Tree_Catenated_Members().a0);
                if (ord_pos <= maybe_last_ord_pos_seen)
                {
                    return null;
                }
                Int64 ord_pos_within_succ = ord_pos - (1 + maybe_last_ord_pos_seen);
                return Discrete__node__maybe_at(node.Tree_Catenated_Members().a1, ord_pos_within_succ);
            default:
                throw new NotImplementedException();
        }
    }

    internal Internal_Dot_Net_String_Unicode_Test_Result Test_Dot_Net_String(String value)
    {
        Internal_Dot_Net_String_Unicode_Test_Result ok_result
            = Internal_Dot_Net_String_Unicode_Test_Result.Valid_Is_All_BMP;
        for (Int32 i = 0; i < value.Length; i++)
        {
            if (Char.IsSurrogate(value[i]))
            {
                if ((i+1) < value.Length
                    && Char.IsSurrogatePair(value[i], value[i+1]))
                {
                    ok_result = Internal_Dot_Net_String_Unicode_Test_Result.Valid_Has_Non_BMP;
                    i++;
                }
                else
                {
                    return Internal_Dot_Net_String_Unicode_Test_Result.Is_Malformed;
                }
            }
        }
        return ok_result;
    }

    internal void Array__Collapse(MDER_Array array)
    {
        array.tree_root_node = Discrete__Collapsed_Struct(array.tree_root_node);
    }

    internal void Set__Collapse(MDER_Set set, Boolean want_indexed = false)
    {
        set.tree_root_node = Discrete__Collapsed_Struct(set.tree_root_node, want_indexed);
    }

    internal void Bag__Collapse(MDER_Bag bag, Boolean want_indexed = false)
    {
        bag.tree_root_node = Discrete__Collapsed_Struct(bag.tree_root_node, want_indexed);
    }

    private Internal_MDER_Discrete_Struct Discrete__Collapsed_Struct(
        Internal_MDER_Discrete_Struct node, Boolean want_indexed = false)
    {
        // Note: want_indexed only causes Indexed result when Singular
        // or Arrayed_MM otherwise would be used; None still also used.
        switch (node.local_symbolic_type)
        {
            case Internal_Symbolic_Discrete_Type.None:
                // Node is already collapsed.
                // In theory we should never get here assuming that any
                // operations which would knowingly result in the empty
                // Array are optimized to return MDER_Array_C0 directly.
                return this.__machine.MDER_Array_C0.tree_root_node;
            case Internal_Symbolic_Discrete_Type.Singular:
                if (!want_indexed)
                {
                    // Node is already collapsed.
                    return node;
                }
                Internal_Multiplied_Member lsm = node.Local_Singular_Members();
                return new Internal_MDER_Discrete_Struct {
                    local_symbolic_type = Internal_Symbolic_Discrete_Type.Indexed,
                    members = new Dictionary<MDER_Any, Internal_Multiplied_Member>()
                        {{lsm.member, lsm}},
                    cached_members_meta = new Internal_Cached_Members_Meta {
                        tree_member_count = lsm.multiplicity,
                        tree_all_unique = (lsm.multiplicity == 1),
                        tree_relational = (lsm.member._WKBT()
                            == Internal_Well_Known_Base_Type.MDER_Tuple),
                    },
                };
            case Internal_Symbolic_Discrete_Type.Arrayed:
                // Node is already collapsed.
                return node;
            case Internal_Symbolic_Discrete_Type.Arrayed_MM:
                if (!want_indexed)
                {
                    // Node is already collapsed.
                    return node;
                }
                List<Internal_Multiplied_Member> ary_src_list = node.Local_Arrayed_MM_Members();
                Dictionary<MDER_Any, Internal_Multiplied_Member> ary_res_dict
                    = new Dictionary<MDER_Any, Internal_Multiplied_Member>();
                foreach (Internal_Multiplied_Member m in ary_src_list)
                {
                    if (!ary_res_dict.ContainsKey(m.member))
                    {
                        ary_res_dict.Add(m.member, m.Clone());
                    }
                    else
                    {
                        ary_res_dict[m.member].multiplicity ++;
                    }
                }
                return new Internal_MDER_Discrete_Struct {
                    local_symbolic_type = Internal_Symbolic_Discrete_Type.Indexed,
                    members = ary_res_dict,
                    cached_members_meta = new Internal_Cached_Members_Meta {
                        tree_member_count = ary_src_list.Count,
                        tree_all_unique = node.cached_members_meta!.tree_all_unique,
                        tree_relational = node.cached_members_meta.tree_relational,
                    },
                };
            case Internal_Symbolic_Discrete_Type.Catenated:
                Internal_MDER_Discrete_Struct n0 = Discrete__Collapsed_Struct(node.Tree_Catenated_Members().a0);
                Internal_MDER_Discrete_Struct n1 = Discrete__Collapsed_Struct(node.Tree_Catenated_Members().a1);
                if (n0.local_symbolic_type == Internal_Symbolic_Discrete_Type.None)
                {
                    return n1;
                }
                if (n1.local_symbolic_type == Internal_Symbolic_Discrete_Type.None)
                {
                    return n0;
                }
                // Both child nodes have at least 1 member, so we will
                // use Arrayed format for merger even if the input
                // members are the same value.
                // This will die if multiplicity greater than an Int32.
                return new Internal_MDER_Discrete_Struct {
                    local_symbolic_type = Internal_Symbolic_Discrete_Type.Arrayed,
                    members = Enumerable.Concat(
                        (n0.local_symbolic_type == Internal_Symbolic_Discrete_Type.Singular
                            ? Enumerable.Repeat(n0.Local_Singular_Members().member,
                                (Int32)n0.Local_Singular_Members().multiplicity)
                            : n0.Local_Arrayed_Members()),
                        (n1.local_symbolic_type == Internal_Symbolic_Discrete_Type.Singular
                            ? Enumerable.Repeat(n1.Local_Singular_Members().member,
                                (Int32)n1.Local_Singular_Members().multiplicity)
                            : n1.Local_Arrayed_Members())
                    ),
                    cached_members_meta = new Internal_Cached_Members_Meta(),
                        // TODO: Merge existing source meta where efficient,
                        // tree_member_count and tree_relational in particular.
                };
            case Internal_Symbolic_Discrete_Type.Indexed:
                // Node is already collapsed.
                return node;
            case Internal_Symbolic_Discrete_Type.Unique:
                Internal_MDER_Discrete_Struct uni_pa = Discrete__Collapsed_Struct(
                    node: node.Tree_Unique_Members(), want_indexed: true);
                if (uni_pa.local_symbolic_type == Internal_Symbolic_Discrete_Type.None)
                {
                    return uni_pa;
                }
                Dictionary<MDER_Any, Internal_Multiplied_Member> uni_src_dict
                    = uni_pa.Local_Indexed_Members();
                return new Internal_MDER_Discrete_Struct {
                    local_symbolic_type = Internal_Symbolic_Discrete_Type.Indexed,
                    members = uni_src_dict.ToDictionary(
                        m => m.Key, m => new Internal_Multiplied_Member(m.Key, 1)),
                    cached_members_meta = new Internal_Cached_Members_Meta {
                        tree_member_count = uni_src_dict.Count,
                        tree_all_unique = true,
                        tree_relational = uni_pa.cached_members_meta!.tree_relational,
                    },
                };
            case Internal_Symbolic_Discrete_Type.Summed:
                Internal_MDER_Discrete_Struct n0_ = Discrete__Collapsed_Struct(
                    node: node.Tree_Summed_Members().a0, want_indexed: true);
                Internal_MDER_Discrete_Struct n1_ = Discrete__Collapsed_Struct(
                    node: node.Tree_Summed_Members().a1, want_indexed: true);
                if (n0_.local_symbolic_type == Internal_Symbolic_Discrete_Type.None)
                {
                    return n1_;
                }
                if (n1_.local_symbolic_type == Internal_Symbolic_Discrete_Type.None)
                {
                    return n0_;
                }
                Dictionary<MDER_Any, Internal_Multiplied_Member> n0_src_dict
                    = n0_.Local_Indexed_Members();
                Dictionary<MDER_Any, Internal_Multiplied_Member> n1_src_dict
                    = n1_.Local_Indexed_Members();
                Dictionary<MDER_Any, Internal_Multiplied_Member> res_dict
                    = new Dictionary<MDER_Any, Internal_Multiplied_Member>(n0_src_dict);
                foreach (Internal_Multiplied_Member m in n1_src_dict.Values)
                {
                    if (!res_dict.ContainsKey(m.member))
                    {
                        res_dict.Add(m.member, m);
                    }
                    else
                    {
                        res_dict[m.member] = new Internal_Multiplied_Member(m.member,
                            res_dict[m.member].multiplicity + m.multiplicity);
                    }
                }
                return new Internal_MDER_Discrete_Struct {
                    local_symbolic_type = Internal_Symbolic_Discrete_Type.Indexed,
                    members = res_dict,
                    cached_members_meta = new Internal_Cached_Members_Meta(),
                        // TODO: Merge existing source meta where efficient,
                        // tree_member_count and tree_relational in particular.
                };
            default:
                throw new NotImplementedException();
        }
    }

    internal MDER_Any? Array__maybe_Pick_Arbitrary_Member(MDER_Array array)
    {
        return Discrete__maybe_Pick_Arbitrary_Node_Member(array.tree_root_node);
    }

    internal Boolean Array__Is_Relational(MDER_Array array)
    {
        return Discrete__Tree_Relational(array.tree_root_node);
    }

    internal MDER_Any? Set__maybe_Pick_Arbitrary_Member(MDER_Set set)
    {
        return Discrete__maybe_Pick_Arbitrary_Node_Member(set.tree_root_node);
    }

    internal Boolean Set__Is_Relational(MDER_Set set)
    {
        return Discrete__Tree_Relational(set.tree_root_node);
    }

    internal MDER_Any? Bag__maybe_Pick_Arbitrary_Member(MDER_Bag bag)
    {
        return Discrete__maybe_Pick_Arbitrary_Node_Member(bag.tree_root_node);
    }

    private MDER_Any? Discrete__maybe_Pick_Arbitrary_Node_Member(Internal_MDER_Discrete_Struct node)
    {
        if (node.cached_members_meta!.tree_member_count == 0)
        {
            return null;
        }
        switch (node.local_symbolic_type)
        {
            case Internal_Symbolic_Discrete_Type.None:
                return null;
            case Internal_Symbolic_Discrete_Type.Singular:
                return node.Local_Singular_Members().member;
            case Internal_Symbolic_Discrete_Type.Arrayed:
                return node.Local_Arrayed_Members()[0];
            case Internal_Symbolic_Discrete_Type.Arrayed_MM:
                return node.Local_Arrayed_MM_Members()[0].member;
            case Internal_Symbolic_Discrete_Type.Catenated:
                return Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Catenated_Members().a0)
                    ?? Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Catenated_Members().a1);
            case Internal_Symbolic_Discrete_Type.Indexed:
                return node.Local_Indexed_Members().First().Value.member;
            case Internal_Symbolic_Discrete_Type.Unique:
                return Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Unique_Members());
            case Internal_Symbolic_Discrete_Type.Summed:
                return Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Summed_Members().a0)
                    ?? Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Summed_Members().a1);
            default:
                throw new NotImplementedException();
        }
    }

    internal Boolean Bag__Is_Relational(MDER_Bag bag)
    {
        return Discrete__Tree_Relational(bag.tree_root_node);
    }

    private Boolean Discrete__Tree_Relational(Internal_MDER_Discrete_Struct node)
    {
        if (node.cached_members_meta!.tree_relational is null)
        {
            Boolean tr = true;
            switch (node.local_symbolic_type)
            {
                case Internal_Symbolic_Discrete_Type.None:
                    tr = true;
                    break;
                case Internal_Symbolic_Discrete_Type.Singular:
                    tr = node.Local_Singular_Members().member._WKBT()
                        == Internal_Well_Known_Base_Type.MDER_Tuple;
                    break;
                case Internal_Symbolic_Discrete_Type.Arrayed:
                    MDER_Any m0 = Discrete__maybe_Pick_Arbitrary_Node_Member(node)!;
                    tr = m0._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                        && Enumerable.All(
                            node.Local_Arrayed_Members(),
                            m => m._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                                && Tuple__Same_Heading((MDER_Tuple)m, (MDER_Tuple)m0)
                        );
                    break;
                case Internal_Symbolic_Discrete_Type.Arrayed_MM:
                    MDER_Any m0_ = Discrete__maybe_Pick_Arbitrary_Node_Member(node)!;
                    tr = m0_._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                        && Enumerable.All(
                            node.Local_Arrayed_MM_Members(),
                            m => m.member._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                                && Tuple__Same_Heading((MDER_Tuple)m.member, (MDER_Tuple)m0_)
                        );
                    break;
                case Internal_Symbolic_Discrete_Type.Catenated:
                    MDER_Any pm0 = Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Catenated_Members().a0)!;
                    MDER_Any sm0 = Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Catenated_Members().a1)!;
                    tr = Discrete__Tree_Relational(node.Tree_Catenated_Members().a0)
                        && Discrete__Tree_Relational(node.Tree_Catenated_Members().a1)
                        && (pm0 is null || sm0 is null || Tuple__Same_Heading((MDER_Tuple)pm0, (MDER_Tuple)sm0));
                    break;
                case Internal_Symbolic_Discrete_Type.Indexed:
                    MDER_Any im0 = Discrete__maybe_Pick_Arbitrary_Node_Member(node)!;
                    tr = im0._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                        && Enumerable.All(
                            node.Local_Indexed_Members().Values,
                            m => m.member._WKBT() == Internal_Well_Known_Base_Type.MDER_Tuple
                                && Tuple__Same_Heading((MDER_Tuple)m.member, (MDER_Tuple)im0)
                        );
                    break;
                case Internal_Symbolic_Discrete_Type.Unique:
                    tr = Discrete__Tree_Relational(node.Tree_Unique_Members());
                    break;
                case Internal_Symbolic_Discrete_Type.Summed:
                    tr = Discrete__Tree_Relational(node.Tree_Summed_Members().a0)
                        && Discrete__Tree_Relational(node.Tree_Summed_Members().a1);
                    MDER_Tuple pam0 = (MDER_Tuple)Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Summed_Members().a0)!;
                    MDER_Tuple eam0 = (MDER_Tuple)Discrete__maybe_Pick_Arbitrary_Node_Member(node.Tree_Summed_Members().a1)!;
                    if (pam0 is not null && eam0 is not null)
                    {
                        tr = tr && Tuple__Same_Heading(pam0, eam0);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            node.cached_members_meta.tree_relational = tr;
        }
        return (Boolean)node.cached_members_meta.tree_relational;
    }

    internal MDER_Heading Tuple__Heading(MDER_Tuple tuple)
    {
        return this.__machine.MDER_Heading(new HashSet<String>(tuple.attrs.Keys));
    }

    internal MDER_Heading? Tuple__maybe_Heading(MDER_Tuple? tuple)
    {
        if (tuple is null)
        {
            return null;
        }
        return this.__machine.MDER_Heading(new HashSet<String>(tuple.attrs.Keys));
    }

    internal Boolean Tuple__Same_Heading(MDER_Tuple t1, MDER_Tuple t2)
    {
        if (Object.ReferenceEquals(t1, t2))
        {
            return true;
        }
        Dictionary<String, MDER_Any> attrs1 = t1.attrs;
        Dictionary<String, MDER_Any> attrs2 = t2.attrs;
        return (attrs1.Count == attrs2.Count)
            && Enumerable.All(attrs1, attr => attrs2.ContainsKey(attr.Key));
    }

    internal Boolean Tuple__Has_Heading(MDER_Tuple t, MDER_Heading h)
    {
        Dictionary<String, MDER_Any> attrs = t.attrs;
        HashSet<String> attr_names = h.attr_names;
        return (attrs.Count == attr_names.Count)
            && Enumerable.All(attr_names, attr_name => attrs.ContainsKey(attr_name));
    }

    internal MDER_Any MDER_Text_from_UTF_8_MDER_Blob(MDER_Blob value)
    {
        Byte[] octets = value._octet_members();
        UTF8Encoding enc = new UTF8Encoding
        (
            encoderShouldEmitUTF8Identifier: false,
            throwOnInvalidBytes: true
        );
        try
        {
            String s = enc.GetString(octets);
            Internal_Dot_Net_String_Unicode_Test_Result tr = Test_Dot_Net_String(s);
            if (tr == Internal_Dot_Net_String_Unicode_Test_Result.Is_Malformed)
            {
                return this.__machine.Simple_MDER_Excuse("X_Unicode_Blob_Not_UTF_8");
            }
            return this.__machine.MDER_Text(
                s,
                (tr == Internal_Dot_Net_String_Unicode_Test_Result.Valid_Has_Non_BMP)
            );
        }
        catch
        {
            return this.__machine.Simple_MDER_Excuse("X_Unicode_Blob_Not_UTF_8");
        }
    }
}
