<%@ Page Title="" Language="C#" MasterPageFile="~/HostelMod/hostelsite.master" AutoEventWireup="true"
    CodeFile="StudentSearch.aspx.cs" Inherits="HostelMod_StudentSearch" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Student Search</span></div>
        </center>
    </div>
    <div>
        <center>
            <div>
                <img src="~/image/university1.jpg" align="middle" width="10px">
                <div class="maindivstyle" style="height: auto; width: 1000px;">
                    <table style="height: auto; margin-left: 0px; margin-top: 10px; margin-bottom: 10px;
                        padding: 6px;">
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_collegename" Text="College" runat="server" CssClass="txtheight"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_college" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 260px; height: 200px;">
                                                <asp:CheckBox ID="cb_clg" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_clg_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_clg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_clg_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txt_college"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_hostel" Text="Hostel Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_hostelname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 160px; height: 200px;">
                                                <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_hostelname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_hostelname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_hostelname"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblbuildname" Text="Building Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_buildingname" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p5" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_buildname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_buildname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_buildname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_buildname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_buildingname"
                                                PopupControlID="p5" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_floorname" Text="Floor Name" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_floorname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p6" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="200px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_floorname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_floorname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_floorname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_floorname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_floorname"
                                                PopupControlID="p6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_roomname" Text="Room Name" Width="86px" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_roomname" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="150px" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_roomname" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_roomname_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_roomname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblroomname_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_roomname"
                                                PopupControlID="p7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblnum" runat="server" Text="Roll No"></asp:Label>
                                    <asp:DropDownList ID="ddlrollno" runat="server" AutoPostBack="True" CssClass="textbox1 ddlheight1"
                                        OnSelectedIndexChanged="ddlrollno_SelectedIndexChanged">
                                        <asp:ListItem>Roll No</asp:ListItem>
                                        <asp:ListItem>Reg No</asp:ListItem>
                                        <asp:ListItem>Adm No</asp:ListItem>
                                        <asp:ListItem>App No</asp:ListItem>
                                        <asp:ListItem>Name</asp:ListItem>
                                        <asp:ListItem>Hostel Id</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtno" runat="server" CssClass="textbox textbox1" Width="150px"
                                        AutoPostBack="True"></asp:TextBox>
                                    <%--OnTextChanged="txtno_TextChanged"--%>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderroll" runat="server" TargetControlID="txtno"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    &nbsp&nbsp&nbsp
                                    <asp:Button ID="btn_go" Text="Go" CssClass=" textbox btn1" runat="server" OnClientClick="return valid2()"
                                        OnClick="btn_go_Click" Style="float: right;" />
                                </td>
                            </tr>
                        </table>
                    </table>
                </div>
                <br />
                <br />
                <table>
                    <tr>
                        <div id="divtable" runat="server">
                            <table class="tabl" style="width: 409px;">
                                <tr>
                                    <center>
                                        <FarPoint:FpSpread ID="Fpload1" OnCellClick="Cell_Click1" runat="server" BorderStyle="Solid"
                                            OnButtonCommand="Fpload_OnButtonCommand" OnPreRender="Fpspread_render" AutoPostBack="false"
                                            Visible="false" BorderWidth="0px" Style="overflow: auto; width: auto; border: 0px solid #999999;
                                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                            Pager-Mode="Both" Pager-Position="Bottom" Pager-PageCount="100" class="spreadborder">
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                </tr>
                            </table>
                        </div>
                    </tr>
                </table>
                <table>
                    <br />
                    <tr>
                        <div style="text-align: center;">
                            <asp:Label ID="lbprint" runat="server" Visible="false" Style="color: Red;"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="false" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Visible="false" onkeypress="return keyvalue(this)"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:Button ID="btn_excel" runat="server" Text="Export Excel" Visible="false" Font-Bold="true"
                                Font-Names="Book Antiqua" OnClick="btn_excel_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" Visible="false" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                    </tr>
                </table>
                <table class="tabl" style="top: -28px; left: 400px; position: absolute; width: 110px;
                    border-color: Gray; border-width: thin; height: 80px;">
                    <tr>
                        <td>
                            <asp:ImageButton ID="studImage" runat="server" Visible="false" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <asp:Label ID="lblStageCost" runat="server" Visible="false" Font-Size="Large" ForeColor="Green"></asp:Label>
                    </tr>
                </table>
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
        <center>
            <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: 471px; width: 617px;">
                    <asp:ImageButton ID="imgbtn_popclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -38px; margin-left: 300px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <table>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Label1" Text="Basic Details" runat="server" ForeColor="green" Font-Bold="true"
                                    Font-Size="X-Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_name1" Text="Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblnametext1" Text="" runat="server"></asp:Label>
                            </td>
                            <td rowspan="5">
                                <asp:Image ID="image3" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 130px;
                                    width: 100px;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_instut1" Text="Institutions" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblintitutext1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroll1" Text="Roll No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblrolltxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblreg1" Text="Reg No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblregtxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblfather1" Text="Father's Name &nbsp;&nbsp;&nbsp;" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblfathertxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcourse1" Text="Course" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcoursetxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblhostel1" Text="Hostel" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhosteltxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblphne" Text="Phone No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblphnetxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroom" Text="Room No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblroomtxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblfloor" Text="Floor" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblfloor1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblblock" Text="Block" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblblocktxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblgender1" Text="Gender" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblgendertxt" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbldob1" Text="DOB" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lbldobtxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblemail1" Text="Email" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblemailtxt1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbladdress" Text="Address" runat="server"></asp:Label>
                            </td>
                            <td colspan="2" style="max-width: 50px">
                                <asp:Label ID="lbladdress1" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow2" runat="server" visible="false" class="popupstyle popupheight"
                style="height: auto; width: auto;">
                <br />
                <div class="subdivstyle" style="background-color: White; height: auto; width: 900px;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -38px; margin-left: 436px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <table>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Lblviewdet" Text="View Details" runat="server" ForeColor="Red" Font-Bold="true"
                                    Font-Size="X-Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                    <table frame="box">
                        <tr style="background: #00ffdb80;">
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="lblinformation" Text="Student Information" runat="server" ForeColor="green"
                                    Font-Bold="true" Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lblstud2" Text="Student Name &nbsp;&nbsp;&nbsp;" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblstudtxt2" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroll2" Text="Roll No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblrolltxt2" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcourse2" Text="Course" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcoursetxt2" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblhostel2" Text="Hostel" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhosteltxt2" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblphne2" Text="Phone No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblphnetxt2" Text="" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="Fpuser" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Width="780px" Style="overflow: auto; border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                    class="spreadborder">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <br />
                        <br />
                    </table>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow3" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: auto; width: 792px;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -35px; margin-left: 383px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <table>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Label3" Text="Hostel Attendance" runat="server" ForeColor="Red" Font-Bold="true"
                                    Font-Size="X-Large"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 82px; right: 909px;">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtFromDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Height="20px" Width="86px" Style="" OnTextChanged="txtFromDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 76px">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtToDate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Width="86px" OnTextChanged="txtToDate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="dd/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp&nbsp&nbsp
                                <asp:Button ID="Button1" Text="Go" CssClass=" textbox btn1" BackColor="#ba00ff80"
                                    runat="server" OnClientClick="return valid2()" OnClick="btngo1_Click" Style="float: right;" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table frame="box">
                        <tr style="background: #00ffdb80;">
                            <td colspan="5" style="text-align: center">
                                <asp:Label ID="Lblinfo" Text="Student Information &nbsp;&nbsp;&nbsp;" Visible="false"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblstud3" Text="Student Name &nbsp;&nbsp;&nbsp;" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblstudtxt3" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroll3" Text="Roll No" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblrolltxt3" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcourse3" Text="Course" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcoursetxt3" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblhostel3" Text="Hostel" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhosteltxt3" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroom3" Text="Room No" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblroomtxt3" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div id="att" runat="server" visible="false">
                        <table frame="box" width="150px" align="center">
                            <tr style="background: #00ffdb80;">
                                <td align="center">
                                    <asp:Label ID="lblinfor" runat="server" Text="Attendance Details" Visible="true"
                                        Font-Bold="true" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%-- <p style="width: 691px;" align="center">--%>
                                    <asp:Label ID="lbl_count1" runat="server" Visible="true" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <%-- </p>--%>
                            <%--<p style="width: 691px;" align="center">--%>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_count2" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <%--</p>--%>
                            <%--<p style="width: 691px;" align="center">--%>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_count3" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <%-- </p>--%>
                            <%-- <p style="width: 691px;" align="center">--%>
                            <td align="center">
                                <asp:Label ID="lbl_count4" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                            </td>
                            </tr>
                            <%-- </p>--%>
                            <tr>
                                <%-- <p style="width: 691px;" align="center">--%>
                                <td align="center">
                                    <asp:Label ID="lbl_count5" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                                    <%-- </p>--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <br />
                    <br />
                    <center>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="400px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            class="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow4" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: auto; width: auto;">
                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -35px; margin-left: 486px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <table>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Label2" Text="Mess Attendance" runat="server" ForeColor="Red" Font-Bold="true"
                                    Font-Size="X-Large"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 82px; right: 909px;">
                                </asp:Label>
                            </td>
                            <td>
                            <asp:UpdatePanel ID="updatepanel7" runat="server">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtFromDate1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="20px" Width="86px" Style="" OnTextChanged="txtFromDate1_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtFromDate1" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                 </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                                <td>
                                <asp:Label ID="Label6" runat="server" Text="To Date" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Style="height: 21px; width: 76px">
                                </asp:Label></td>
                                <td>
                                <asp:UpdatePanel ID="updatepanel8" runat="server">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtToDate1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="86px" OnTextChanged="txtToDate1_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtToDate1" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                 </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp&nbsp&nbsp
                                <asp:Button ID="Button2" Text="Go" CssClass=" textbox btn1" BackColor="#ba00ff80"
                                    runat="server" OnClientClick="return valid2()" OnClick="btngo2_Click" Style="float: right;" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table frame="box">
                        <tr style="background: #00ffdb80;">
                            <td colspan="5" style="text-align: center">
                                <asp:Label ID="Label15" Text="Student Information &nbsp;&nbsp;&nbsp;" Visible="false"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblstud4" Text="Student Name &nbsp;&nbsp;&nbsp;" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblstudtxt4" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroll4" Text="Roll No" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblrolltxt4" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcourse4" Text="Course" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcoursetxt4" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblhostel4" Text="Hostel" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhosteltxt4" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <div id="messatt" runat="server" visible="false">
                        <table>
                            <tr style="background: #00ffdb80;">
                                <td colspan="5" style="text-align: center">
                                    <asp:Label ID="Label16" Text="Student Mess Attendance Getails &nbsp;&nbsp;&nbsp;"
                                        Visible="false" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <p style="width: 691px;" align="center">
                                    <asp:Label ID="Label17" runat="server" Visible="true" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </p>
                                <p style="width: 691px;" align="center">
                                    <asp:Label ID="Label18" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </p>
                                <p style="width: 691px;" align="center">
                                    <asp:Label ID="Label19" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </p>
                            </tr>
                        </table>
                    </div>
                    <table>
                        <tr>
                            <td>
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Width="980px" Style="overflow: auto; border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                                    class="spreadborder">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </td>
                        </tr>
                        <br />
                        <br />
                    </table>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow5" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: auto; width: 738px;">
                    <asp:ImageButton ID="ImageButton4" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -35px; margin-left: 352px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <table>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Label4" Text="In and Out" runat="server" ForeColor="Red" Font-Bold="true"
                                    Font-Size="X-Large"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="From Date" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 21px; width: 82px; right: 909px;">
                                </asp:Label>
                            </td>
                            <td>
                             <asp:UpdatePanel ID="updatepanel9" runat="server">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtFromDate2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="20px" Width="86px" Style="" OnTextChanged="txtFromDate2_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txtFromDate2" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                  </ContentTemplate>
                                </asp:UpdatePanel>
                                </td>
                                <td>
                                <asp:Label ID="Label8" runat="server" Text="To Date" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Style="height: 21px; width: 76px">
                                </asp:Label></td>
                                <td>
                                 <asp:UpdatePanel ID="updatepanel10" runat="server">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtToDate2" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="86px" OnTextChanged="txtToDate2_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txtToDate2" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                  </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                &nbsp&nbsp&nbsp
                                <asp:Button ID="Button3" Text="Go" CssClass=" textbox btn1" BackColor="#ba00ff80"
                                    runat="server" OnClientClick="return valid2()" OnClick="btngo3_Click" Style="float: right;" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table frame="box">
                        <tr style="background: #00ffdb80;">
                            <td colspan="5" style="text-align: center">
                                <asp:Label ID="Label20" Text="Student Information &nbsp;&nbsp;&nbsp;" Visible="false"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblstud5" Text="Student Name &nbsp;&nbsp;&nbsp;" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblstudtxt5" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroll5" Text="Roll No" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblrolltxt5" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblcourse5" Text="Course" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcoursetxt5" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblhost5" Text="Hostel" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblhosttxt5" Text="" Visible="false" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="Label10" runat="server" Visible="true" Font-Bold="true" ForeColor="Black"></asp:Label>
                            </p>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="Label11" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                            </p>
                            <p style="width: 691px;" align="right">
                                <asp:Label ID="Label12" runat="server" Visible="false" Font-Bold="true" ForeColor="Black"></asp:Label>
                            </p>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderStyle="Solid" BorderWidth="0px"
                        Width="100px" Style="height: 5000px; border: 0px solid #999999; border-radius: 10px;
                        background-color: White; box-shadow: 0px 0px 8px #999999;" class="spreadborder">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                </div>
            </div>
        </center>
        <center>
            <div id="popwindow6" runat="server" visible="false" class="popupstyle popupheight">
                <br />
                <div class="subdivstyle" style="background-color: White; height: 268px; width: 469px;">
                    <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -35px; margin-left: 218px;"
                        OnClick="imagebtnpopclose_Click" />
                    <br />
                    <table>
                        <tr>
                            <td colspan="3" style="text-align: center">
                                <asp:Label ID="Label9" Text="Confirm Room Transfer" runat="server" ForeColor="LawnGreen"
                                    Font-Bold="true" Font-Size="X-Large"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblhostel6" Text="Hostel Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upp1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="Cbo_HostelName" runat="server" AutoPostBack="True" Font-Bold="True"
                                            OnSelectedIndexChanged="Cbo_HostelName_SelectedIndexChanged" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="130px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblblock6" Text="Block Name" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlblock6" Style="" runat="server" CssClass="fontbold" Width="124px"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlblock_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Floor Name" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="cbofloorname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="130px" AutoPostBack="True" OnSelectedIndexChanged="cbofloorname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblroom6" Text=" Room No" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="Cbo_Room" runat="server" AutoPostBack="True" Font-Bold="True"
                                            OnSelectedIndexChanged="Cbo_Room_SelectedIndexChanged" Width="130px" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Date" Style="" CssClass="fontbold"></asp:Label>
                            </td>
                            <td>
                             <asp:UpdatePanel ID="updatepanel11" runat="server">
                                    <ContentTemplate>
                                <asp:TextBox ID="txtdate" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="20px" Width="86px" Style="" OnTextChanged="txtdate_TextChanged" AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txtdate" Format="dd/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                                  </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblreason" runat="server" Style="top: 10px; left: 6px;" Text="Reason"></asp:Label>
                                <asp:TextBox ID="txt_reason" runat="server" CssClass="txtcaps txtheight2">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Lbl" Text="" Visible="false" ForeColor="red" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btn_save1" Text="RoomTransfer" Width="91px" ForeColor="white" BackColor="red"
                                    runat="server" CssClass="textbox btn2" OnClick="btnpopsave_Click" OnClientClick="return valid1()" />
                                <%--<asp:Button ID="btn_update" Text="Update" OnClick="btnupdate_Click" CssClass="textbox btn2"
                                    OnClientClick="return gym()" runat="server" />
                                <asp:Button ID="btn_delete" Text="Delete" OnClick="btndelete_Click" CssClass="textbox btn2"
                                    OnClientClick="return gym()" runat="server" />--%>
                                <asp:Button ID="btn_exit1" Text="Exit" runat="server" ForeColor="white" BackColor="green"
                                    CssClass="textbox btn2" OnClick="btnpopexit_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </center>
        <center>
        </center>
    </div>
</asp:Content>
