<%@ Page Title="" Language="C#" MasterPageFile="~/HRMOD/HRSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="HR_Year_Alter.aspx.cs" Inherits="HR_Year_Alter" %>

<%@ Register Src="~/UserControls/PrintMaster.ascx" TagName="printmaster" TagPrefix="InsproPlus" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">HR Year</span>
            </div>
        </center>
        <center>
            <div class="maindivstyle" style="width: 1000px; height: 600px;">
                <br />
                <center>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <%--  <div align="left"  style="width: 220px; height: 35px; border-radius: 10px; border: 1px solid Gray;" >
                                    --%>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rb_leave" Visible="false" runat="server" Text="Leave" Width="100px"
                                                    GroupName="same" AutoPostBack="true" OnCheckedChanged="rb_leave_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rb_Payprocess" Visible="false" runat="server" Text="Pay Process"
                                                    Width="100px" GroupName="same" AutoPostBack="true" OnCheckedChanged="rb_Payprocess_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                    <%--   </div>
                                    --%>
                                </td>
                                <td>
                                    <table class="maintablestyle" width="450px">
                                        <tr>
                                            <td>
                                            
                                                <asp:Label ID="lblcol" runat="server" Font-Bold="true" Style="font-family: 'Book Antiqua'"
                                                    Text="College Name"></asp:Label>
                                            </td>
                                            <td>
                                             <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                                                <asp:DropDownList ID="ddlcol" runat="server" Width="242px" CssClass="textbox ddlheight4"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlcol_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                             <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                                            
                                                <asp:Button ID="btn_go" runat="server" Visible="false" CssClass="textbox textbox1 btn1"
                                                    Text="Go" OnClick="btn_go_Click" />
                                                    </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                   
                                            </td>

                                            <td>
                                             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                                <asp:Button ID="btn_addnew" runat="server" CssClass="textbox textbox1 btn2" Text="Add New"
                                                    OnClick="btn_addnew_Click" />
                                                     </ContentTemplate>
                                                    </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <asp:Label ID="lblerr" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                        Font-Size="Medium"></asp:Label>
                         <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                    <center>
                        <div id="Divspread" runat="server" visible="false" style="width: 850px; height: 350px;
                            overflow: auto; border: 1px solid Gray; background-color: White;">
                            <br />
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" ShowHeaderSelection="false"
                                OnButtonCommand="FpSpread1_ButtonCommand" CssClass="spreadborder" ActiveSheetViewIndex="0"
                                OnPreRender="FpSpread1_SelectedIndexChanged">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </div>
                    </center>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    
                    <center>
                     <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                        <div id="btndiv" runat="server">
                            <asp:Button ID="btnSelect" runat="server" Text="Select HR Year" OnClick="btnSelect_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn2"
                                Width="140px" />
                            <asp:Button ID="btnmod" runat="server" Text="Modify" OnClick="btnmod_Click" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn2" />
                        </div>
                           </ContentTemplate>
                    </asp:UpdatePanel>
                    </center>
                 
                    <br />
                    <div id="rptprint" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblvalidation1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtexcelname" runat="server" MaxLength="15" CssClass="textbox textbox1 txtheight4"
                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="filt_extenderexcel" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel_Click" Font-Size="Medium" CssClass="textbox textbox1 btn2"
                                        Width="140px" Text="Export To Excel" />
                                </td>
                                <td>
                                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn2"
                                        Width="100px" />
                                </td>
                                <td>
                                    <InsproPlus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                </td>
                                <%--<td>
                                <asp:Button ID="btnmod" runat="server" Text="Modify" OnClick="btnmod_Click" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" CssClass="textbox textbox1 btn4" />
                            </td>--%>
                            </tr>
                        </table>
                    </div>
                </center>
                <center>
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                    <div id="popper1" runat="server" visible="false" style="height: 70em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .40); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../images/close.png" Style="height: 30px;
                            width: 30px; position: absolute; margin-top: 12px; margin-left: 230px;" OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <center>
                            <div style="height: 400px; width: 500px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA;
                                border-radius: 10px; background-color: White;">
                                <br />
                                <div>
                                    <center>
                                        <span style="color: Green; font-size: large;">HR Year Creation</span>
                                    </center>
                                </div>
                                <br />
                                <div>
                                    <center>
                                        <table cellpadding="5">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rb_radleave" Visible="false" runat="server" Text="Leave" Width="100px"
                                                        GroupName="radsame" AutoPostBack="true" OnCheckedChanged="rb_radleave_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rb_radpaypro" Visible="false" runat="server" Text="Pay Process"
                                                        Width="100px" GroupName="radsame" AutoPostBack="true" OnCheckedChanged="rb_radpaypro_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblCollege" runat="server" Text="College Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlcoll" runat="server" CssClass="textbox ddlheight3" Width="250px"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblyearstart" runat="server" Text="HR Year Start"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtdatestart" runat="server" Width="100px" Style="top: 10px; left: 304px;"
                                                        AutoPostBack="true" OnTextChanged="txtdatestart_Change" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="caldatestart" TargetControlID="txtdatestart" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                    <span style="color: Red;">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblyearend" runat="server" Text="HR Year End"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtdateend" runat="server" Width="100px" Style="top: 10px; left: 304px;"
                                                        AutoPostBack="true" OnTextChanged="txtdateend_Change" onfocus="return myFunction(this)"
                                                        onclick="return displaydateerr()" CssClass="textbox textbox1"></asp:TextBox>
                                                    <asp:CalendarExtender ID="caldateend" TargetControlID="txtdateend" runat="server"
                                                        CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                    </asp:CalendarExtender>
                                                    <span style="color: Red;">*</span> <span>
                                                        <asp:Label ID="lbldateerr" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rb_monthfrm" runat="server" Text="Month By From Date" GroupName="same"
                                                        AutoPostBack="true" OnCheckedChanged="rb_monthfrm_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rb_monthto" runat="server" Text="Month By To Date" GroupName="same"
                                                        AutoPostBack="true" OnCheckedChanged="rb_monthto_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                         <asp:UpdatePanel ID="Upsave" runat="server">
            <ContentTemplate>
                                        <center>
                                            <asp:Button ID="btnupdate" runat="server" Text="Update" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" CssClass="textbox btn2 textbox1" OnClick="btnupdate_Click"
                                                OnClientClick="return valid()" Visible="false" />
                                            <asp:Button ID="btndelete" runat="server" Text="Delete" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Font-Bold="true" CssClass="textbox btn2 textbox1" OnClick="btndelete_Click"
                                                Visible="false" />
                                            <asp:Button ID="btnsave" runat="server" Text="Save" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" OnClick="btnsave_Click" Visible="false" OnClientClick="return valid()"
                                                CssClass="textbox btn2 textbox1" />
                                            <asp:Button ID="btnexit" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Font-Bold="true" Text="Exit" CssClass="textbox btn2 textbox1" OnClick="btnexit_Click" />
                                        </center>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </center>
                                </div>
                            </div>
                        </center>
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </center>
                 <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
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
                                                <asp:Button ID="btnerrclose" CssClass=" textbox textbox1 btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
                <center>
                  <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
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
                                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                                                    <asp:Button ID="btnyes" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnyes_Click" Text="Yes" runat="server" />
                                                    <asp:Button ID="btnno" CssClass=" textbox textbox1 btn2 comm" Style="height: 28px;
                                                        width: 65px;" OnClick="btnno_Click" Text="No" runat="server" />
                                                        </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </center>

                
            <%--Progress bar for go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Upsave">
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
    <%--Progress bar for UpGenerate--%>

            </div>
        </center>
    </body>
    </html>
</asp:Content>
