import BaseRequest from "@/apis/baseRequest";
import BaseLoading from "@/components/base/BaseLoading.vue";
import BaseToastMsg from "@/components/base/BaseToastMsg.vue";
import Enum from "@/commons/enum";
import Resourse from "@/commons/resource";
import Const from "@/commons/const";
import BasePopup from "@/components/base/BasePopup.vue";
import ProductDetail from "../ProductDetail.vue";
import InputFilter from "../InputFilter.vue";

export default {
  components: {
    BaseLoading,
    ProductDetail,
    BaseToastMsg,
    BasePopup,
    InputFilter,
  },
  data() {
    return {
      money: {
        thousands: ".",
        precision: 0,
      },

      // api
      apiRouter: "Products",
      isClicked: true,

      //#region data-combobox

      // khai báo mảng để chứa dữ liệu chế độ hiển thị
      showValue: Const.Show.Value.All,
      showOptions: [
        {
          showOptionValue: Const.Show.Value.All,
          showOptionName: Const.Show.OptionName.All,
        },
        {
          showOptionValue: Const.Show.Value.Yes,
          showOptionName: Const.Show.OptionName.Yes,
        },
        {
          showOptionValue: Const.Show.Value.No,
          showOptionName: Const.Show.OptionName.No,
        },
      ],

      // khai báo mảng để chứa dữ liệu trạng thái kinh doanh
      stateValue: Const.State.Value.All,
      stateOptions: [
        {
          stateOptionValue: Const.State.Value.All,
          stateOptionName: Const.State.OptionName.All,
        },
        {
          stateOptionValue: Const.State.Value.Business,
          stateOptionName: Const.State.OptionName.Business,
        },
        {
          stateOptionValue: Const.State.Value.Stop,
          stateOptionName: Const.State.OptionName.Stop,
        },
      ],

      //#endregion

      //#region product
      // biến chứa danh sách hàng hóa
      productList: [],
      // id hàng hóa
      productId: "",
      // đối tượng hàng hóa
      product: {
        ProductCodeSKU: null,
        ProductName: null,
        ProductCategoryId: null,
        PurchasePrice: 0,
        SellingPrice: 0,
        CalculationUnitId: null,
        ProductProperty: null,
        ProductParentId: null,
        ProductBarCode: null,
        Description: null,
        ProductImageId: null,
        BusinessStatus: Const.State.Value.Business,
        ShowStatus: Const.Show.Value.Yes,
        Products: null,
      },
      isShowPageSize: false,
      titleDelete: "",

      pageSize: 50,
      pageIndex: 1,
      totalPage: 1,
      totalRecord: 0,
      txtPageIndex: 1,
      /* Đối tượng tìm kiếm */
      objFilter: {},
      /* Danh sách đổi tượng tìm kiếm */
      listObjFilters: [],
      // đối tượng để sắp xếp dữ liệu
      objSort: {
        Column: null,
        SortOrder: Enum.Sort.Asc,
      },
      sortOrder: Enum.Sort.Asc,

      // form product
      isShowDetail: false,
      formMode: null,

      //#endregion

      //#region toast msg + checkbox

      // toast message
      isShowToastMsg: false,
      toastMsg: {
        Title: "",
        Type: "",
      },
      /**
       * btn check all ở thead
       */
      isCheckAll: false,
      /** mảng chứa hàng hóa được chọn */
      selectedProducts: [],

      //#endregion

      isLoading: false,

      // Popup
      isShowPopup: false,
      popup: {
        Title: "",
        Status: "",
      },
    };
  },

  watch: {
    pageIndex() {
      this.getAllProducts();
      this.txtPageIndex = this.pageIndex;
    },

    showValue(){

        let objFilter = {
          Column: 'ShowStatus',
          Value: this.showValue,
          Operator: Enum.Operator.EqualTo,
          ValueType: Const.ValueType.Int,
          AdditionalOperator: Enum.AdditionalOperator.And,
        };
        this.onChangeInputValue(objFilter);

    },
    stateValue(){
        let objFilter = {
          Column: 'BusinessStatus',
          Value: this.stateValue,
          Operator: Enum.Operator.EqualTo,
          ValueType: Const.ValueType.Int,
          AdditionalOperator: Enum.AdditionalOperator.And,
        };
        this.onChangeInputValue(objFilter);
    }



  },

  filters: {
    /**
     * chuyển dạng số sang tiền tệ
     * createdBy:: namnguyen(20/12/2022)
     */
    formatNumber2Currency(number) {
      if (number) {
        // convert to string
        let result = number + "";
        result = result.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
        return result;
      }
      return "";
    },

    /**
     * chuyển tình trạng hiển thị sang tên
     * createdBy:: namnguyen(20/12/2022)
     */
    covertShowName(number) {
      if (number === 1) {
        return "Có";
      } else if (number === 0) {
        return "Không";
      } else {
        return "";
      }
    },

    /**
     * chuyển tình trạng kinh doanh sang tên
     * createdBy:: namnguyen(20/12/2022)
     */
    covertBusinessStateName(number) {
      if (number === 1) {
        return "Đang kinh doanh";
      } else if (number === 0) {
        return "Ngừng kinh doanh";
      } else {
        return "";
      }
    },
  },

  created() {
    this.getAllProducts();
  },

  methods: {
    //#region hiển thị dữ liệu

    /**
     * lấy dữ liệu hàng hóa
     * createdBy:: namnguyen(20/12/2022)
     */
    getAllProducts() {
      try {
        
        let me = this;
        // clean data
        let sort = "";
        let filters = "";
        if (this.objSort.Column == null) sort = "";
        else sort = JSON.stringify(this.objSort);
        filters = JSON.stringify(this.listObjFilters);
        // hiện loading
        me.isLoading = true;
        // Call api
        setTimeout(function () {
          BaseRequest.get(
            `${me.apiRouter}/FilterProducts?pageSize=${me.pageSize}&pageIndex=${me.pageIndex}&objectFilters=${filters}&objectSort=${sort}`
          )
            .then((response) => {
              // gán dữ liệu trả về từ api
              me.productList = response.data.Data;
              me.totalRecord = response.data.TotalRecord;
              me.totalPage = response.data.TotalPage;
              // gán productId bằng id của bản ghi đầu tiên
              me.productId = me.productList[0].ProductId;
              // set thông báo delete nếu ân nút xóa
              me.titleDelete =
                me.productList[0].ProductName +
                "-" +
                me.productList[0].ProductCodeSKU;
              // ẩn loading
              me.isLoading = false;
            })
            .catch((e) => {
              me.isLoading = false;
              console.log(e);
            });
        }, 1000);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm sắp xếp dữ liệu
     * createdBy: namnguyen(24/01/2022)
     */
    sorting(columnName, sortOrder) {
      try {
        let me = this;
        me.objSort.Column = columnName;
        if (sortOrder == Enum.Sort.Asc) {
          me.objSort.SortOrder = Enum.Sort.Asc;
          me.sortOrder = Enum.Sort.Desc;
        } else {
          me.objSort.SortOrder = Enum.Sort.Desc;
          me.sortOrder = Enum.Sort.Asc;
        }
        me.getAllProducts();

        if (columnName == "ProductName") {
          this.isClicked = !this.isClicked;
        }
        else{
          this.isClicked=true;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Chọn 1 đối tượng trong table
     * Author: TTKien(21/01/2022)
     */
    onClickRowActive(productId, productName, productCode) {
      this.titleDelete = productName + "-" + productCode;
      this.productId = productId;
    },

    /**
     * hàm filter dữ liệu ==========================================================================
     * createdBy: namnguyen(24/01/2022)
     */
    // changeFilter(columnName, event) {
    //   const value = event.currentTarget.value;
    //   if (value === "") {
    //     this.listObjFilters = this.listObjFilters.filter(
    //       (f) => f.Column != columnName
    //     );
    //   } else {
    //     const filter = this.listObjFilters.find((f) => f.Column == columnName);
    //     if (filter) {
    //       filter.Value = value;
    //     } else {
    //       this.listObjFilters.push({
    //         Column: columnName,
    //         Operator: 0,
    //         Value: value,
    //         ValueType: "String",
    //       });
    //     }
    //   }
    //   this.getAllProducts();
    // },

    /**
     * hàm filter dữ liệu
     * createdBy: namnguyen(26/01/2022)
     */
    onChangeInputValue(objFilter) {
      try {
        let me = this;
        me.listObjFilters = me.listObjFilters || [];
        if (objFilter.Value === "" || objFilter.Value == null) {
          const index = me.listObjFilters.findIndex(f => f.Column == objFilter.Column);
          me.listObjFilters.splice(index, 1);
        }
        // obj có giá trị
        else {
          const filter = me.listObjFilters.find(f => f.Column == objFilter.Column);
          if (filter) {
            filter.Value = objFilter.Value;
            filter.Operator = objFilter.Operator;
          }
          else {
            me.listObjFilters.push(objFilter);
          }
        }
        me.getAllProducts();
      } catch (error) {
        console.log(error);
      }
    },

    // ==========================================================================================

    /**
     * hàm làm mới dữ liệu
     * createdBy: namnguyen(24/01/2022)
     */
    btnRefreshOnClick() {
      try {
        this.pageIndex = 1;
        this.selectedProducts = [];
        this.objSort.Column = null;
        this.getAllProducts();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm hiển thị dropdown số trang
     * createdBy: namnguyen(18/01/2022)
     */
    showPagesize() {
      try {
        this.isShowPageSize = !this.isShowPageSize;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm chọn số trang
     * createdBy:: namnguyen(25/01/2022)
     */
    selectPagesize(pageSize) {
      try {
        this.isShowPageSize = !this.isShowPageSize;
        this.pageSize = pageSize;
        this.pageIndex = 1;
        this.getAllProducts();
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm thay đổi input số trang
     * createdBy:: namnguyen(25/01/2022)
     */
    changeInputPageIndex() {
      try {
        if (this.txtPageIndex >= 1 && this.txtPageIndex <= this.totalPage) {
          this.pageIndex = this.txtPageIndex;
        }

        if (this.txtPageIndex < 1) {
          this.txtPageIndex = 1;
          this.pageIndex = this.txtPageIndex;
        }
        if (this.txtPageIndex > this.totalPage) {
          this.txtPageIndex = this.totalPage;
          this.pageIndex = this.txtPageIndex;
        }
      } catch (error) {
        console.log(error);
      }
    },

    //#endregion

    //#region CUD

    /**
     * ẩn form chi tiết hàng hóa
     * createdBy:: namnguyen(25/01/2022)
     */
    hideProductDetail() {
      try {
        this.isShowDetail = false;
        // reset dữ liệu hàng hóa
        this.product = {
          ProductCodeSKU: null,
          ProductName: null,
          ProductCategoryId: null,
          PurchasePrice: 0,
          SellingPrice: 0,
          CalculationUnitId: null,
          ProductProperty: null,
          ProductParentId: null,
          ProductBarCode: null,
          Description: null,
          ProductImageId: null,
          BusinessStatus: 1,
          ShowStatus: 1,
          Products: null,
        };
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * sự kiện click vào btn thêm mới
     * createdBy:namnguyen(18/01/2022)
     */
    onClickBtnAdd() {
      try {
        this.toastMsg.Title = Resourse.ToastMsg.Success.Add;
        this.formMode = Const.FormMode.Add;
        this.isShowDetail = true;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * sự kiện click vào btn nhân bản
     * createdBy:namnguyen(18/01/2022)
     */
    onClickBtnClone() {
      try {
        let me = this;
        let tempCode;
        let tempBarCode;
        me.toastMsg.Title = Resourse.ToastMsg.Success.Add;
        //this.productId = this.selectedProducts[0];
        me.formMode = Const.FormMode.Add;
        BaseRequest.get(`${me.apiRouter}/${me.productId}`)
          .then((response) => {
            me.product = response.data;
            // Call api lấy mã sku mới
            BaseRequest.get(
              `${me.apiRouter}/NewProductCodeSKU?inputText=${me.product.ProductName}`
            )
              .then((res) => {
                me.product.ProductCodeSKU = res.data;
                tempCode = res.data;
                BaseRequest.get(`${this.apiRouter}/NewProductBarCode`)
                  .then((response) => {
                    tempBarCode = response.data;
                    me.product.ProductBarCode = String(tempBarCode);
                    if (me.product.Products.length > 0) {
                      for (let i = 0; i < me.product.Products.length; i++) {
                        // gán mã code sku mới cho các hàng hóa con
                        tempCode = me.product.Products[i].ProductCodeSKU.slice(
                          me.product.Products[i].ProductCodeSKU.indexOf("-")
                        );
                        me.product.Products[i].ProductCodeSKU =
                          res.data + tempCode;

                        // gán mã vạch mới cho các hàng hóa con
                        me.product.Products[i].ProductBarCode = String(tempBarCode + i + 1);
                      }
                    }
                    me.isShowDetail = true;
                  })
                  .catch((res) => {
                    console.log(res);
                  });

                // me.product.ProductCodeSKU = res.data;
                // if(me.product.Products.length>0){

                //   for (let i = 0; i < me.product.Products.length; i++) {
                //       tempCode = me.product.Products[i].ProductCodeSKU.slice(me.product.Products[i].ProductCodeSKU.indexOf('-'));
                //       me.product.Products[i].ProductCodeSKU = res.data + tempCode;
                //   }
                // }
                // me.isShowDetail = true;
              })
              .catch((res) => {
                console.log(res);
              });
          })
          .catch((res) => {
            console.log(res);
          });
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * sự kiện click vào btn cập nhập
     * createdBy:namnguyen(18/01/2022)
     */
    onClickBtnUpdate() {
      try {
        this.toastMsg.Title = Resourse.ToastMsg.Success.Update;
        this.formMode = Const.FormMode.Edit;
        //this.productId = this.selectedProducts[0];
        this.getProductById(this.productId);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * sự kiện click vào btn delete
     * createdBy:namnguyen(18/01/2022)
     */
    onClickBtnDelete() {
      try {
        let title = "";
        if (this.selectedProducts.length > 1) {
          title = Resourse.PopUp.Title.DeleteMultiple;
        } else {
          title = Resourse.PopUp.TitleWithParam(this.titleDelete);
        }
        this.showPopupWarning(title);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * sự kiện click button xác nhận xóa
     * createdBy:: namnguyen (19/01/2022)
     */
    deleteProduct() {
      try {
        let me = this;
        let productIds = [];
        if (me.selectedProducts.length < 1) {
          productIds.push(me.productId);
        } else {
          productIds = me.selectedProducts;
        }
        // Call api
        BaseRequest.post(`${me.apiRouter}/DeleteProducts`, productIds)
          .then(function () {
            // load lại dữ liệu
            me.getAllProducts();
            // hiện toast message
            me.toastMsg.Title = Resourse.ToastMsg.Success.Delete;
            setTimeout(() => {
              me.showToastMsg();
            }, 100);
            me.hideToastMsg();
            // ẩn popup
            me.hidePopup(false);
            me.selectedProducts = [];
            this.isCheckAll = !this.isCheckAll;
          })
          .catch(function (e) {
            console.log(e);
          });
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * sự kiện double click vào dòng trong table
     * createdBy:namnguyen(18/01/2022)
     */
    onDblClickRow(id) {
      try {
        this.toastMsg.Title = Resourse.ToastMsg.Success.Update;
        this.formMode = Const.FormMode.Edit;
        this.productId = id;
        this.getProductById(this.productId);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * lấy sản phẩm theo id
     * createdBy:: namnguyen (20/12/2022)
     */
    getProductById(id) {
      BaseRequest.get(`${this.apiRouter}/${id}`)
        .then((response) => {
          this.product = response.data;
          if (this.product.Products) {
            this.product.Products.forEach((pro) => {
              pro.EntityState = Enum.EditMode.None;
            });
          }
          // Show modal
          this.isShowDetail = true;
        })
        .catch((res) => {
          console.log(res);
        });
    },

    //#endregion

    //#region checkbox

    /**
     * sự kiện click vào checkbox check tất cả
     * createdBy:: namnguyen(25/01/2022)
     */
    onClickCheckAll() {
      try {
        this.isCheckAll = !this.isCheckAll;
        this.selectedProducts = [];
        if (this.isCheckAll) {
          // Check all
          for (var key in this.productList) {
            this.selectedProducts.push(this.productList[key].ProductId);
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * cập nhập lại nút check tất cả
     * createdBy:: namnguyen(25/01/2022)
     */
    updateCheckAll(productName, productCode) {
      try {
        // set title cho câu lệnh xóa
        this.titleDelete = productName + "-" + productCode;

        if (this.selectedProducts.length == this.productList.length) {
          this.isCheckAll = true;
        } else {
          this.isCheckAll = false;
        }
      } catch (error) {
        console.log(error);
      }
    },

    //#endregion

    //#region pop up

    /**
     * hiển thị thông báo khi xóa
     * param {title} nội dung thông báo
     * createdBy: namnguyen (19/01/2022)
     */
    showPopupWarning(title) {
      try {
        this.isShowPopup = true;
        this.popup.Status = Resourse.PopUp.Status.Warning;
        this.popup.Title = title;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hiển thị thống báo khi có lỗi sảy ra
     * param {title} nội dung thông báo
     * createdBy: namnguyen (19/01/2022)
     */
    showPopupDanger(title) {
      try {
        this.isShowPopup = true;
        this.popup.Status = Resourse.PopUp.Status.Danger;
        this.popup.Title = title;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * ẩn pop-up
     * createdBy: namnguyen (19/01/2022)
     */
    hidePopup() {
      try {
        this.isShowPopup = false;
        this.popup.Status = "";
        this.popup.Title = "";
      } catch (error) {
        console.log(error);
      }
    },

    //#endregion

    //#region toast message

    /**
     * hiển thị toast message
     * createdBy: namnguyen (23/01/2022)
     */
    showToastMsg() {
      try {
        this.isShowToastMsg = true;
        this.toastMsg.Type = Resourse.ToastMsg.Type.Success;
        setTimeout(this.hideToastMsg, 2500);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * ẩn toast message
     */
    hideToastMsg() {
      this.isShowToastMsg = false;
    },

    //#endregion
  },
};