<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            text-align: left;
        }
    </style>
    <style>
         #Button1
         {
                 background-color: #06D995;
        border: 0 none;   
    border-radius: 7px;
    color: White;
    cursor: pointer;
    font-family: Trebuchet MS;
    font-size: 16px;
    font-weight: bold;
    height: 45px;
    margin-top: 17px;
    width: 250px;
    
         }
         #Button2
         {
                 background-color: #06D995;
        border: 0 none;   
    border-radius: 7px;
    color: White;
    cursor: pointer;
    font-family: Trebuchet MS;
    font-size: 16px;
    font-weight: bold;
    height: 28px;
    margin-top: 17px;
    width: 65px;
    
         }
          #Button3
         {
                 background-color: #06D995;
        border: 0 none;   
    border-radius: 7px;
    color: White;
    cursor: pointer;
    font-family: Trebuchet MS;
    font-size: 16px;
    font-weight: bold;
    height: 28px;
    margin-top: 17px;
    width: 65px;
    
         }
        #Button111:hover
        {
            background-color:#0A854B;
            border-bottom:5px solid #61B450;
            
        }
      .textbox {
   
    padding: 10px 10px 10px 25px;
    width: 270px;
    border: 1px solid #CCC;
    -moz-border-radius: 5px;
    -webkit-border-radius: 5px;
    border-radius: 5px;
    -moz-box-shadow: 0 1px 1px #ccc inset, 0 1px 0 #fff;
    -webkit-box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
    box-shadow: 0 1px 1px #CCC inset, 0 1px 0 #FFF;
}
.txt1
{
     background: #F1F1F1 url(images/txt.png) no-repeat;
    background-position: 6px 1px !important;
}
.txt2
{
     background: #F1F1F1 url(images/txt.png) no-repeat;
    background-position: 6px -46px  !important;
}

.textbox:focus {
    background-color: #FFF;
    border-color: #E8C291;
    outline: none;
    -moz-box-shadow: 0 0 0 1px #e8c291 inset;
    -webkit-box-shadow: 0 0 0 1px #E8C291 inset;
    box-shadow: 0 0 0 1px #E8C291 inset;
}
        .big
        {
            
            height: auto;
            width: 100%;
            position: fixed;
            top: 0px;
            left: 0px;
    animation: slide 25s linear;
    animation-iteration-count: infinite;
        }
        @keyframes slide {
  0% {
    left: 0;
    top: 0;
  }
  50% {
    
    top: -100px;
  }
  100% {
   
    top: 0;
  }
}
      
    </style>
    <script>
        function hideLabel() {
            document.getElementById("<%=pwdvalidation.ClientID%>").style.display = "none";
        }
    
    </script>
