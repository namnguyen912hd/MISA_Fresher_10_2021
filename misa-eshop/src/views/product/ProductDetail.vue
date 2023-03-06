<template>
  <div>
    <div class="m-content m-content-detail" v-if="isShowDetail">
      <div
        class="m-content-action m-detail-action"
        style="border-bottom: 1px solid #ddd"
      >
        <button class="m-btn m-btn-save" @click="onClickSubmit()"
          v-shortkey="['ctrl', 's']" @shortkey="onClickSubmit()"
          title="Ctrl S">
          <div class="m-icon m-icon-16 m-icon-save"></div>
          Lưu
        </button>
        <button class="m-btn m-btn-cancel" @click="hideFormData()"
          v-shortkey="['ctrl', 'b']" @shortkey="hideFormData()"
          title="Ctrl B">
          <div class="m-icon m-icon-16 m-icon-cancel"></div>
          Hủy bỏ
        </button>
      </div>

      <div class="m-content-grid m-detail-info">
        <form>
          <div class="m-info m-info-basic">
            <div class="m-title">Thông tin cơ bản</div>

            <div class="m-info-item">
              <div class="m-info-name">Trạng thái kinh doanh</div>
              <div class="m-info-content">
                <label class="container">
                  <input
                    id="rdbussiness"
                    type="radio"
                    value="1"
                    v-model="product.BusinessStatus"
                    name="business"
                    checked="checked"
                  />
                  <div class="checkmark">
                    <div></div>
                  </div>
                </label>
                <label style="padding: 0 5px" for="rdbussiness"
                  >Đang kinh doanh</label
                >

                <label class="container">
                  <input
                    id="rdnotbussiness"
                    type="radio"
                    value="0"
                    v-model="product.BusinessStatus"
                    name="business"
                  />
                  <div class="checkmark">
                    <div></div>
                  </div>
                </label>
                <label style="padding: 0 5px" for="rdnotbussiness"
                  >Ngừng kinh doanh</label
                >
              </div>
            </div>

            <div class="m-info-item">
              <div class="m-info-name">
                Tên hàng hóa <span style="color: red">*</span>
              </div>
              <div class="m-info-content">
                <input
                  class="m-input"
                  type="text"
                  v-model="product.ProductName"
                  @blur="getNewProductCodeSKUFromApi(product.ProductName)"
                  ref="txtProductName"
                  :class="{
                    'm-form-error': submitted && $v.product.ProductName.$error,
                  }"
                />
                <div
                  class="m-icon m-icon-16 m-icon-exclam"
                  v-if="submitted && $v.product.ProductName.$error"
                  v-tooltip.right="{
                    content:
                      submitted && $v.product.ProductName.$error
                        ? 'Trường này không được để trống'
                        : null,
                  }"
                ></div>
              </div>
            </div>

            <div class="m-info-item">
              <div class="m-info-name">Nhóm hàng hóa</div>
              <div class="m-info-content">
                <v-select
                  :options="productCategories"
                  label="ProductCategoryName"
                  :reduce="(option) => option.ProductCategoryId"
                  v-model="product.ProductCategoryId"
                >
                  <template #list-header> </template>
                  <template v-slot:option="option">
                    {{ option.ProductCategoryName }}
                  </template>
                  <template v-slot:no-options="{ search, searching }">
                    <template v-if="searching">
                      Không tìm thấy giá trị <em>{{ search }}</em
                      >.
                    </template>
                    <em v-else style="opacity: 0.5">Điền vào ô để tìm kiếm</em>
                  </template>
                </v-select>
                <div class="m-btn m-btn-quickadd">
                  <div class="m-bg-image"></div>
                </div>
              </div>
            </div>

            <div class="m-info-item">
              <div class="m-info-name">Mã SKU</div>
              <div class="m-info-content">
                <input
                  class="m-input"
                  type="text"
                  v-model="product.ProductCodeSKU"
                />
              </div>
            </div>

            <div class="m-info-item">
              <div class="m-info-name" style="display: flex">
                Giá mua
                <div
                  class="m-icon m-icon-16 m-icon-question"
                  title="Giá mua gần nhất"
                ></div>
              </div>
              <div class="m-info-content">
                <money
                  class="m-input m-input-price m-text-right"
                  v-model="product.PurchasePrice"
                  v-bind="money"
                  :class="{ 'm-disable-input': tags.length > 0 }"
                ></money>
              </div>
            </div>

            <div class="m-info-item">
              <div class="m-info-name">Giá bán</div>
              <div class="m-info-content">
                <money
                  class="m-input m-input-price m-text-right"
                  v-model="product.SellingPrice"
                  v-bind="money"
                  :class="{ 'm-disable-input': tags.length > 0 }"
                ></money>
              </div>
            </div>

            <div class="m-info-item">
              <div class="m-info-name">Đơn vị tính</div>
              <div class="m-info-content">
                <div class="m-info-content">
                  <v-select
                    :options="calculationUnits"
                    label="CalculationUnitName"
                    :reduce="(option) => option.CalculationUnitId"
                    v-model="product.CalculationUnitId"
                  >
                    <template #list-header> </template>
                    <template v-slot:option="option">
                      {{ option.CalculationUnitName }}
                    </template>
                    <template v-slot:no-options="{ search, searching }">
                      <template v-if="searching">
                        Không tìm thấy giá trị <em>{{ search }}</em
                        >.
                      </template>
                      <em v-else style="opacity: 0.5"
                        >Điền vào ô để tìm kiếm</em
                      >
                    </template>
                  </v-select>
                  <div class="m-btn m-btn-quickadd">
                    <div class="m-bg-image"></div>
                  </div>
                </div>
              </div>
            </div>

            <div class="m-info-item" style="display: flex; align-items: unset">
              <label class="m-checkbox-input">
                Hiển thị trên màn hình bán hàng
                <input
                  type="checkbox"
                  checked="checked"
                  value="1"
                  v-model="product.ShowStatus"
                />
                <span class="m-checkbox-checked"></span>
              </label>
              <div
                class="m-icon m-icon-16 m-icon-question"
                title="Hàng hóa được bán trực tiếp"
              ></div>
            </div>
          </div>

          <div class="m-info m-info-property">
            <div class="m-title">Thông tin thuộc tính</div>

            <div class="m-info-item">
              <div class="m-info-name">Thuộc tính</div>
              <div class="m-info-content" style="height: auto">
                <div class="m-info-color">
                  <span style="margin-left: 10px; height: auto">Màu sắc</span>
                </div>
                <div class="m-tags-container">
                  <div
                    class="m-tag"
                    v-for="(tag, index) in tags"
                    :key="'m-tag' + index"
                  >
                    <span
                      v-if="activeTag !== index"
                      @click="activeTag = index"
                      >{{ tag }}</span
                    >
                    <input
                      class="m-input"
                      v-else
                      v-model="tags[index]"
                      v-focus
                      :style="{ width: tag.length + 'ch' }"
                      @keyup.enter="activeTag = null"
                      @blur="activeTag = null"
                    />
                    <span @click="removeTag(index)"
                      ><div class="m-icon-removetag"></div
                    ></span>
                  </div>
                  <input v-model="tagValue" @keyup.enter="addTag" />
                </div>
              </div>
            </div>

            <div
              class="m-info-item"
              style="align-items: unset"
              v-if="tags.length > 0"
            >
              <div class="m-info-name">Chi tiết thuộc tính</div>
              <div class="m-info-content" style="width: 88%">
                <div class="m-property-container">
                  <table class="m-table">
                    <thead class="m-text-left">
                      <tr>
                        <th style="min-width: 300px">Tên hàng hóa</th>
                        <th style="min-width: 140px">Mã SKU</th>
                        <th style="min-width: 140px">Mã vạch</th>
                        <th style="min-width: 110px">Giá mua</th>
                        <th style="min-width: 200px">Giá bán</th>
                        <th style="width: 40px"></th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr
                        v-for="(product, index) in productDetails"
                        :key="index"
                      >
                        <td>
                          <input
                            @keyup.enter="
                              changeInputProductDetail(product.ProductId)
                            "
                            type="text"
                            tabindex="-1"
                            class="m-input"
                            v-model="product.ProductName"
                          />
                        </td>
                        <td>
                          <input
                            @keyup.enter="
                              changeInputProductDetail(product.ProductId)
                            "
                            type="text"
                            tabindex="-1"
                            class="m-input"
                            v-model="product.ProductCodeSKU"
                          />
                        </td>
                        <td>
                          <input
                            @keyup.enter="
                              changeInputProductDetail(product.ProductId)
                            "
                            type="text"
                            tabindex="-1"
                            class="m-input"
                            v-model="product.ProductBarCode"
                          />
                        </td>

                        <td class="m-text-right">
                          <money
                            @keyup.enter.native="
                              changeInputProductDetail(product.ProductId)
                            "
                            tabindex="-1"
                            class="m-input m-input-price m-text-right"
                            v-model="product.PurchasePrice"
                            v-bind="money"
                          ></money>
                        </td>
                        <td class="m-text-right">
                          <money
                            @keyup.enter.native="
                              changeInputProductDetail(product.ProductId)
                            "
                            tabindex="-1"
                            class="m-input m-input-price m-text-right"
                            v-model="product.SellingPrice"
                            v-bind="money"
                          ></money>
                        </td>
                        <td class="m-text-center">
                          <i
                            @click="
                              removeProductDetail(index, product.ProductId)
                            "
                            style="color: rgb(250, 49, 49); cursor: pointer"
                            class="fa fa-trash"
                            aria-hidden="true"
                          ></i>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>

          <div class="m-info m-info-plus">
            <div class="m-title">Thông tin bổ sung</div>

            <div class="m-info-item" style="align-items: unset">
              <div class="m-info-name">Mô tả</div>
              <div class="m-info-content">
                <textarea
                  style="padding: 10px; font-size: 13"
                  name=""
                  class="m-content-area"
                  cols="55"
                  rows="5"
                  v-model="product.Description"
                ></textarea>
              </div>
            </div>

            <div class="m-info-item" style="align-items: unset">
              <div class="m-info-name">Ảnh hành hóa</div>
              <div class="m-info-content" style="align-items: unset">
                <div class="m-img-container">
                  <div
                    class="m-img-symbol"
                    v-if="product.ProductImageId == null"
                  >
                    <div class="m-icon m-icon-16 m-icon-selectimg"></div>
                    Biểu tượng
                  </div>
                  <div
                    class="m-img"
                    v-if="product.ProductImageId == null"
                  ></div>
                  <!-- object-fit: contain; -->
                  <img
                    style="width: 180px; height: 169px; margin-bottom: 4px"
                    v-if="product.ProductImageId != null"
                    v-bind:src="
                      'https://localhost:44348/api/v1/ProductImages/Image?id=' +
                      product.ProductImageId
                    "
                    alt=""
                  />
                  <div class="m-input-file">
                    <label for="upload-photo">...</label>
                    <input
                      type="file"
                      name="photo"
                      id="upload-photo"
                      @change="onFileSelected"
                      accept="image/png,image/jpg, image/gif, image/jpeg"
                    />
                  </div>
                </div>
                <div class="m-img-info" style="color: #b3b3b3">
                  <br /><br />
                  - Lựa chọn biểu tượng để thay thế nếu không có ảnh
                  <br /><br />
                  - Định dạng ảnh (.jpg, .jpeg, .png, .gif) và dung lượng nhỏ
                  hơn 5MB
                </div>
              </div>
            </div>
          </div>
        </form>
      </div>

      <div
        class="m-content-action m-detail-action"
        style="border-bottom: 1px solid #ddd"
      >
        <button class="m-btn m-btn-save" @click="onClickSubmit()"
          v-shortkey="['ctrl', 's']" @shortkey="onClickSubmit()"
          title="Ctrl S">
          <div class="m-icon m-icon-16 m-icon-save"></div>
          Lưu
        </button>
        <button class="m-btn m-btn-cancel" @click="hideFormData()"
          v-shortkey="['ctrl', 'b']" @shortkey="hideFormData()"
          title="Ctrl B">
          <div class="m-icon m-icon-16 m-icon-cancel"></div>
          Hủy bỏ
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import ProductDetail from "./js/productdetail"
export default ProductDetail;
</script>

<style lang="css" scoped>
</style>
