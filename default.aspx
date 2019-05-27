<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="AdminVisor._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta http-equiv="X-UA-Compatible" content="ie=edge"/>
    <link rel="stylesheet" href="lib/bootstrap-4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/default_style.css" />
    <link rel="stylesheet" href="css/header.css" />
    <script type="text/javascript" src="lib/jquery/jquery-2.2.4.min.js"></script>
    <script type="text/javascript" src="lib/popper/popper.min.js"></script>
    <script type="text/javascript" src="lib/bootstrap-4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="lib/toastr/toastr.min.css" />
    <script type="text/javascript" src="lib/toastr/toastr.min.js"></script>
    <script type="text/javascript" src="js/toastr_object.js"></script>
    <title>Visors</title>
</head>
<body>
    <nav class="navbar navbar-expand-xl navbar-dark bg-dark">
        <div id="logo">
            <img src="images/logo.png" />
            <a href="#">Gestor Visors</a>
        </div>
    </nav>
    <div class="text-center" id="container">
        <form id="form-signin" action="default.aspx" method="post">
            <h1 class="h3 mb-3 font-weight-normal">Iniciar Sessió</h1>
            <label for="usuari" class="sr-only">Usuari</label>
            <input type="text" id="usuari" class="form-control" placeholder="Usuari" required="" autofocus="" name="usuari"/>
            <label for="password" class="sr-only">Password</label>
            <input type="password" id="password" class="form-control" placeholder="Contrasenya" required="" name="password"/>   
            <button class="btn btn-lg btn-primary btn-block" type="button" onclick="fn_validar()">Iniciar Sessió</button>
        </form>
     </div>

     <script type="text/javascript">
        
        // enter -> enviar formulari
        $("#password").keyup(function(event){
            if(event.keyCode === 13){
                event.preventDefault();
                fn_validar();
            }
        });

        var resultat = JSON.parse(<%=msgIni%>);
        if (resultat.correcte=="no"){
            toastr.error(resultat.missatge, "Error");
        }

        function fn_validar(){
            document.getElementById("form-signin").submit();
        }


    </script>
</body>
</html>
