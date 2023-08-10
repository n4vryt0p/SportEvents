"use strict"

$(document).ready(function () {
    localStorage.removeItem('picProfile');
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
    $.ajax({
        url: '../auth/logout/challenge',
        type: 'post',
        data: null,
        headers: {
            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
        },
        success: function (data) {
            $.unblockUI();
            if (data == null) {
                window.location.href = "../auth/login";
            } else {
                window.location.href = "../auth/login?ReturnUrl=" + encodeURIComponent(data);
            }
        },
        error: function (data) {
            $.unblockUI();
            Swal.fire({
                title: 'To completely logout press Continue',
                showDenyButton: false,
                showCancelButton: false,
                confirmButtonText: 'Continue',
            }).then((result) => {
                /* Read more about isConfirmed, isDenied below */
                if (result.isConfirmed) {
                    window.location.reload(true);
                }
            })
        },
    });
});

function logoutBro() {
    $('html').block({ message: null });
    let dataX = document.getElementById("returnUrl").value
    if (dataX == null) {
        window.location.href = "../auth/login";
    } else {
        window.location.href = "../auth/login?ReturnUrl=" + encodeURIComponent(data);
    }
}
