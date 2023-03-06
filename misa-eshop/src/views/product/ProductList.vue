<template>
  <div>
    <div class="m-content" v-show="isShowDetail == false">
      <div class="m-content-action">
        <div class="m-action-add" @click="onClickBtnAdd()"
          v-shortkey="['ctrl', '1']" @shortkey="onClickBtnAdd()"
          title="Ctrl 1">
          <div class="m-icon m-icon-16 m-add-icon"></div>
          Thêm mới
          <i
            style="
              padding-left: 35px;
              margin-top: -5px;
              padding-right: 0;
              font-size: 10px;
            "
            class="fa fa-sort-desc"
            aria-hidden="true"
          ></i>
        </div>
        <div
          class="m-action-clone"
          @click="onClickBtnClone()"
          :class="{ 'm-disable': selectedProducts.length > 1 }"
        >
          <div class="m-icon m-icon-16 m-clone-icon"></div>
          Nhân bản
        </div>
        <div
          @click="onClickBtnUpdate()"
          class="m-action-update"
          :class="{ 'm-disable': selectedProducts.length > 1 }"
          v-shortkey="['ctrl', 'e']" @shortkey="onClickBtnUpdate()"
          title="Ctrl E"
        >
          <div class="m-icon m-icon-16 m-update-icon"></div>
          Sửa
        </div>
        <div class="m-action-delete" @click="onClickBtnDelete()"
          v-shortkey="['ctrl', 'd']" @shortkey="onClickBtnDelete()"
          title="Ctrl D">
          <!-- :class="{ 'm-disable': selectedProducts.length < 1 }" -->
          <div class="m-icon m-icon-16 m-delete-icon"></div>
          Xóa
        </div>
        <div class="m-action-refresh" @click="btnRefreshOnClick()"
          v-shortkey="['ctrl', 'y']" @shortkey="btnRefreshOnClick()"
          title="Ctrl Y">
          <div class="m-icon m-icon-16 m-refresh-icon"></div>
          Nạp
        </div>
      </div>

      <div class="m-content-grid">
        <!-- --table -->
        <table class="m-table">
          <thead class="m-text-left">
            <tr>
              <th style="min-width: 17px">
                <label class="m-checkbox-input">
                  <input
                    type="checkbox"
                    @click="onClickCheckAll()"
                    v-model="isCheckAll"
                  />
                  <span class="m-checkbox-checked"></span>
                </label>
              </th>
              <th style="min-width: 114px">
                <div class="m-thead-content" style="min-width: 112px">
                  <div
                    class="m-thead-name"
                    @click="sorting('ProductCodeSKU', sortOrder)"
                  >
                    Mã SKU
                  </div>

                  <input-filter
                    :column="'ProductCodeSKU'"
                    :valueType="'String'"
                    @onChangeInputValue="onChangeInputValue"
                  >
                  </input-filter>
                </div>
              </th>
              <th style="min-width: 299px">
                <div class="m-thead-content" id="thProductName">
                  <div
                    class="m-thead-name"
                    @click="sorting('ProductName', sortOrder)"
                  >
                    Tên hàng hóa
                    <i
                      v-if="isClicked"
                      style="color: #919191; padding-left: 5px"
                      class="fa fa-long-arrow-up"
                      aria-hidden="true"
                    ></i>
                    <i
                      v-else
                      style="color: #919191; padding-left: 5px"
                      class="fa fa-long-arrow-down"
                      aria-hidden="true"
                    ></i>
                  </div>
                  <input-filter
                    :column="'ProductName'"
                    :valueType="'String'"
                    @onChangeInputValue="onChangeInputValue"
                  >
                  </input-filter>
                </div>
              </th>
              <th style="min-width: 114px">
                <div class="m-thead-content">
                  <div
                    class="m-thead-name"
                    @click="sorting('ProductCategoryName', sortOrder)"
                  >
                    Nhóm hàng hóa
                  </div>
                  <input-filter
                    :column="'ProductCategoryName'"
                    :valueType="'String'"
                    @onChangeInputValue="onChangeInputValue"
                  >
                  </input-filter>
                </div>
              </th>
              <th>
                <div class="m-thead-content">
                  <div
                    class="m-thead-name"
                    @click="sorting('CalculationName', sortOrder)"
                  >
                    Đơn vị tính
                  </div>
                  <input-filter
                    :column="'CalculationName'"
                    :valueType="'String'"
                    @onChangeInputValue="onChangeInputValue"
                  >
                  </input-filter>
                </div>
              </th>
              <th style="min-width: 108px">
                <div class="m-thead-content">
                  <div
                    class="m-thead-name"
                    @click="sorting('PurchasePrice', sortOrder)"
                  >
                    Giá mua
                  </div>
                  <input-filter
                    :column="'PurchasePrice'"
                    :valueType="'Int32'"
                    @onChangeInputValue="onChangeInputValue"
                  >
                  </input-filter>
                </div>
              </th>
              <th style="min-width: 109px">
                <div class="m-thead-content">
                  <div
                    class="m-thead-name"
                    @click="sorting('SellingPrice', sortOrder)"
                    title="Giá bán trung bình"
                  >
                    Giá bán TB
                  </div>
                  <input-filter
                    :column="'SellingPrice'"
                    :valueType="'Int32'"
                    @onChangeInputValue="onChangeInputValue"
                  >
                  </input-filter>
                </div>
              </th>

              <th style="min-width: 154px">
                <div class="m-thead-content" style="width: 176px">
                  <div
                    style="padding-bottom: 8px"
                    class="m-thead-name"
                    title="Hiển thị trên màn hình bán hàng"
                  >
                    Hiển thị trên MH bán hàng
                  </div>
                  <div class="m-thead-action m-cbx-show">
                    <v-select
                      :options="showOptions"
                      label="showOptionName"
                      :reduce="(option) => option.showOptionValue"
                      v-model="showValue"
                    >
                      <template #list-header> </template>
                      <template v-slot:option="option">
                        {{ option.showOptionName }}
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
                  </div>
                </div>
              </th>

              <th style="width: 114px">
                <div class="m-thead-content">
                  <div class="m-thead-name" style="padding-bottom: 8px">
                    Trạng thái
                  </div>
                  <div class="m-thead-action m-cbx-state">
                    <v-select
                      :options="stateOptions"
                      label="stateOptionName"
                      :reduce="(option) => option.stateOptionValue"
                      v-model="stateValue"
                    >
                      <template #list-header> </template>
                      <template v-slot:option="option">
                        {{ option.stateOptionName }}
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
                  </div>
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="product in productList"
              :key="product.ProductId"
              @dblclick="onDblClickRow(product.ProductId)"
              @click="
                onClickRowActive(
                  product.ProductId,
                  product.ProductName,
                  product.ProductCodeSKU
                )
              "
              :class="{ 'row-active': product.ProductId == productId }"
            >
              <td>
                <label class="m-checkbox-input">
                  <input
                    type="checkbox"
                    v-bind:value="product.ProductId"
                    v-model="selectedProducts"
                    @change="
                      updateCheckAll(
                        product.ProductName,
                        product.ProductCodeSKU
                      )
                    "
                  />
                  <span class="m-checkbox-checked"></span>
                </label>
              </td>
              <td>{{ product.ProductCodeSKU }}</td>
              <td>
                <span
                  class="m-tr-action"
                  @click="onDblClickRow(product.ProductId)"
                  >{{ product.ProductName }}</span
                >
              </td>
              <td>{{ product.ProductCategoryName }}</td>
              <td>{{ product.CalculationName }}</td>
              <td class="m-text-right">
                {{ product.PurchasePrice | formatNumber2Currency }}
              </td>
              <td class="m-text-right">
                {{ product.SellingPrice | formatNumber2Currency }}
              </td>
              <td>{{ product.ShowStatus | covertShowName }}</td>

              <td>{{ product.BusinessStatus | covertBusinessStateName }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="m-content-pagination">
        <div class="m-pagination-left">
          <div
            class="m-icon m-icon-24 m-icon-first"
            :class="{ 'm-disable': pageIndex < 2 }"
            @click="pageIndex = 1"
          ></div>
          <div
            class="m-icon m-icon-24 m-icon-prev"
            :class="{ 'm-disable': pageIndex < 2 }"
            @click="pageIndex--"
          ></div>
          <div class="m-input-pagesize">
            Trang
            <input
              class="m-input"
              v-mask="'#####'"
              type="text"
              v-model="txtPageIndex"
              @keyup.enter="changeInputPageIndex()"
            />
            trên {{ totalPage }}
          </div>
          <div
            class="m-icon m-icon-24 m-icon-next"
            :class="{ 'm-disable': pageIndex >= totalPage }"
            @click="pageIndex++"
          ></div>
          <div
            class="m-icon m-icon-24 m-icon-end"
            :class="{ 'm-disable': pageIndex >= totalPage }"
            @click="pageIndex = totalPage"
          ></div>
          <div
            class="m-icon m-icon-24 m-icon-refresh-24"
            @click="getAllProducts()"
          ></div>
          <div class="m-pagesize-container" style="position: relative">
            <div class="m-select-pagesize" @click="showPagesize()">
              <div class="m-display-pagesize">
                <span style="width: 22px">{{ pageSize }}</span>
                <i class="fa fa-angle-down" aria-hidden="true"></i>
              </div>
            </div>
            <div class="m-option-pagesize" v-if="isShowPageSize">
              <div class="m-option" @click="selectPagesize(100)">100</div>
              <div class="m-option" @click="selectPagesize(50)">50</div>
              <div class="m-option" @click="selectPagesize(30)">30</div>
              <div class="m-option" @click="selectPagesize(15)">15</div>
            </div>
          </div>
        </div>
        <div class="m-pagination-right">
          <div v-if="totalRecord > 0">
            Hiển thị {{ pageIndex * pageSize - pageSize + 1 }} -
            {{
              pageSize * pageIndex > totalRecord
                ? totalRecord
                : pageSize * pageIndex
            }}
            trên {{ totalRecord }} kết quả
          </div>
        </div>
      </div>

      <!-- toast messege -->
      <base-toast-msg
        :isShowToastMsg="isShowToastMsg"
        :toastMsg="toastMsg"
        @showToastMsg="showToastMsg"
      >
      </base-toast-msg>

      <base-loading :isLoading="isLoading"></base-loading>
    </div>
    <!-- form product -->
    <product-detail
      :apiRouter="apiRouter"
      :productId="productId"
      :isShowDetail="isShowDetail"
      :product="product"
      :formMode="formMode"
      @hideProductDetail="hideProductDetail"
      @getAllProducts="getAllProducts"
      @showToastMsg="showToastMsg"
      @showPopupDanger="showPopupDanger"
    >
    </product-detail>

    <!-- pop up -->
    <base-popup
      :isShowPopup="isShowPopup"
      :popup="popup"
      @onClickClosePopup="hidePopup"
      @deleteProduct="deleteProduct"
    >
    </base-popup>
  </div>
</template>

<script>
import ProductList from './js/productlist';
export default ProductList;
</script>

<style scoped>

</style>
