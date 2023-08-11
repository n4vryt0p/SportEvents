"use strict"
var antiForgeryToken;
$(document).ready(function () {
    antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
});

function save(e) {
    e.preventDefault();
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
        url: '../users/edit',
        type: 'put',
        data: $("#meLogin").serialize(),
        headers: {
            "RequestVerificationToken": antiForgeryToken
        },
        success: function (data) {
            Swal.fire("Success!", "Your info edited", "error");
            $.unblockUI();
        },
        error: function (data) {
            $.unblockUI();
            Swal.fire("Server Error!", data.responseText, "error");
        },
    });

    return false;
}

function del(e) {
    e.preventDefault();
    Swal.fire({
        title: 'Are you sure to delete your account?',
        showDenyButton: true,
        confirmButtonText: 'Delete',
        denyButtonText: `Don't delete`,
    }).then((result) => {
        if (result.isConfirmed) {
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
                url: '../users/delete?uId=' + document.getElementById("uId").value,
                type: 'delete',
                headers: {
                    "RequestVerificationToken": antiForgeryToken
                },
                success: function (data) {
                    $.unblockUI();
                    Swal.fire('Saved!', 'Your Account deleted, you will be logout', 'success').then((result) => {
                        window.location.href = "../auth/logout";
                    })
                },
                error: function (data) {
                    $.unblockUI();
                    Swal.fire("Server Error!", data.responseText, "error");
                },
            });
        } else if (result.isDenied) {
            Swal.fire('Okay', '', 'info')
        }
    })
    

    return false;
}