"use strict"

var input = '', antiForgeryToken;
function subsmits(e) {
    e.preventDefault();
    $.ajax({
        url: '../auth/register',
        type: 'post',
        data: $("#lpsLogin").serialize(),
        headers: {
            "RequestVerificationToken": antiForgeryToken
        },
        success: function (data) {
            $.unblockUI();
            Swal.fire("Success!", "Your account created", "error").then((result) => {
                window.location.href = "../auth/login";
            });
        },
        error: function (data) {
            $.unblockUI();
            Swal.fire("Server Error!", data.responseText, "error");
        },
    });
    return false;
}
