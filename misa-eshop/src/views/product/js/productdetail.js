import BaseRequest from "@/apis/baseRequest";
import { required } from "vuelidate/lib/validators";
import Const from "@/commons/const";
import Enum from "@/commons/enum";
import Resource from "@/commons/resource";

export default {
  components: {},
  props: ["isShowDetail", "formMode", "product", "productId", "apiRouter"],

  data() {
    return {
      money: {
        thousands: ".",
        precision: 0,
      },

      tagValue: "",
      tags: [],
      activeTag: null,
      calculationUnits: [],
      productCategories: [],
      // chế độ submit (true: submit, false: không submit)
      submitted: false,
      productDetails: [],
      selectedFile: null,
    };
  },
  watch: {
    /**
     * hàm focus vào ô input tên hàng hóa
     * createdBy: namnguyen(21/01/2022)
     */
    isShowDetail() {
      try {
        if (this.isShowDetail == true) {
          setTimeout(() => {
            this.$refs.txtProductName.focus();
          }, 10);
        }
        if (this.product.ProductProperty) {
          this.tags = this.convertString2Array(this.product.ProductProperty);
          //this.tags = this.createTags(this.product.Products);
        }
        this.cloneProductDetails();
      } catch (error) {
        console.log(error);
      }
    },
  },
  created() {
    this.getCategories(), this.getUnits();
  },
  // kiểm tra dữ liệu
  validations: {
    product: {
      ProductName: {
        required,
      },
    },
  },
  methods: {

    // createTags(object){
    //   if(object){
    //     for (let item in object) {
    //       this.tags.push(item.ProductName)
    //   }
    //   }
    // },


    //#region  binding dữ liệu cho các combobox
    /**
     * lấy dữ liệu nhóm hàng hóa
     * createdBy:: namnguyen(24/01/2022)
     */
    getCategories() {
      BaseRequest.get("ProductCategories")
        .then((response) => {
          // gán dữ liệu trả về từ api
          this.productCategories = response.data;
        })
        .catch((e) => {
          console.log(e);
        });
    },
    /**
     * lấy dữ liệu nhóm hàng hóa
     * createdBy:: namnguyen(24/01/2022)
     */
    getUnits() {
      BaseRequest.get("CalculationUnits")
        .then((response) => {
          // gán dữ liệu trả về từ api
          this.calculationUnits = response.data;
        })
        .catch((e) => {
          console.log(e);
        });
    },
    //#endregion

    /**
     * ẩn form
     * createdBy: namnguyen(19/01/2022)
     */
    hideFormData() {
      try {
        this.tags = [];
        this.tagValue = "";
        this.submitted = false;
        this.$emit("hideProductDetail");
      } catch (error) {
        console.log(error);
      }
    },

    //#region xử lí tags

    /**
     * hàm chuyển string sang mảng
     * createdBy: namnguyen(19/01/2022)
     */
    convertString2Array(str) {
      try {
        let arr = [];
        if (str) {
          str = str.toString();
          arr = str.split(",");
          return arr;
        }
      } catch (error) {
        console.log(error);
        return [];
      }
    },
    /**
     * hàm thêm 1 tag
     * createdBy: namnguyen(19/01/2022)
     */
    addTag() {
      try {
        if (this.tagValue != "") {
          if (this.tags) {
            if (this.tags.indexOf(this.tagValue) === -1) {
              this.tags.push(this.tagValue);
              this.generateFakeProduct(this.tags);
            }
          }
        }
        this.tagValue = "";
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * hàm xóa tag
     * createdBy: namnguyen(19/01/2022)
     */
    removeTag(index) {
      try {
        let tempProductDetails = this.productDetails;
        let tempArr = [];
        for (const [key, value] of Object.entries(this.productDetails)) {
          if (key != index) {
            console.log(value);
            tempArr.push(this.productDetails[key]);
          }
        }
        this.productDetails = tempArr;

        this.tags.splice(index, 1);
        if (this.tags.length === 0) {
          this.productDetails = [];
        }

        // set entitystate cho item bị xóa khi đang ở chế độ sửa
        if (this.formMode == Const.FormMode.Edit) {
          if (index < Object.keys(this.product.Products).length) {
            if (tempProductDetails[index].ProductId != null) {
              for (let i in tempProductDetails) {
                if (
                  tempProductDetails[i].ProductId ==
                  this.product.Products[index].ProductId
                ) {
                  this.product.Products[i].EntityState = Enum.EditMode.Delete;
                }
              }
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm tạo dữ liệu fake
     * createdBy: namnguyen(19/01/2022)
     */
    generateFakeProduct(tags) {
      try {
        // khai báo biến hàng hóa giả
        let fakeProduct = {
          ProductId: null,
          ProductCodeSKU: null,
          ProductName: null,
          ProductBarCode: null,
          ProductImageId: null,
          ProductCategoryId: this.product.ProductCategoryId,
          PurchasePrice: this.product.PurchasePrice,
          SellingPrice: this.product.SellingPrice,
          CalculationUnitId: this.product.CalculationUnitId,
          Description: this.product.Description,
          BusinessStatus: this.product.BusinessStatus,
          ShowStatus: this.product.ShowStatus,
          EntityState: null,
        };
        let fakeCodeSKU = null;
        // lấy độ dài của tags
        let lengthTags = tags.length - 1;
        // check biến tên hàng hóa
        if (!this.product.ProductName) {
          // nếu tên hàng hóa null
          // gán tên hàng hóa đc tạo ra
          fakeProduct.ProductName = "(" + tags[lengthTags] + ")";
          // gán mã code hàng hóa
          fakeCodeSKU = "-" + tags[lengthTags].substring(0, 1) + "1";
          fakeProduct.ProductCodeSKU =
            this.getNewProductCodeSKUFake(fakeCodeSKU);
        } else {
          // tên hàng hóa có giá trị
          // gán tên hàng hóa đc tạo ra
          fakeProduct.ProductName =
            this.product.ProductName + "(" + tags[lengthTags] + ")";
          // gán mã code hàng hóa
          fakeCodeSKU =
            this.product.ProductCodeSKU +
            "-" +
            tags[lengthTags].substring(0, 1).toUpperCase() +
            "1";
          fakeProduct.ProductCodeSKU =
            this.getNewProductCodeSKUFake(fakeCodeSKU);
        }
        fakeProduct.EntityState = Enum.EditMode.Add;
        if (this.productDetails) {
          this.productDetails.push(fakeProduct);
        } else {
          this.productDetails = [];
          this.productDetails.push(fakeProduct);
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm tạo ProductCodeSKU fake
     * createdBy: namnguyen(19/01/2022)
     */
    getNewProductCodeSKUFake(text) {
      try {
        text = text.toString().toUpperCase();
        let maxNumber = 0;
        let arr = [];

        for (let i in this.productDetails) {
          if (
            this.productDetails[i].ProductCodeSKU.toUpperCase().slice(0, -1) ==
            text.slice(0, -1)
          ) {
            arr.push(parseInt(this.productDetails[i].ProductCodeSKU.slice(-1)));
          }
        }
        if (arr.length > 0) {
          maxNumber = arr[arr.length - 1] + 1;
          text = text.slice(0, -1) + maxNumber.toString();
          return text.toUpperCase();
        } else {
          return text.toUpperCase();
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * hàm xóa chi tiết hàng hóa
     * param {index}: chỉ mục hàng hóa
     * param {id}: id hàng hóa
     * createdBy: namnguyen(19/01/2022)
     */
    removeProductDetail(index, id) {
      try {
        //this.removeTag(index);
        let tempArr = [];
        for (const [key, value] of Object.entries(this.productDetails)) {
          if (key != index) {
            console.log(value);
            tempArr.push(this.productDetails[key]);
          }
        }
        this.productDetails = tempArr;

        this.tags.splice(index, 1);
        if (this.tags.length === 0) {
          this.productDetails = [];
        }

        if (this.formMode == Const.FormMode.Edit) {
          for (let i in this.product.Products) {
            if (this.product.Products[i].ProductId == id) {
              this.product.Products[i].EntityState = Enum.EditMode.Delete;
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * clone danh sách hàng hóa con
     * createdBy: namnguyen(22/01/2022)
     */
    cloneProductDetails() {
      this.productDetails = JSON.parse(JSON.stringify(this.product.Products));
    },

    /**
     * hàm cập nhập giá trị input
     * createdBy: namnguyen(22/01/2022)
     */
    changeInputProductDetail(id) {
      try {
        if (this.formMode == Const.FormMode.Edit) {
          if (id != null) {
            for (let i in this.productDetails) {
              if (this.productDetails[i].ProductId == id) {
                this.productDetails[i].EntityState = Enum.EditMode.Upadate;
                break;
              }
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    //#endregion

    //#region save data

    /**
     * hàm lấy mã hàng hóa từ api
     * createdBy: namnguyen(20/01/2022)
     */
    getNewProductCodeSKUFromApi(text) {
      try {
        if (text && this.formMode == Const.FormMode.Add) {
          text = text.normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/[^a-zA-Z ]/g, "");
          BaseRequest.get(
            `${this.apiRouter}/NewProductCodeSKU?inputText=${text}`
          )
            .then((res) => {
              this.product.ProductCodeSKU = res.data;
            })
            .catch((res) => {
              console.log(res);
            });
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * sự kiện click vào submit button
     * createdBy: namnguyen(24/01/2022)
     */
    onClickSubmit() {
      try {
        //debugger; // eslint-disable-line
        let me = this;
        me.submitted = true;
        // validate dữ liệu
        me.$v.$touch();
        if (me.$v.$invalid) {
          // nếu dữ liệu không hợp lệ --> đưa ra thông báo
          me.validateData();
        } else {
          if (!me.checkDuplicate()) {
            console.log("..")
          } else {
            // dữ liệu hợp lệ --> add/update
            //debugger; // eslint-disable-line
            if (this.formMode == Const.FormMode.Add) {
              me.product = this.cleanData(me.product);
              BaseRequest.post(me.apiRouter, me.product)
                .then(() => {
                  this.$emit("getAllProducts");
                  this.submitted = false;
                  this.hideFormData();
                  this.$emit("showToastMsg");
                })
                .catch(function (res) {
                  const status = res.response.status;
                  switch (status) {
                    case 400:
                      // hiển thị thông báo cho user
                      console.log(res.response);
                      me.$emit("showPopupDanger", res.response.data.userMsg[0]);
                      break;
                    default:
                      me.$emit("showPopupDanger", "Có lỗi xảy ra. Vui lòng thử lại.");
                      break;
                  }
                });
            } else {
              // update data

              me.product = this.cleanData(me.product);

              BaseRequest.put(me.apiRouter + "/" + me.productId, me.product)
                .then(() => {
                  this.$emit("getAllProducts");
                  this.submitted = false;
                  this.hideFormData();
                  this.$emit("showToastMsg");
                })
                .catch(function (res) {
                  console.log(res.response.data);
                  const status = res.response.status;
                  switch (status) {
                    case 400:
                      // hiển thị thông báo lỗi cho người dùng
                      me.$emit("showPopupDanger", res.response.data.userMsg[0]);
                      break;
                    default:
                      break;
                  }
                });
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * clean dữ liệu để gửi lên api
     * createdBy: namnguyen(24/01/2022)
     */
    cleanData(product) {
      try {
        // định dạng dữ liệu giá cả
        product.PurchasePrice = this.convertString2Int(product.PurchasePrice);
        product.SellingPrice = this.convertString2Int(product.SellingPrice);
        // định dạng dữ liệu thuộc tính
        product.ProductProperty = this.tags.toString();
        if (product.ShowStatus == false) {
          product.ShowStatus = 0;
        } else {
          product.ShowStatus = 1;
        }

        if (this.formMode == Const.FormMode.Add) {
          product.Products = this.productDetails;
        } else {
          if (this.product.Products != null) {
            for (let i in this.product.Products) {
              console.log(this.product.Products[i].EntityState);
              if (
                this.product.Products[i].EntityState === Enum.EditMode.Delete
              ) {
                this.productDetails.push(this.product.Products[i]);
              }
            }
          }
          product.Products = this.productDetails;
        }

        if (product.Products) {
          product.Products.forEach((pro) => {
            if (pro.ProductId == null) {
              delete pro.ProductId;
            }
            pro.PurchasePrice = this.convertString2Int(pro.PurchasePrice);
            pro.SellingPrice = this.convertString2Int(pro.SellingPrice);
          });
        }
        return product;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * chuyuển dạng string sang int
     */
    convertString2Int(str) {
      try {
        let result = 0;
        if (str !== null) {
          result = parseInt(str.replace(".", ""));
        }
        return result;
      } catch (error) {
        return parseInt(str);
      }
    },
    /**
     * validate dữ liệu
     * createdBy: namnguyen(22/01/2022)
     */
    validateData() {
      // check tên hàng hóa
      if (!this.$v.product.ProductName.required) {
        return;
      }
    },

    /**
     * check trùng dữ liệu trên giao diện và server
     * createdBy: namnguyen(22/01/2022)
     */
    checkDuplicate() {
      let result = true;
      let me = this;
      try {
        // mảng tạm chứa mã vạch
        let arrBarCode = [];
        // mảng tạm chứa code sku
        let arrCodeSKU = [];

        // thêm code sku hàng hóa cha vào mảng
        arrCodeSKU.push(me.product.ProductCodeSKU);

        if (me.productDetails) {
          me.productDetails.forEach((pro) => {
            arrCodeSKU.push(pro.ProductCodeSKU);
            if (pro.ProductBarCode) {
              arrBarCode.push(pro.ProductBarCode);
            }
          });
        }

        // check mã code sku trên giao diện
        if (
          me.checkDuplicateItemInArr(
            Resource.NameProperty.ProductCodeSKU,
            arrCodeSKU
          )
        ) {
          result = false;
          return;
        }

        // check mã vạch trên giao diện
        if (
          result &&
          me.checkDuplicateItemInArr(
            Resource.NameProperty.ProductBarCode,
            arrBarCode
          )
        ) {
          result = false;
        }
      } catch (error) {
        result = false;
        console.log(error);
      }
      return result;
    },

    /**
     * check mảng có dữ liệu trùng không
     * createdBy: namnguyen(24/01/2022)
     */
    checkDuplicateItemInArr(titile, arr) {
      try {
        let result = false;
        let me = this;
        let duplicateCodeSKUs = arr.filter(
          (item, index) => index !== arr.indexOf(item)
        );
        if (duplicateCodeSKUs.length > 0) {
          me.$emit(
            "showPopupDanger",
            Resource.PopUp.TitleDuplicateWithParam(
              titile,
              duplicateCodeSKUs.toString()
            )
          );
          result = true;
        }
        return result;
      } catch (error) {
        console.log(error);
      }
    },

    //#endregion

    //#region upload ảnh
    onFileSelected(event) {
      try {
        let me = this;
        this.selectedFile = event.target.files[0];
        // let maxImgSize = Const.MaxImgSize;
        if (this.selectedFile.size > Const.MaxImgSize) {
          me.$emit(
            "showPopupDanger",
            Resource.Message.ValidateNotValid.OverSizeImg
          );
          return;
        }
        let data = new FormData();
        data.append("Files", this.selectedFile);
        BaseRequest.post(`ProductImages/UploadImage`, data)
          .then(function (res) {
            me.product.ProductImageId = res.data;
          })
          .catch(function (e) {
            console.log(e);
          });
      } catch (error) {
        console.log(error);
      }
    },

    //#endregion
  },
  directives: {
    focus: {
      inserted: (el) => {
        el.focus();
      },
    },
  },
};