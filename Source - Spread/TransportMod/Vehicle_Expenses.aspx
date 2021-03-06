﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TransportMod/TransportSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Vehicle_Expenses.aspx.cs" Inherits="Vehicle_Expenses" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <head>
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_lblerr').innerHTML = "";

            }
        </script>
        <link href="Styles/css/Style.css" rel="Stylesheet" type="text/css" />
    </head>
    
    <body>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
        <asp:Panel ID="header_Panel" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Style="left: -16px; position: absolute; width: 1088px; height: 21px">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lbl_Header" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="Vehicle Diesel Expenses Cumulative Report"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <%--<asp:LinkButton ID="back_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/reports.aspx" CausesValidation="False">Back</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="home_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="logout_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Small" ForeColor="White" CausesValidation="False" OnClick="logout_btn_Click">Logout</asp:LinkButton>--%>
        </asp:Panel>
        <br />
        
        <asp:Panel ID="Panel1" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="left:-16px;
            top: 180px; position: absolute; width: 1088px; height: 21px">
        </asp:Panel>
        <table style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
            left: 10px; border-right-style: solid; background-color: lightblue; border-width: 1px;">
            <tr>

                <td>
                    <asp:Label ID="lblselectcollege" runat="server" Text="Select College" Font-Bold="True"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtcollege" runat="server" class="Dropdown_Txt_Box" Text="" Font-Bold="True"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>
                    <%--<asp:DropDownList ID="ddlselectcollege" runat="server" Width="120px" Font-Bold="True"
                        Font-Size="Medium" Font-Names="Book Antiqua">
                    </asp:DropDownList>--%>
                    <asp:Panel ID="pclg" runat="server" CssClass="MultipleSelectionDDL" Height="147px">
                        <asp:CheckBox ID="chekclg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chekclg_CheckedChanged" />
                        <asp:CheckBoxList ID="cheklist_clg" runat="server" Font-Size="Medium" Font-Bold="True"
                            Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="cheklist_clg_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txtcollege"
                        PopupControlID="pclg" Position="Bottom">
                    </asp:PopupControlExtender>
                </td>
                <br />
                <br />
                <td>
                    <asp:Label ID="lblfrommonth" runat="server" Text="From Month" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlfrommonth" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Width="80px">
                        <asp:ListItem Value="01">January</asp:ListItem>
                        <asp:ListItem Value="02">February</asp:ListItem>
                        <asp:ListItem Value="03">March</asp:ListItem>
                        <asp:ListItem Value="04">April</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">June</asp:ListItem>
                        <asp:ListItem Value="07">July</asp:ListItem>
                        <asp:ListItem Value="08">August</asp:ListItem>
                        <asp:ListItem Value="09">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                </td>
                <td>
                    <asp:Label ID="lblfromyear" runat="server" Text="Year" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlfromyear" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Width="60px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltomonth" runat="server" Text="To Month" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltomonth" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Width="80px">
                        <asp:ListItem Value="01">January</asp:ListItem>
                        <asp:ListItem Value="02">February</asp:ListItem>
                        <asp:ListItem Value="03">March</asp:ListItem>
                        <asp:ListItem Value="04">April</asp:ListItem>
                        <asp:ListItem Value="05">May</asp:ListItem>
                        <asp:ListItem Value="06">June</asp:ListItem>
                        <asp:ListItem Value="07">July</asp:ListItem>
                        <asp:ListItem Value="08">August</asp:ListItem>
                        <asp:ListItem Value="09">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbltoyear" runat="server" Text="Year" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltoyear" runat="server" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Width="60px">
                    </asp:DropDownList>
                </td>
                <td>
                <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                    <ContentTemplate>
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnClick="btngo_Click" />
                        </ContentTemplate>
                </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        </ContentTemplate>
     </asp:UpdatePanel>

        <table>
         <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                
                    <FarPoint:FpSpread ID="Fpvehicle" runat="server" Width="650px" Visible="false">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="false">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </td>

            </tr>
             </ContentTemplate>
    </asp:UpdatePanel>
            <tr>
                <td>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()">
                    </asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
            <tr>
            
                <td>
                    <asp:Label ID="lblerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="" ForeColor="Red"></asp:Label>
                </td>
                 
            </tr>
            </ContentTemplate>
     </asp:UpdatePanel>
        </table>
        
    </body>
           

     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
    </html>
</asp:Content>
