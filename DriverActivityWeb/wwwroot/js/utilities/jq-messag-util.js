const notificationUtil = (function (options) {

    //alertify.success('Current position : ' + alertify.get('notifier', 'position'));

    const success = (data) => {
        //alertify.success(data);
        toastr["success"](data);
    };

    const error = (data) => {
        //alertify.error(data);
        toastr["error"](data);
    };

    const warning = (data) => {
        //alertify.warning(data);
        toastr["warning"](data);
    };

    const message = (data) => {
        //alertify.message(data);
        toastr["info"](data);
    }

    const popup = ({ title, body, okText, cancelText, showCloseButton, showCancelButton }) => {
        okText = okText ?? 'OK';
        cancelText = cancelText ?? 'Cancel';
        Swal.fire({
            title: `<strong><u>${title}</u></strong>`,
            icon: 'error',
            html: body,
            showCloseButton: showCloseButton ?? false,
            showCancelButton: showCancelButton ?? false,
            focusConfirm: false,
            confirmButtonText: `<i class="fa fa-check"></i> ! ${okText}`,
            confirmButtonAriaLabel: `${okText}`,
            cancelButtonText: `<i class="fa fa-times"></i> ${cancelText}`,
            cancelButtonAriaLabel: `${cancelText}`
        })
    }

    const confirmation = ({ title, okText, cancelText, data }, callback) => {
        Swal.fire({
            title: `${title}`,
            //showDenyButton: true,
            showCancelButton: true,
            confirmButtonText: `${okText}`,
            cancelButtonText: `${cancelText}`,
            //denyButtonText: `${cancelText}`,
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                // Swal.fire('Saved!', '', 'success')
                callback(result);
            }
            /* else if (result.isDenied) {
                 Swal.fire('Changes are not saved', '', 'info')
             }*/

        })
    }

    const serverError = (errors) => {

    }

    const position = 'bottom-right';
    toastr.options = {
        "positionClass": "toaster-css",
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    let result = {};
    result.success = success;
    result.error = error;
    result.warning = warning;
    result.message = message;
    //result.confirmation = confirmation;
    result.popup = popup;
    result.serverError = serverError;
    result.confirmation = confirmation;

    return result;

})();