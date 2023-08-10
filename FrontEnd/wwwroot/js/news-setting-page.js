"use strict"
const typeX = [{
    id: 0,
    text: 'Non Kategori',
    class: 'secondary',
}, {
    id: 1,
    text: 'Korporate',
    class: 'primary',
}, {
    id: 2,
    text: 'Industri',
    class: 'warning',
}, {
    id: 3,
    text: 'Kompetitor',
    class: 'danger',
    }
];

const sentimentX = [{
    id: 0,
    text: 'Non Kategori',
}, {
    id: 1,
    text: 'Positif',
}, {
    id: 2,
    text: 'Netral',
}, {
    id: 3,
    text: 'Negatif',
    }
];

$(document).ready(function () {

    var dataGrid = $('#newsGrid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `/sportevents/Read`,
            insertUrl: `/sportevents/Create`,
            updateUrl: `/sportevents/Edit`,
            deleteUrl: `/sportevents/Delete`,
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
            allowedPageSizes: [5, 15, 50],
            showPageSizeSelector: true,
            showInfo: true,
            showNavigationButtons: true,
        },
        columns: [
            {
                dataField: 'eventName',
                caption: 'Event Name',
                allowSorting: false,
                allowFiltering: false,
                validationRules: [{
                    type: 'required',
                    message: 'Event Name is required.',
                }],
            },
            {
                dataField: 'eventType',
                caption: 'Event Type',
                allowSorting: false,
                allowFiltering: false,
                validationRules: [{
                    type: 'required',
                    message: 'Event Type is required.',
                }],
            },
            {
                dataField: 'eventDate',
                caption: 'Event Date',
                dataType: "date",
                allowSorting: false,
                allowFiltering: false,
                validationRules: [{
                    type: 'required',
                    message: 'Event Date is required.',
                }],
            },
            {
                dataField: 'organizerId',
                caption: 'Organizer',
                dataType: "string",
                allowSorting: false,
                allowFiltering: false,
                lookup: {
                    dataSource: window.$organizer,
                    valueExpr: 'id',
                    displayExpr: 'text',
                },
            },
        ],
        editing: {
            mode: 'popup',
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            popup: {
                title: 'Sport Event Info',
                showTitle: true,
                width: 700,
                height: 525,
            },
            form: {
                items: [{
                    itemType: 'group',
                    colCount: 1,
                    colSpan: 2,
                    items: ['eventName', 'eventType', 'eventDate', 'organizerId'],
                }],
            },
        },
    }).dxDataGrid("instance");
});
