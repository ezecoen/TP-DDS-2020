﻿var data = {
    model: {
        fecha: null,
        idMedioDePago: null,
        idPrestadorDeServicios: null,
        montoTotal: 0,
        items: [],
    },
    docsComerciales: []
}

$("#agregarItem").click(function () {

    data.model.items.push({
        cant: $("#cantItem").val(),
        descripcion: $("#descripcionItem").val(),
        valor: $("#valorItem").val(),

    })

    $("#listaItems").append('<li id="' + $("#descripcionItem").val() + '" class="list-group-item">' +
        $("#descripcionItem").val() + '&nbsp;-&nbsp;' +
        '$' + $("#valorItem").val() + '&nbsp;-&nbsp;' +
        $("#cantItem").val() + ' unidades&nbsp;' +
        '<button id="eliminarItem" value="' + $("#descripcionItem").val() + '" class="btn btn-outline-danger btn-sm" aria-label="Close">Eliminar</button ></li >');

    $("#descripcionItem").val('');
    $("#valorItem").val(1);
    $("#cantItem").val(1);

    console.log(data.model.items);
})

$(document).on("click", "#eliminarItem", function () {
    const index = data.model.items.findIndex(i => i.descripcion === $(this).val());
    data.model.items.splice(parseInt(index), 1);
    $('[id="' + $(this).val() + '"]').remove();

    console.log(data.model.items);
})

$("#agregarDoc").click(function () {

    data.docsComerciales.push($('#documento').val())

    $("#listaDocs").append('<li id="' + $("#documento").val() + '" class="list-group-item">' +
        $("#documento").val() + '&nbsp;-&nbsp;' +
        '<button id="eliminarDoc" value="' + $("#documento").val() + '" class="btn btn-outline-danger btn-sm" aria-label="Close">Eliminar</button ></li >');

    $("#documento").val('');

    console.log(data.docsComerciales);
})

$(document).on("click", "#eliminarDoc", function () {
    const index = data.docsComerciales.findIndex(i => i === $(this).val());
    data.docsComerciales.splice(parseInt(index), 1);
    $('[id="' + $(this).val() + '"]').remove();

    console.log(data.docsComerciales);
})



$("#submit").click(function () {

    data.model.fecha = $('#fecha').val();
    data.model.idMedioDePago = parseInt($("input[id=medioDePago]").val());
    data.model.idPrestadorDeServicios = parseInt($("input[id=proveedor]").val());
    data.model.montoTotal = data.model.items.reduce((a, b) => a + b.valor * b.cant, 0);

    console.log(data);

    $.ajax({
        type: "POST",
        url: "/compra/egreso/add",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data.model),
        crossDomain: true,
        success: function (data) {
            window.location.href = data;
        },
        error: function (err) {
            console.log(err)
        }
    })
})
