<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Designation_Master_Alter.aspx.cs" Inherits="Designation_Master_Alter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .stNew
        {
            text-transform: uppercase;
        }
        .stNew1
        {
            border: 1px solid gray;
            padding: 4px;
        }
    </style>
    <body>
        <script type="text/javascript">
            function valid() {
                var idval = "";
                var empty = "";
                var id = "";
                var value1 = "";
                idval = document.getElementById("<%=txt_desigacr.ClientID %>").value;
                if (idval.trim() == "") {
                    idval = document.getElementById("<%=txt_desigacr.ClientID %>");
                    idval.style.borderColor = 'Red';
                    empty = "E";

                }
                var idval1 = document.getElementById("<%=txt_designame.ClientID %>").value;
                if (idval1.trim() == "") {
                    idval1 = document.getElementById("<%=txt_designame.ClientID %>");
                    idval1.style.borderColor = 'Red';
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

            function get(txt1) {
                $.ajax({
                    type: "POST",
                    url: "Designation_Master_Alter.aspx/CheckDesAcronym",
                    data: '{desAcronym: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function OnSuccess(response) {
                var mesg = $("#screrr")[0];
                switch (response.d) {
                    case "0":
                        mesg.style.color = "green";
                        mesg.innerHTML = "";
                        break;
                    case "1":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Acronym Exist!";
                        document.getElementById("<%=txt_desigacr.ClientID %>").value = "";
                        break;
                    case "2":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Please Enter Acronym";
                        break;
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error occurred";
                        break;
                }
            }

            function getdes(txt1) {
                $.ajax({
                    type: "POST",
                    url: "Designation_Master_Alter.aspx/CheckDesName",
                    data: '{desName: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessdes,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function OnSuccessdes(response) {
                var mesg = $("#screrr1")[0];
                switch (response.d) {
                    case "0":
                        mesg.style.color = "green";
                        mesg.innerHTML = "";
                        break;
                    case "1":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Designation Exist!";
                        document.getElementById("<%=txt_designame.ClientID %>").value = "";
                        break;
                    case "2":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Please Enter Designation";
                        break;
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error occurred";
                        break;
                }
            }

        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Style="font-size: x-large;
                            color: Green;" Text="Designation Master"></asp:Label>
                        <br />
                    </div>
                </center>
                <div class="maindivstyle" style="height: auto; width: 1000px;">
                    <br />
                    <table class="maintablestyle" width="800px" height="40px">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_clg" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="College Name"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_col" runat="server" Width="250px" CssClass="textbox1 ddlheight4"
                                    OnSelectedIndexChanged="ddl_col_Change" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stream" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Visible="false" runat="server" Text="Stream"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" Visible="false" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_stream" runat="server" Height="15px" CssClass="textbox textbox1 txtheight"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 100px;">
                                            <asp:CheckBox ID="cb_stream" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_stream_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_stream" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_stream_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txt_stream"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_desname" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Designation Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_desname" runat="server" MaxLength="15" Height="15px" Style="font-family: 'Book Antiqua';
                                    font-size: Medium;" CssClass="textbox textbox1 txtheight2"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetDesig" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_desname"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="panelbackground">
                                </asp:AutoCompleteExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:CheckBox ID="chkpriority" runat="server" OnCheckedChanged="chkpriority_Change"
                                    AutoPostBack="true" Text="Priority" />
                                &nbsp;
                                <asp:LinkButton ID="lb_deptpr" runat="server" Text="Department Priority" OnClick="lb_deptpr_click"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Large"></asp:LinkButton>
                                &nbsp;
                                <asp:Button ID="btngo" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btngo_click" />
                                <asp:Button ID="btnaddnew" runat="server" Text="Add New" CssClass="textbox textbox1 btn2"
                                    OnClick="btnaddnew_click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <asp:Label ID="lblerrgo" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                    <div id="div1" runat="server" visible="true" style="width: 850px; height: 350px;
                        overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                        <br />
                        <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="true" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Width="750px" Height="300px" OnCellClick="Cellcont_Click"
                            OnPreRender="Fpspread1_render" OnButtonCommand="Fpspread1_buttoncommand" CssClass="spreadborder"
                            ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <div id="PriorityDiv" runat="server" visible="false">
                        <center>
                            <asp:Button ID="btnsetpriority" runat="server" Text="Set Priority" CssClass="textbox textbox1 btn2"
                                OnClick="btnsetpriority_Click" Visible="false" />
                            <asp:Button ID="btnresetpriority" runat="server" Text="Reset" CssClass="textbox textbox1 btn2"
                                OnClick="btnresetpriority_Click" Visible="false" />
                        </center>
                    </div>
                    <br />
                    <div id="rportprint" runat="server" visible="true">
                        <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrporttname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                        <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel_Click" Font-Size="Medium" CssClass="textbox textbox1" Text="Export To Excel"
                            Width="127px" Height="35px" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                </div>
                <center>
                    <div id="poppernew" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 95px; margin-left: 360px;" OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 690px; width: 750px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <br />
                                <asp:Label ID="lbl_newdesig" runat="server" Style="font-size: large; color: #790D03;"
                                    Text="New Designation"></asp:Label>
                                <br />
                            </center>
                            <div>
                                <center>
                                    <table cellpadding="6" style="padding: 30px;">
                                        <tr>
                                            <td>
                                                College
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlnewcol" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlnewcol_Change"
                                                    CssClass="textbox1 ddlheight3" Width="250px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Designation Code
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdescode" runat="server" Enabled="false" onfocus="return myFunction(this)"
                                                    CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_desigacr" runat="server" Text="Designation Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_desigacr" runat="server" MaxLength="6" onfocus="return myFunction(this)"
                                                    onblur="return get(this.value)" CssClass="textbox textbox1 txtheight4 stNew stNew1"></asp:TextBox>
                                                <span id="spnacr" style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                    id="screrr"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_desigacr"
                                                    FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-()" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_designame" runat="server" Text="Designation Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_designame" runat="server" MaxLength="50" onfocus="return myFunction(this)"
                                                    onblur="return getdes(this.value)" CssClass="textbox textbox1 txtheight4 stNew1"></asp:TextBox>
                                                <span id="Span1" style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                    id="screrr1"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_designame"
                                                    FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_streamlst" runat="server" Text="Stream"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_streamlst" CssClass="textbox textbox1 ddlheight stNew1"
                                                    runat="server" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_staftyp" runat="server" Text="Staff Type"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Button ID="btnstaf" runat="server" Text="+" CssClass="textbox textbox1 btn stNew"
                                                    Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnstaf_Click" />
                                                <asp:DropDownList ID="ddlstaftyp" CssClass="textbox textbox1 ddlheight2 stNew1" runat="server"
                                                    AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:Button ID="btnstafmin" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox textbox1 btn stNew" OnClick="btnstafmin_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_staffacr" Visible="false" runat="server" Text="Staff Type Acronym"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_staffacr" Visible="false" runat="server" MaxLength="6" CssClass="textbox textbox1 txtheight4 stNew"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                                <center>
                                    <div>
                                        <FarPoint:FpSpread ID="Fpspreaddept" runat="server" Visible="true" CssClass="spreadborder"
                                            OnButtonCommand="Fpspreaddept_ButtonCommand" OnUpdateCommand="Fpspreaddept_OnUpdateCommand"
                                            ShowHeaderSelection="false">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </div>
                                </center>
                                <center>
                                    <br />
                                    <br />
                                    <div>
                                        <asp:Button ID="btn_save" runat="server" Visible="true" Width="88px" Height="32px"
                                            CssClass="textbox textbox1" Text="Save" OnClientClick="return valid()" OnClick="btn_save_Click" />
                                        <asp:Button ID="btn_update" runat="server" Visible="true" Width="88px" Height="32px"
                                            CssClass="textbox textbox1" Text="Update" OnClientClick="return valid()" OnClick="btn_update_Click" />
                                        <asp:Button ID="btndel" runat="server" Text="Delete" OnClick="btndel_Click" Width="88px"
                                            Height="32px" CssClass="textbox textbox1" />
                                        <asp:Button ID="btn_exit" runat="server" Visible="true" Width="88px" Height="32px"
                                            CssClass="textbox textbox1" Text="Exit" OnClick="btn_exit_Click" />
                                    </div>
                                </center>
                            </div>
                        </div>
                    </div>
                    <div id="popdept" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="imgdept" runat="server" ImageUrl="../images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 98px; margin-left: 353px;" OnClick="imgdept_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: 644px; width: 750px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <center>
                                <asp:Label ID="lblpopheader" runat="server" Text="Department Priority" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
                            </center>
                            <br />
                            <table id="Table1" class="maintablestyle" runat="server">
                                <tr>
                                    <td>
                                        College
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlcoldept" runat="server" Width="250px" CssClass="textbox1 ddlheight4"
                                            OnSelectedIndexChanged="ddlcoldept_Change" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Department
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upddept" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtdept" runat="server" Height="15px" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pdept" runat="server" CssClass="multxtpanel" Style="height: 200px;">
                                                    <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_dept_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="pdeptpop" runat="server" TargetControlID="txtdept"
                                                    PopupControlID="pdept" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBox ID="chkdeptpriority" runat="server" Text="Priority" OnCheckedChanged="chkdeptpriority_Change"
                                            AutoPostBack="true" />
                                        &nbsp; &nbsp;
                                        <asp:CheckBox ID="chkdeptpriority1" runat="server" Text="Priority1" OnCheckedChanged="chkdeptpriority1_Change"
                                            AutoPostBack="true" />
                                        &nbsp; &nbsp;
                                        <asp:Button ID="btnpopdeptgo" runat="server" Text="Go" CssClass="textbox textbox1 btn2"
                                            OnClick="btnpopdeptgo_click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <center>
                                <div id="divdept" runat="server" visible="true" style="width: 700px; height: 375px;
                                    overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                                    <br />
                                    <FarPoint:FpSpread ID="Fpspreadpopdept" runat="server" Visible="true" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="1px" Width="650px" Height="300px" OnCellClick="Cellpopdept_Click"
                                        OnPreRender="Fpspreadpopdept_render" OnButtonCommand="Fpspreadpopdept_buttoncommand"
                                        CssClass="spreadborder" ShowHeaderSelection="false">
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>
                                </div>
                            </center>
                            <br />
                            <center>
                                <div id="DptPriorityDiv" runat="server" visible="false">
                                    <asp:Button ID="btnsetdeptpriority" runat="server" Text="Set Priority" CssClass="textbox textbox1 btn2"
                                        OnClick="btnsetdeptpriority_click" />
                                    <asp:Button ID="btnresetdeptpriority" runat="server" Text="Reset" CssClass="textbox textbox1 btn2"
                                        OnClick="btnresetdeptpriority_click" />
                                    <asp:Button ID="btnexitdept" runat="server" Text="Exit" CssClass="textbox textbox1 btn2"
                                        OnClick="btnexitdept_click" />
                                </div>
                            </center>
                        </div>
                    </div>
                </center>
                <center>
                    <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                        <center>
                            <div id="panel_addreason" runat="server" visible="false" class="table" style="background-color: White;
                                height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                margin-top: 200px; border-radius: 10px;">
                                <table style="line-height: 30px">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_addreason" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txt_addreason" runat="server" MaxLength="25" Width="200px" CssClass="textbox textbox1"
                                                onkeypress="display1()"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="line-height: 35px">
                                            <asp:Button ID="btn_addreason" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1 btn2" OnClick="btn_addreason_Click" />
                                            <asp:Button ID="btn_exitreason" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1 btn2" OnClick="btn_exitaddreason_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="imgDiv1" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div4" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblconfirm" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnyes" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btnno" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnno_Click" Text="No" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="imgDivdel" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblconfirmdel" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnyesdel" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnyesdel_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btnnodel" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnnodel_Click" Text="No" runat="server" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <center>
                    <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                        OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
        <div>
        </div>
    </body>
    </html>
</asp:Content>
