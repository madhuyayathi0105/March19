<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Staff_FingerPrintReg.aspx.cs" Inherits="Staff_FingerPrintReg" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
   <script type="text/javascript">
       function display() {
           document.getElementById("<%=lblsmserror.ClientID %>").innerHTML = "";
       }
   </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <center>
                    <br />
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Finger Print Report</span></div>
                </center>
                <div id="Div1" class="maindivstyle" runat="server" style="width: 1000px; height: auto;">
                    <br />
                    <table id="Table2" class="maintablestyle" runat="server" width="920px">
                        <tr>
                            <td>
                                College
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlpopclg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlpopclg_change"
                                    CssClass="textbox1 ddlheight3" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox1 ddlheight3" Width="250px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Staff Category
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlstaffcat" runat="server" CssClass="textbox1 ddlheight3">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkincrel" runat="server" Text="Include Relieved Staff" />
                            </td>
                            <td>
                                Staff With
                            </td>
                            <td>
                                <fieldset id="fldfinger" runat="server" style="border-radius: 5px;" width="250px">
                                    <asp:RadioButton ID="rbfingerid" runat="server" Text="FingerID" GroupName="fin" Checked="true" />
                                    <asp:RadioButton ID="rbnofingerid" runat="server" Text="No FingerID" GroupName="fin" />
                                </fieldset>
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btngo" runat="server" Text="GO" CssClass="textbox1 btn2" OnClick="btngo_click" />
                                <asp:Button ID="btnreport" runat="server" Text="Add" CssClass="textbox1 btn2" OnClick="btnreport_click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblpoperr" runat="server" Text="" Visible="false" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Larger" ForeColor="Red"></asp:Label>
                    <br />
                    <br />
                    <center>
                        <div id="divpopspr" runat="server" style="width: 900px; height: 350px; border-radius: 10px;
                            background-color: White;">
                            <FarPoint:FpSpread ID="Fpspreadpop" runat="server" Visible="false" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" Width="900px" Height="350px" Style="margin-left: 2px;"
                                class="spreadborder" OnButtonCommand="Fpspreadpop_ButtonCommand" ShowHeaderSelection="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    <br />
                    <br />
                    <div id="rprint" runat="server" visible="true">
                        <asp:Label ID="lblsmserror" Text="Please Enter Your Report Name" Font-Size="Large"
                            Font-Names="Book Antiqua" Visible="false" ForeColor="Red" runat="server" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblexcel" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtexcel" onkeypress="display()" CssClass="textbox textbox1" runat="server"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcel"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnexcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox textbox1 btn2" Width="150px" Text="Export Excel"
                            OnClick="btnexcel_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Print" OnClick="btnprintmaster_Click" CssClass="textbox textbox1 btn2" Width="100px"/>
                        <asp:Button ID="btndelete" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Delete" OnClick="btndelete_Click" CssClass="textbox textbox1 btn2" Width="100px" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                    <br />
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="poperrjs" runat="server" visible="false" style="height: 100em; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="../images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-top: 39px; margin-left: 430px;"
                    OnClick="imagebtnpopcloseadd_Click" />
                <br />
                <br />
                <div class="subdivstyle" style="background-color: White; overflow: auto; width: 900px;
                    height: 690px;" align="center">
                    <br />
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Staff Finger Print Registration</span>
                    </center>
                    <br />
                    <table id="Table1" class="maintablestyle" runat="server" style="width: 875px;">
                        <tr>
                            <td>
                                College
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcoll" runat="server" CssClass="textbox1 ddlheight3" Width="250px"
                                    OnSelectedIndexChanged="ddlcoll_Change" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdept" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Height="200px">
                                            <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popdept" runat="server" TargetControlID="txtdept" PopupControlID="pnldept"
                                            Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Staff Category
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_staffc" runat="server" ReadOnly="true" CssClass="textbox txtheight3">--Select--</asp:TextBox>
                                        <asp:Panel ID="P3" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                            border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                            box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                            <asp:CheckBox ID="cb_staffc" runat="server" Text="Select All" OnCheckedChanged="cb_staffc_CheckedChange"
                                                AutoPostBack="true" />
                                            <asp:CheckBoxList ID="cbl_staffc" runat="server" OnSelectedIndexChanged="cbl_staffc_SelectedIndexChange"
                                                AutoPostBack="true">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_staffc"
                                            PopupControlID="P3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Staff List
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updddlstaff" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlstafflst" runat="server" CssClass="textbox1 ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlstafflst_change" Width="250px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Machine ID
                            </td>
                            <td>
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>--%>
                                <asp:TextBox ID="txt_macid" runat="server" OnTextChanged="txt_macid_Change" AutoPostBack="true"
                                    MaxLength="50" CssClass="textbox textbox1 txtheight3"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetMacID" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_macid"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                            </td>
                            <td>
                                Finger ID
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlfingerid" runat="server" CssClass="textbox1 ddlheight3"
                                            Width="145px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btnmatch" runat="server" Text="Match" CssClass="textbox1 btn2" OnClick="btnmatch_click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="lblerr" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Larger" ForeColor="Red" Visible="false"></asp:Label>
                    <br />
                    <br />
                    <div id="sp_div" runat="server" visible="true" style="width: 800px; height: 350px;
                        border-radius: 10px; background-color: White;">
                        <FarPoint:FpSpread ID="FpSpread" runat="server" Visible="false" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px" Width="800px" Height="350px" Style="margin-left: 2px;"
                            class="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <br />
                    <asp:Button ID="btnsave" runat="server" Text="Save" Visible="false" CssClass="textbox1 btn2"
                        OnClick="btnsave_click" />
                    <asp:Button ID="btnexit" runat="server" Text="Exit" CssClass="textbox1 btn2" OnClick="btnexit_click" />
                    <br />
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="alertpopwindow" runat="server" class="popupstyle popupheight1" visible="false"
                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 90px;
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
                                                Text="Ok" runat="server" OnClick="btnerrclose_Click" />
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
