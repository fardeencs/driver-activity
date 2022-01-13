const jqClient = (function (options) {


    const BASE_URL = 'https://localhost:7124/',
        LOGIN_URL = '/login/home/authenticate',
        REGISTER_URL = '/login/home/register'
        ;
    this.defaults = {
        showLoading: true,
        loaderContainerId: "#divLoader",
        loaderText: "Loading...",
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            commonUtil.serverError(XMLHttpRequest);
        }
    };
    // Extend defaults into options
    let o = $.extend({}, this.defaults, options);
    let http = {};
    http.Get = httpGet;
    http.Post = httpPost;
    http.DownloadFile = downloadFile;
    http.DownloadFileInChunk = downloadFileInChunk;
    http.DownloadFileStream = downloadStream;
    http.GetBase64 = getBase64;
    http.GetVideoUrl = getVideoUrl;
    http.GetFileUrl = getFileUrl;
    http.ViewFile = viewFile;
    http.Login = login;
    http.Register = register;


    function httpGet({ url, jsonData, onSuccess, showLoading, loaderText, onError, dataType, returnType,
        config, btnLoaderId, containerLoaderId, containerTopPos, containerLeftPos }) {
        config = config ?? {};
        //if (showLoading != null ? showLoading : o.showLoading)
        //    showLoader(loaderText != null ? loaderText : o.loaderText);

        const token = storeUtil.getStore(storeUtil.STORE_KEY.TOKEN, false, false);
        //if (!token) {
        //    window.location.href = LOGIN_URL;
        //    return;
        //}
            //window.location.replace(`${jqClient.BaseUrl}login.html`);

        if (btnLoaderId) {
            commonUtil.btnProgress(btnLoaderId);
        }
        if (containerLoaderId) {
            commonUtil.inlineElementProgress(containerLoaderId, false, containerTopPos, containerLeftPos);
        }

        if (!url)
            return 1; // Incorrect Parameters

        if (onError == null)
            onError = o.error;

        //dataType = dataType || "json";
        //returnType = returnType || 'json';

        if (typeof (jsonData) === "object" && config?.dataType === 'json')
            jsonData = JSON.stringify(jsonData);

        $.ajax({
            type: "GET",
            url,
            cache: false,
            data: jsonData,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', `Bearer ${token}`);
            },
            ...config,
            //dataType: dataType,
            //contentType: function () {
            //    switch (returnType) {
            //        case 'json':
            //            return 'application/json; charset=utf-8';
            //        case 'text':
            //            return 'text/plain';
            //        case 'buffer':
            //            return 'arraybuffer';
            //        case 'html':
            //        default:
            //            return 'text/html';
            //    }
            //}(returnType === 'json' ? 'application/json; charset=utf-8' : ''),
            success: function (response) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                if (containerLoaderId) {
                    commonUtil.inlineElementProgress(containerLoaderId, true, containerTopPos, containerLeftPos);
                }
                onSuccess(response);
            },
            error: function (xhr) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                if (containerLoaderId) {
                    commonUtil.inlineElementProgress(containerLoaderId, true, containerTopPos, containerLeftPos);
                }
                //commonUtil.serverError(xhr);
                onError(xhr);
            }
        });

        return 0; // Successful
    }

    function httpPost({ url, jsonData, showLoading, loaderText, onSuccess, onError, dataType, returnType,
        config, btnLoaderId, containerLoaderId, containerTopPos, containerLeftPos }) {
        config = config ?? {};
        const token = storeUtil.getStore(storeUtil.STORE_KEY.TOKEN, false, false);
        //if (showLoading != null ? showLoading : o.showLoading)
        //    showLoader(loaderText != null ? loaderText : o.loaderText);
        if (btnLoaderId) {
            commonUtil.btnProgress(btnLoaderId);
        }
        if (containerLoaderId) {
            commonUtil.inlineElementProgress(containerLoaderId, false, containerTopPos, containerLeftPos);
        }

        if (!url)
            return 1; // Incorrect Parameters

        if (onError == null)
            onError = o.error;

        //if (!config?.dataType)
        //    config.dataType = 'json';
        //if (!config?.contentType)
        //    config.contentType = "application/json; charset=utf-8";

        returnType = returnType || 'json';

        //dataType = dataType || "json";
        //returnType = returnType || 'json';
        //const type = commonUtil.getDataType(jsonData);
        if (config?.dataType === 'json') {
            config.contentType = "application/json; charset=utf-8";
        }

        if (typeof (jsonData) === "object" && config?.dataType === 'json') {
            jsonData = JSON.stringify(jsonData);
        }

        $.ajax({
            type: "POST",
            url,
            cache: false,
            data: jsonData,
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', `Bearer ${token}`);
            },
            ...config,
            //dataType: dataType,
            //contentType: "application/json; charset=utf-8",
            //contentType: function () {
            //    switch (returnType) {
            //        case 'json':
            //            return 'application/json; charset=utf-8';
            //        case 'text':
            //            return 'text/plain';
            //        case 'buffer':
            //            return 'arraybuffer';
            //        case 'html':
            //        default:
            //            return 'text/html';
            //    }
            //}(returnType === 'json' ? 'application/json; charset=utf-8' : ''),
            //success: onSuccess,
            //error: onError
            success: function (response) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                if (containerLoaderId) {
                    commonUtil.inlineElementProgress(containerLoaderId, true, containerTopPos, containerLeftPos);
                }
                onSuccess(response);
            },
            error: function (xhr) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                if (containerLoaderId) {
                    commonUtil.inlineElementProgress(containerLoaderId, true, containerTopPos, containerLeftPos);
                }
                //commonUtil.serverError(xhr);
                onError(xhr);
            }

        });

        return 0; // Successful
    };

    function login({ username, password, btnLoaderId }) {
        btnLoaderId = btnLoaderId ?? 'btnLogin';
        commonUtil.btnProgress(btnLoaderId);
        $.ajax({
            type: "POST",
            url: LOGIN_URL,
            cache: false,
            data: JSON.stringify({ username, password}),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                if (response && response?.data) {
                    const { id, name, token, username } = response?.data;
                    storeUtil.setStore(storeUtil.STORE_KEY.TOKEN, token, false, false);
                    //storeUtil.setStore(storeUtil.STORE_KEY.USERNAME, username, true);
                    //storeUtil.setStore(storeUtil.STORE_KEY.USER_ID, id + '', true);
                    window.location.href = '/home/index';
                }
            },
            error: function (xhr) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                commonUtil.serverError(xhr);
            }
        });
    };

    function register({ name, username, password, staffId, btnLoaderId }) {
        btnLoaderId = btnLoaderId ?? 'btnRegister';
        commonUtil.btnProgress(btnLoaderId);
        $.ajax({
            type: "POST",
            url: REGISTER_URL,
            cache: false,
            data: JSON.stringify({ userName: username, nameEn: name, staffId, password }),
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                if (response && response?.data) {
                    window.location.href = '/home';
                }
            },
            error: function (xhr) {
                if (btnLoaderId) {
                    commonUtil.btnProgress(btnLoaderId, true);
                }
                commonUtil.serverError(xhr);
            }
        });
    };

    function downloadFile({ url, params, blobName, storageContainerType, fileName, elementId, loaderId, isDownload = true, arg0, arg1, isCssLoader }, callback) {
        if (isCssLoader && loaderId) {
            const isQueued = commonUtil.cssLoader({ appendId: loaderId, id: loaderId });
            if (isQueued) return;
        } else if (!isCssLoader && loaderId) {
            commonUtil.createLoader(loaderId, true);
        }

        let o = $.extend({}, this.defaults, options);
        url = url || '/Blob/GetFile';
        data = params || { blobName, storageContainerType };
        $.ajax({
            type: "GET",
            url,
            data: data,
            xhrFields: {
                responseType: 'blob' // to avoid binary data being mangled on charset conversion
            },
            error: function (xhr) {
                if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                } else if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId, false, true);
                }
                commonUtil.serverError(xhr);
            },
            success: function (blob, status, xhr) {
                if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId);
                } else if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                }
                if (!isDownload) {
                    if (callback) {
                        if (arg0 || arg1) {
                            callback({ blob, arg0, arg1 });
                        } else {
                            callback(blob);
                        }
                        return;
                    }
                }

                // check for a filename
                const filename = fileName || blobName || "";
                const disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    const matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                }

                if (typeof window.navigator.msSaveBlob !== 'undefined') {
                    // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                    window.navigator.msSaveBlob(blob, filename);
                } else {
                    const URL = window.URL || window.webkitURL;

                    const downloadUrl = URL.createObjectURL(blob);
                    commonUtil.downloadFileUrl(downloadUrl, filename);
                }
            }
        });
    }

    function downloadFileInChunk({ url, params, blobName, storageContainerType, fileName, elementId, loaderId, isDownload = true, arg0, arg1, isCssLoader }, callback) {
        if (isCssLoader && loaderId) {
            const isQueued = commonUtil.cssLoader({ appendId: loaderId, id: loaderId });
            if (isQueued) return;
        } else if (!isCssLoader && loaderId) {
            commonUtil.createLoader(loaderId, true);
        }

        let o = $.extend({}, this.defaults, options);
        url = url || '/Blob/GetFileInChunk';
        data = params || { blobName, storageContainerType };
        var last_response_len = false;
        $.ajax({
            type: "GET",
            url,
            data: data,
            //responseType: 'arraybuffer',
            xhrFields: {
                // responseType: 'blob', // to avoid binary data being mangled on charset conversion
                onprogress: function (e) {
                    var this_response, response = e.currentTarget.response;
                    if (last_response_len === false) {
                        this_response = response;
                        last_response_len = response.length;
                    }
                    else {
                        this_response = response.substring(last_response_len);
                        last_response_len = response.length;
                    }
                    //console.log('chunk download', this_response);
                    console.log('chunk download', last_response_len, e.loaded);
                    if (!isDownload) {
                        if (callback) {
                            if (arg0 || arg1) {
                                return callback({ blob: this_response, arg0, arg1, loaded: e.loaded });
                            } else {
                                return callback(this_response);
                            }
                        }
                    }
                }
            }
        })
            .done(function (data) {
                //console.log('Complete response = ' + data);
                console.log('Complete response', data.length);
                if (!isDownload) {
                    if (callback) {
                        if (arg0 || arg1) {
                            //return callback({ blob: data, arg0, arg1 });
                        } else {
                            // return callback(data);
                        }
                    }
                }
            })
            .fail(function (data) {
                console.log('Error: ', data);
            });
    }

    function downloadStream({ url, params, blobName, storageContainerType, fileName, elementId, loaderId, isDownload = true, arg0, arg1, isCssLoader }, callback) {
        if (isCssLoader && loaderId) {
            const isQueued = commonUtil.cssLoader({ appendId: loaderId, id: loaderId });
            if (isQueued) return;
        } else if (!isCssLoader && loaderId) {
            commonUtil.createLoader(loaderId, true);
        }

        let o = $.extend({}, this.defaults, options);
        url = url || '/Blob/GetFileStream';
        //url = url || '/Blob/GetVideoUrl';
        data = params || { blobName, storageContainerType };

        $.ajax({
            type: "GET",
            url,
            data: data,
            xhrFields: {
                responseType: 'blob' // to avoid binary data being mangled on charset conversion
            },
            /* xhrFields: {
                 //responseType: 'blob',
                 onprogress: function (e) {
                     var this_response, response = e.currentTarget?.response;
                     if (last_response_len === false) {
                         this_response = response;
                         last_response_len = response?.length;
                     }
                     else {
                         this_response = response.substring(last_response_len);
                         last_response_len = response?.length;
                     }
                     //console.log('chunk download', this_response);
                     console.log('chunk download', last_response_len, e.loaded);
                     if (!isDownload) {
                         if (callback) {
                             if (arg0 || arg1) {
                                 return callback({ blob: this_response, arg0, arg1, loaded: e.loaded  });
                             } else {
                                 return callback(this_response);
                             }
                         }
                     }
                 }
             }
             */
            error: function (xhr) {
                if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                } else if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId, false, true);
                }
                commonUtil.serverError(xhr);
            },
            success: function (blob, status, xhr) {
                if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId);
                } else if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                }
                const length = xhr.getResponseHeader('Content-Length');
                const fileType = xhr.getResponseHeader('File-Type');
                //console.log('Content-Length', length);
                if (callback) {
                    return callback({ blob, fileType, arg0, arg1 });
                }
            }

        })
        /* .done(function (data) {
             //console.log('Complete response = ' + data);
             console.log('Complete response');
             if (!isDownload) {
                 if (callback) {
                     if (arg0 || arg1) {
                         //return callback({ blob: data, arg0, arg1 });
                     } else {
                         // return callback(data);
                     }
                 }
             }
         })
         .fail(function (data) {
             console.log('Error: ', data);
         });*/


    }

    function getVideoUrl({ url, params, blobName, storageContainerType, fileName, loaderId, arg0, arg1, isCssLoader }, callback) {
        if (isCssLoader && loaderId) {
            const isQueued = commonUtil.cssLoader({ appendId: loaderId, id: loaderId });
            if (isQueued) return;
        } else if (!isCssLoader && loaderId) {
            commonUtil.createLoader(loaderId, true);
        }

        let o = $.extend({}, this.defaults, options);
        url = url || '/Blob/GetVideoUrl';
        data = params || { blobName, storageContainerType };

        $.ajax({
            type: "GET",
            url,
            data: data,

            error: function (xhr) {
                if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                } else if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId, false, true);
                }
                commonUtil.serverError(xhr);
            },
            success: function (result, status, xhr) {
                if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId);
                } else if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                }
                //const length = xhr.getResponseHeader('Content-Length');
                //const fileType = xhr.getResponseHeader('File-Type');
                //const fileType = xhr.getResponseHeader('Content-Type');
                //console.log('Content-Length', length);
                if (callback) {
                    return callback({ data: result, blobName, arg0, arg1 });
                }
            }

        })
        /* .done(function (data) {
             //console.log('Complete response = ' + data);
             console.log('Complete response');
             if (!isDownload) {
                 if (callback) {
                     if (arg0 || arg1) {
                         //return callback({ blob: data, arg0, arg1 });
                     } else {
                         // return callback(data);
                     }
                 }
             }
         })
         .fail(function (data) {
             console.log('Error: ', data);
         });*/


    }

    function getBase64({ url, params, blobName, storageContainerType, elementId, arg0, arg1 }, callback) {
        url = url || '/Blob/GetBase64File';
        data = params || { blobName, storageContainerType };
        /* if (!url)
             return 1; // Incorrect Parameters
             
 
         if (onError == null)
             onError = o.error;
             */

        $.ajax({
            type: "GET",
            url,
            data: data,
            error: function (xhr) {
                callback(xhr);
            },
            success: function (blob, status, xhr) {
                callback({ data: blob, blobName, arg0, arg1 });
            },
            error: callback
        });
    }

    function getFileUrl({ url, params, blobName, storageContainerType, elementId, arg0, arg1 }, callback) {
        url = url || '/Blob/GetFileUrl';
        data = params || { blobName, storageContainerType };
        /* if (!url)
             return 1; // Incorrect Parameters
             
 
         if (onError == null)
             onError = o.error;
             */

        $.ajax({
            type: "GET",
            url,
            data: data,
            error: function (xhr) {
                callback(xhr);
            },
            success: function (result, status, xhr) {
                return callback({ data: result, blobName, arg0, arg1 });
            },
            error: callback
        });
    };

    function showLoader(loaderText) {
        $(o.loaderContainerId).text(loaderText);
        $(o.loaderContainerId).fadeIn("fast");
    };


    function viewFile({ url, params, blobName, storageContainerType, fileName, appendId, loaderId,
        isDownload, arg0, arg1, isCssLoader, loaderClazz, isView, isBlob, isUrl, isPopup, blobUrl }, callback) {
        isPopup = isPopup ?? true;
        if (isCssLoader && loaderId) {
            const isQueued = commonUtil.cssLoader({ appendId: loaderId, id: loaderId, clazz: loaderClazz });
            if (isQueued) return;
        } else if (!isCssLoader && loaderId) {
            commonUtil.createLoader(loaderId, true);
        }

        const icon = commonUtil.getFileIcon(blobName);
        if (icon.trim() === commonUtil.FILES_TYPE.IMAGE)
            isDownload = isDownload ?? false;
        else if (icon.trim() === commonUtil.FILES_TYPE.PDF)
            isDownload = isDownload ?? false;
        else if (icon.trim() === commonUtil.FILES_TYPE.VIDEO)
            isDownload = isDownload ?? false;
        else
            isDownload = true;

        //isDownload = isDownload ?? true;


        if (blobUrl) {
            if (!isDownload) {
                const filename = blobName ?? fileName;
                const modelData = {
                    url: blobUrl,
                    uiFileName: fileName,
                    blobName
                };
                commonUtil.openModel(modelData, filename, false);
            }
            else if (isDownload) {
                const filename = fileName ?? blobName;
                commonUtil.downloadFileUrl(blobUrl, filename);
            }
            if (isCssLoader && loaderId) {
                commonUtil.cssLoader({ id: loaderId, isHide: true });
            } else if (!isCssLoader && loaderId) {
                commonUtil.createLoader(loaderId, false, true);
            }
            return;
        }

        let responseType = 'blob';
        if (isBlob || isUrl) {
            responseType = isBlob ? 'blob' : 'json';
        }
        else {
            if (icon.trim() === commonUtil.FILES_TYPE.IMAGE)
                responseType = 'json';
            else if (icon.trim() === commonUtil.FILES_TYPE.PDF)
                responseType = 'json';
            else if (icon.trim() === commonUtil.FILES_TYPE.VIDEO)
                responseType = 'json';
            else if (icon.trim() === commonUtil.FILES_TYPE.EXCEL)
                responseType = 'json';
            else if (icon.trim() === commonUtil.FILES_TYPE.WORD)
                responseType = 'json';
            /*            else if (icon.trim() === commonUtil.FILES_TYPE.MP3)
                            responseType = 'json';*/
            /*    else if (icon.trim() === commonUtil.FILES_TYPE.TEXT)
                    responseType = 'json';*/
            else
                responseType = 'blob';
        }


        let o = $.extend({}, this.defaults, options);
        url = url ?? (responseType == 'blob' ? '/Blob/GetFile' : '/Blob/GetFileUrl');
        const datestring = commonUtil.getTimeFilename(blobName);
        const _fileName = commonUtil.isRTL(fileName) ? datestring : (fileName ?? datestring);
        data = params || { blobName, storageContainerType, fileName: _fileName, isDownload: isDownload };
        let xhrFields = {};
        if (responseType == 'blob') {
            xhrFields.responseType = 'blob'; // to avoid binary data being mangled on charset conversion
        }

        $.ajax({
            type: "GET",
            url,
            data: data,
            xhrFields: xhrFields,
            error: function (xhr) {
                if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                } else if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId, false, true);
                }
                commonUtil.serverError(xhr);
            },
            success: function (blob, status, xhr) {
                if (!isCssLoader && loaderId) {
                    commonUtil.createLoader(loaderId);
                } else if (isCssLoader && loaderId) {
                    commonUtil.cssLoader({ id: loaderId, isHide: true });
                }
                if (!isDownload && !isView) {
                    if (callback) {
                        if (arg0 || arg1) {
                            callback({ blob, arg0, arg1 });
                        } else {
                            callback(blob);
                        }
                        return;
                    }
                }

                // check for a filename
                let filename = fileName ?? (blobName ?? "");
                const disposition = xhr.getResponseHeader('Content-Disposition');
                if (disposition && disposition.indexOf('attachment') !== -1) {
                    const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    const matches = filenameRegex.exec(disposition);
                    if (matches != null && matches[1]) filename = matches[1].replace(/['"]/g, '');
                }
                if (!isDownload && isView && isPopup) {
                    commonUtil.openModel(blob, filename, false);
                    return;
                }
                else if (!isDownload && isView && !isPopup) {
                    commonUtil.openModel(blob, filename, false, false, appendId, blobName, storageContainerType);
                    return;
                }
                else if (isDownload && responseType == 'json') {
                    commonUtil.downloadFileUrl(blob.url, filename);
                    return;
                }

                if (responseType != 'blob') return;

                if (typeof window.navigator.msSaveBlob !== 'undefined') {
                    // IE workaround for "HTML7007: One or more blob URLs were revoked by closing the blob for which they were created. These URLs will no longer resolve as the data backing the URL has been freed."
                    window.navigator.msSaveBlob(blob, filename);
                } else {
                    const URL = window.URL || window.webkitURL;
                    const downloadUrl = URL.createObjectURL(blob);
                    commonUtil.downloadFileUrl(downloadUrl, filename);
                }
            }
        });
    };

    $(document).ready(function () {

        $(o.loaderContainerId).ajaxStop(function () {
            $(this).stop(true, true).fadeOut("fast");
        });

    });




    return http;
})();
