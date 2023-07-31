<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link href="https://fonts.googleapis.com/css?family=Roboto:300,400&display=swap" rel="stylesheet">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/css/all.min.css" integrity="sha512-KfkfwYDsLkIlwQp6LFnl8zNdLGxu9YAA1QvwINks4PhcElQSvqcyVLLD9aMhXd13uQjoXtEKNosOWaZqXgel0g==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@600;700&display=swap" rel="stylesheet">

    <link rel="stylesheet" href="fonts/icomoon/style.css">

    <link rel="stylesheet" href="css/owl.carousel.min.css">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="css/bootstrap.min.css">

    <!-- Style -->

    <link href="../Style/Signupp.css" rel="stylesheet" />
    <title>Login #</title>

</head>
<body>

    <form id="form1" runat="server">

        <div>

            <img class="img-style" src="../images/sign-up.png" />

            <div class="container" id="container">
                <div class="form-container sign-up-container">
                    <form action="#">
                    </form>
                </div>
                <div class="form-container sign-in-container">
                    <form action="#">
                        <h1 class="card-title">Sign Up    </h1>

                        <span>or use your account</span>

                        <div class="social-container">
                            <a href="#" class="social"><i class="fab fa-facebook-f"></i>&nbsp  </a>
                            <a href="#" class="social"><i class="fab fa-google-plus-g"></i>&nbsp  </a>
                        </div>

                        <div class="inp-style">

                            <label for="inp" class="inp">
                                <input type="text" id="email" placeholder="&nbsp;" />
                                <span class="label">Email</span>
                                <span class="focus-bg"></span>
                            </label>

                            <label for="inp" class="inp">
                                <input type="text" id="ph no." placeholder="&nbsp;" />
                                <span class="label">Phone Number </span>
                                <span class="focus-bg"></span>
                            </label>

                            <label for="inp" class="inp">
                                <input type="password" id="pwsd" placeholder="&nbsp;" />
                                <span class="label">Password</span>
                                <span class="focus-bg"></span>
                            </label>


                        </div>

                        <br />
                        <br />
                        <a href="#">Forgot your password?</a>
                        <button>Sign Up</button>


                    </form>

                </div>

            </div>

        </div>

    </form>

</body>
</html>
