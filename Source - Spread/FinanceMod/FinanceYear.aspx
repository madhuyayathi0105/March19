<%@ Page Title="" Language="C#" MasterPageFile="~/FinanceMod/FinanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="FinanceYear.aspx.cs" Inherits="FinanceYear" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
        <script type="text/javascript">

            function valid() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txtacr.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtacr.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                var idval1 = document.getElementById("<%=txtacc.ClientID %>").value;
                if (idval1.trim() == "") {
                    idval1 = document.getElementById("<%=txtacc.ClientID %>");
                    idval1.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtdatestart.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtdatestart.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                idval = document.getElementById("<%=txtdateend.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txtdateend.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";
                }
                if (empty.trim() != "") {
                    return false;
                }
                else {
                    return true;
                }
            }

            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }


            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }

            function displaydateerr() {
                document.getElementById('<%=lbldateerr.ClientID %>').innerHTML = "";
            }


            function get(txt1) {
                $.ajax({
                    type: "POST",
                    url: "FinanceYear.aspx/CheckAcronym",
                    data: '{Acronym: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function OnSuccess(response) {
                var mesg = $("#screrr1")[0];
                switch (response.d) {
                    case "0":
                        mesg.style.color = "green";
                        mesg.innerHTML = "Acronym not exist";
                        break;
                    case "1":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Acronym Exist";
                        document.getElementById("<%=txtacr.ClientID %>").value = "";
                        break;
                    // case "2":           
                    //   mesg.style.color = "red";           
                    // mesg.innerHTML = "Acronym in Uppercase";           
                    // break;           
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error occurred";
                        break;
                }
            }

            function getacc(txt2) {
                $.ajax({
                    type: "POST",
                    url: "FinanceYear.aspx/checkAcctName",
                    data: '{acname: "' + txt2 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: Success,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function Success(response) {
                var mesg1 = $("#screrr2")[0];
                switch (response.d) {
                    case "0":
                        mesg1.style.color = "green";
                        mesg1.innerHTML = "Account Name not exist";
                        break;
                    case "1":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Account Name exist";
                        document.getElementById("<%=txtacc.ClientID %>").value = "";
                        break;
                    case "2":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Please enter Account Name";
                        break;
                    case "error":
                        mesg1.style.color = "red";
                        mesg1.innerHTML = "Error occurred";
                        break;
                }
            }

            function myFunCaps(id) {
                var txt = document.getElementById(id);
                var value = txt.value;
                txt.value = value.charAt(0).toUpperCase() + value.substr(1).toLowerCase();
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Financial Year</span></div>
                <%--<div style="margin-left: 780px;">
                    <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="Blue" PostBackUrl="~/Finance.aspx" CausesValidation="False">Back</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Small" ForeColor="Blue" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton ID="lb2" runat="server" Visible="true" OnClick="lb2_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" CausesValidation="False">Logout</asp:LinkButton>
                </div>--%>
                <center>
                    <div>
                        <div class="maindivstyle" style="width: 900px; height: 440px;">
                            <br />
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcol" runat="server" Text="College Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcolload" runat="server" CssClass="textbox textbox1 ddlheight6"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlcolload_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblval" runat="server" Text="Account Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_accname" runat="server" CssClass="textbox textbox1 txtheight2"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="paccname" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                    <asp:CheckBox ID="cbaccname" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cbaccname_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblaccname" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        OnSelectedIndexChanged="cblaccname_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pextender" runat="server" TargetControlID="txt_accname"
                                                    PopupControlID="paccname" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                                                                                                                   <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
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
                                        <asp:Button ID="bttngo" runat="server" CssClass="textbox textbox1 btn1" Text="Go"
                                            OnClick="bttngo_Click" />

                                                                        </ContentTemplate>
                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Button ID="bttnadd" runat="server" CssClass="textbox textbox1 btn2" Text="Add New"
                                            OnClick="bttnNew_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="lblerr" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                Font-Size="Medium"></asp:Label>
                            
                            <br />

                             <asp:GridView ID="gridview1" runat="server" ShowFooter="false"
                        AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="OnRowDataBound" >
                        <%-- --%>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
            </asp:GridView>

                            <br />
                            <center>
                                <div id="btns" runat="server" visible="false">
                                    <asp:Button ID="btnSelect" runat="server" Text="Select Financial Year" OnClick="btnSelect_Click"
                                        CssClass="textbox textbox1 btn2" Width="160px" />
                                    <asp:Button ID="btnmod" runat="server" Text="Modify" OnClick="btnmod_Click" CssClass="textbox textbox1 btn2" />
                                </div>
                            </center>
                        </div>
                        <br />
                        <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"
                                CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox textbox1 btn2"
                                Width="130px" Text="Export To Excel" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox textbox1 btn2" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </div>
                </center>
                <center>
                    <div id="popper1" runat="server" visible="false" class="popupstyle popupheight1 ">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 12px; margin-left: 320px;" OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <center>
                            <div style="background-color: White; height: 350px; width: 700px; border: 5px solid #0CA6CA;
                                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <div>
                                    <center>
                                        <div>
                                            <span class="fontstyleheader" style="color: Green;">Financial Year Creation</span></div>
                                    </center>
                                </div>
                                <br />
                                <div>
                                    <center>
                                        <table cellpadding="4">
                                            <tr style="display: none;">
                                                <td>
                                                    <asp:Label ID="lblCode" runat="server" Text="Code"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtcode" CssClass="textbox textbox1 txtheight1" onkeyup="return myFunCaps(this.id)"
                                                        Enabled="false" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCollege" runat="server" Text="College Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcol" runat="server" CssClass="textbox textbox1 ddlheight6"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcol_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <%--<asp:Label ID="lblcolname" runat="server" Visible="false" Font-Bold="true" Style="color: DarkGreen;"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblacc" runat="server" Text="Account Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtacc" CssClass="textbox textbox1 txtheight5" runat="server" onfocus="return myFunction(this)"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="filExtendNewS" runat="server" TargetControlID="txtacc"
                                                        FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=",. ">
                                                    </asp:FilteredTextBoxExtender>
                                                    <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                        id="screrr2"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblacr" runat="server" Text="Account Acronym"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtacr" Style="text-transform: uppercase;" CssClass="textbox textbox1 txtheight"
                                                        MaxLength="20" runat="server" onfocus="return myFunction(this)"></asp:TextBox>
                                                    <span style="color: Red; font-style: italic;">*</span> <span style="font-weight: bold;
                                                        font-size: larger;" id="screrr1"></span>
                                                    <asp:FilteredTextBoxExtender ID="txtext" runat="server" TargetControlID="txtacr"
                                                        FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblyearstart" runat="server" Text="Financial Year Start"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtdatestart" runat="server" AutoPostBack="true" OnTextChanged="txtdatestart_Change"
                                                        CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:CalendarExtender ID="caldatestart" TargetControlID="txtdatestart" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                    <span style="color: Red;">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblyearend" runat="server" Text="Financial Year End"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtdateend" runat="server" AutoPostBack="true" OnTextChanged="txtdateend_Change"
                                                        onfocus="return myFunction(this)" onclick="return displaydateerr()" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:CalendarExtender ID="caldateend" TargetControlID="txtdateend" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                    <span style="color: Red;">*</span><span><asp:Label ID="lbldateerr" runat="server"
                                                        ForeColor="Red" Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label></span>
                                                </td>
                                                <%--<td>
                                                <asp:Label ID="lbldis" runat="server" Text="District"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdis" CssClass="textbox textbox1" onfocus="return myFunction(this)"
                                                    Width="300px" runat="server"></asp:TextBox>
                                            </td>--%>
                                            </tr>
                                        </table>
                                        <br />
                                        <center>
                                            <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="textbox btn2 textbox1"
                                                OnClick="btnupdate_Click" OnClientClick="return valid()" Visible="false" />
                                            <asp:Button ID="btndelete" runat="server" Text="Delete" CssClass="textbox btn2 textbox1"
                                                OnClick="btndelete_Click" Visible="false" />
                                            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_Click" Visible="false"
                                                OnClientClick="return valid()" CssClass="textbox btn2 textbox1" />
                                            <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox btn2 textbox1"
                                                OnClick="btnexit_Click" />
                                        </center>
                                    </center>
                                </div>
                            </div>
                        </center>
                    </div>
                </center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox btn1 textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                <center>
                    <div id="imgdiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div4" runat="server" class="table" style="background-color: White; height: 150px;
                                width: 250px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 150px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnyes" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                        OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btnno" CssClass="textbox btn1" Style="height: 28px; width: 65px;"
                                                        OnClick="btnno_Click" Text="No" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
            </div>
        </center>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            
        </Triggers>
    </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>
