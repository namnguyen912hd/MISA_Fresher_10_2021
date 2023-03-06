const Enum = {
    Sort: {
        Asc: 0,
        Desc: 1,
    },
    EditMode: {
        None: 0,
        Add: 1,
        Upadate: 2,
        Delete: 3,
    },


    Operator: {
      Contain: 0,
      EqualTo: 1,
      BeginWith: 2,
      EndWith: 3,
      NotContain: 4,
      LessThan: 5,
      LessThanOrEqualTo: 6,
      MoreThan: 7,
      MoreThanOrEqualTo: 8,
    },

    ValueType: {
      Int: "Int32",
      String: "String",
    },
    
    AdditionalOperator: {
      And: "AND"
    }
}

export default Enum;