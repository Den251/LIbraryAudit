var dataTable;


$(document).ready(function () {
    loadDataTable();
});


function loadDataTable(url) {
    var urlAjax = url;
    if (typeof urlAjax === 'undefined') {
        
        urlAjax = "/book/getall/"

    }
    
    dataTable = $('#DT_load').DataTable({
        destroy: true,
        "ajax": {
            "url": " " + urlAjax + " ",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            
            { "data": "title", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "reserved", "width": "10%" },
            
            
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Book/Update?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/book/Delete?id='+${data})>
                            Delete
                        </a>
                        &nbsp;
                        <a class='btn btn-warning text-white' id='Reserve${data}' style='cursor:pointer; width:90px;'
                            onclick=Reserve(${data})> 
                            Reserve
                        </a>
                        &nbsp;
                        <a class='btn btn-warning text-white' id='Archive${data}' style='cursor:pointer; width:90px;'
                            onclick=Archive(${data})> 
                            Archive
                        </a>
                        </div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Reserve(id) {
    
      
    $.ajax({
        url: "/book/reserve?id=" + id,
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        cache: false,
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $('#DT_load').DataTable().ajax.reload()
            }
            else {
                toastr.error(data.message);
            }
        }
        
        
    });

    
}


function Archive(Id) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "PUT",
                url: "/book/archive?id=" + Id,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        $('#DT_load').DataTable().ajax.reload()
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function getAlphabeticOrder() {
    
                dataTable = $('#DT_load').DataTable({
                    destroy: true,
                    "ajax": {
                        "url": "/book/GetAllAlphabeticaly",
                        "type": "GET",
                        "datatype": "json"
                    },
                    
                    "columns": [

                        { "data": "title", "width": "20%" },
                        { "data": "author", "width": "20%" },
                        { "data": "reserved", "width": "10%" },


                        {
                            "data": "id",
                            "render": function (data) {
                                return `<div class="text-center">
                        <a href="/Book/Update?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/book/Delete?id='+${data})>
                            Delete
                        </a>
                        &nbsp;
                        <a class='btn btn-warning text-white' id='Reserve${data}' style='cursor:pointer; width:90px;'
                            onclick=Reserve(${data})> 
                            Reserve
                        </a>
                        &nbsp;
                        <a class='btn btn-warning text-white' id='Archive${data}' style='cursor:pointer; width:90px;'
                            onclick=Archive(${data})> 
                            Archive
                        </a>
                        </div>`;
                            }, "width": "40%"
                        }
                    ],
                    "language": {
                        "emptyTable": "no data found"
                    },
                    "width": "100%"
                });
                
            }

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        //dataTable.ajax.reload();
                        $('#DT_load').DataTable().ajax.reload()
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}