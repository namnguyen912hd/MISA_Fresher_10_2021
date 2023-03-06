let Const= {
    ValueType: {
        Int: "Int32",
        String: "String",
    },
    AdditionalOperator: {
        And: "AND"
    },
    State:{
        OptionName:{
            All: "Tất cả",
            Business: "Đang kinh doanh",
            Stop:"Ngừng kinh doanh"
        },
        Value:{
            All: null,
            Business: 1,
            Stop:0
        }
    },
    Show:{
        OptionName:{
            All: "Tất cả",
            Yes: "Có",
            No:"Không"
        },
        Value:{
            All: null,
            Yes: 1,
            No: 0
        }
    },
    FormMode:{
        Add: "Add",
        Edit: "Edit"
    },
    MaxImgSize: 5*1024*1024,
    
    
  };
  export default Const;
  