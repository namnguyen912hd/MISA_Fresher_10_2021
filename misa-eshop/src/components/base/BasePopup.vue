<template>
  <div class="m-popup-box" :class="{ 'm-popup-show': isShowPopup }">
    <div class="m-popup">
        <div class="m-popup-header"> 
            <div v-if="popup.Status == 'Warning'">Xóa dữ liệu</div>
            <div v-if="popup.Status == 'Danger'">MISA Eshop</div>
            <div @click="onClickClose()" class="m-icon m-icon-16 m-close-popup"></div>
        </div>
        <div class="m-mess-line"></div>
      <div class="m-popup-content">
          <div
            class="m-popup-icon m-icon-warning"
            v-if="popup.Status == 'Warning'"
          ></div>
          <div
            class="m-popup-icon m-icon-danger"
            v-if="popup.Status == 'Danger'"
          ></div>
          <!-- Messenger -->
          <div class="m-messenger" v-if="popup.Status == 'Warning'">
            Bạn có chắc chắn muốn xóa hàng hóa [{{ popup.Title }}] không?
          </div>

          <div class="m-messenger" v-if="popup.Status == 'Danger'">
            {{ popup.Title }}
          </div>

        
      </div>
      <div class="m-mess-line"></div>
      <div class="m-popup-bottom">
        <div v-if="popup.Status == 'Warning'" class="m-flex-between">
          <!-- Button close -->
          <button class="m-btn m-btn-cancel" @click="onClickClose()" style="
                display: flex;
                align-items: center;
                font-size: 13px;
                width: 66px;
                padding: 5px 3px;
                margin-right: 10px;
              ">
            <div
              class="m-icon m-icon-16 m-icon-cancel"
              
            ></div>
            Hủy bỏ
          </button>
          <!-- Button comfirm -->
          <button class="m-btn m-btn-delete" @click="deleteProduct()">
            <i class="fa fa-trash-o" aria-hidden="true"></i>
            Xóa
          </button>
        </div>

        <div v-if="popup.Status == 'Danger'" class="m-flex-end">
           <button class="m-btn" @click="onClickClose()">
            Đồng ý
          </button>
        </div>
      </div>
    </div>
    <div class="m-popup-background"></div>
  </div>
</template>

<script>

export default {
  data() {
    return {
      icon: "",
    };
  },

  props: ["isShowPopup", "popup"],

  methods: {
    /**
     * xóa nhiều
     * createdBy: namnguyen (24/12/2021)
     */
    deleteProduct() {
      this.$emit("deleteProduct");
    },

    /**
     * sự kiện click vào button 'hủy'
     * createdBy: namnguyen (23/12/2021)
     */
    onClickClose() {
      this.$emit("onClickClosePopup");
    },

    
  },
};
</script>