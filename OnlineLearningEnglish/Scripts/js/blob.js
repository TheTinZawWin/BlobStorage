var token = $("input[name=__RequestVerificationToken]").val();
$(document).ready(function () {
  //  getBlobFile();
    createGrid();
});
function createGrid() {
    $('#grid').w2grid({
        name: 'grid_column',
        show: {
            footer: true,
            toolbar: false,
            skipRecords: false,
            toolbarSearch: false
        },
        multiSelect: false,
        multiSearch: false,

        columns: [
            { field: 'Id', caption: 'Id', size: '30%', sortable: true },
            { field: 'Name', caption: 'Name', size: '30%', sortable: true },
            { field: 'Storagename', caption: 'Storagename', size: '30%', sortable: true },
            { field: 'StorageData', caption: 'StorageData', size: '30%', sortable: true },
            { field: 'Statuscode', caption: 'Statuscode', size: '30%', sortable: true }
        ],
        multisearch: false,
        searches: [
            { field: 'Id', caption: 'Id', type: 'int' },
            { field: 'Name', caption: 'Name', type: 'text' },
            { field: 'Storagename', caption: 'Storagename', type: 'text' },

            { field: 'StorageData', caption: 'StorageData', type: 'text' },
            { field: 'Statuscode', caption: 'Statuscode', type: 'text' }
        ],
        sortData: [{ field: 'Id', direction: 'DESC' }],

    });
    w2ui['grid_column'].load('/Blob/GetBlobStorageDataList');
}
$('#btn-customer-file-upload').on('click', function () {
    console.log("hi");
    var file = $('#UploadFile').get(0).files;
    var formData = new FormData();
    var fileCheck = document.getElementById("UploadFile");
    if (fileCheck.files.length == 0) {
        window.alert(" Please select file.");
        return;
    }
    formData.append('ContractNo', '@Model.ParentModel.ContractNo');
    formData.append('BlobStorageDataSeq', "0");
    formData.append('BlobFile', file[0]);

    $.ajax({

        type: "Post",
        url: '@Url.Content("~/Blob/BlobStorageData/")',
        data: formData,
        contentType: false,
        processData: false,
    }).done(function (data) {

        if (updateGrid()) {
            window.alert(data.ret);
        }
    }).fail(function (error) {
        alert(error.status + '<--and--> ' + error.statusText);
    });
});


$('#btn-blob-modify').on('click', function () {
    var file = $('#UploadFile').get(0).files;
    var index = w2ui['grid_column'].getSelection(true);
    if (index.length == 0) {
        showMustSelectRecordMessage();
        return;
    }

    var fileCheck = document.getElementById("UploadFile");
    if (fileCheck.files.length == 0) {
        window.alert(" Please select new file.");
        return;
    }
    var data = w2ui['grid_column'].get(Number(index) + 1);

    var formData = new FormData();
    formData.append('ContractNo', '@Model.ParentModel.ContractNo');
    formData.append('BlobStorageDataSeq', data.BlobDataStorageSeq);
    formData.append('BlobFile', file[0]);


    $.ajax({

        type: "Post",
        url: '@Url.Content("~/Customer/BlobFileUpdate/")',
        data: formData,
        contentType: false,
        processData: false,
    }).done(function (data) {
        if (updateGrid()) {
            window.alert(data.ret);
        }
    }).fail(function (error) {
        alert(error.status + '<--and--> ' + error.statusText);
    });
});
$('#btn-blob-delete').on('click', function () {
    var file = $('#UploadFile').get(0).files;
    var index = w2ui['grid_column'].getSelection(true);
    if (index.length == 0) {
        showMustSelectRecordMessage();
        return;
    }

    var sel_rec_ids = w2ui['grid_column'].getSelection();
  
    var sel_record = w2ui['grid_column'].get(sel_rec_ids[0]);
    $.ajax({
        url: '/Blob/BlobFileDelete',
        type: 'POST',
        dataType: 'Json',
        data: { "__RequestVerificationToken": token, "id": sel_record.Id },
        success: function (data) {
            w2ui['grid_column'].load('/Blob/GetBlobStorageDataList');
            alert("delete success");

        },
        error: function () {
        }
    });
});
$('#btn-blob-view').on('click', function () {
    var index = w2ui['grid_column'].getSelection(true);
    if (index.length == 0) {
        alert("Select one record");
        return;
    }
    console.log(w2ui['grid_column'].getSelection());
    var Griddata = w2ui['grid_column'].get(Number(index) + 1);
    var id =$("#grid").w2field().get().id;
    var file = Griddata.StorageData;
    var fileName = Griddata.name;
    var fileId = Griddata.id;
    var fields = file.split('.');

    console.log(fields[5]);
    if (w2ui['grid_column'].records.length == 0) {
        alert("Please select one file to view.");
        return;
    }

    if (fields[5] === 'xlsx' || fields[5] === 'xls' || fields[5] === 'xlsm' || fields[5] === 'csv' || fields[5] === 'txt') {
        window.open(file, '_blank');
        $('#imgBlob').addClass('display-none');
        $('#blobView').addClass('display-none');
        $('#downloadImageId').addClass('display-none');
        $('#Modal-BlobdetailPopup').modal('hide');
        window.alert("Only Image and PDF can preview.");
        return;
    }

    else if (fields[5] === 'pdf') {
        $('#downloadImageId').addClass('display-none');
        $('#imgBlob').addClass('display-none');
        $('#blobView').removeClass('display-none');
        $('#blobView').attr("src", file);
        $('#blobView').attr('Width', '800px');
        $('#blobView').attr('Height', '600px');
        $('#Modal-BlobdetailPopup').attr("Height", "656px");
        $('#Modal-BlobdetailPopup').attr("Width", "100%");
        $('#Modal-BlobdetailPopup').modal('show');
    }
    else {


        var formData = new FormData();
        formData.append('ContractNo', '@Model.ParentModel.ContractNo');
        formData.append('BlobStorageDataSeq', fileId);
        formData.append('BlobFile', "blob");


        $.ajax({

            type: "Post",
            url: '@Url.Content("~/Customer/GetFileFromBlob/")',
            data: formData,
            contentType: false,
            processData: false,
        }).done(function (data) {
            $('#downloadImageId').attr('download', fileName);
            $('#downloadImageId').attr('href', 'data:image/png;base64,' + data);
            $('#downloadImageId').removeClass('display-none');
            $('#imgBlob').removeClass('display-none');
            $('#blobView').addClass('display-none');
            $('#imgBlob').attr("src", Griddata.StorageData);

            $('#Modal-BlobdetailPopup').attr("Height", "250px");
            $('#Modal-BlobdetailPopup').attr("Width", "450px");
            $('#Modal-BlobdetailPopup').modal('show');

        }).fail(function (error) {
            alert(error.status + '<--and--> ' + error.statusText);
        });

    }


});

