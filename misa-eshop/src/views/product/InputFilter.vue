<template>
  <div class="m-filter">
    <div class="m-btn m-btn-context">    
      <v-select
        class="m-modal-col-6"
        :get-option-label="(option) => option.Name"
        :options="operators"
        :reduce="(option) => option.Value"
        v-model="operator"
        @input="sltOperatorOnChange($event)"
      >
        <template #option="{ Show }">
          {{ Show }}
        </template>
      </v-select>
    </div>
    <input
      type="text"
      class="m-input m-input-thead"
      v-model="inputValue"
      @keyup.enter="onChangeInputValue"
    />
  </div>
</template>
<script>
import InputFilter from "./js/inputfilter"
export default InputFilter;
</script>

<style lang="css">
    @import url('./css/inputfilter.css');
</style>
