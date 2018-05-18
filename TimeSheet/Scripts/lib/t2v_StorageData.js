var t2v_StorageData =
    {
        authorizationID: 0,
        GetLocalStorage: function () {
            return window.localStorage;
        },
        SetLocalStorageValue: function (key, value) {

            var localStorage = t2v_StorageData.GetLocalStorage();

            if (localStorage.length > 30) {
                for (i = 0; i < 10; i++) {
                    localStorage.removeItem(localStorage.key(i));
                }
            }
            localStorage.setItem(key, value);
                  
        },
        GetLocalStorageValue: function (key) {
            var localStorage = t2v_StorageData.GetLocalStorage();
            return localStorage.getItem(key);
        },
        DeleteLocalStorageValue: function (key) {
            var localStorage = t2v_StorageData.GetLocalStorage();
            localStorage.removeItem(key);
        },
        SavePageSearchCondition: function (page, searchcondition)
        {
            var localStorage = t2v_StorageData.GetLocalStorage();
            localStorage.removeItem(page);
            localStorage.setItem(page, searchcondition);
        },
        GetPageSearchCondition: function (page)
        {
            var localStorage = t2v_StorageData.GetLocalStorage();
            return localStorage.getItem(page);
        }
    }