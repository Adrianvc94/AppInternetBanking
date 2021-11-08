<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AppWebInternetBanking._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron homepage_img">
      <!--    <h1 style="color:white">Internet Banking</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
      <img src="img/homepage.jpg" class="img-fluid homepage_img" alt="..."> -->
    </div>

    <div class="row">
        <div class="col-md-3">
            <h2>Mantenimientos</h2>
            <p>
                La sección de mantenimientos permite visualizar, crear, actualizar y eliminar los datos de las diferentes secciones 
                que existen en la aplicación. 
            </p>
           
        </div>
        <div class="col-md-3">
            <h2>Procesos</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
           
        </div>
        <div class="col-md-3">
            <h2>Reportes</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
           
        </div>
         <div class="col-md-3">
            <h2>Reportes</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
          
        </div>
    </div>

</asp:Content>
