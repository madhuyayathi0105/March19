<%@ Page Title="" Language="C#" MasterPageFile="~/Hostelmod/hostelsite.master" AutoEventWireup="true"
    CodeFile="Hosteladmissionprocess.aspx.cs" Inherits="Hosteladmissionprocess" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script src="Styles/Scripts/jquery-latest.min.js" type="text/javascript"></script>
    <style type="text/css">
        .rdbstyle input[type=radio]
        {
            display: none;
        }
        .rdbstyle input[type=radio] + label
        {
            display: inline-block;
            margin: -2px;
            padding: 4px 12px;
            margin-bottom: 0;
            font-size: 14px;
            line-height: 20px;
            color: #993399;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
            background-image: -o-linear-gradient(top,#fff,#e6e6e6);
            background-image: linear-gradient(to bottom,#fff,#e6e6e6);
            background-repeat: repeat-x;
            border: 1px solid #ccc;
            border-color: #e6e6e6 #e6e6e6 #bfbfbf;
            border-color: rgba(0,0,0,0.1) rgba(0,0,0,0.1) rgba(0,0,0,0.25);
            border-bottom-color: #b3b3b3;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff',endColorstr='#ffe6e6e6',GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            -webkit-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
        }
        .rdbstyle input[type=radio]:checked + label
        {
            background-image: none;
            outline: 0;
            -webkit-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            border-bottom-color: #b3b3b3;
            border-bottom-style: solid;
            border-bottom-color: #89D17C;
            border-bottom-width: medium;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
        .lnk:hover
        {
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            color: Green;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 1000px;">
            <br />
            <center>
                <asp:Label ID="lbl_header" Text="Hostel Admission Process" ForeColor="Green" CssClass="fontstyleheader"
                    runat="server"></asp:Label>
            </center>
            <center>
                <asp:RadioButtonList ID="rdbtype" runat="server" OnSelectedIndexChanged="rdbtype_SelectedIndexChanged"
                    AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="4" Style="margin-left: -641px;"
                    BorderColor="#999999" Font-Bold="True" CssClass="rdbstyle">
                    <asp:ListItem Value="0">Hostel Applied</asp:ListItem>
                    <asp:ListItem Value="1">Shortlist</asp:ListItem>
                    <asp:ListItem Value="2">Admitted</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Panel ID="Panel1" BorderColor="#993333" runat="server" BorderStyle="Solid" Style="height: 104px;
                    width: 935px;">
                    <table id="firsttable" runat="server">
                        <tr>
                            <td>
                                Institution Name
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddl_collegename" runat="server" Width="250px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddl_collegename_SelectedIndexchange" CssClass="ddlheight textbox1">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                Batch
                                <%-- </td>
                        <td>--%>
                                <asp:DropDownList ID="ddl_batch" Height="27px" runat="server" CssClass=" textbox1 txtheight"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%-- </td>
                        <td>--%>
                                Edu Level
                            </td>
                            <td>
                                <asp:DropDownList ID="ddledu" runat="server" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddledu_SelectedIndexchange"
                                    CssClass="ddlheight textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbl_degree" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel2" runat="server" Width="200px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_degree" runat="server" OnCheckedChanged="cb_degree_SelectedIndexChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="Panel2" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_department" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" Width="250px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_dept" runat="server" OnCheckedChanged="cb_dept_SelectedIndexChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Religion
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_religion" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" Width="250px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_religion" runat="server" OnCheckedChanged="cb_religion_SelectedIndexChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_religion" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_religion"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Community
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_comm" runat="server" ReadOnly="true" Height="20px" CssClass="textbox txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel5" runat="server" Width="250px" Height="170px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_comm" runat="server" OnCheckedChanged="cb_comm_SelectedIndexChanged"
                                                Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cbl_comm" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_comm_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_comm"
                                            PopupControlID="Panel5" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Order by
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_orderby" Height="27px" Width="130px" runat="server" CssClass=" textbox1 txtheight">
                                    <asp:ListItem Value="order by ISNULL (tt.priority2,0) asc">Religion</asp:ListItem>
                                    <asp:ListItem Value="order by isnull(ts.priority1,0) asc">Communitity</asp:ListItem>
                                    <asp:ListItem Value="order by securedmark desc">Mark</asp:ListItem>
                                    <asp:ListItem Value="order by noofattempts asc">Attempts</asp:ListItem>
                                    <asp:ListItem Value="order by cityp asc">State</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_colord" runat="server" Text="Report Type"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_colord" runat="server" CssClass="ddlheight4 textbox1">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btn_go" CssClass="btn1 textbox1" runat="server" Text="Go" Width="60px"
                                    OnClick="btn_go_Click" />
                            </td>
                            <td colspan="4" align="left">
                                <asp:LinkButton ID="linkapplication" CssClass="lnk" Visible="false" runat="server"
                                    Text="Admission From" OnClick="linkapplication_click"></asp:LinkButton>
                                <asp:LinkButton ID="linkundertaking" CssClass="lnk" Visible="false" runat="server"
                                    Text="Undertaking Form" OnClick="linkundertaking_click"></asp:LinkButton>
                            </td>
                            <td colspan="4" align="right">
                                <asp:ImageButton ID="imgbtn_columsetting" runat="server" Width="30px" Height="30px"
                                    Text="All" ImageUrl="~/Hostel Gete Images/images (1)ppp.jpg" OnClick="imgbtn_all_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <div>
                    <br />
                    <div>
                        <asp:Label ID="lbl_toapplied" Style="float: right; margin-right: 50px;" ForeColor="Red"
                            runat="server"></asp:Label>
                    </div>
                    <FarPoint:FpSpread ID="FpSpread3" runat="server" Visible="false" BorderStyle="NotSet"
                        BorderWidth="0px" OnButtonCommand="FpSpread3_command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" BackColor="White">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <%-- <asp:Button ID="btnprintpdf" CssClass="btnapprove1" runat="server" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Pdf" Style="border: 1px solid orange;"
                    Width="100px" Visible="false" OnClick="btnprintpdf_Click" />--%>
                    <asp:Button ID="btn_movetoshort" CssClass="btnapprove1" runat="server" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Move To Shortlist" Style="border: 1px solid orange;"
                        Width="150px" Visible="false" OnClick="btn_popadmissionfeemovetoshort_Click" />
                    <asp:Button ID="btn_movetoadmit" CssClass="btnapprove1" runat="server" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Move To Admit" Style="border: 1px solid orange;"
                        Width="135px" Visible="false" OnClick="btn_movetoadmit_Click" />
                    <asp:Button ID="btn_movetoreject" CssClass="btnapprove1" runat="server" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Reject" Style="border: 1px solid orange;"
                        Width="135px" Visible="false" OnClick="btn_movetoreject_Click" />
                </div>
            </center>
            <center>
                <div id="pop_roomselection" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <asp:ImageButton ID="imagebtn" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 132px; margin-left: 317px"
                            OnClick="imagebtnpop1close_Click" />
                        <br />
                        <div id="Div2" runat="server" class="table" style="background-color: White; height: auto;
                            width: 665px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 120px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <asp:Label ID="Label1" Text="Select the Room Details" ForeColor="Green" CssClass="fontstyleheader"
                                    runat="server"></asp:Label><br />
                                <br />
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td>
                                                Hostel Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_hostelname" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddl_hostelname_SelectedIndexchange" CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Building Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_building" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddl_building_SelectedIndexchange" CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Floor Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_floorname" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddl_floorname_SelectedIndexchange" CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                Room Name
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_roomname" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddl_roomname_SelectedIndexchange" CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn1" Text="?" runat="server" OnClick="btnroomdetails_Click" CssClass="textbox btn" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Room Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_roomtype" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddl_floorname_SelectedIndexchange" CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_studenttype" Text="Student Type" runat="server"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="Radiobtnstype" runat="server" Font-Names="Book Antiqua"
                                                    Style="margin-left: 0px;" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0">Veg</asp:ListItem>
                                                    <asp:ListItem Value="1">Non Veg</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td> 
                                                <br />
                                                <asp:CheckBox ID="cb_feesallot" runat="server" Text="Fees Allot" AutoPostBack="true"
                                                    OnCheckedChanged="cbfeeallot_click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="admissionformfee" runat="server">
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="fintable" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                Room Rent Header
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_rrh" Width="150px" Height="30px" AutoPostBack="true" runat="server"
                                                    CssClass="ddlheight textbox1" OnSelectedIndexChanged="ddl_roorentH_Selectedindex_Changed">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_rrl" runat="server" Text="Room Rent Ledger"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_rrl" Width="150px" Height="30px" runat="server" CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Room Rent Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_roomrent" TextMode="SingleLine" MaxLength='10' placeholder="Rs"
                                                    runat="server" Height="20px" CssClass="textbox textbox1" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_roomrent"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                Student Admission Fee
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_studentledger" Enabled="false" TextMode="SingleLine" MaxLength='10'
                                                    runat="server" Height="20px" CssClass="textbox textbox1" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_studentledger"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                    <center>
                                        <div>
                                            <br />
                                            <asp:Button ID="btn_save" CssClass="btn1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Save" Style="border: 1px solid orange;" Width="150px"
                                                Visible="false" OnClick="btn_Save_Click" />
                                            <br />
                                            <br />
                                        </div>
                                    </center>
                                </center>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="popwindow3" runat="server" class="popupstyle" visible="false" style="height: 50em;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0;">
                    <asp:ImageButton ID="imgbtn3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 394px;"
                        OnClick="imagebtnpop3close_Click" />
                    <br />
                    <br />
                    <div style="background-color: White; height: 550px; width: 820px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <div>
                                <span style="color: Green;" class="fontstyleheader">Room Details</span></div>
                            <br />
                        </center>
                        <center>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_pop3vaccant" Text="Vacant Type" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_pop3vaccant" runat="server" Width="125px" CssClass="textbox ddlheight2 textbox1">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Filled</asp:ListItem>
                                            <asp:ListItem>Un Filled</asp:ListItem>
                                            <asp:ListItem>Partially Filled</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblinclude" Text="Include:" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chck1" runat="server" Text="All" Font-Names="Book Antiqua" Font-Size="Medium"
                                            OnCheckedChanged="chck1_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBoxList ID="roomchecklist" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" Font-Names="Book Antiqua" OnSelectedIndexChanged="roomchecklist_SelectedIndexChanged"
                                            Font-Size="Medium">
                                            <asp:ListItem Value="0">Max.Student</asp:ListItem>
                                            <asp:ListItem Value="1">Avl.Student</asp:ListItem>
                                            <asp:ListItem Value="2">Room Cost</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_gopop3" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_gopop3_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <br />
                        <center>
                            <asp:Label ID="lblpop3err" runat="server" Style="color: Red;"></asp:Label></center>
                        <br />
                        <div id="div3" runat="server" style="width: 810px; height: 180px; overflow: auto">
                            <center>
                                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="NotSet"
                                    BorderWidth="0px" ActiveSheetViewIndex="0" Style="width: 810px; height: 180px;
                                    overflow: auto">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA" AutoPostBack="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread></center>
                        </div>
                        <br />
                        <center>
                            <table class="maintablestyle" runat="server" id="tblStatus" style="border-bottom-style: solid;
                                border-top-style: solid; border-left-style: solid; border-width: 0px;" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="toalrooms" runat="server" Text="Total No.of Rooms :" Font-Bold="True"
                                            Font-Names="Book Antiqua" Width="197px" Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="totalvaccants" runat="server" Text="Total No.of Vacant :" Font-Bold="True"
                                            Font-Names="Book Antiqua" Width="282px" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="gy" runat="server" Width="20px" Height="20px" BackColor="GreenYellow"></asp:Label>
                                        <asp:Label ID="fill" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="109px"></asp:Label>
                                        <asp:Label ID="cor" runat="server" Width="20px" Height="20px" BackColor="Coral"></asp:Label>
                                        <asp:Label ID="partialfill" runat="server" Text="Partially Filled" Font-Bold="True"
                                            Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="mis" runat="server" Width="20px" Height="20px" BackColor="MistyRose"></asp:Label>
                                        <asp:Label ID="unfill" runat="server" Text="Unfilled" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="145px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </div>
            </center>
            <center>
                <div id="applicationfees_div" runat="server" visible="false" style="height: 100%;
                    z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0; left: 0px;">
                    <center>
                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 132px; margin-left: 213px"
                            OnClick="imagebtnpop1close1_Click" />
                        <br />
                        <div id="Div4" runat="server" class="table" style="background-color: White; height: auto;
                            width: 450px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 120px;
                            border-radius: 10px;">
                            <center>
                                <br />
                                <asp:Label ID="Label2" Text="Hostel Admission Form Fee" ForeColor="Green" CssClass="fontstyleheader"
                                    runat="server"></asp:Label><br />
                                <br />
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td>
                                                Hostel Admission Header
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_hosteladmissionH" Width="150px" Height="30px" AutoPostBack="true"
                                                     runat="server" CssClass="ddlheight textbox1" OnSelectedIndexChanged="ddl_hosteladmissionH_Selectedindex_Changed">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Hostel Admission Ledger"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_hosteladmissionL" Width="150px" Height="30px" runat="server"
                                                    CssClass="ddlheight textbox1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Hostel Admission Amount
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_hosteladmission" TextMode="SingleLine" MaxLength='10' placeholder="Rs"
                                                    runat="server" Height="20px" CssClass="textbox textbox1" Width="100px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_hosteladmission"
                                                    FilterType="numbers,custom" ValidChars=".">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button ID="btn_shortlist" CssClass="btn1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Save" Style="border: 1px solid orange;" Width="150px"
                                                    OnClick="btn_movetoshort_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
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
</asp:Content>
