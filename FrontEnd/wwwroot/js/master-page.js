"use strict"
$(document).ready(function () {
    $('#masterGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `/organizers/Read`,
            insertUrl: `/organizers/Create`,
            updateUrl: `/organizers/Edit`,
            deleteUrl: `/organizers/Delete`,
            onBeforeSend(method, ajaxOptions) {
                let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
                if (antiForgeryToken) {
                    ajaxOptions.headers = {
                        "RequestVerificationToken": antiForgeryToken
                    };
                };
            },
        }),
        remoteOperations: true,
        rowAlternationEnabled: true,
        paging: {
            pageSize: 10,
        },
        pager: {
            visible: true,
            allowedPageSizes: [5, 10, 'all'],
            showPageSizeSelector: true,
            showInfo: true,
            showNavigationButtons: true,
        },
        columns: [
            {
                dataField: 'id',
                visible: false,
                allowSorting: false,
                allowFiltering: false,
            },
            {
                dataField: 'organizerName',
                caption: 'Organizer Name',
                validationRules: [{
                    type: 'required',
                    message: 'Organizer Name is required.',
                }],
                allowSorting: false,
                allowFiltering: false,
            },
            {
                dataField: 'imageLocation',
                caption: 'Image Location',
                validationRules: [{
                    type: 'required',
                    message: 'Image Location is required.',
                }],
                allowSorting: false,
                allowFiltering: false,
            }
        ],
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'Organizer Info',
                showTitle: true,
                width: 700,
                height: 525,
            },
            form: {
                items: [{
                    itemType: 'group',
                    colCount: 1,
                    colSpan: 2,
                    items: ['organizerName', 'imageLocation'],
                }],
            },
        },
    });
});
