<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="panel.aspx.cs" Inherits="AdminVisor.panel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta http-equiv="X-UA-Compatible" content="ie=edge"/>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
    <script type="text/javascript" src="lib/jquery/jquery-3.4.1.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="css/header.css" />
    <link rel="stylesheet" href="css/panell.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.8.1/css/all.css">
    <link rel="stylesheet" href="lib/toastr/toastr.min.css" />
    <script type="text/javascript" src="lib/toastr/toastr.min.js"></script>
    <script type="text/javascript" src="js/toastr_object.js"></script>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.14.2/dist/bootstrap-table.min.css">
    <script type="text/javascript" src="https://unpkg.com/bootstrap-table@1.14.2/dist/bootstrap-table.min.js"></script>
    <link rel="stylesheet" href="lib/bootstrap-select/bootstrap-select-1.13.9/dist/css/bootstrap-select.min.css" />
    <script type="text/javascript" src="lib/bootstrap-select/bootstrap-select-1.13.9/dist/js/bootstrap-select.min.js"></script>
    <title>Visors</title>
</head>
<body>
    <nav class="navbar navbar-expand-xl navbar-dark bg-dark">
        <div id="logo">
            <img src="images/logo.png" />
            <a href="#">Gestors Visors</a>
        </div>
        <ul class="nav">
          <a class="nav-link menu-item" href="#">Projectes</a>
          <a class="nav-link menu-item" href="#">Grups</a>
          <a class="nav-link menu-item" href="#">Serveis</a>
          <a class="nav-link menu-item" href="#">Mapes</a>
        </ul>
        <div>
            Usuari: <span id="nomUsuari"></span>
            <a class="btn btn-danger" href="default.aspx" role="button"><i class="fas fa-sign-out-alt"></i>Sortir</a>
        </div>
    </nav>
    <div id="container">
        <h1 id="title"></h1>
        <div id="button_group">
            <div class="btn-group" data-toggle="buttons">
                  <label class="btn btn-primary active" >
                    <input type="radio" name="options"> Editar
                  </label>
                  <label class="btn btn-primary">
                    <input type="radio" name="options"> Eines
                  </label>
                  <label class="btn btn-primary">
                    <input type="radio" name="options"> Mapes
                  </label>
                  <label class="btn btn-primary">
                    <input type="radio" name="options"> Serveis
                  </label>
            </div>
            <button type="button" class="btn btn-success font-weight-bold" id="nou" data-toggle="modal" data-target="#modal">
                <i class="fas fa-plus"></i>
                Nou
            </button>
        </div>
        <div id="content">
            <table id="table"></table>
        </div>   
    </div>

    <div class="modal fade" id="modal" tabindex="-1" role="dialog" aria-labelledby="modal" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h4 class="modal-title" id="modal-title"></h4>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <form id="form"></form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
            <button type="button" class="btn btn-primary enviar"></button>
          </div>
        </div>
      </div>
    </div>
    <div class="modal fade" id="modal2" tabindex="-1" role="dialog" aria-labelledby="modal" aria-hidden="true">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h4 class="modal-title" id="H1"></h4>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <h5>Projecte <span id="title_projecte"></span></h5>
            <div id="modal2_content"></div>
          </div>
      </div>
    </div>
