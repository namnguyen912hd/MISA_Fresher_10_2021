/**
* formatting number to currency
* @param {number} number 
* createdBy: namnguyen (05/11/2021) 
*/
import Vue from 'vue'

Vue.filter('formatNumber2Currency', function (value) {
    if (value) {
        // convert to string
        let result = value + "" ; 
        result = result.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
        return result;
    }
    return ""
  })