function createBloGrid() {
    $('#grid').w2grid({
        name: 'grid_column',
        limit: 0,
        postData: {
            'screenId': '@Model.ScreenId',
            'ContractNo': '@Model.ParentModel.ContractNo',
        },
        method: 'GET',
        show: {
            footer: true,
            toolbar: false,
            skipRecords: false,
            toolbarSearch: false
        },
        multiSelect: false,
        multiSearch: false,
        columns: [
            { field: 'BlobDataStorageSeq', caption: 'Id', size: '30%', sortable: true },
            { field: 'UploadedDateTime', caption: 'UploadedDateTime', size: '70%', sortable: true },
            { field: 'Name', caption: 'Name', size: '30%', sortable: true },
            { field: 'StorageData', caption: 'StorageData', size: '70%', sortable: true },
            { field: 'StaffName', caption: 'StaffName', size: '70%', sortable: true }
        ],
        sortData: [
            { field: 'BlobDataStorageSeq', direction: 'desc' }
        ]
    });
}

function getBlobFile() {
    console.log("hi");
    $("#tbBlob").DataTable({
        "ajax": {
            "url": '/Blob/GetBlobStorageDataList',
            "type": "POST",
            "data": { "__RequestVerificationToken": token}
        },
        "columnDefs": [
            {
                targets: [-1], 'searchable': false, 'orderable': false,
                render: function (data, type, row, meta) {
                    if (data) {
                        return '<input type="checkbox" id="' + $('<div/>').text(data).html() + '" class="chkResourceID_RCLR move-left"  value="' + $('<div/>').text(data).html() + '">';
                    }
                    else {
                        return '';
                    }
                }
            },

            {
                "targets": [0],
                "createdCell": function (td, cellData, rowData, row, col) {
                    $(td).addClass('left');
                }
            },
            {
                "targets": [9],
                "createdCell": function (td, cellData, rowData, row, col) {
                    $(td).addClass('right');
                }
            }
            //},
            //{
            //    "targets": [6,7],
            //    "visible": false
            //}

        ],
        "bDestroy": true,
        "responsive": true,
        "lengthMenu": [[10, 50, 100, 500, 5000, -1], [10, 50, 100, 500, 5000, "All"]],
        "order": [9, 'asc'],
        "language": {
            "loadingRecords": "<i class='fa fa-spinner fa-spin'></i> Loading data...",
            "sEmptyTable": "No Closed Resource(s)"
        },
        "rowCallback": function (row, data, index) {
            $(row).addClass("rowcheck_RCLR");
            $(row).attr('data-resourceid', data[9]);
        },
        "bPaginate": true,
        "deferRender": true,
        "fnInitComplete": function () {
            $(document).ready(function () {
                HideLoadingOverlay();
            });
        },
        //drawCallback: function () {
        //    var pagination = $(this).closest('.dataTables_wrapper').find('.dataTables_paginate');
        //    pagination.toggle(this.api().page.info().pages > 1);
        //}
    });

}