</head>
<body oncontextmenu="return false">
    <div>
        <%-- <img class="big" src="The New College.jpg">--%>
        <asp:Image ID="img_college" runat="server" Visible="true" ImageUrl="~/Handler/Handler.ashx"
            CssClass="big" />
    </div>
    <div style="height: 2000px; width: 100%; background-color: rgba(0,0,0,0.5); position: fixed;
        top: 0px; left: 0px;">
    </div>
    <form id="form1" runat="server">
    <div style="height: 85px; width: 100%; background-color: White; position: absolute;
        top: 0px; left: 0px;">
        <center>
            <asp:Image ID="img_com" runat="server" ImageUrl="~/Handler/Handler6.ashx" Style="height: 110px;
                width: 99%; border: 7px solid white;" />
        </center>
    </div>
    <center>
        <div style="box-shadow: 0 2px 26px 3px; height: 370px; position: relative; top: 150px;
            width: 400px;">
            <img src="images/ins.png" style="margin-top: -54px; position: absolute; right: 312px;
                transform: rotate(-18deg);" />
            <center>
                <%--<span id="spnsettings"  style="margin-left:200px; color:Blue;">    Best View In Mozilla Firefox </span>--%>
                <div style="height: 69px; width: 100%; color: white; font-weight: bold; font-size: 17px;">
                    <div style="color: Gray; font-family: Trebuchet MS; height: 55px; line-height: 18px;
                        margin-top: 10px; position: relative; text-align: center; top: 19px; width: 377px;">
                        <br />
                        <asp:Label ID="clgename" runat="server" Visible="false" Text="collegename"></asp:Label>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtuname" runat="server" CssClass="textbox txt1" placeholder="User Name"
                                        onkeypress="hideLabel()" Style="font-family: Trebuchet MS; font-size: medium;
                                        font-weight: bold; height: 26px; text-indent: 12px; width: 250px;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtuname"
                                        ErrorMessage="User Name is required." ToolTip="User Name is required." ForeColor="Red"
                                        Style="top: 250px; left: 912px; position: absolute; height: 16px; width: 8px">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" CssClass="textbox txt2"
                                        placeholder="PassWord" Style="margin-top: 12px; font-family: Trebuchet MS; font-size: medium;
                                        font-weight: bold; height: 26px; text-indent: 12px; width: 250px;"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="pwdvalidation" runat="server" Text="Username or Password is incorrect"
                                        Font-Names="Trebuchet MS" Font-Size="Small" ForeColor="#CC0000" Style="margin-left: 85px;"
                                        Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #3180c2;">
                                    <center>
                                        <br />
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Keep me signed in" Style="height: 21px;
                                            color: White; width: 156px;" Font-Names="Trebuchet MS" Font-Size="15px" Font-Bold="True" />
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="Login" Font-Names="Trebuchet MS"
                                            Font-Size="24px" Font-Bold="True" />
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #3180c2; font-size: 14px;">
                                    <br />
                                    <center>
                                        <span style="color: White; font-family: Trebuchet MS; position: relative; top: -5px;">
                                            Powered By</span>
                                        <img src="images/palpap.jpg" style="height: 20px;" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </center>
        </div>
    </center>
    <div style="height: 0px;" class="style2">
        <table>
            <tr style="width: 100%">
                <tr>
                    <td>
                    </td>
                </tr>
                <td align="center" style="height: 20px">
                    <asp:Label ID="collegename" runat="server" Text="collegename" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="X-Large" ForeColor="White" Height="20px" Width="949px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 10px">
                    <asp:Label ID="address" runat="server" Font-Bold="True" Height="10px" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 10px">
                    <asp:Label ID="category" runat="server" Font-Size="Small" Font-Bold="True" Height="10px"
                        Font-Names="Book Antiqua" ForeColor="White"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="height: 10px">
                    <asp:Label ID="appro" runat="server" Text="Approved by AICTE - Accredited by NBA"
                        Font-Bold="True" Height="10px" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="White"
                        Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblmsgcredit" runat="server" Text="Credit" ForeColor="White" Font-Bold="true"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <center>
        <div id="Div1" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="Div2" runat="server" class="table" style="background-color: White; height: 146px;
                    width: 281px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label1" Visible="true" runat="server" Text="Enter OTP" Style="color: Blue;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtOtp" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        Style="font-family: Trebuchet MS; font-size: medium; font-weight: bold; width: 120px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <center>
                                        <asp:Button ID="Button2" CssClass=" textbox btn1 textbox1" Style="font-weight: bold;
                                            height: 32px; width: 65px;" Text="OK" runat="server" OnClick="Button2_Click"  /><%--OnClick="Button2_Click"--%>
                                        <asp:Button ID="Button3" CssClass=" textbox btn1 textbox1" Style="font-weight: bold;
                                            height: 32px; width: 90px;" Text="Cancel" runat="server" OnClick="Button3_Click" /><%-- OnClick="Button3_Click"--%>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" Visible="false" runat="server" Text="Invalid OTP" Style="color: Blue;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    </form>
    <img src="images/logo.png" style="position: absolute; height: 75px; bottom: 90px;" />
    <span id="spnsettings" style="position: absolute; height: 75px; bottom: 20px; color: Blue;">
        Best View In Mozilla Firefox </span>
</body>
</html>
