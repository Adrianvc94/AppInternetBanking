<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AppWebInternetBanking.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Iniciar sesion</title>
   
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Frank+Ruhl+Libre:wght@400;500&family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet" />    
    <link rel="icon" type="image/x-icon" href="~/img/logo.ico"/>
    <style>
        body {
            font-family: 'Roboto', sans-serif;
        }

        * {
            box-sizing: border-box;
        }



        /* Full-width input fields */
        input[type=text], input[type=password] {
            width: 85%;
            padding: 15px;
            margin: 15px 0 15px 0;
            display: inline-block;
            border: none;
            background: #f1f1f1;
        }



            /* Add a background color when the inputs get focus */
            input[type=text]:focus, input[type=password]:focus {
                background-color: #ddd;
                outline: none;
            }



        /* Set a style for all buttons */
        button {
            background-color: #04AA6D;
            color: white;
            padding: 14px 20px;
            margin: 8px 0;
            border: none;
            cursor: pointer;
            width: 100%;
            opacity: 0.9;
        }



            button:hover {
                opacity: 1;
            }



        /* Extra styles for the cancel button */
        .cancelbtn {
            background-color: gray;
            color: white;
            padding: 14px 20px;
            margin: 8px 0;
            border: none;
            cursor: pointer;
            width: 100%;
            opacity: 0.9;
        }



        .normalbtn {
            background-color: black;
            color: white;
            padding: 12px 20px;
            margin: 0 auto;
            margin-bottom: 20px;
            border: none;
            border-radius: 10px;
            cursor: pointer;
            width: 70%;
            opacity: 0.9;
            font-size: 20px;
            display: block;
            box-shadow: rgba(0, 0, 0, 0.12) 0px 1px 3px, rgba(0, 0, 0, 0.24) 0px 1px 2px;
        }

            .normalbtn:hover {
                background-color: #292929;
                box-shadow: rgba(0, 0, 0, 0.16) 0px 3px 6px, rgba(0, 0, 0, 0.23) 0px 3px 6px;
            }

        .btn_register {
            color: black;
            font-size: 18px;
            margin: 10px auto 0 auto;
            border: none;
            cursor: pointer;
            width: 40%;
            text-decoration: none;
            display: block;
            text-align: center;
        }

            .btn_register:hover {
                color: black;
                text-decoration: underline;
            }



        button:hover {
            opacity: 1;
        }




        /* Add padding to container elements */
        .container {
            padding: 16px;
        }



        /* The Modal (background) */
        .modal {
            display: normal; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: #292929;
            /*            padding-top: 50px;*/
        }



        /* Modal Content/Box */
        .modal-content {
            display: flex;
            background-color: #FFFFFF;
            margin: 5% auto 0 auto;
            border: 0px;
            width: 85%;
            height: 75%;
            /*             width: 100%; 
            height: 100%;*/

            box-shadow: 0 1px 17px 1px rgb(0 0 0 / 9%);
            /*            padding: 20px 50px;*/
        }

        /* The Close Button (x) */
        .close {
            position: absolute;
            right: 35px;
            top: 15px;
            font-size: 40px;
            font-weight: bold;
            color: #f1f1f1;
        }

        .form_container {
            padding: 50px 50px;
            margin: auto 0;
            width: 60%;
        }

        .img_container {
            text-align: center;
            margin: 0px auto 0px auto;
            display: block;
            background-image: url("/img/login.jpg");
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;
            opacity: 0.9;
            width: 60%;
        }

        .img_container__title {
            text-align: end;
            /*            width: 60px;*/
            padding-right: 50px;
            margin-bottom: 10px;
            color: white;
            font-weight: 500;
            font-size: 45px;
            font-family: 'Frank Ruhl Libre', serif;
        }

        .img_container__line {
            height: 0.2px;
            width: 67%;
            background-color: white;
            margin: 0 50px 0 auto;
        }

        .inputText_container{
            display:flex;
            align-items:center;
            justify-content: space-evenly;
        }

        .inputText_container__image{
            width: 25px;
            height: 25px;
            display:  inline-block;
            margin-right: 10px;
        }



        .close:hover,
        .close:focus {
            color: #f44336;
            cursor: pointer;
        }



        /* Clear floats */
        .clearfix::after {
            content: "";
            clear: both;
            display: table;
        }

        input {
            width: 100%;
            height: 50px;
            border-radius: 6px;
            background-color: #f2f2f2;
            border: none;
            padding: 6px 12px;
            font-weight: 500;
            color: #606060;
            font-size: 16px;
            margin-top: 10px;
        }

        h1 {
            color: black;
        }

        .button_container {
            width: 80%;
            margin: 0 auto;
            padding-top: 0;
        }



        /* Change styles for cancel button and signup button on extra small screens */
        @media screen and (max-width: 300px) {
            .cancelbtn, .signupbtn {
                width: 100%;
            }
        }
    </style>
</head>
<body>
    <div id="myModal" class="modal">


        <form class="modal-content animate" runat="server">

            <div class="img_container">
                <!-- <img src="img/ulacit.jpg" /> -->
                <h1 class="img_container__title">Internet Banking</h1>
                <div class="img_container__line"></div>
            </div>

            <div class="form_container">

                <div class="container input_container">

                    <h1>Iniciar Sesión</h1>

                    <div class="inputText_container">
                        <img src="https://cdn-icons-png.flaticon.com/512/1077/1077063.png" class="inputText_container__image"/>
                        <asp:TextBox ID="txtUsername" runat="server" Placeholder="Ingrese su nombre de usuario"></asp:TextBox>
                    </div>
                     <asp:RequiredFieldValidator ID="rfqvUsername" runat="server" ErrorMessage="El nombre de usuario es requerido"
                            ControlToValidate="txtUsername" ForeColor="Maroon"></asp:RequiredFieldValidator>


                    <div class="inputText_container">
                        <img src="https://cdn-icons-png.flaticon.com/512/1000/1000966.png" class="inputText_container__image"/>
                          <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" Placeholder="Ingrese su clave"></asp:TextBox>
                    </div>
                  
                    <asp:RequiredFieldValidator ID="rfqvPassword" runat="server" ErrorMessage="El password es requerido"
                        ControlToValidate="txtPassword" ForeColor="Maroon"></asp:RequiredFieldValidator>

                    <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Maroon"></asp:Label>

                </div>

                <div class="container button_container">



                    <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="normalbtn" OnClick="btnAceptar_Click" />

                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Registro.aspx" CssClass="btn_register">Registrarme</asp:HyperLink>

                    <!-- <input type="reset" value="Limpiar" class="cancelbtn" /> -->


                </div>

            </div>


        </form>
    </div>
</body>
</html>
