<template>
  <!-- toast message -->

    <div class="m-toast-box" v-if="isShowToastMsg">
      <div class="m-toast-msg">
        <div class="m-toast m-toast-icon">
          <i class="fa fa-check-circle" aria-hidden="true" ></i>
        </div>
        <div class="m-toast-text">{{ toastMsg.Title }}</div>
        <div class="m-toast-close"><i class="fa fa-times" aria-hidden="true"></i></div>
      </div>
    </div>

</template>

<script>
//import { eventBus } from "../../main";
export default {
  data() {
    return {
      
    };
  },
  created(){

  },
  props:[
      "isShowToastMsg",
      "toastMsg",
      "showToastMsg"
  ]
  ,
  methods: {

    /**
     * hiển thị toast message
     * createdBy: namnguyen (12/12/2021)
     */
    showToastMessage: function() {
      this.$emit("showToastMsg");
    },

  },
  
};
</script>

<style scoped>
 @import url('../../assets/css/component/toastmsg.css');
</style>
