<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="fullandfinalsettlement.aspx.cs" Inherits="fullandfinalsettlement" %>

<%@ Register Src="~/Usercontrols/GridPrintMaster.ascx" TagName="GridPrintMaster"
    TagPrefix="InsproplusGrid" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        function printTTOutput() {
            var panel = document.getElementById("<%=printdiv.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }

    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <div>
                    <center>
                        <div>
                            <span class="fontstyleheader" style="color: #008000">Staff Full and Final Settlement</span></div>
                    </center>
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                            <div class="maindivstyle" style="width: 1000px; height: auto;">
                                <br />
                                <div>
                                    <center>
                                        <table class="maintablestyle" width="400px">
                                            <tr align="center">
                                                <td>
                                                    <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight3" Width="250px"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_Change">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table class="maintablestyle">
                                            <tr>
                                                <td colspan="12">
                                                    Month & Year
                                                    <asp:DropDownList ID="ddl_mon" runat="server" OnSelectedIndexChanged="ddl_mon_Change"
                                                        AutoPostBack="true" CssClass="textbox1 ddlheight1">
                                                        <asp:ListItem Value="1">Jan</asp:ListItem>
                                                        <asp:ListItem Value="2">Feb</asp:ListItem>
                                                        <asp:ListItem Value="3">Mar</asp:ListItem>
                                                        <asp:ListItem Value="4">Apr</asp:ListItem>
                                                        <asp:ListItem Value="5">May</asp:ListItem>
                                                        <asp:ListItem Value="6">June</asp:ListItem>
                                                        <asp:ListItem Value="7">July</asp:ListItem>
                                                        <asp:ListItem Value="8">Aug</asp:ListItem>
                                                        <asp:ListItem Value="9">Sep</asp:ListItem>
                                                        <asp:ListItem Value="10">Oct</asp:ListItem>
                                                        <asp:ListItem Value="11">Nov</asp:ListItem>
                                                        <asp:ListItem Value="12">Dec</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddl_year" runat="server" CssClass="textbox1 ddlheight1">
                                                    </asp:DropDownList>
                                                    Staff Code
                                                    <asp:TextBox ID="txtstaffcode" runat="server" OnTextChanged="txtstaff_txtchanged"
                                                        AutoPostBack="true" CssClass="textbox textbox1 txtheight2" Width="126px"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstaffcode"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                    Staff Name
                                                    <asp:TextBox ID="txtstaffname" runat="server" OnTextChanged="txtname_txtchanged"
                                                        AutoPostBack="true" CssClass="textbox textbox1 txtheight5" Width="177px"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtstaffname"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="txtsearchpan">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Department
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updept" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_dept" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                                                <asp:CheckBox ID="cb_dept" runat="server" AutoPostBack="true" OnCheckedChanged="cb_dept_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_dept_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="pnlextnder" runat="server" PopupControlID="pnldept"
                                                                TargetControlID="txt_dept" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    Designation
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updesi" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_desig" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnldes" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                                                <asp:CheckBox ID="cb_desig" runat="server" AutoPostBack="true" OnCheckedChanged="cb_desig_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_desig" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_desig_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" PopupControlID="pnldes"
                                                                TargetControlID="txt_desig" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    Staff Category
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upstaffcat" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_staffcat" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnl_staffcat" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="200px">
                                                                <asp:CheckBox ID="cb_staffcat" runat="server" AutoPostBack="true" OnCheckedChanged="cb_staffcat_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_staffcat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_staffcat_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" PopupControlID="pnl_staffcat"
                                                                TargetControlID="txt_staffcat" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Staff Type
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updstafftype" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_stafftyp" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="pnlstafftyp" runat="server" CssClass="multxtpanel" Height="200px"
                                                                Width="200px">
                                                                <asp:CheckBox ID="cb_stafftyp" runat="server" AutoPostBack="true" OnCheckedChanged="cb_stafftyp_CheckedChanged"
                                                                    Text="Select All" />
                                                                <asp:CheckBoxList ID="cbl_stafftyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_stafftyp_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" PopupControlID="pnlstafftyp"
                                                                TargetControlID="txt_stafftyp" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Upgobtn" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_click" CssClass="textbox textbox1 btn1" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <br />
                                    <br />
                                    <center>
                                        <asp:UpdatePanel ID="upgo" runat="server">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                                <div id="div1" runat="server" visible="false" style="border-radius: 10px; overflow: auto;">
                                                    <asp:GridView ID="grdfinalsettlement" Width="1000px" runat="server" ShowFooter="false"
                                                        AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua"
                                                        toGenerateColumns="false" ShowHeaderWhenEmpty="true" OnRowCreated="OnRowCreated"
                                                        OnRowDataBound="grdfinalsettlement_RowDataBound" OnSelectedIndexChanged="SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Staff Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="stfcode" runat="server" Text='<%#Eval("stfCode") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Staff Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="stfname" runat="server" Text='<%#Eval("stfName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Department">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="deptname" runat="server" Text='<%#Eval("deptName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Designation">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="designame" runat="server" Text='<%#Eval("desigName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="grdfinalsettlement" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </center>
                                    </br> </br>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </center>
        </div>
        <asp:UpdatePanel ID="popup" runat="server">
            <ContentTemplate>
                <div id="popwindow" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="btnClose" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 53px; margin-left: 1209px;"
                        OnClick="btnClose_Click" />
                    <br />
                    <br />
                    <br />
                    <center>
                        <div id="div4" style="background-color: White; height: 1429px; font-family: Book Antiqua;
                            font-weight: bold; width: 975px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                            border-radius: 10px;">
                            <br />
                            <br />
                            <br />
                            <center>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                <div id="printdiv" runat="server">
                                    <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                                        <tr>
                                            <th class="marginSet" align="center" colspan="6">
                                                <span id="spCollegeName" class="headerDisp" runat="server"></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th class="marginSet" align="center" colspan="6">
                                                <span id="spAddr" class="headerDisp1" runat="server"></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th class="marginSet" align="center" colspan="6">
                                                <span id="spReportName" class="headerDisp1" runat="server"></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="marginSet" colspan="3" align="center">
                                                <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                                            </td>
                                            <td class="marginSet" colspan="3" align="right">
                                                <span id="spSem" class="headerDisp1" runat="server"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="marginSet" colspan="3" align="left">
                                                <span id="spProgremme" class="headerDisp1" runat="server"></span>
                                            </td>
                                            <td class="marginSet" colspan="3" align="right">
                                                <span id="spSection" class="headerDisp1" runat="server"></span>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="div2" runat="server" visible="false" style="border-radius: 10px; overflow: auto;">
                                        <asp:GridView ID="grdgenfinalset" Width="815px" runat="server" ShowFooter="false"
                                            AutoGenerateColumns="true" Font-Names="Book Antiqua" toGenerateColumns="true"
                                            OnRowCreated="OnRowCreated_finalsettlement" OnRowDataBound="grdgenfinalset_RowDataBound"
                                            OnSelectedIndexChanged=" SelectedIndexChanged_finalsettlement">
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                            <%--  <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>--%>
                                        </asp:GridView>
                                    </div>
                                    <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: right">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </div>
                    </center>
                    <%--progressBar for AdditionalDetails--%>
                    <center>
                        <div id="rptprint1" runat="server" visible="false">
                            <br />
                            <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                Height="35px" CssClass="textbox textbox1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                            <button id="btnPrint" runat="server" visible="true" height="29px" width="62px" onclick="return printTTOutput();"
                                style="font-weight: bold; font-size: medium; font-family: Book Antiqua;">
                                Direct Print
                            </button>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <center>
            <asp:UpdateProgress ID="UpdateProgress12" runat="server" AssociatedUpdatePanelID="Upgobtn">
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
            <asp:ModalPopupExtender ID="ModalPopupExtender12" runat="server" TargetControlID="UpdateProgress12"
                PopupControlID="UpdateProgress12">
            </asp:ModalPopupExtender>
        </center>
    </body>
</asp:Content>
