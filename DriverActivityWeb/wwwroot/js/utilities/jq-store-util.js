const storeUtil = (function (options) {

    const SECRECT_KEY = 'ert@#&^%0906fhgjkhljlSDKJLJ88215';

    const STORE_KEY = {
        TOKEN: '_TOKEN',
        USERNAME: '_USER_NAME',
        USER_ID:'_USER_ID'
    };

    const setStore = (key, value, isEncode, isParse) => {
        if (!value) return;
        isEncode = isEncode ?? false;
        isParse = isParse ?? true;
        if (isEncode) {
            const encrypted = CryptoJS.AES.encrypt(value, SECRECT_KEY);
            window.localStorage.setItem(key, encrypted);
            return;
        }
        else {
            let parsData = value;
            if (isParse) {
                parsData  = JSON.stringify(value);
            }
            window.localStorage.setItem(key, parsData);
            return;
        }
    };

    const getStore = (key, isEncode, isParse) => {
        const data = window.localStorage.getItem(key);
        if (!data) return null;

        isEncode = isEncode ?? false;
        isParse = isParse ?? true;
        if (isEncode) {
            const decrypted = CryptoJS.AES.decrypt(data, SECRECT_KEY);
            return decrypted;
        }
        else {
            let parsData = data;
            if (isParse) {
                return JSON.parse(parsData);
            }
            return parsData;
        }
    };

    const clearAll = () => {
        window.localStorage.clear();
    };


    //const encrypted = CryptoJS.AES.encrypt("Message", SECRECT_KEY);

    //const decrypted = CryptoJS.AES.decrypt(encrypted, SECRECT_KEY);


    let result = {};
    result.setStore = setStore;
    result.getStore = getStore;
    result.STORE_KEY = STORE_KEY;
    result.clearAll = clearAll;


    return result;

})();