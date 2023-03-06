const axios = require('axios');

const apiUrl = 'https://localhost:44348/api/v1/'

export default {
    
    /**
     * lấy dữ liệu từ api
     * @param {*} url đường dẫn
     * @returns dữ liệu trả về từ api
     * createdBy: namnguyen(17/01/2022)
     */
    get(url){
        return axios.get(apiUrl + url)
    },

    /**
     * gửi dữ liệu lên api để thêm mới
     * @param {*} url đường dẫn
     * @param {object} data dữ liểu đẩy lên
     * @returns mã code và message từ api
     * createdBy: namnguyen(17/01/2022)
    */
    post(url, data){
        return axios.post(
            apiUrl + url,
            data
        )
    },

    /**
     * gửi dữ liệu lên api để cập nhập
     * @param {*} url đường dẫn
     * @param {object} data dữ liểu đẩy lên
     * @returns mã code và message từ api
     * createdBy: namnguyen(17/01/2022)
    */
    put(url, data){
        return axios.put(
            apiUrl + url,
            data
        )
    },

    /**
     * xóa dữ liệu
     * @param {*} url đường dẫn
     * @param {string} id id dữ liệu
     * @returns mã code và message từ api
     * createdBy: namnguyen(17/01/2022)
    */
    delete(url, id){
        return axios.delete(
            apiUrl + url + '/' + id
        )
    },
    
    apiUrl : apiUrl


};