</body>
    <script type="text/javascript">
        var resultat;
        var enviar;
        var content;

        function setTitle() {
            $("#title").text("<%=title%>");
            $("#modal .modal-title").text("<%=title%>");
        }

        function revisarGrupBotons(){
            if($("#title").text() == "Projectes"){
                $(".btn-group").show().children().show();
            }else{
                $(".btn-group").hide().children().hide();
            }
        }

        function omplirModal(trSeleccionat){
            enviar.unbind("click");
            var inputs = $(".input-select");
            if($(this).text().trim() == "Nou"){
                enviar.text("Crear");
                enviar.on("click", function(){
                    var valors = '{"accio":"insertar_'+$("#title").text()+'"';
                    for(var i = 0; i < inputs.length; i++){
                        var id_aux = inputs[i].id;
                        var titol = id_aux.substring(0, id_aux.indexOf("_"));
                        valors += ',"'+titol + '":"' + inputs[i].value+'"';
                    }
                    valors += "}";
                    $.ajax({
                        type: "GET",
                        url: "query.aspx",
                        data: JSON.parse(valors)
                    }).done(function (data) {
                        if (data.indexOf("Error")!=-1){
                            toastr.error("Error en la consulta: "+data, "Error");
                        }
                        else{
                            $('#modal').modal('hide');
                            toastr.success("Registre inserit correctament");
                            llistar();
                        }
                    }).fail(function (a, b) {});
                });
                for(var i = 0; i < inputs.length; i++){
                    if($(inputs[i]).attr('readonly') == "readonly")inputs[i].value = " ";
                    else inputs[i].value = "";
                }
            }else{
                enviar.text("Modificar");
                enviar.on("click", function(){
                    var valors = '{"accio":"modificar'+$("#title").text()+'"';
                    for(var i = 0; i < inputs.length; i++){
                        var id_aux = inputs[i].id;
                        var titol = id_aux.substring(0, id_aux.indexOf("_"));
                        valors += ',"'+titol + '":"' + inputs[i].value+'"';
                    }
                    valors += "}";
                    $.ajax({
                        type: "GET",
                        url: "query.aspx",
                        data: JSON.parse(valors)
                    }).done(function (data) {
                        if (data.indexOf("Error")!=-1){
                            toastr.error("Error en la consulta: "+data, "Error");
                        }
                        else{
                            $('#modal').modal('hide');
                            toastr.success("Registre modificat correctament");
                            llistar();
                        }
                    }).fail(function (a, b) {});
                });
                for(var i = 0; i < inputs.length; i++){
                    inputs[i].value = trSeleccionat[i].innerHTML;
                }
            }
        }

        function crearTaula() {
            var columnes = [];
            var c = resultat[0];
            $("#form").empty();
            for(var colName in c){
                var aux = (colName.charAt(0).toUpperCase() + colName.slice(1));
                if(colName == "idGrup" || colName == "idServei" || (colName == "id" && $("#title").text() != "Serveis")){
                    $("#form").append(`
                        <div class="form-group">
                            <label for="${colName}_nou" class="col-form-label">${aux}:</label>
                            <input type="text" class="form-control input-select" id="${colName}_nou" name="${colName}_nou" readonly >
                        </div>
                    `);
                }else if(colName == "estaDisponible"){
                     $("#form").append(`
                                     <div class="form-group">
                                            <label for="${colName}_nou" class="col-form-label">${aux}:</label>
                                            <select id="${colName}_nou" class="form-control input-select" name="${colName}_nou">
                                                <option value="true">true</option>
                                                <option value"false">false</option>
                                            </select>
                                        </div>
                                        `);
                }
                else{
                    $("#form").append(`
                        <div class="form-group">
                            <label for="${colName}_nou" class="col-form-label">${aux}:</label>
                            <input type="text" class="form-control input-select" id="${colName}_nou" name="${colName}_nou">
                        </div>
                    `);
                }
                columnes.push({
                    field: colName,
                    title: aux
                });
            }
            $('#table').bootstrapTable({
                          data: resultat,
                          search: true,
                          columns: [columnes],
                          theadClasses: "thead-dark"
                        });
            $("tr").on("click", function(){
                var trs = $("tr");
                if(this != trs[0]){
                    var aux = $(".active").text().trim();
                    if(aux == "Editar"){
                
                        for(var i = 0; i < trs.length; i++){
                            if(this == trs[i]){
                                omplirModal($(this)[0].cells);
                                $('#modal').modal('show');
                            }
                        }
                    }else{
                        omplirModal2(aux, $(this)[0].cells);
                        $('#modal2').modal('show');
                    } 
                }
            });
        }

        function omplirModal2(nom, obj){
             $("#modal2 .modal-title").text(nom);
             $("#modal2 #title_projecte").text(obj[1].innerHTML);
             var valors = '{"accio" : "'+nom+'", "id" : "'+obj[0].innerHTML+'"}';
             $.ajax({
                    type: "GET",
                    url: "query.aspx",
                    data: JSON.parse(valors)
                }).done(function (data) {
                    if (data.indexOf("Error")!=-1){
                        toastr.error("Error en la consulta: "+data, "Error");
                    }
                    else{
                        var resultats = data.split("|");
                        var tot;
                        var totSeleccionat;
                        var grups;
                        $("#modal2_content").empty();
                        var resultat = `
                                        <div class="select-group">
                                            <div class="form-group row">
                                                <label class="col-sm-2 control-label" for="totLlistat">${nom}:</label>
                                                <div class="col-sm-10">
                                                    <select class="selectpicker" id="selectTots" data-live-search="true" title="Seleccionar..." name="totLlistat"></select>
                                                </div>
                                            </div>
                                        `;
                        if(nom == "Serveis"){
                            resultat += `<div class="form-group row">
                                                <label for="grupLlistat" class="control-label col-sm-2">Grups:</label>
                                                <div class="col-sm-10">
                                                    <select class="selectpicker" data-live-search="true" id="grupTots" title="Seleccionar..." name="grupLlistat"></select>
                                                </div>
                                            </div>
                                        `;
                        }
                        resultat += `
                                            <button type="button" class="btn btn-success" id="afegirBtn">Afegir</button>
                                         </div>
                                            <h5>${nom} Disponibles</h5>
                                            <ul class="list-group list-group-flush" id="llistat_afegits"></ul>
                                    `;
                        $("#modal2_content").append(resultat);
                        $('.selectpicker').selectpicker();
                        if(resultats[0] != "cap"){
                            tot = JSON.parse(resultats[0]);
                            for(var i = 0; i < tot.length; i++){
                                $("#selectTots").append('<option value="'+tot[i].id+'-'+obj[0].innerHTML+'">'+tot[i].nom+'</option>');
                            }
                            $("#afegirBtn").attr("disabled", false);
                        }else{
                            $("#afegirBtn").attr("disabled", true);
                        }
                        if(nom == "Serveis"){
                            if(resultats[2] != "cap"){
                                grups = JSON.parse(resultats[2]);
                                for(var i = 0; i < grups.length; i++){
                                    $("#grupTots").append('<option value="'+grups[i].id+'">'+grups[i].nom+'</option>');
                                }
                            }
                        }
                        if(resultats[1] == "cap"){
                            $("#llistat_afegits").append(`
                                                        <li class="list-group-item">Cap</li>
                                                        `);
                        }else{
                            totSeleccionat = JSON.parse(resultats[1]);
                            if(nom != "Serveis"){
                                for(var i = 0; i < totSeleccionat.length; i++){
                                    $("#llistat_afegits").append(`
                                            <li class="list-group-item">
                                                    ${totSeleccionat[i].nom}<button type="button" class="btn btn-danger eliminarBtn" id="${totSeleccionat[i].id}">Eliminar</button></li>
                                            `);
                                }
                            }else{
                                for(var i = 0; i < totSeleccionat.length; i++){
                                $("#llistat_afegits").append(`
                                    <li class="list-group-item">
                                            ${totSeleccionat[i].nom} - ${totSeleccionat[i].nom_grup}<button type="button" class="btn btn-danger eliminarBtn" id="${totSeleccionat[i].id}">Eliminar</button></li>
                                    `);
                                }
                            }
                        }
                        $("#afegirBtn").on("click", function(){
                            var valors;
                            if(nom != "Serveis") valors = '{"accio":"afegir_'+getNomTaula(nom)+'", "id":"'+$("#selectTots").val().split("-")[0]+'", "idProjecte":"'+$("#selectTots").val().split("-")[1]+'"}';
                            else valors = '{"accio":"afegir_'+getNomTaula(nom)+'", "id":"'+$("#selectTots").val().split("-")[0]+'", "idProjecte":"'+$("#selectTots").val().split("-")[1]+'", "idGrup":"'+$("#grupTots").val()+'"}';
                            $.ajax({
                                type: "GET",
                                url: "query.aspx",
                                data: JSON.parse(valors)
                            }).done(function (data) {
                                if (data.indexOf("Error")!=-1){
                                    toastr.error("Error en la consulta: "+data, "Error");
                                }
                                else{
                                    omplirModal2(nom, obj);
                                }
                            }).fail(function (a, b) {});
                        });
                        $(".eliminarBtn").on("click", function(){
                            var valors = '{"accio":"eliminar_'+getNomTaula(nom)+'", "id":"'+$(this).attr("id")+'"}';
                            console.log(JSON.parse(valors));
                            $.ajax({
                                type: "GET",
                                url: "query.aspx",
                                data: JSON.parse(valors)
                            }).done(function (data) {
                                if (data.indexOf("Error")!=-1){
                                    toastr.error("Error en la consulta: "+data, "Error");
                                }
                                else{
                                    omplirModal2(nom, obj);
                                }
                            }).fail(function (a, b) {});
                        });
                    }
                }).fail(function (a, b) {});
        }

        function getNomTaula(nom){
            if(nom == "Eines")return "eina";
            if(nom == "Mapes")return "projecte_mapa_de_fons";
            else return "servei";
        }

        function llistar(){
            $.ajax({
                type: "GET",
                url: "query.aspx",
                data: { 
                    accio: "llistar_"+$("#title").text()
                }
            }).done(function (data) {
                if (data.indexOf("Error")!=-1){
                    //toastr["warning"]("Error el la consulta: " + data);
                }
                else{
                    resultat = JSON.parse(data);
                    content.empty();
                    content.append(`<table id="table"></table>`);
                    crearTaula();
                    $('#table').bootstrapTable('refresh', {data: resultat});
                }
            }).fail(function (a, b) {

            });
        }
        
        $(document).ready(function(){
            $("#nomUsuari").text("<%=nomUsuari%>");
            content = $("#content");
            setTitle();
            revisarGrupBotons();
            enviar = $(".enviar");
            llistar();
            $("#nou").on("click", omplirModal);
            $(".menu-item").on("click", function () {
                if($("#title").text() != $(this).text()) $(this).attr("href", "panel.aspx?panel=" + $(this).text()+"&nomUsuari=<%=nomUsuari%>");
            });   
        });
    </script>
</html>
