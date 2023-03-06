<template>
  <div v-if="isLoading">
    <div class="m-modal"></div>
    <div class="lds-spinner">
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
    <!-- <div>Đang load dữ liệu</div> -->
  </div>
</template>

<script>
export default {
  name: "BaseLoading",
  props: {
    isLoading: {
      Type: Boolean,
      default: false,
    },
  },
};
</script>

<style scoped>
@import url("../../assets/css/component/loading.css");
</style>