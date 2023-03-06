const Resource = {
    Message: {
        ValidateNotValid:{
            ProductNameIsEmpty: "Tên hàng hóa không được để trống",
            OverSizeImg: "Ảnh không tải được do vượt quá dung lượng. Vui lòng chọn ảnh có dung lượng nhỏ hơn 5 MB."
        },
        Warning:"Có lỗi sảy ra vui lòng gọi MISA để nhận trợ giúp !"
    },
    ToastMsg:{
        Success:{
            Add: "Thêm mới thành công",
            Update: "Cập nhập thành công",
            Delete: "Xóa thành công",
        },
        Type:{
            Success: "Success"
        }
    },
    PopUp:{
        Status:{
            Danger: "Danger",
            Warning: "Warning",
        },
        Title:{
            DeleteMultiple: "đã chọn",
        },
        // title cho xóa
        TitleWithParam (param){ return `${param}`},
        // title cho check trùng code sku
        TitleDuplicateWithParam (name, value){ return `${name} [${value}] đã khai báo cho nhiều hàng hóa. Vui lòng kiểm tra lại`}

    },
    NameProperty:{
        ProductCodeSKU: "Mã SKU",
        ProductBarCode: "Mã vạch"
    }

}

export default Resource;