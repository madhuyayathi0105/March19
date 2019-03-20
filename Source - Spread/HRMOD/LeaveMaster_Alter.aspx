<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="LeaveMaster_Alter.aspx.cs" Inherits="LeaveMaster_Alter" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .stNew
        {
            text-transform: none;
        }
        .stcap
        {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">

        function check() {
            var id = "";
            var value1 = "";
            var idval = "";
            var empty = "";
            id = document.getElementById("<%=txt_leavename.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_leavename.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txt_shrtfm.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_shrtfm.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            if (empty != "") {
                return false;
            }
            else {

                return true;
            }
        }

        function display(x) {
            x.style.borderColor = "#c4c4c4";
        }
        function display() {
            document.getElementById('<%=lbl_validation.ClientID %>').innerHTML = "";
        }

        function getCatAcr(txt) {
            $.ajax({
                type: "POST",
                url: "LeaveMaster_Alter.aspx/checkCatAcr",
                data: '{CatAcr: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: Success,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function Success(response) {
            var mesg1 = $("#screrr")[0];
            switch (response.d) {
                case "0":
                    mesg1.style.color = "green";
                    mesg1.innerHTML = "";
                    break;
                case "1":
                    mesg1.style.color = "red";
                    document.getElementById('<%=txt_shrtfm.ClientID %>').value = "";
                    mesg1.innerHTML = "Already Exist!";
                    break;
                case "2":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Enter Short Form";
                    break;
                case "error":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Error occurred";
                    break;
            }
        }

        function getCatName(txt) {
            $.ajax({
                type: "POST",
                url: "LeaveMaster_Alter.aspx/checkCatName",
                data: '{CatName: "' + txt + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: onSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }
        function onSuccess(response) {
            var mesg1 = $("#screrrname")[0];
            switch (response.d) {
                case "0":
                    mesg1.style.color = "green";

                    mesg1.innerHTML = "";
                    break;
                case "1":
                    mesg1.style.color = "red";
                    document.getElementById('<%=txt_leavename.ClientID %>').value = "";
                    mesg1.innerHTML = "Already Exist!";
                    break;
                case "2":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Enter LeaveName";
                    break;
                case "error":
                    mesg1.style.color = "red";
                    mesg1.innerHTML = "Error occurred";
                    break;
            }
        }

    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Leave Master</span></div>
            </center>
        </div>
        <center>
            <div class="maindivstyle">
                <%--maincontent--%>
                <center>
                    <div>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox1 ddlheight5"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_college_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" Visible="false" CssClass="textbox1 btn2" Text="Go"
                                        OnClick="btn_go_Click" />
                                </td>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_addnew" runat="server" CssClass="textbox1 btn2" Text="Add New"
                                            Style="font-weight: bold;" OnClick="btn_addnew_Click" />
                                    </center>
                                </td>
                                <td>
                                    <asp:Label ID="lbLeavereason" runat="server" Text="Leave Reason Mapping"></asp:Label>
                                    <asp:DropDownList ID="ddlleavemapping" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_leavereason_click">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="1">P</asp:ListItem>
                                        <asp:ListItem Value="2">A</asp:ListItem>
                                        <asp:ListItem Value="3">PER</asp:ListItem>
                                        <asp:ListItem Value="4">OD</asp:ListItem>
                                        <asp:ListItem Value="5">LA</asp:ListItem>
                                        <asp:ListItem Value="6">RL</asp:ListItem>
                                        <asp:ListItem Value="7">NA</asp:ListItem>
                                        <asp:ListItem Value="8">H</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_streamplus" Text="+" runat="server" Visible="false" OnClick="btn_streamplus_OnClick"
                                        Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;
                                        border-radius: 6px;" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_leave" runat="server" Visible="false" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox ddlstyle ddlheight3"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_streamminus" Text="-" runat="server" Visible="false" OnClick="btn_streamminus_OnClick"
                                        Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;
                                        border-radius: 6px;" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_saveval" Text="Save" runat="server" Visible="false" OnClick="btn_Save_OnClick"
                                        Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;
                                        border-radius: 6px;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                   <%-- <asp:CheckBox ID="chkpriority" runat="server" OnCheckedChanged="chkpriority_Change"
                                        AutoPostBack="true" Text="Priority" />--%>
                                    &nbsp;
                                    <asp:LinkButton ID="lb_deptpr" runat="server" Text="Leave Priority" OnClick="Leavepriority_click"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Large"></asp:LinkButton>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <center>
                    <div id="Plusapt" runat="server" visible="false" class="popupstyle popupheight1"
                        style="position: fixed; width: 100%; z-index: 1000; height: 100%; border-style: 1px;">
                        <center>
                            <div id="Div112" runat="server" class="table" style="background-color: White; height: 150px;
                                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                border-radius: 10px;">
                                <center>
                                    <br />
                                    <table style="line-height: 30px">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="headerapt" runat="server" Text="Leave Reason Mapping" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txt_addstream" runat="server" MaxLength="25" CssClass="textbox txtheight2"
                                                    Style="font-weight: bold; width: 200px; font-family: book antiqua; font-size: medium;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="line-height: 35px">
                                                <asp:Button ID="btn_plusAdd" Text=" Add " Visible="false" runat="server" OnClick="btn_plusAdd_OnClick"
                                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; border-radius: 6px;" />
                                                <asp:Button ID="btn_Plusexit" Text=" Exit " runat="server" OnClick="btn_Plusexit_OnClick"
                                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium; border-radius: 6px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </center>
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <center>
                    <div id="div1" runat="server" visible="true" style="width: 850px; height: 350px;
                        overflow: auto; border: 1px solid Gray; background-color: White;">
                        <br />
                        <%--<FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="600px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            class="spreadborder" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchange">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>--%>
                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                        <asp:GridView ID="grdleave" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                            Font-Names="Book Antiqua" ShowHeader="false" toGenerateColumns="false" OnRowDataBound="grdleave_RowDataBound"
                            OnRowCreated="grdleave_OnRowCreated" OnSelectedIndexChanged="grdleave_SelectedIndexChanged">
                            <%-- --%>
                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                        </asp:GridView>
                    </div>
                    <br />
                    <div id="rptprint" runat="server" visible="true" style="font-weight: bold;">
                        <asp:Label ID="lbl_validation" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false" onkeypress="display()"></asp:Label>
                        <asp:Label ID="lbl_rptname" runat="server" Text="Report Name" Style="font-family: Book Antiqua;
                            font-size: medium;"></asp:Label>
                        <asp:TextBox ID="txt_excelname" runat="server" Width="180px" onkeypress="display()"
                            CssClass="textbox textbox1"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_excel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btn_excel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                        <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txt_excelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btn_printmaster" runat="server" Text="Print" OnClick="btn_printmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <insproplus:printmaster runat="server" id="Printcontrol" visible="false" />
                    </div>
                </center>
                <br />
                <center>
                    <div id="addnew" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 95px; margin-left: 278px;"
                            OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div id="add" runat="server" style="background-color: White; height: 340px; width: 584px;
                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <asp:Label ID="lbl_newdesg" runat="server" Style="font-size: large; color: #790D03;"
                                    Text="Leave Details"></asp:Label>
                                <br />
                                <br />
                                <br />
                            </center>
                            <div>
                                <center>
                                    <table style="line-height: 40px;">
                                        <tr>
                                            <td>
                                                College
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_popclg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_popclg_Change"
                                                    CssClass="textbox1 ddlheight3" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_leavename" runat="server" Text="Leave Name"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_leavename" runat="server" MaxLength="15" Height="25px" CssClass="textbox textbox1 stNew"
                                                    Width="150px" onfocus=" return display(this)" onblur="return getCatName(this.value)"></asp:TextBox>
                                                <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                    id="screrrname"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_leavename"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:Label ID="oldlname" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_shrtfm" runat="server" Text="Short Form"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_shrtfm" runat="server" Height="25px" CssClass="textbox textbox1 stcap"
                                                    Width="150px" onfocus=" return display(this)" onblur="return getCatAcr(this.value)"
                                                    MaxLength="3"></asp:TextBox>
                                                <span style="color: Red;">*</span><span style="font-weight: bold; font-size: larger;"
                                                    id="screrr"></span>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_shrtfm"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars="  ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButton ID="rb_earn" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                                    Text="Earn Leave"></asp:RadioButton>
                                                <%--Checked="true"--%>
                                                <asp:RadioButton ID="rb_tpres" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                                    Text="Treated as Present"></asp:RadioButton>
                                                <asp:RadioButton ID="rb_gnrl" runat="server" RepeatDirection="Horizontal" GroupName="same"
                                                    Text="LOP"></asp:RadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <br />
                                        <br />
                                        <div>
                                            <asp:Button ID="btn_save" runat="server" Visible="true" CssClass="textbox1 btn2"
                                                Text="Save" Style="font-weight: bold;" OnClientClick="return check()" OnClick="btn_save_Click" />
                                            <asp:Button ID="btndel" runat="server" Visible="true" CssClass="textbox1 btn2" Text="Delete"
                                                Style="font-weight: bold;" OnClientClick="return check()" OnClick="btndel_Click" />
                                            <asp:Button ID="btn_exit" runat="server" Visible="true" CssClass="textbox1 btn2"
                                                Text="Exit" Style="font-weight: bold;" OnClick="btn_exit_Click" />
                                        </div>
                                    </center>
                                </center>
                            </div>
                        </div>
                    </div>
                </center>
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
                            <asp:Label ID="lblpopheader" runat="server" Text="Leave Priority" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
                        </center>
                        <br />
                        <center>
                            <div id="divdept" runat="server" visible="true" style="width: 520px; height: 375px;
                                overflow: auto; border: 1px solid Gray; background-color: White; border-radius: 10px;">
                                <br />
                                <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                <asp:GridView ID="grdpri" Width="500px" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                    Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true" toGenerateColumns="false"
                                    AllowPaging="true" PageSize="100" OnRowDataBound="grdpri_RowDataBound" OnPageIndexChanging="grdpri_OnPageIndexChanged">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leave Name">
                                            <ItemTemplate>
                                                <asp:Label ID="leavename" runat="server" Text='<%#Eval("LeaveName") %>' Style="text-align: left;"
                                                    Width="100px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Short Form">
                                            <ItemTemplate>
                                                <asp:Label ID="shortname" runat="server" Text='<%#Eval("ShortName") %>' Style="text-align: left;"
                                                    Width="50px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Leave PK">
                                            <ItemTemplate>
                                                <asp:Label ID="leavepk" runat="server" Text='<%#Eval("LeavePk") %>' Style="text-align: left;"
                                                    />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Set Priority">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="selectchk" runat="server" Text='<%#Eval("chkval") %>' Style="text-align:center;"
                                                    Width="50px" AutoPostBack="true" OnCheckedChanged="setpriority_checkedchange" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Priority">
                                            <ItemTemplate>
                                                <asp:Label ID="priority" runat="server" Text='<%#Eval("Priority") %>' Style="text-align:center;"
                                                    Width="50px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                </asp:GridView>
                            </div>
                        </center>
                        <br />
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
                        <br />
                    </div>
                </div>
            </div>
        </center>
        <center>
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
                                        <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass=" textbox1 btn2 comm" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </center>
    </body>
    </html>
</asp:Content>
