<%@ Page Title="" Language="C#" MasterPageFile="~/Requestmod/RequestSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Request.aspx.cs" Inherits="Request" EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .maindivstylesize
        {
            height: 3000px;
            width: 1200px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 100%;
        }
        .col1
        {
            float: left;
            width: 50%;
        }
        .col2
        {
            float: right;
            width: 50%;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .div1
        {
            height: auto;
            border: 1px solid;
            overflow: auto;
        }
        .fontstyleheaderrr
        {
            font-family: Book Antiqua;
            font-size: larger;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:UpdatePanel ID="upall" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    function DisplayLoadingDiv() {
                        document.getElementById("<%=divImageLoading.ClientID %>").style.display = "block";
                    }
                    function HideLoadingDiv() {
                        document.getElementById("<%=divImageLoading.ClientID %>").style.display = "none";
                    }

                    function Redirect1_Click(objRef, colIndex) {
                        //Get the Row based on checkbox
                        var row = objRef.parentNode.parentNode;
                        var rowIndex = row.rowIndex - 1;
                        document.getElementById("<%=hid.ClientID %>").value = rowIndex;
                        var ms = document.getElementById("<%=hid.ClientID %>").value;
                    }

                    function QuantityChange(objRef, colIndex) {

                        var row = objRef.parentNode.parentNode;

                        var rowIndex = row.rowIndex - 1;

                        if (rowIndex.toString() != "") {
                            var Avaqty = document.getElementById('MainContent_SelectdptGrid_lbl_Available_' + rowIndex.toString());
                            var txtQty = document.getElementById('MainContent_SelectdptGrid_txt_quantity_' + rowIndex.toString());

                            var QtyVal = 0.0;
                            var AvaVal = 0.0;

                            if (Avaqty.innerHTML != "" && txtQty.value.trim() != "") {

                                AvaVal = parseFloat(Avaqty.innerHTML);
                                QtyVal = parseFloat(txtQty.value);
                                if (AvaVal < QtyVal) {

                                    txtQty.value = (QtyVal - AvaVal).toString();
                                }
                                else if (AvaVal > QtyVal) {

                                    txtQty.value = 0;
                                }

                            }


                        }

                    }
                </script>
                <div>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <div>
                        <center>
                            <div>
                                <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Request</span></div>
                        </center>
                    <%--    magesh--%>
                    <center>

                                    <div id="Div5" runat="server" visible="false" style="height: 0px; z-index: 1000;
                    width: 0px; background-color: rgba(54, 25, 25, .2); position: absolute; top: 5px;
                    left: 0px;">
                     <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" visible="true" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: 207px; margin-left: 981px;"
                                OnClick="btn_popclose_Click" />

                                 
                                
                                        <div id="div_gatestudview" runat="server" class="table" style="background-color: White; height: 1000px;
                            width: 1000px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 211px;
                            border-radius: 10px;">
                                            <div id="divreq" style="margin-left: -224px" runat="server" visible="true">
                                                <br />
                                                <center>
                                                    <table>
                                                     <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                <asp:TextBox ID="TextBox2" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>  
                                                </td>
                                               <td width="50px">
                                                    <asp:Label ID="Label20" Width="70px" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                 <td>
                                                    <asp:TextBox ID="TextBox4" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            
                                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="TextBox4"
                                                                runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        
                                                </td>
                                            </tr>
                                                        <tr>
                                                            <td>
                                                                Roll No
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_rollreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Student Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_namereq" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Batch Year
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gatebatch" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Degree
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_degreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Department
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_deptreq" runat="server" Enabled="false" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Semester
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_semreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Section
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_secreq" runat="server" Enabled="false" CssClass="textbox textbox1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Apply Date
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_appldatereq" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                                <asp:CalendarExtender ID="calapplreqdt" runat="server" TargetControlID="txt_appldatereq"
                                                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr id="hos2" runat="server">
                                                            <td>
                                                                Hostel Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gatehostel" runat="server" CssClass="textbox textbox1 txtheight5"
                                                                    Width="100px" Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <%--<tr id="hos3" runat="server">
                                                            <td>
                                                                Building Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gatebuli" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>--%>
                                                        <tr id="hos4" runat="server">
                                                            <td>
                                                                Floor Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_gateflr" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                            </tr>
                                                            <tr id="hos5" runat="server">
                                                                <td>
                                                                    Room Name
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_gatermname" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                           <%-- <tr>
                                                                <td>
                                                                    Room Type
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txt_gateroom" runat="server" CssClass="textbox textbox1" Width="100px"
                                                                        Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>--%>
                                                    </table>
                                                    <br />
<br />
<br />
                                                    <br />
                                                     <center>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label29" runat="server" Text="Reason for Gate Pass"></asp:Label>
                                                                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="ddlheight5 textbox textbox1"
                                                                    onchange="reason(this)" onfocus="return myFunction(this)">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TextBox3" runat="server" Style="display: none; float: right"
                                                                    onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </center>
                                                <br />
                                                <div style="float: left; width: 550px; margin-left: 260px">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label30" runat="server" Text="Apply Date"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TextBox6" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    AutoPostBack="true"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txtapply">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender14" TargetControlID="txtapply" Format="dd/MM/yyyy"
                                                                    runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label31" runat="server" Text="Request By"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="DropDownList2" runat="server" CssClass="ddlheight4 textbox textbox1"
                                                                    onchange="req_by(this)" onfocus="return myFunction(this)">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="TextBox8" runat="server" CssClass="textbox textbox1 txtheight5"
                                                                    Style="display: none; float: right;" onfocus="return myFunction(this)"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </div>
                                                    <br />
                                                    <br />
                                                      <div style="float: left; margin-left: 535px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label21" runat="server" Text="Expected Date From"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtfromdate1" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                    AutoPostBack="true"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txtfromdate">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender12" TargetControlID="txtfromdate" Format="dd/MM/yyyy"
                                                                    runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label27" runat="server" Text="Expected Time Exit"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlhour1" Width="50px" runat="server" CssClass="ddlheight textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlmin1" Width="50px" runat="server" CssClass="ddlheight textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlsession1" runat="server" Width="50px" CssClass="ddlheight textbox textbox1">
                                                                    <asp:ListItem>AM</asp:ListItem>
                                                                    <asp:ListItem>PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="exp_date_to" runat="server" Text="Expected Date To"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txttodate1" runat="server" AutoPostBack="true" CssClass="textbox textbox1 txtheight1"> </asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" FilterType="Custom,Numbers"
                                                                    ValidChars="/" runat="server" TargetControlID="txttodate">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:CalendarExtender ID="CalendarExtender13" TargetControlID="txttodate" Format="dd/MM/yyyy"
                                                                    runat="server" Enabled="True">
                                                                </asp:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="TextBox7" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="" Style="opacity: 0; height: 0; width: 0;"></asp:TextBox>
                                                                <asp:Label ID="Label28" runat="server" Text="Expected Time Entry"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlendhour1" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlendmin1" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlenssession1" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                                    <asp:ListItem>AM</asp:ListItem>
                                                                    <asp:ListItem>PM</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                </center>
                                            </div>
                                            <div id="divright" runat="server" visible="true" style="margin-left: 512px; margin-top: -410px">
                                                <div style="background-color: White; height: 400px; width: 306px;">
                                                    <div style="height: 400px; overflow: auto;">
                                                        <asp:GridView ID="grdshow" runat="server" Visible="false" AutoGenerateColumns="false"
                                                            GridLines="Both">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Month" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lbl_month" runat="server" Text='<%#Eval("month") %>'></asp:Label>
                                                                            <%-- <asp:Label ID="lbl_monno" runat="server" Text='<%#Eval("monthno") %>'></asp:Label>--%>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allowed Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="lblleave" runat="server" Text='<%#Eval("allleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Granted Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="grantleave" runat="server" Text='<%#Eval("grantleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remaining Permission" HeaderStyle-BackColor="#0CA6CA"
                                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                                                    <ItemTemplate>
                                                                        <center>
                                                                            <asp:Label ID="balleave" runat="server" Text='<%#Eval("balleave") %>'></asp:Label>
                                                                        </center>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="margin-left: -195px; margin-top: -200px">
                                                <asp:Image ID="image8" runat="server" ToolTip="Student's Photo" ImageUrl="" Style="height: 110px;
                                                    width: 130px;" />
                                            </div>
                                        </div>
                                     
                                     
                                    </div>
                                </center>
                    </div>
                    <center>
                        <div class="maindivstyle maindivstylesize">
                            <br />
                            <center>
                                <table class="table" width="960px">
                                    <tr>
                                        <td id="td_item" runat="server" align="center" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_item" runat="server" Width="50px" Height="50px" ImageUrl="~/request_img/index.jpg"
                                                OnClick="imgbtn_item_Click" />
                                            <br />
                                            <asp:Label ID="lbl_itemreq" runat="server" Style="top: 10px; left: 6px;" Text="Item Request"></asp:Label>
                                        </td>
                                        <td id="td_sev" align="center" runat="server" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_service" runat="server" Width="50px" Height="50px" OnClick="imgbtn_service_Click"
                                                ImageUrl="~/request_img/images.jpg" />
                                            <br />
                                            <asp:Label ID="lbl_sevice" runat="server" Style="top: 10px; left: 6px;" Text="Service"></asp:Label>
                                        </td>
                                        <td id="td_vist" align="center" runat="server" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_visitor" runat="server" Width="50px" Height="50px" OnClick="imgbtn_visitor_Click"
                                                ImageUrl="~/request_img/visit.jpg" />
                                            <br />
                                            <asp:Label ID="lbl_visitor" runat="server" Style="top: 10px; left: 6px;" Text="Visitor Appointment"></asp:Label>
                                        </td>
                                        <td id="td_comp" align="center" runat="server" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_comp" runat="server" Width="50px" Height="50px" OnClick="imgbtn_comp_Click"
                                                ImageUrl="~/request_img/compl.jpg" />
                                            <br />
                                            <asp:Label ID="lbl_comp" runat="server" Style="top: 10px; left: 6px;" Text="Complaints "></asp:Label>
                                        </td>
                                        <td id="td_lv" align="center" runat="server" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_leave" runat="server" Width="50px" Height="50px" OnClick="imgbtn_leave_Click"
                                                ImageUrl="~/request_img/leave.png" />
                                            <br />
                                            <asp:Label ID="lbl_lv" runat="server" Style="top: 10px; left: 6px;" Text="Leave Request "></asp:Label>
                                        </td>
                                        <td id="td_othr" align="center" runat="server" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imbtn_othgr" runat="server" Width="50px" Height="50px" OnClick="imbtn_othgr_Click"
                                                ImageUrl="~/request_img/gate.jpg" />
                                            <br />
                                            <asp:Label ID="img_othr" runat="server" Style="top: 10px; left: 6px;" Text="GatePass Request"></asp:Label>
                                        </td>
                                        <td id="td_event" align="center" runat="server" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_event" runat="server" Width="50px" Height="50px" OnClick="imgbtn_event_Click"
                                                ImageUrl="~/request_img/mic-podium-02.jpg" />
                                            <br />
                                            <asp:Label ID="lbl_event" runat="server" Style="top: 10px; left: 6px;" Text="Event Request"></asp:Label>
                                        </td>
                                          <td id="td_library" runat="server" align="center" style="width: 200px; font-size: larger;">
                                            <asp:ImageButton ID="imgbtn_library" runat="server" Width="50px" Height="50px"  OnClick="imgbtn_library_Click" ImageUrl="~/request_img/Library.jpg"  />
                                            <br />
                                            <asp:Label ID="lbl_library" runat="server" Style="top: 10px; left: 6px;" Text="Book Request"></asp:Label>
                                        </td>
                                        <%--<td>  <asp:RadioButton ID="RadioButton3" runat="server" Text="Suggestion" GroupName="s1" /></td>--%>
                                    </tr>
                                </table>
                            </center>
                            <br />
                            <asp:Label ID="lbl_headername" runat="server" Visible="false" Style="color: #008000;
                                font-size: x-large"></asp:Label>
                            <br />
                            <div id="div_gate_reqstn" runat="server" visible="false">
                                <br />
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 500px;
                                    height: 40px; margin-left: -444px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_gate_collegename" runat="server" Text="College"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_gat_collegename" runat="server" CssClass="ddlheight5 textbox textbox1"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddl_gat_collegename_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdo_student" runat="server" GroupName="ee" Text="Student" Checked="true"
                                                    OnCheckedChanged="rdo_student_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdo_staff" runat="server" GroupName="ee" Text="Staff" OnCheckedChanged="rdo_staff_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                    height: 40px; margin-left: 540px; margin-top: -44px">
                                    <table>
                                        <tr>
                                            <td>
                                                Requisition No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_reqtn_gate" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                </asp:TextBox>
                                            </td>
                                            <td width="50px">
                                                <asp:Label ID="lbl_reqtn_gate" Width="70px" Text="Req Date" runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_reqtn_gate_date" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_reqtn_gate_date"
                                                            runat="server" CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <center>
                                <div id="div_itmreqst" runat="server" visible="false">
                                    <%--  <span style="color:#008000; font-size:x-large">Item Request</span>
                <br />
                <br />--%>
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td width="105px">
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_reqno" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_date" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="Updp_date" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_date" ReadOnly="false" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_date" runat="server"
                                                                Format="dd/MM/yyyy">
                                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table>
                                        <tr>
                                            <td width="105px">
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_own" runat="server" Text="Own Department" OnCheckedChanged="cb_own_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBox ID="cb_other" runat="server" Text="Other Department" OnCheckedChanged="cb_other_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Department
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_dept" TextMode="SingleLine" ReadOnly="true" onfocus="return myFunction(this)"
                                                    runat="server" Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                <asp:Button ID="btn_dept" runat="server" Text="?" CssClass="newtextbox btn" OnClick="btn_dept_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_reqremarks" runat="server" TextMode="MultiLine" CssClass="newtextbox textbox2"
                                                    Height="20px" Width="300px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_reqremarks" runat="server" TargetControlID="txt_reqremarks"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_() ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Expected Date
                                            </td>
                                            <td width="100px">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_exdate" runat="server" CssClass="newtextbox txtheight textbox2"
                                                            AutoPostBack="true" OnTextChanged="txt_exdate_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_exdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_additem" runat="server" CssClass="newtextbox btn2" Text="Add Item"
                                                    OnClick="btn_additem_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="griddiv" runat="server" style="width: 850px; height: 300px; margin-left: 21px;"
                                        class="spreadborder">
                                        <asp:GridView ID="SelectdptGrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnRowDataBound="typegrid_OnRowDataBound"
                                            OnRowCommand="SelectdptGrid_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cb_select" runat="server" Checked="true"  />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Measure">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_itemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Available Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Available" runat="server" Text='<%# Eval("Available") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_quantity" runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>'
                                                            Width="80px" CssClass="newtextbox" ></asp:TextBox>
                                                           <%-- onchange="QuantityChange(this)"--%>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_quantity"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </center>

                              
                                        <center>
                                            <span id="sp_appstaff_Item"  class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                Approval Permission Staff</span></center>
                                        <asp:GridView ID="grid_Item_approvalstaff" runat="server" Visible="false" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_grid_Item_approvalstaff">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno1" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_code1" runat="server" Text='<%#Eval("StaffCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname1" runat="server" Text='<%#Eval("StaffName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldept1" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldegn1" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstage1" runat="server" Text='<%#Eval("Stage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                   
                            <%--***********end of div_item*******--%>
                            <%--    *****************************Start Div_Library****************************--%>
                         <center>
                                <div id="div_lib" runat="server" visible="false">
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td width="105px">
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Text_lib_ReqNo" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label32" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="Text_lib_date" ReadOnly="false" runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender15" TargetControlID="Text_lib_date" runat="server"
                                                                Format="dd/MM/yyyy">
                                                                <%--CssClass="cal_Theme1 ajax__calendar_active"--%>
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                        <table>
                                       
                                        <tr>
                                            <td width="105px">
                                                Library Name
                                            </td>
                                            <td colspan="3">
                                               <asp:DropDownList ID="ddl_libname" Visible="true" runat="server" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                </asp:DropDownList>
                                             
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_reqlibremarks" runat="server" TextMode="MultiLine" CssClass="newtextbox textbox2"
                                                    Height="20px" Width="300px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" TargetControlID="txt_reqlibremarks"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_() ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="105px">
                                                Expected Date
                                            </td>
                                            <td width="100px">
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_libexdate" runat="server" CssClass="newtextbox txtheight textbox2"
                                                            AutoPostBack="true" OnTextChanged="txt_libexdate_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender16" TargetControlID="txt_libexdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_Addlib" runat="server" CssClass="newtextbox btn2" Text="Add Book"
                                                    OnClick="btn_Addlib_Click" />
                                            </td>
                                        </tr>
                                    </table>

                                        <div id="Divlibgrid" runat="server" style="width: 850px; height: 300px; margin-left: 21px;"
                                        class="spreadborder">
                                        <asp:GridView ID="SelectlibGrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno1" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbl_select" runat="server" Checked="true"  />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Title">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Author">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAuthor" runat="server" Text='<%# Eval("Author") %>'></asp:Label>
                                                        <asp:Label ID="lblbookid" runat="server" Visible="false" Text='<%# Eval("bookid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                              
                                                 
                                                <asp:TemplateField HeaderText="No.Of Copy">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_copy" runat="server" Style="text-align: center;" Text='<%# Eval("copy") %>'
                                                            Width="80px" CssClass="newtextbox"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_copy"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    </div>
                                    </center>

                                          <center>
                                            <span id="sp_appstaff_Book"  class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                Approval Permission Staff</span></center>
                                        <asp:GridView ID="GridView_BookApprovalstaff" runat="server" Visible="false" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_grid_Book_approvalstaff">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno2" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_staffcode1" runat="server" Text='<%#Eval("StaffCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffname1" runat="server" Text='<%#Eval("StaffName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffdept1" runat="server" Text='<%#Eval("Department") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffdegn1" runat="server" Text='<%#Eval("Designation") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstaffstage1" runat="server" Text='<%#Eval("Stage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>


                      <%--  ********************************End Of the Div_Library******************************--%>



                            <center>
                                <div id="div_service" runat="server" visible="false">
                                    <%-- <span style="color:#008000; font-size:x-large">Service</span>
                  <br />
                  <br />--%>
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_serreqno" runat="server" ReadOnly="true" CssClass="newtextbox  textbox1 txtheight">
                                                    </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_serreqdate" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_serreqdate" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_serreqdate" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="cext_serreqdate" TargetControlID="txt_serreqdate" runat="server"
                                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_serown" runat="server" Text="Own Department" OnCheckedChanged="cb_serown_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <asp:CheckBox ID="cb_serother" runat="server" Text="Other Department" OnCheckedChanged="cb_serother_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Department
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_serdept" TextMode="SingleLine" ReadOnly="true" onfocus="return myFunction(this)"
                                                    runat="server" Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                <asp:Button ID="btn_dept1" runat="server" Text="?" CssClass="newtextbox btn" OnClick="btn_dept1_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Remarks
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_serremarks" runat="server" TextMode="MultiLine" CssClass="newtextbox textbox2"
                                                    Height="20px" Width="300px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_serremarks" runat="server" TargetControlID="txt_serremarks"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_() ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--Suggested vendor--%>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_sersugvendor" runat="server" Visible="false" CssClass="newtextbox  textbox1 txtheight5">
                                                </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_sersugvendor" runat="server" TargetControlID="txt_sersugvendor"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-@&">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_sersugvendor" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetVendorDet" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sersugvendor"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="panelbackground">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--Suggested Service Location--%>
                                            </td>
                                            <td colspan="3">
                                                <asp:RadioButton ID="rb_indoor" Visible="false" runat="server" Text="InDoor" GroupName="s2"
                                                    Checked="true" AutoPostBack="true" />
                                                <asp:RadioButton ID="rb_outdoor" Visible="false" runat="server" Text="OutDoor" GroupName="s2"
                                                    AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Expected Date
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="upp_serexpdate" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txt_serexpdate" runat="server" CssClass="newtextbox txtheight textbox2"
                                                            AutoPostBack="true" OnTextChanged="txt_serexpdate_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="cext_serexpdate" TargetControlID="txt_serexpdate" runat="server"
                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                        </asp:CalendarExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_seradditem" runat="server" CssClass="newtextbox btn2" Text="Add Item"
                                                    OnClick="btn_seradditem_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="sergriddiv" runat="server" style="width: 850px; height: 300px; margin-left: 21px;"
                                        class="spreadborder">
                                        <asp:GridView ID="Sergrid" runat="server" AutoGenerateColumns="false" Width="800px"
                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White" OnRowDataBound="sertypegrid_OnRowDataBound"
                                            OnRowCommand="serSelectdptGrid_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cb_select" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="gridddl_loc" runat="server" AutoPostBack="false">
                                                            <asp:ListItem Text="Indoor" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Outdoor" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SuggestVendor">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="gridddl_sugvendor" runat="server" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_seritemcode" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_seritemname" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Measure">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_seritemmeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_serquantity" runat="server" Style="text-align: center;" Text='<%# Eval("Quantity") %>'
                                                            Width="80px" CssClass="newtextbox"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="Ftext_serqty" runat="server" TargetControlID="txt_serquantity"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </center>
                            <%-- *******end of service*****--%>
                            <%--****************end of event*****************--%>
                            <center>
                                <div id="Newdiv" runat="server" visible="false" style="height: 50em; z-index: 100000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 55px; margin-left: 338px;"
                                        OnClick="imagebtnpopclose1_Click" />
                                    <br />
                                    <br />
                                    <br />
                                    <center>
                                        <div style="background-color: White; height: 500px; width: 700px; border: 5px solid #0CA6CA;
                                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                            <br />
                                            <br />
                                            <center>
                                                <span style="font-size: large; color: Green;">Department Name</span>
                                            </center>
                                            <br />
                                            <div style="overflow: auto; width: 620px; height: 312px; border: 1px solid Gray;">
                                                <asp:GridView ID="dptgrid" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                                    HeaderStyle-ForeColor="White">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblsno" runat="server" Text="<%#
        Container.DisplayIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbcheck" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DeptCode">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldeptcode" runat="server" Text='<%# Eval("DeptCode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DeptName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldeptname" runat="server" Text='<%# Eval("DeptName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="300px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <br />
                                            <asp:CheckBox ID="cbselectall" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cbselectAll_Change" Style="margin-left: -156px; position: absolute;" />
                                            <asp:Button ID="btndeptsave" runat="server" Text="Add" CssClass="textbox btn2" OnClick="btndept_save" />
                                            <asp:Button ID="btndeptexit" runat="server" Text="Exit" CssClass="textbox btn2" OnClick="btndept_exit" />
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <%--*********popup for dept*********--%>
                            <center>
                                <div id="div_visitor" runat="server" visible="false">
                                    <br />
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_visitorreqno" runat="server" ReadOnly="true" CssClass="newtextbox textbox1 txtheight"> </asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_visitorreqdate" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_visitorreqdate" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_visitorreqdate" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="cext_visitorreqdate" TargetControlID="txt_visitorreqdate"
                                                                runat="server" CssClass="cal_Theme1
        ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table>
                                      
                                        <tr>
                                            <td>
                                                Name
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_name" TextMode="SingleLine" runat="server" onfocus="return myFunction(this)"
                                                    Height="20px" CssClass="newtextbox textbox1" Width="300px" AutoPostBack="true"
                                                    OnTextChanged="txt_name_TextChanged" onblur="return getdet(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_name" runat="server" TargetControlID="txt_name"
                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=". ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                                    ServiceMethod="GetVendCompDet" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_name"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Designation
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_visitorDesg" Visible="false" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="newtextbox textbox1" Width="300px"></asp:TextBox>
                                                <asp:Button ID="btn_desgplus" Visible="true" runat="server" Text="+" CssClass="textbox btn"
                                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_desgplus_Click" />
                                                <asp:DropDownList ID="ddl_designation" Visible="true" runat="server" CssClass="textbox textbox1 ddlstyle
        ddlheight5">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_desgminus" runat="server" Visible="true" Text="-" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_desgminus_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Department
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_visitorDept" Visible="false" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="newtextbox
        textbox1" Width="300px"></asp:TextBox>
                                                <asp:Button ID="btn_deptplus" runat="server" Visible="true" Text="+" CssClass="textbox btn"
                                                    Font-Size="Medium" Font-Names="Book
        Antiqua" OnClick="btn_deptplus_Click" />
                                                <asp:DropDownList ID="ddl_department" runat="server" Visible="true" CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_deptminus" runat="server" Visible="true" Text="-" Font-Size="Medium"
                                                    Font-Names="Book Antiqua" CssClass="textbox btn" OnClick="btn_deptminus_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Phone.No
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_visitorph" Visible="true" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="newtextbox
        textbox1" MaxLength="15" placeholder="Ex:044-786577898"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_visitorph" runat="server" TargetControlID="txt_visitorph"
                                                    FilterType="numbers,custom" ValidChars="-">
                                                </asp:FilteredTextBoxExtender>
                                                Mobile.No
                                                <asp:TextBox ID="txt_visitormob" Visible="true" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="newtextbox textbox1" MaxLength="10" placeholder="Ex:9000000000"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_visitormob" runat="server" TargetControlID="txt_visitormob"
                                                    FilterType="numbers,custom" ValidChars="-
        ">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td>Address</td>
                                        <td> <asp:TextBox ID="txt_address" Visible="true"  runat="server"
                                                    Height="26px" Width="300px" CssClass="txtcaps txtheight4" MaxLength="500" >
                                                    </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_address"
                                                    FilterType="lowercaseletters,uppercaseletters, numbers,custom" ValidChars="-/@$,. :">
                                                </asp:FilteredTextBoxExtender></td>
                                        </tr>
                                        <tr>
                                         <td>
                            <asp:Label ID="lbl_cty" runat="server" Style="top: 10px; left: 6px;" Text="City"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_cty" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                             
                        </td>
                                        </tr>
                                        <tr>
                                        <td>
                            <asp:Label ID="lbl_dis" runat="server" Style="top: 10px; left: 6px;" Text="District"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_dis" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender13" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getdis" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_dis"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground"> </asp:AutoCompleteExtender>
                            
                        </td>
                                        </tr>
                                        <tr>
                                          <td>
                            <asp:Label ID="lbl_stat" runat="server" Style="top: 10px; left: 6px;" Text="State"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_stat" runat="server" CssClass="txtcaps txtheight4"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="AutoCompleteExtender12" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="getstat" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_stat"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="panelbackground"> </asp:AutoCompleteExtender>
                             
                        </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                E-Mail
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_visitoremail" Visible="true" TextMode="SingleLine" runat="server"
                                                    Height="20px" CssClass="newtextbox textbox1" Width="300px" onfocus="return myFunction(this)"
                                                    onblur="return checkEmail(this)"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_visitoremail" runat="server" TargetControlID="txt_visitoremail"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="!#$%&'*+-/=?^_`{|}~ @.">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                          <tr><%--magesh 7.6.18--%>
                                            <td>
                                                Company Name
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_cname" TextMode="SingleLine" runat="server" Height="20px" 
                                                    CssClass="newtextbox textbox1" AutoPostBack="true" OnTextChanged="txt_cname_TextChanged"
                                                    Width="300px"> <%--magesh 7.6.18//onfocus="return myFunction(this)"--%></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_cname" runat="server" TargetControlID="txt_cname"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=".&,@-_() ">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:AutoCompleteExtender ID="acext_cname" runat="server" DelimiterCharacters=""
                                                    Enabled="True" ServiceMethod="GetVendComp" MinimumPrefixLength="0" CompletionInterval="100"
                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cname"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="txtsearchpan">
                                                </asp:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Meet To
                                            </td>
                                            <td colspan="3">
                                                <asp:CheckBox ID="cb_dept" runat="server" Checked="true" Text="Department" AutoPostBack="true"
                                                    OnCheckedChanged="cb_dept_CheckedChanged" onchange="return checkchange1(this.value)"
                                                    onfocus="return myFunction(this)" />
                                                <asp:CheckBox ID="cb_individual" runat="server" Text="Individual" AutoPostBack="true"
                                                    onchange="checkchange2(this)" onfocus="return myFunction(this)" OnCheckedChanged="cb_individual_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <div id="div_dept" runat="server" visible="false" onfocus="return myFunction(this)">
                                                    <span id="deptmsg"></span><span id="indimsg"></span>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_dept_to" Text="Department To" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_dept_to" runat="server" Width="210px" Height="20px" onfocus="return myFunction(this)"
                                                                    onchange="return checkdepartment(this.value)" onkeyup="return checkdepartment(this.value)"
                                                                    CssClass="textbox1 textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_dept_to"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="auto_dept" runat="server" DelimiterCharacters="" Enabled="True"
                                                                    ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept_to" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="btn_stud_deptto_add" runat="server" Text="Add" CssClass="textbox btn1 textbox1"
                                                                    OnClientClick="return change3();" OnClick="btn_stud_deptto_add_Click" />
                                                                <asp:Button ID="btn_stud_deptto_rmv" runat="server" Width="55px" Text="Remove" CssClass="textbox btn1 textbox1"
                                                                    OnClientClick="return change31();" OnClick="btn_stud_deptto_rmv_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_to1" runat="server" Visible="false" onchange="return checkdepartment(this.value)"
                                                                    onkeyup="return checkdepartment(this.value)" onfocus="return myFunction(this)"
                                                                    Width="210px" Height="20px" CssClass="textbox1
        textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_to1"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="auto_dept1" runat="server" DelimiterCharacters="" Enabled="True"
                                                                    ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_to1" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_dept_cc" Text="" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_dept_cc" runat="server" Width="210px" onchange="return checkdepartment(this.value)"
                                                                    onkeyup="return checkdepartment(this.value)" onfocus="return myFunction(this)"
                                                                    Height="20px" CssClass="textbox1
        textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_dept_cc"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="auto_dept2" runat="server" DelimiterCharacters="" Enabled="True"
                                                                    ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_dept_cc" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="btn_stud_deptcc_add" runat="server" CssClass="textbox btn1 textbox1"
                                                                    Text="Add" OnClientClick="return change4();" OnClick="btn_stud_deptcc_add_Click" />
                                                                <asp:Button ID="btn_stud_deptcc_remove" runat="server" Width="55px" CssClass="textbox
        btn1 textbox1" Text="Remove" OnClientClick="return change41();" OnClick="btn_stud_deptcc_remove_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_cc1" runat="server" Visible="false" onchange="return checkdepartment(this.value)"
                                                                    onkeyup="return checkdepartment(this.value)" onfocus="return myFunction(this)"
                                                                    Width="210px" Height="20px" CssClass="textbox1
        textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_cc1"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="auto_dept3" runat="server" DelimiterCharacters="" Enabled="True"
                                                                    ServiceMethod="Getdept" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                                                    CompletionSetCount="10" ServicePath="" TargetControlID="txt_cc1" CompletionListCssClass="autocomplete_completionListElement"
                                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="div_indiv" runat="server" style="margin-left: 11px" visible="false" onfocus="return myFunction(this)">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_indiv" Text="Individual To" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_indiv" runat="server" Width="210px" onchange="return checkindiv(this.value)"
                                                                    onkeyup="return checkindiv(this.value)" onfocus="return myFunction(this)" Height="20px"
                                                                    CssClass="textbox1 textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender55" runat="server" TargetControlID="txt_indiv"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="autostudindi1" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="btn_stud_indito_add" runat="server" CssClass="textbox btn1 textbox1"
                                                                    Text="Add" OnClientClick="return change5();" OnClick="btn_stud_indito_add_Click" />
                                                                <asp:Button ID="btn_stud_indito_rmv" Width="55px" runat="server" CssClass="textbox
        btn1 textbox1" Text="Remove" OnClientClick="return change51();" OnClick="btn_stud_indito_rmv_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_indiv1" runat="server" Visible="false" onchange="return checkindiv(this.value)"
                                                                    onkeyup="return checkindiv(this.value)" onfocus="return myFunction(this)" Width="210px"
                                                                    Height="20px" CssClass="textbox1
        textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_indiv1"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="autostudindi2" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv1"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_indiv_cc" Text="" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_indiv_cc" runat="server" Width="210px" onchange="return checkindiv(this.value)"
                                                                    onkeyup="return checkindiv(this.value)" onfocus="return myFunction(this)" Height="20px"
                                                                    CssClass="textbox1
        textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender57" runat="server" TargetControlID="txt_indiv_cc"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="autostudindi3" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_indiv_cc"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                                <asp:Button ID="btn_stud_indicc_add" runat="server" CssClass="textbox btn1 textbox1"
                                                                    Text="Add" OnClientClick="return change6();" OnClick="btn_stud_indicc_add_Click" />
                                                                <asp:Button ID="btn_stud_indicc_rmv" runat="server" Width="55px" CssClass="textbox
        btn1 textbox1" Text="Remove" OnClientClick="return change61();" OnClick="btn_stud_indicc_rmv_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_cc2" runat="server" Visible="false" onchange="return checkindiv(this.value)"
                                                                    onkeyup="return checkindiv(this.value)" onfocus="return myFunction(this)" Width="210px"
                                                                    Height="20px" CssClass="textbox1
        textbox"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender58" runat="server" TargetControlID="txt_cc2"
                                                                    FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" .-">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:AutoCompleteExtender ID="autostudindi4" runat="server" DelimiterCharacters=""
                                                                    Enabled="True" ServiceMethod="Getstaff" MinimumPrefixLength="0" CompletionInterval="100"
                                                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_cc2"
                                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="panelbackground">
                                                                </asp:AutoCompleteExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Purpose
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_visitorpurpose" runat="server" onfocus="return
        myFunction(this)" Height="20px" Width="300px" TextMode="MultiLine" CssClass="newtextbox
        textbox2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_visitorpurpose" runat="server" TargetControlID="txt_visitorpurpose"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr id="ex" runat="server" visible="false">
                                            <td>
                                                Expected Date
                                            </td>
                                            <td colspan="4">
                                                <div>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:UpdatePanel ID="upp_visitdate" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txt_visitdate" runat="server" CssClass="txtcaps newtextbox textbox1 txtheight"
                                                                            AutoPostBack="true" OnTextChanged="txt_visitdate_TextChanged"></asp:TextBox>
                                                                        <asp:CalendarExtender ID="CalendarExtender5" TargetControlID="txt_visitdate" runat="server"
                                                                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                                                        </asp:CalendarExtender>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                Time
                                                                <asp:DropDownList ID="ddl_hrs" runat="server" CssClass="txtcaps" Height="25px" Width="50px">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddl_mins" runat="server" CssClass="txtcaps" Height="25px" Width="50px">
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddl_ampm" runat="server" CssClass="txtcaps" Height="25px" Width="50px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="imgdiv5" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <center>
                                            <div id="panel_description2" runat="server" visible="false" class="table" style="background-color: White;
                                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                margin-top: 200px; border-radius: 10px;">
                                                <table>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_description3" runat="server" Text="Description" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:TextBox ID="txt_department" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                                                margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <br />
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btn_deptadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox
        btn1" OnClick="btn_deptadddesc1_Click" />
                                                            <asp:Button ID="btn_deptexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox
        btn1" OnClick="btn_deptexitdesc1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </div>
                                    <div id="imgdiv6" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <center>
                                            <div id="panel_description3" runat="server" visible="false" class="table" style="background-color: White;
                                                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                margin-top: 200px; border-radius: 10px;">
                                                <table>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_description4" runat="server" Text="Description" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:TextBox ID="txt_designation" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                                                margin-left: 13px" Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <br />
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btn_desgadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_desgadddesc1_Click" />
                                                            <asp:Button ID="btn_desgexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_desgexitdesc1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </div>
                                </div>
                            </center>
                            <br />
                            <%-- ********end
        of visitor******--%>
                            <center>
                                <div id="div_complaints" runat="server" visible="false">
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_compreqno" runat="server" ReadOnly="true" CssClass="newtextbox
        textbox1 txtheight"> </asp:TextBox>
                                                </td>
                                                <td width="60px">
                                                    <asp:Label ID="lbl_compreqdate" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="upp_compreqdate" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_compreqdate" runat="server" ReadOnly="true" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                                            <asp:CalendarExtender ID="cext_compreqdate" TargetControlID="txt_compreqdate" runat="server"
                                                                CssClass="cal_Theme1
        ajax__calendar_active" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <table>
                                        <tr>
                                            <td>
                                                Complaints
                                            </td>
                                            <td colspan="3">
                                                <asp:Button ID="btn_compplus" runat="server" Text="+" CssClass="textbox
        btn" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_compplus_Click" />
                                                <asp:DropDownList ID="ddl_complaints" runat="server" onfocus="return myFunction(this)"
                                                    CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_compminus" runat="server" Text="-" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox btn" OnClick="btn_compminus_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Complaints Regarding
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txt_regards" runat="server" onfocus="return myFunction(this)" Rows="15"
                                                    TextMode="MultiLine" Style="resize: none; height: 80px;" CssClass="newtextbox
        textbox1" Width="300px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="ftext_regards" runat="server" TargetControlID="txt_regards"
                                                    FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()-">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Suggestions
                                            </td>
                                            <td colspan="3">
                                                <asp:Button ID="btn_sugplus" runat="server" Text="+" CssClass="textbox
        btn" Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btn_sugplus_Click" />
                                                <asp:DropDownList ID="ddl_suggestions" runat="server" onfocus="return myFunction(this)"
                                                    CssClass="textbox textbox1 ddlstyle ddlheight5">
                                                </asp:DropDownList>
                                                <asp:Button ID="btn_sugminus" runat="server" Text="-" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox btn" OnClick="btn_sugminus_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="imgdiv3" runat="server" visible="false" style="height: 100em; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <center>
                                            <div id="panel_description" runat="server" visible="false" class="table" style="background-color: White;
                                                height: 160px; width: 580px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                margin-top: 180px; border-radius: 10px;">
                                                <table>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_description11" runat="server" Text="Description" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:TextBox ID="txt_complaints" Rows="4" TextMode="MultiLine" runat="server" Width="450px"
                                                                Style="font-family: 'Book Antiqua'; margin-left: 13px; resize: none; height: 50px;"
                                                                Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_complaints" runat="server" TargetControlID="txt_complaints"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btn_compadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_compadddesc1_Click" />
                                                            <asp:Button ID="btn_compexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_compexitdesc1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </div>
                                    <div id="imgdiv4" runat="server" visible="false" style="height: 100em; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <center>
                                            <div id="panel_description1" runat="server" visible="false" class="table" style="background-color: White;
                                                height: 160px; width: 580px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                margin-top: 180px; border-radius: 10px;">
                                                <table>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_description2" runat="server" Text="Description" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:TextBox ID="txt_sggestions" Rows="4" TextMode="MultiLine" runat="server" Width="450px"
                                                                Style="font-family: 'Book Antiqua'; margin-left: 13px; resize: none; height: 50px;"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="ftext_sggestions" runat="server" TargetControlID="txt_sggestions"
                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="&' @.()-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btn_sugadddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_sugadddesc1_Click" />
                                                            <asp:Button ID="btn_sugexitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btn_sugexitdesc1_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </center>
                                    </div>
                                </div>
                                <br />
                            </center>
                            <div id="div_save" runat="server" visible="true">
                                <asp:Button ID="btn_reqsave" runat="server" Visible="false" CssClass="textbox btn2"
                                    Text="Save" OnClick="btn_reqsave_Click"  OnClientClick="return DisplayLoadingDiv();"/><%--OnClientClick="return
        IsNotEmpty();" --%>
                                <asp:Button ID="btn_reqclear" runat="server" Visible="false" CssClass="textbox
        btn2" Text="Clear" OnClick="btn_reqclear_Click" />
                                <asp:Panel ID="dynamictxt" runat="server" Height="400px" Width="300px">
                                </asp:Panel>
                            </div>
                            <%-- **************end of complaints****************--%>
                            <center>
                                <div id="div_leavereq" runat="server" visible="false">
                                    <div class="maindivstyle" align="center" style="border-radius: 7px; width: 400px;
                                        height: 40px; margin-left: 500px;">
                                        <table>
                                            <tr>
                                                <td>
                                                    Requisition No
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_rqstn_leave" runat="server" Enabled="false" ReadOnly="true" CssClass="newtextbox textbox1 txtheight"> </asp:TextBox>
                                                </td>
                                                <td width="50px">
                                                    <asp:Label ID="lbl_rqstn_leave" Width="80px" Text="Req Date" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_time_rqstn_leave" Enabled="false" runat="server" ReadOnly="false"
                                                                CssClass="newtextbox txtheight textbox2" AutoPostBack="true" OnTextChanged="txt_time_rqstn_leave_TextChanged"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender9" TargetControlID="txt_time_rqstn_leave"
                                                                runat="server" Format="dd/MM/yyyy">
                                                            </asp:CalendarExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>
                                                <table style="margin-top: 10px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label13" runat="server" Text="Staff Code"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_staff_code" CssClass="textbox textbox1 txtheight" runat="server"
                                                                AutoPostBack="true" OnTextChanged="txt_staff_code_TextChanged"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txt_staff_code"
                                                                FilterType="LowercaseLetters,UppercaseLetters,numbers,custom" ValidChars=" &/">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:AutoCompleteExtender ID="autoindi_indi4" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff_code"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:Button ID="Btn_Staff_Code" runat="server" CssClass="btn textbox textbox1" Text="?"
                                                                OnClick="Btn_Staff_Code_Click" />
                                                        </td>
                                                        <td style="width: 80px;">
                                                            <asp:Label ID="Label14" runat="server" Text="Apply
        Date"></asp:Label>
                                                        </td>
                                                        <td style="width: 60px;">
                                                            <asp:TextBox ID="txt_applydate" Enabled="false" CssClass="textbox textbox1 txtheight"
                                                                runat="server"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender6" TargetControlID="txt_applydate" Format="d/MM/yyyy"
                                                                runat="server">
                                                            </asp:CalendarExtender>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txt_applydate"
                                                                FilterType="Custom,Numbers" ValidChars="/">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label15" runat="server" Text="Staff Name"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txt_staff_name" CssClass="textbox textbox1 txtheight5" runat="server"
                                                                OnTextChanged="txt_staff_name_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender5" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staff_name"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label22" runat="server" Text="Department"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txt_dep" ReadOnly="true" runat="server" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label23" runat="server" Text="Designation"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_des" ReadOnly="true" runat="server" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                                                        </td>
                                                        <td rowspan="4" colspan="2">
                                                            <div style="overflow: auto; margin-left: 23px;">
                                                                <asp:Image ID="imagestaff" runat="server" ImageUrl="" ToolTip="Staff Photo" Style="height: 110px;
                                                                    width: 130px;" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label24" runat="server" Text="Leave Type"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:DropDownList ID="ddl_leave_type" CssClass="textbox textbox1 ddlheight4" runat="server"
                                                                ToolTip="Select The Leave Type">
                                                            </asp:DropDownList>
                                                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddl_leave_type_SelectedIndexChanged"--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label25" runat="server" Visible="false" Text="Leave Mode"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="rdlist" runat="server" Text="Full Day" Visible="false" onfocus="return myFunction(this)"
                                                                onchange="return rbtime(this.value);" GroupName="ii" />
                                                            <asp:RadioButton ID="rdlist1" runat="server" Text="Half Day" Visible="false" GroupName="ii"
                                                                onfocus="return myFunction(this)" onchange="return rbchange_leave(this.value);" />
                                                            <%--<%--<asp:RadioButtonList ID="rdlist" runat="server"
        OnSelectedIndexChanged="rdlist_SelectedIndexChanged" AutoPostBack="false" RepeatDirection="Horizontal"
        onchange= "rbchange_leave(this)" onfocus="return myFunction(this)"> <asp:ListItem
        Value="0">Full Day</asp:ListItem> <asp:ListItem Value="1">Half Day</asp:ListItem>
        </asp:RadioButtonList>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_sess" runat="server" Text="Session" Style="display: none;" onfocus="return myFunction(this)"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_sess" CssClass="ddlheight textbox textbox1" runat="server"
                                                                Style="display: none;" onfocus="return myFunction(this)">
                                                                <asp:ListItem>Morning</asp:ListItem>
                                                                <asp:ListItem>Evening</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%-- <td style="height: 6px;">
                                </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_from" runat="server" Text="From
        Date"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_frm" runat="server" CssClass="textbox
        textbox1 txtheight" AutoPostBack="true" OnTextChanged="txt_frm_TextChanged"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender7" TargetControlID="txt_frm" Format="dd/MM/yyyy"
                                                                runat="server">
                                                            </asp:CalendarExtender>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txt_frm"
                                                                FilterType="Custom,Numbers" ValidChars="/">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                        <td style="width: 60px;">
                                                            <asp:Label ID="lbl_to" runat="server" Text="To Date" onfocus="return
        myFunction(this)"></asp:Label>
                                                        </td>
                                                        <td style="width: 60px;">
                                                            <asp:TextBox ID="txt_to" runat="server" onfocus="return myFunction(this)" CssClass="textbox textbox1 txtheight"
                                                                AutoPostBack="true" OnTextChanged="txt_to_TextChanged"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender8" TargetControlID="txt_to" Format="dd/MM/yyyy"
                                                                runat="server">
                                                            </asp:CalendarExtender>
                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_to"
                                                                FilterType="Custom,Numbers" ValidChars="/">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:Calendar ID="cal" runat="server" Visible="false"></asp:Calendar>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="div_GV1" runat="server" visible="false" style="width: 360px; height: 140px;
                                                                overflow: auto;">
                                                                <asp:GridView ID="GV1" runat="server" Visible="true" AutoGenerateColumns="false"
                                                                    GridLines="Both" OnRowDataBound="OnRowDataBound_gv1">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtdate" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'
                                                                                    CssClass="textbox txtheight"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Morning" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chk_mrng" runat="server" Checked="true" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Evening" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chk_evng" runat="server" Checked="true" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                       <asp:TemplateField HeaderText="Hours" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center" >
                    <ItemTemplate>
                        <div style="position: relative;">
                            <div id="div5" style="position: relative;" runat="server">
                                <asp:UpdatePanel ID="upnlPeriod1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txthour" Visible="true" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                            ReadOnly="true">-- Select --</asp:TextBox>
                                        <asp:Panel ID="pnlhour" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="380px">
                                            <asp:CheckBox ID="chkhr" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                                AutoPostBack="True" OnCheckedChanged="chkhr_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblhr"  CssClass="commonHeaderFont" runat="server">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExthr" runat="server" TargetControlID="txthour"
                                            PopupControlID="pnlhour" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                      
                    </ItemTemplate>
                </asp:TemplateField>

                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_holidayalert" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr id="trincharge" runat="server" visible="false">
                                                        <td>
                                                            <asp:Label ID="lblstfinchrge" runat="server" Text="Staff Incharge"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="ddlstfincharge" CssClass="ddlheight3 textbox textbox1" style="width:250px;" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label26" runat="server" Text="Reason"></asp:Label>
                                                        </td>
                                                        <%-- <td colspan="3">
                                                        <asp:DropDownList ID="ddl_leavereason" CssClass="ddlheight3 textbox textbox1" runat="server"
                                                            onchange="leavereason(this)" onfocus="return myFunction(this)">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txt_reason" runat="server" CssClass="textbox textbox1 txtheight4"
                                                            Style="display: none; float: right; margin-right: 36px" onfocus="return myFunction(this)"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txt_reason"
                                                            FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" *$%@!-.">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>--%>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtleavereason" runat="server" Visible="true" 
                                                                Width="270px"></asp:TextBox>
<%--                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txtleavereason"
                                                                FilterType="UppercaseLetters,Custom,Numbers" ValidChars="/ & ,.-_*">CssClass="textbox textbox1 txtheight
                                                            </asp:FilteredTextBoxExtender>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 6px;">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <div style="width: 523px; height: 350px; overflow: auto; margin-top: 20px; margin-left: -12px">
                                                    <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                                        RowStyle-HorizontalAlign="Right" OnRowDataBound="GridView2_RowDataBound" OnDataBound="GridView2_databound"
                                                        OnRowCommand="GridView2_RowCommand" OnSelectedIndexChanged="GridView2_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label ID="lbl_sno" runat="server" Width="60px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                    </center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="White" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div>
                                        <asp:Label ID="lbl_errormsg" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                                    </div>
                                    <div style="margin-left: 414px; margin-top: -48px; overflow: auto;">
                                        <table>
                                            <tr>
                                                <td>
                                                    <%-- <asp:Button ID="batchbtn" runat="server" Font-Bold="True" BorderStyle="None" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Alternate Schedule" CssClass="cursorptr" ForeColor="Blue"
                                                        Font-Underline="true" OnClick="altbatch_click" />--%>
                                                    <asp:LinkButton ID="batchbtn" runat="server" Font-Bold="True" BorderStyle="None"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Alternate Schedule" CssClass="cursorptr"
                                                        ForeColor="Blue" OnClick="altbatch_click"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnk_AlterStaff" runat="server" Font-Bold="True" BorderStyle="None"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Alternate Schedule" CssClass="cursorptr"
                                                        ForeColor="Blue" OnClick="lnk_AlterStaff_click"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:Label ID="gy" runat="server" style="display:inline-block;background-color:Tomato;height:20px;width:20px;margin-top: 28px;"></asp:Label>
                                                    <asp:Label ID="fill" runat="server" Text="Leave Not Available" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Width="159px"></asp:Label>
                                                    <asp:Label ID="cor" runat="server" Width="20px" Height="20px" BackColor="#A4F9C9"></asp:Label>
                                                    <asp:Label ID="partialfill" runat="server" Text="Partially Available" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Width="152px" Font-Size="Medium"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <div style="width: 960px; overflow: auto;">
                                        <center>
                                            <span id="sp_appsub" class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                Alternate Subject Details</span></center>
                                        <asp:GridView ID="grid_altersub" runat="server" Visible="true" AutoGenerateColumns="false"
                                            GridLines="Both" Width="940px" OnDataBound="grid_altersub_databound" OnRowDataBound="grid_altersub_rowdatabound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday0" ReadOnly="true" runat="server" Text='<%#Eval("Dummy0") %>'
                                                            ></asp:Label>
                                                    </ItemTemplate>
                                                     <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Day-Hour" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday1" ReadOnly="true" runat="server" Text='<%#Eval("Dummy") %>'
                                                           ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday22" ReadOnly="true" runat="server" Text='<%#Eval("Dummy5") %>'
                                                           ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday2" ReadOnly="true" runat="server" Text='<%#Eval("Dummy3") %>'
                                                            Width="150px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Subject" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday3" ReadOnly="true" runat="server" Text='<%#Eval("Dummy4") %>'
                                                            ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alter Staff Code" HeaderStyle-BackColor="#0CA6CA"
                                                    HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday44" ReadOnly="true" runat="server" Text='<%#Eval("Dummy6") %>'
                                                            ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="150px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alter Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday4" ReadOnly="true" runat="server" Text='<%#Eval("Dummy2") %>'
                                                            ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alter Subject" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtday5" ReadOnly="true" runat="server" Text='<%#Eval("Dummy1") %>'
                                                            ></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <div style="width: 870px; height: 114px; overflow: auto;">
                                        <center>
                                            <span id="sp_appstaf" class="fontstyleheaderrr" runat="server" visible="false" style="color: #008000;">
                                                Approval Permission Staff</span></center>
                                        <asp:GridView ID="grid_approvalstaff" runat="server" Visible="true" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="OnRowDataBound_grid_approvalstaff">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1sno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_code" runat="server" Text='<%#Eval("Dummy0") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Staff Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblname" runat="server" Text='<%#Eval("Dummy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Department" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldept" runat="server" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Designation" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldegn" runat="server" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stage" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblstage" runat="server" Text='<%#Eval("Dummy3") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <asp:Button ID="Btn_Apply_Leave" runat="server" OnClientClick="return DisplayLoadingDiv();"
                                        OnClick="Btn_Apply_Leave_Click" CssClass="btn2 textbox textbox1" Text="Save" />
                                    <asp:Button ID="Btn_Cancel" runat="server" OnClick="Btn_Cancel_Click" CssClass="btn2 textbox textbox1"
                                        Text="Clear"  />
                                </div>
                            </center>
                            <%-- ********end
        of leave******--%>
                            <center>
                                <div id="panelrollnopop" runat="server">
                                    <div id="gatepass_stud" runat="server" visible="false">
                                        <table class="maindivstyle" width="900px">
                                        <tr>
                                        <td colspan="4">

                                        <asp:RadioButton ID="Chkdesch" runat="server" Text="Dayscholar Student"
                                OnCheckedChanged="Chkdesch_checkedchanged"                    GroupName="Attendance" AutoPostBack="true" />
                               
                       
                            <asp:RadioButton ID="Chkhostel" runat="server" Text="Hostel Student" GroupName="Attendance"
                                OnCheckedChanged="Chkhostel_checkedchanged" AutoPostBack="true" /> 
                                      
                       
                            <asp:RadioButton ID="Chkall" runat="server" Text="All" GroupName="Attendance" Checked="true"
                          OnCheckedChanged="Chkhostel_checkedchanged"       AutoPostBack="true" /> 
                                        </td>
                                        </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="Batch"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlbatch1" runat="server" AutoPostBack="True" CssClass="textbox textbox1 ddlheight1"
                                                        OnSelectedIndexChanged="ddlbatch1_selectedchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="Degree"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddldegree1" runat="server" AutoPostBack="True" CssClass="ddlheight1 textbox textbox1"
                                                        OnSelectedIndexChanged="ddldegree1_selectedchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label4" runat="server" Text="Branch"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddldepart1" runat="server" AutoPostBack="True" CssClass="ddlheight3 textbox textbox1"
                                                        OnSelectedIndexChanged="ddldepart1_selectedchanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="Sem"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsem1" runat="server" AutoPostBack="True" CssClass="ddlheight1
        textbox textbox1" OnSelectedIndexChanged="ddlsem1_selectedchanged" Width="76px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Sec"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlsec1" runat="server" AutoPostBack="True" CssClass="ddlheight1
        textbox textbox1" OnSelectedIndexChanged="ddlsec1_selectedchanged" Width="72px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label7" runat="server" Text="Hostel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txthostgelname1" runat="server" CssClass="textbox
        textbox1 txtheight3" Width="100px" ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="Panel9" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                                <asp:CheckBox ID="Checkhostel1" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="Checkhostel1_checkedchanged" />
                                                                <asp:CheckBoxList ID="ddlhostel1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlhostel1_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txthostgelname1"
                                                                PopupControlID="Panel9" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label16" Width="110px" runat="server" Text="Building Name"> </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txt_Build" runat="server" CssClass="textbox textbox1 txtheight3"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="PBuild" runat="server" CssClass="multxtpanel multxtpanleheight" Height="134px"
                                                                Width="139px">
                                                                <asp:CheckBox ID="checkBuild" runat="server" Font-Bold="True" Font-Names="Book
        Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="checkBuild_checkedchanged" />
                                                                <asp:CheckBoxList ID="cheklist_Build" runat="server" Font-Size="Medium" Font-Bold="True"
                                                                    Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="cheklist_Build_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender15" runat="server" TargetControlID="txt_Build"
                                                                PopupControlID="pBuild" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label8" runat="server" Text="Floor"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updatepanel13" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtfloor" runat="server" CssClass="textbox textbox1 txtheight2"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel14" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                                <asp:CheckBox ID="Checkfloor" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="Checkfloor_checkedchanged" />
                                                                <asp:CheckBoxList ID="ddlfloor" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlfloor_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="txtfloor"
                                                                PopupControlID="panel14" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label9" Width="80px" runat="server" Text="Room Type"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updatepanel14" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtroomno" runat="server" CssClass="textbox textbox1 txtheight1"
                                                                ReadOnly="true">--Select--</asp:TextBox>
                                                            <asp:Panel ID="panel15" runat="server" CssClass="multxtpanel multxtpanleheight" Height="90px">
                                                                <asp:CheckBox ID="Checkroom" runat="server" Text="Select All" AutoPostBack="True"
                                                                    OnCheckedChanged="Checkroom_checkedchanged" />
                                                                <asp:CheckBoxList ID="ddlroom" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlroom_selectedchanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender14" runat="server" TargetControlID="txtroomno"
                                                                PopupControlID="panel15" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="Name"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox5" runat="server" CssClass="textbox textbox1 txtheight3" Width="214px"
                                                        AutoPostBack="true" OnTextChanged="TextBox5_TextChanged"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getnamegatpass" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox5"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label18" runat="server" Text="Roll
        No"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="TextBox78" runat="server" CssClass="textbox
        textbox1 txtheight3" Text="" AutoPostBack="true"   Width="214px" OnTextChanged="TextBox78_TextChanged"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender4" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getroll" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox78"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Go" OnClick="Button1_Click" CssClass="btn1 textbox1 textbox" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <asp:Label ID="erroemesssagelbl" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                                        <br />
                                        <div id="divscrll" runat="server" style="border-style: none; border-color: inherit;
                                            border-width: 2px; height: 248px; width: 942px; padding-left: 7px; overflow: auto;">
                                            <table align="center">
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" AutoGenerateColumns="False" Style="Width: auto;"    
                                                            Height="100px" OnRowDataBound="gridview1_OnRowDataBound" OnRowCommand="grd_commond">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsno" runat="server" Text='<%#Container.DataItemIndex+1
        %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle ForeColor="White" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_CheckedChanged" /></HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkup3" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>

                                                               <%-- magesh 24.5.18 onclick="Check_Click1(this);--%>
                                                               <asp:TemplateField HeaderText="View" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-ForeColor="White">
                            <ItemTemplate>
                                <%-- <asp:Button ID="btn" Text="OD" runat="server"  CommandName="Select"  OnClientClick="Check_Click(this);" />--%>
                                 <asp:Button ID="Redirect1" runat="server"
                    CommandName="Redirect"
                    Text="View"  onclick="Redirect2_Click" OnClientClick="Redirect1_Click(this);" />
                                    
                            </ItemTemplate>
                              </asp:TemplateField><%-- magesh 24.5.18 onclick="Check_Click1(this);--%>

                                                                <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblrollno" runat="server" Text='<%# Eval("Roll_No") %>' Style="Width: auto;"  ></asp:Label>
                                                                        <asp:Label ID="lblgatepass" runat="server" Visible="false" Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblreg" runat="server" Text='<%# Eval("Reg_No") %>' Style="Width: auto;"  ></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Batch Year" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblstudtype" runat="server" Width="156px" Text='<%# Eval("Batch_Year") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblstuddeg" runat="server" Width="356px" Text='<%# Eval("Degree") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblname" runat="server" Width="200px" Text='<%# Eval("stud_name")
        %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Hostel
        Name" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblhostel" runat="server" Width="200px" Text='<%# Eval("HostelName") %>'></asp:Label>
                                                                        <asp:Label ID="lblhostelcode" runat="server" Width="200px" Text='<%# Eval("HostelMasterPK") %>'
                                                                            Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Floor
        Name" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblfloorno" runat="server" Width="156px" Text='<%# Eval("Floor_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Room No" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblroomno" runat="server" Width="156px" Text='<%#
        Eval("Room_Name") %>'></asp:Label>
                                                                        <asp:Label ID="lblstatus1" runat="server" Visible="false" Width="100px" Font-Size="Medium"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <%-- <ItemStyle HorizontalAlign="Center" Width="50px" />--%>
                                                                    <HeaderStyle Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Permission Count" HeaderStyle-BackColor="#0CA6CA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblper_count" runat="server" Width="156px" Text='<%# Eval("HostelGatePassPerCount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                                                    <HeaderStyle ForeColor="White" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="Permission Count" HeaderStyle-BackColor="#0CA6CA">
                                        <ItemTemplate>
                                            <asp:Label ID="lblper_appl" runat="server" Visible="false" Width="156px" Text='<%# Eval("Gate_AllowUnApprove") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="30px" />
                                        <HeaderStyle ForeColor="White" />
                                    </asp:TemplateField>--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <FarPoint:FpSpread ID="fpcammarkstaff" runat="server" AutoPostBack="false" CssClass="cur"
                                                            BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Height="300" Width="680"
                                                            HorizontalScrollBarPolicy="AsNeeded" VerticalScrollBarPolicy="Never" Visible="False"
                                                            OnCellClick="fpcammarkstaff_CellClick" OnUpdateCommand="fpcammarkstaff_UpdateCommand">
                                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                                                            </CommandBar>
                                                            <Sheets>
                                                                <FarPoint:SheetView SheetName="Sheet1">
                                                                </FarPoint:SheetView>
                                                            </Sheets>
                                                        </FarPoint:FpSpread>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="div_staff" runat="server" visible="false">
                                        <table class="maindivstyle">
                                            <tr>
                                                <td>
                                                    Staff Code
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_pop_search" runat="server" OnTextChanged="txt_pop_search_TextChanged"
                                                        AutoPostBack="True" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_pop_search"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Staff Name
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_staffnamegate" runat="server" OnTextChanged="txt_staffnamegate_TextChanged"
                                                        AutoPostBack="True" CssClass="textbox textbox1 txtheight5"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender11" runat="server" DelimiterCharacters=""
                                                        Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffnamegate"
                                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="panelbackground">
                                                    </asp:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Department
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_staffdeptgate" runat="server" OnTextChanged="txt_pop_search_TextChanged"
                                                        AutoPostBack="True" CssClass="textbox textbox1 txtheight5" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Designation
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_staffdesgngate" runat="server" OnTextChanged="txt_pop_search_TextChanged"
                                                        AutoPostBack="True" CssClass="textbox textbox1 txtheight5" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </div>
                                    <asp:Panel ID="paneladd" runat="server" Visible="false">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblreasongate" runat="server" Text="Reason for
        Gate Pass"></asp:Label>
                                                        <asp:DropDownList ID="ddlgatepass" runat="server" CssClass="ddlheight5
        textbox textbox1" onchange="reason(this)" onfocus="return myFunction(this)">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_ddlgatepassreson" runat="server" Style="display: none; float: right"
                                                            onfocus="return myFunction(this)" CssClass="textbox textbox1
        txtheight5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                        <br />
                                        <div style="float: left; width: 550px; margin-left: 01px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label19" runat="server" Text="Apply Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtapply" runat="server" Enabled="false" OnTextChanged="txtapply_TextChanged"
                                                            CssClass="textbox textbox1 txtheight1" AutoPostBack="true"> </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" FilterType="Custom,Numbers"
                                                            ValidChars="/" runat="server" TargetControlID="txtapply">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:CalendarExtender ID="CalendarExtender11" TargetControlID="txtapply" Format="dd/MM/yyyy"
                                                            runat="server" Enabled="True">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrequest" runat="server" Text="Request By"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrequest" runat="server" CssClass="ddlheight4
        textbox textbox1" onchange="req_by(this)" onfocus="return myFunction(this)">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txt_gatepassreq" runat="server" CssClass="textbox textbox1 txtheight5"
                                                            Style="display: none; float: right;" onfocus="return myFunction(this)"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblrequestmode" runat="server" Text="Request
        Mode"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlrequestmode" runat="server" CssClass="ddlheight4 textbox textbox1"
                                                            onchange="reqmode(this)" onfocus="return
        myFunction(this)">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txt_gatepassreqmode" runat="server" CssClass="textbox textbox1 txtheight5"
                                                            Style="display: none; float: right;" onfocus="return
        myFunction(this)"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblissueperson" runat="server" Text="Request Staff"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtissueper" runat="server" CssClass="textbox1 textbox txtheight6"> </asp:TextBox>
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender9" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="Getstaffname" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtissueper"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                            ValidChars=", -." runat="server" TargetControlID="txtissueper">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="TextBox1" runat="server" Visible="false" CssClass="textbox1 textbox txtheight6"> </asp:TextBox>
                                                        <asp:Button ID="btnstaff" CssClass="btn textbox textbox1" Text="?" runat="server"
                                                            OnClick="btnstaff_click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="float: left; margin-left: 100px;">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label10" runat="server" Text="Expected Date From"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtfromdate" runat="server" OnTextChanged="txtfromdate_TextChanged"
                                                            CssClass="textbox textbox1 txtheight1" AutoPostBack="true"> </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                                            ValidChars="/" runat="server" TargetControlID="txtfromdate">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="txtfromdate" Format="dd/MM/yyyy"
                                                            runat="server" Enabled="True">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="labExittime" runat="server" Text="Expected Time Exit"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlhour" Width="50px" runat="server" CssClass="ddlheight textbox textbox1">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlmin" Width="50px" runat="server" CssClass="ddlheight
        textbox textbox1">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlsession" runat="server" Width="50px" CssClass="ddlheight textbox textbox1">
                                                            <asp:ListItem>AM</asp:ListItem>
                                                            <asp:ListItem>PM</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Text="Expected Date To"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txttodate" runat="server" OnTextChanged="txttodate_TextChanged"
                                                            AutoPostBack="true" CssClass="textbox textbox1 txtheight1"> </asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender51" FilterType="Custom,Numbers"
                                                            ValidChars="/" runat="server" TargetControlID="txttodate">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:CalendarExtender ID="CalendarExtender41" TargetControlID="txttodate" Format="dd/MM/yyyy"
                                                            runat="server" Enabled="True">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox ID="txtstaff_co" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Text="" Style="opacity: 0; height: 0; width: 0;"></asp:TextBox>
                                                        <asp:Label ID="labentrytime" runat="server" Text="Expected Time Entry"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlendhour" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlendmin" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddlenssession" runat="server" Width="50px" CssClass="ddlheight2 textbox textbox1">
                                                            <asp:ListItem>AM</asp:ListItem>
                                                            <asp:ListItem>PM</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <%-- <table> <tr> <td> <asp:Label ID="lblview" runat="server" Text="View The Attachment"
        Visible="false"></asp:Label> </td> <td> </td> <td> <asp:Button ID="BtnView" CssClass="dropdown"
        runat="server" Text="View" Font-Bold="True" Font-Size="Medium" Font-Names="Book
        Antiqua" OnClick="BtnView_click" Width="50px" Height="25px" Visible="false" /> </td>
        </tr> </table>--%>
                                        <div style="float: left; margin-left: 440px;">
                                            <center>
                                                <asp:Label ID="lblerror1" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </center>
                                            <br />
                                            <center>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn2 textbox textbox1"
                                                                OnClick="btnsave_click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnnew" runat="server" Text="Clear" OnClick="btnnew_click" CssClass="btn2 textbox textbox1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>

                                        </div>
                                    </asp:Panel>
                                   
                                </div>
                            </center>
                            <br />
                            <asp:Panel ID="pan_gatepass" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
                                Font-Bold="true" BackColor="#CCCCCC" Font-Names="Book
        Antiqua" Font-Size="Medium" Width="252px" Height="109px" Style="position: absolute; left: 457px; margin-top: 148px;">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <caption runat="server" id="Capgatepass" title="Leave Reason">
                                                </caption>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txt_gatepass" Width="193px" Height="20px" CssClass="dropdown" runat="server"
                                                    Font-Names="Book Antiqua" TextMode="MultiLine"> </asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_gatepass"
                                                    FilterType="Custom,lowercaseletters,uppercaseletters" ValidChars=",/-() " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="gatepassadd" Width="50px" runat="server" Text="Add" CssClass="dropdown"
                                                    OnClick="gatepassadd_Click" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Height="25px" />
                                                &nbsp;
                                                <asp:Button ID="gatepassexit" Width="50px" runat="server" Text="Exit" CssClass="dropdown"
                                                    OnClick="gatepassexit_Click" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Height="25px" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </asp:Panel>
                              

                            <%--popwindow1--%>
                            <center>
                                <div id="popwindow1" runat="server" visible="false" class="popupstyle popupheight">
                                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 433px;"
                                        OnClick="imagebtnpopclose_Click" />
                                    <br />
                                    <div class="subdivstyle" style="background-color: White; height: 578px; width: 900px;">
                                        <br />
                                        <div>
                                            <asp:Label ID="lbl_selectitem3" runat="server" Style="font-size: large; color: Green;"
                                                Text="Select the Item" Font-Bold="true"></asp:Label>
                                        </div>
                                        <br />
                                        <asp:UpdatePanel ID="upp4" runat="server">
                                            <ContentTemplate>
                                                <table class="maintablestyle">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_itemheader3" runat="server" Text="Item Header Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_itemheader3" runat="server" CssClass="textbox" ReadOnly="true"
                                                                Width="106px" Height="20px">--Select--</asp:TextBox>
                                                            <asp:Panel ID="p5" runat="server" CssClass="multxtpanel" Style="height: 200px; width: 160px;">
                                                                <asp:CheckBox ID="cb_itemheader3" runat="server" Text="Select All" AutoPostBack="true"
                                                                    OnCheckedChanged="cb_itemheader3_CheckedChange" />
                                                                <asp:CheckBoxList ID="cbl_itemheader3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_itemheader_SelectedIndexChange">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupExt5" runat="server" TargetControlID="txt_itemheader3"
                                                                PopupControlID="p5" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_subheadername" runat="server" Text="Sub Header Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_subheadername" runat="server" Height="20px" CssClass="textbox textbox1"
                                                                        ReadOnly="true" Width="120px">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                                                        height: 190px;">
                                                                        <asp:CheckBox ID="cb_subheadername" runat="server" Width="100px" OnCheckedChanged="cb_subheadername_CheckedChange"
                                                                            Text="Select All" AutoPostBack="True" />
                                                                        <asp:CheckBoxList ID="cbl_subheadername" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_subheadername_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_subheadername"
                                                                        PopupControlID="Panel5" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_itemtype3" runat="server" Text="Item Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="Upp5" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_itemname3" runat="server" CssClass="textbox" ReadOnly="true"
                                                                        Width="106px" Height="20px">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="p51" runat="server" CssClass="multxtpanel" Style="height: 300px; width: 200px;">
                                                                        <asp:CheckBox ID="chk_pop2itemtyp" runat="server" Text="Select All" AutoPostBack="true"
                                                                            OnCheckedChanged="chkitemtyp" />
                                                                        <asp:CheckBoxList ID="chklst_pop2itemtyp" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chklstitemtyp">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupExt51" runat="server" TargetControlID="txt_itemname3"
                                                                        PopupControlID="p51" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <span>Search By</span>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_type" runat="server" CssClass="textbox ddlstyle" Height="30px"
                                                                OnSelectedIndexChanged="ddl_type_SelectedIndexChanged" AutoPostBack="True">
                                                                <asp:ListItem Value="0">Item Name</asp:ListItem>
                                                                <asp:ListItem Value="1">Item Code</asp:ListItem>
                                                                <asp:ListItem Value="2">Item Header</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_searchby" Visible="false" placeholder="Search Item Name" runat="server"
                                                                CssClass="textbox textbox1" Height="20px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getnamemm" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_searchitemcode" Visible="false" placeholder="Search Item Code"
                                                                runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getitemcode1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchitemcode"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_searchheadername" Visible="false" placeholder="Search Item Header"
                                                                runat="server" CssClass="textbox textbox1"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getitemheader1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchheadername"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                         <asp:Button ID="btn_newitem" runat="server"  Width="100px" CssClass="textbox btn1" Text="New Item" OnClick="btn_newitem_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_go3" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go3_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                 <center>
                            <div id="plusdiv" runat="server" visible="false" class="popupstyle popupheight1">
                    <center>
                        <div id="panel_addgroup" runat="server" visible="false" class="table" style="background-color: White;
                            height: 140px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table style="line-height: 30px">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_addgroup" runat="server" Text="Add New Item" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                <td align="center">
                                       <asp:Label ID="lbl_itemcod" runat="server" Visible="false" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txt_addnewitem" runat="server" Width="200px" CssClass="textbox textbox1"
                                            ></asp:TextBox>
                                          <asp:AutoCompleteExtender ID="AutoCompleteExtender14" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getitemname" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_addnewitem"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan">
                                                            </asp:AutoCompleteExtender>
                                   
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_addnewitem"
                                        FilterType="UppercaseLetters,lowercaseletters,Numbers,custom" ValidChars=" -_()[]{}';:/\<>,.!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="center" style="line-height: 35px">
                                        <asp:Button ID="btn_addgroup1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_addgroup_Click" />
                                        <asp:Button ID="btn_exitgroup1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                            CssClass="textbox btn2" OnClick="btn_exitaddgroup_Click" />
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
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btn_go3" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <br />
                                        <asp:Label ID="lbl_error3" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                        <center>
                                            <span>Menu Name: </span>
                                            <asp:Label ID="menulbl" runat="server" ForeColor="#0099CC
"></asp:Label></center>
                                        <br />
                                        <div id="div2" runat="server" visible="false" style="width: 850px; height: 318px;
                                            background-color: White;" class="spreadborder">
                                            <div style="width: 550px; float: left;">
                                                <br />
                                                <asp:DataList ID="gvdatass" runat="server" Font-Size="Medium" RepeatColumns="4" Width="500px"
                                                    ForeColor="#333333">
                                                    <AlternatingItemStyle BackColor="White" />
                                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                    <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="CheckBox2" Checked="true" AutoPostBack="true" OnCheckedChanged="selectedmenuchk"
                                                                        runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_itemname" ForeColor="Green" runat="server" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                                    <asp:Label ID="lbl_itemcode" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblitemheadername" ForeColor="Green" Visible="false" runat="server"
                                                                        Text='<%# Eval("ItemHeaderName") %>'></asp:Label>
                                                                    <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                                        Text='<%# Eval("ItemHeaderCode") %>'></asp:Label>
                                                                    <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemUnit") %>'></asp:Label>
                                                                    <asp:Label ID="lbl_Available" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Available") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                </asp:DataList>
                                            </div>
                                            <div style="width: 200px; float: right;">
                                                <%--20.10.15--%>
                                                <br />
                                                <%--  <br />
                        <br />--%>
                                                <asp:GridView ID="selectitemgrid" runat="server" HeaderStyle-BackColor="#0CA6CA"
                                                    AutoGenerateColumns="false" HeaderStyle-ForeColor="White">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="snogv" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="itemnamegv" runat="server" Text='<%# Eval("Item Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle BorderWidth="1px" Width="200px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Code" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="itemcodegv" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("ItemCode") %>'> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%-- <asp:CheckBox ID="CheckBox1" Visible="false" runat="server" />--%>
                                                        <asp:TemplateField HeaderText="Item Headername" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_headername" ForeColor="Green" Visible="false" runat="server" Text='<%# Eval("Header Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Headercode" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_itemheadercode" ForeColor="Red" Visible="false" runat="server"
                                                                    Text='<%# Eval("Header code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Item Unit" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_measureitem" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Item unit") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Available Qty" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Available" Visible="false" runat="server" Text='<%# Eval("Available") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                                        <%-- <asp:Label ID="itemcodegv" runat="server" Text='<%# Eval("item_code") %>'></asp:Label>--%>
                                                        <%-- </asp:TemplateField>--%>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <br />
                                        <center>
                                        <asp:UpdatePanel ID="UpSave" runat="server">
                                                                <ContentTemplate>
                                            <asp:Button ID="btn_itemsave4" runat="server" Text="Save" CssClass="textbox btn2"
                                                OnClientClick="return valid5()" OnClick="btn_itemsave4_Click" />
                                            <asp:Button ID="btn_conexist4" runat="server" Text="Exit" CssClass="textbox btn2"
                                                OnClick="btn_conexit4_Click" />
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                        </center>
                                    </div>
                                </div>
                            </center>
                            <%--  *************************************************************************8--%>
                                         <%--      Popupwindow For Library--%>
                        <center>
                                 <div id="Div_popup_lib" runat="server" visible="false" class="popupstyle popupheight">
        <asp:ImageButton ID="ImageDiv_popup_lib" runat="server" Width="40px" Height="40px"
            ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
            margin-top: 8px; margin-left: 594px;" OnClick="ImageDiv_popup_libclose_Click" />
        <br />
        <div class="subdivstyle" style="background-color: White; height: 750px; width: 1225px;">
            <br />
            <div>
                <asp:Label ID="Label33" runat="server" Style="font-size: large; color: Green;" Text="Select the Book"
                    Font-Bold="true"></asp:Label>
            </div>
            <br />
           <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="Lab_libcollege" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_lib_college" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Width="275px" AutoPostBack="True" OnSelectedIndexChanged="ddl_lib_college_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                           
                            <td>
                                <asp:TextBox ID="TextBox_title" Visible="true" placeholder="Search Book Name" runat="server"
                                    CssClass="textbox textbox1" Height="20px" Width="375px"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender16" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getbooktitle" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox_title"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                            </td>
                            <td>
                                <asp:Button ID="btn_newbook" runat="server" Width="100px" CssClass="textbox btn1"
                                    Text="New Book" OnClick="btn_newbook_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_lib_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_lib_go_Click" />
                            </td>
                        </tr>
                    </table>
                    
              </ContentTemplate>
              <Triggers>
              <asp:PostBackTrigger ControlID="btn_lib_go" />
              </Triggers>
            </asp:UpdatePanel>
               
            <br />
            <asp:Label ID="Labelboorerror" runat="server" ForeColor="Red" Visible="false" Font-Bold="true"
                Font-Size="Medium"></asp:Label>
            <center>
                <span>Book Name: </span>
                <asp:Label ID="Label_BookName" runat="server" ForeColor="#0099CC"></asp:Label>
            </center>
            <br />
            <div id="divselectedbook" runat="server" visible="false" style="width: 1220px; height: 512px;
                background-color: White;" class="spreadborder">
                <div style="width: 800px; float: left;">
                    <br />
                    <asp:DataList ID="gvdatass1" runat="server" Font-Size="Medium" RepeatColumns="4"
                        Width="500px" ForeColor="#333333">
                        <AlternatingItemStyle BackColor="White" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_book"  AutoPostBack="true" OnCheckedChanged="selectedbookchk"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_bookname" ForeColor="Green" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        <asp:Label ID="lbl_bookidno" runat="server" Visible="false" Text='<%# Eval("bookid") %>'></asp:Label>
                                    </td>
                                </tr>
                                </table>
                        </ItemTemplate>
                        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:DataList>
                        <table id="pageing" runat="server" visible="false">
                    <tr>
    <td>
        <asp:Button ID="btnfirst" runat="server" Font-Bold="true" Text="<<" Height="31px" 
                    Width="43px" onclick="btnfirst_Click" /></td>
        <td>
            <asp:Button ID="btnprevious" runat="server" Font-Bold="true" Text="<" Height="31px" 
                    Width="43px" onclick="btnprevious_Click" /></td>
            <td>
                <asp:Button ID="btnnext" runat="server" Font-Bold="true" Text=">" Height="31px"
                    Width="43px" onclick="btnnext_Click" /></td>
                <td>
                    <asp:Button ID="btnlast" runat="server" Font-Bold="true" Text=">>" Height="31px" 
                    Width="43px" onclick="btnlast_Click" /></td>
    </tr>
                    </table>
                </div>
                <div style="width: 400px; float: right;">
                    <br />
                    <asp:GridView ID="selectbookgrid" runat="server" HeaderStyle-BackColor="#0CA6CA"
                        AutoGenerateColumns="false" HeaderStyle-ForeColor="White">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="snogvbook" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Title">
                                <ItemTemplate>
                                    <asp:Label ID="Titlegv" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle BorderWidth="1px" Width="200px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Author" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="Authorgv" ForeColor="Red" runat="server" Text='<%# Eval("Author") %>'> </asp:Label>
                                  <asp:Label ID="bookidgv" runat="server" Visible="false" Text='<%# Eval("bookid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%-- <asp:Label ID="itemcodegv" runat="server" Text='<%# Eval("item_code") %>'></asp:Label>--%>
                            <%-- </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <br />
            <center>
                <div>
                    <asp:Button ID="btn_booksave" runat="server" Text="Save" CssClass="textbox btn2"
                        OnClientClick="return valid5()" OnClick="btn_booksave_Click" />
                    <asp:Button ID="btn_bookexit" runat="server" Text="Exit" CssClass="textbox btn2"
                        OnClick="btn_bookexit_Click" />
                </div>
            </center>
        </div>
    </div>
                                                    </center>

                  <center>
        <div id="div_AddNewbook" runat="server" visible="false" class="popupstyle popupheight1">
            <center>
                <div id="div_AddNewbook1" runat="server" visible="false" class="table" style="background-color: White;
                    height: 190px; width: 670px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <table style="line-height: 30px">
                       <span>
                                <asp:Label ID="Label_newbook" runat="server" Text="Add New Book And Author" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                          </span>
                      <tr>
                       <td>
                                <asp:Label ID="Label35Library" runat="server" Text="Library" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtlib" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                                ReadOnly="true" Width="235px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="plib" runat="server" CssClass="multxtpanel" Width="235px">
                                                                <asp:CheckBox ID="chklib" runat="server" Width="235px" Font-Bold="True" OnCheckedChanged="chklib_CheckedChanged"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chklslib" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Width="235px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklslib_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtlib"
                                                                PopupControlID="plib" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                            </td>
                      </tr>

                        <tr>
                        <td>
                         <asp:Label ID="Label34" runat="server" Text="Book Name" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                        </td>
                            <td align="center">
                                <asp:TextBox ID="Textnewbook" runat="server" Width="510px" CssClass="textbox textbox1"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender15" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getbookname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="Textnewbook"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                
                            </td>
                        </tr>
                         <tr>
                         <td>
                         <asp:Label ID="Label36" runat="server" Text="Author Name" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                        </td>
                            <td align="center">
                                <asp:TextBox ID="TextBox_Author" runat="server" Width="510px" CssClass="textbox textbox1"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender17" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="Getauthorname" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="TextBox_Author"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                
                            </td>
                        </tr>
                    </table>
                   <div>
        <center>
            <asp:Button ID="btn_addgroup_newbook" runat="server" Text="Add" Font-Bold="True"
                Font-Names="Book Antiqua" CssClass="textbox btn2" OnClick="btn_addgroup_newbook_Click" />
            <asp:Button ID="btn_addbookexit" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                CssClass="textbox btn2" OnClick="btn_addbookexit_Click" />
        </center>
    </div>
    <div>
        <asp:Label ID="lblerrorbook" runat="server" Visible="false" ForeColor="red" Font-Size="Smaller"></asp:Label>
    </div>
                </div>
            </center>
        </div>
    </center>                             

                    <%--  End--%>

                            <center>
                                <div id="Div1" runat="server" visible="false" style="height: 50em; z-index: 100000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="ImageButton3" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 26px; margin-left: 408px;"
                                        OnClick="imagebtnpop1close_Click" />
                                    <br />
                                    <br />
                                    <div style="background-color: White; height: 570px; width: 850px; border: 5px solid #0CA6CA;
                                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                        <br />
                                        <span style="font-size: large; color: Green;">Select the Staff Name</span>
                                        <br />
                                        <br />
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <%--                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepratstaff" CssClass="ddlheight4 textbox textbox1" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                          
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="Staff Type">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_stftype" CssClass="ddlheight4 textbox textbox1" runat="server"
                                        OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                  </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label21" runat="server" Text="Designation"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_design" CssClass="ddlheight4 textbox textbox1" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_design_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                           
                                <td>
                                    <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Visible="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstaff"
                                        runat="server" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged" AutoPostBack="true"
                                        Visible="true" CssClass="ddlheight4 textbox textbox1">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                        AutoPostBack="True" Visible="true" CssClass="txtheight3 textbox textbox1"></asp:TextBox>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                        <asp:TextBox ID="txt_search1" runat="server" OnTextChanged="txt_search1_TextChanged"
                                        AutoPostBack="True" Visible="false" CssClass="txtheight3 textbox textbox1"></asp:TextBox>
                                         <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search1"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                <%--</td>
                                <td>--%>
                                                <table class="maintablestyle">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_poupcollege" runat="server" Text="College"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_poupdept" runat="server" Text="Department"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_staff_dept11" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                                        onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel7" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                                        <asp:CheckBox ID="cb_staff_dept11" runat="server" Width="100px" Text="Select All"
                                                                            OnCheckedChanged="cb_staff_dept11_CheckedChanged" AutoPostBack="true" />
                                                                        <asp:CheckBoxList ID="cbl_staff_dept11" runat="server" OnSelectedIndexChanged="cbl_staff_dept11_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_staff_dept11"
                                                                        PopupControlID="panel7" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_popupstafftype" runat="server" Text="Staff Type">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_staff_type11" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                                                        onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel8" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                                        <asp:CheckBox ID="cb_staff_type111" runat="server" Width="100px" Text="Select All"
                                                                            OnCheckedChanged="cb_staff_type111_CheckedChanged" AutoPostBack="true" />
                                                                        <asp:CheckBoxList ID="cbl_staff_type111" runat="server" OnSelectedIndexChanged="cb_staff_type111_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender16" runat="server" TargetControlID="txt_staff_type11"
                                                                        PopupControlID="panel8" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_popdesign" runat="server" Text="Designation"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txt_staff_desg111" runat="server" CssClass="textbox txtheight2"
                                                                        ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                                    <asp:Panel ID="panel10" runat="server" CssClass="multxtpanel multxtpanleheight">
                                                                        <asp:CheckBox ID="cb_staff_desn11" runat="server" Width="100px" Text="Select All"
                                                                            OnCheckedChanged="cb_staff_desn11_CheckedChanged" AutoPostBack="true" />
                                                                        <asp:CheckBoxList ID="cbl_staff_desn11" runat="server" OnSelectedIndexChanged="cbl_staff_desn11_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="txt_staff_desg111"
                                                                        PopupControlID="panel10" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Visible="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlstaff" runat="server" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                                                AutoPostBack="true" Visible="true" CssClass="ddlheight4 textbox textbox1">
                                                                <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                                                <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                                                AutoPostBack="True" Visible="true" CssClass="txtheight3 textbox textbox1" Width="200px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender8" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstaffname1" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txt_search1" runat="server" OnTextChanged="txt_search1_TextChanged"
                                                                AutoPostBack="True" Visible="false" CssClass="txtheight3 textbox textbox1" Width="200px"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender7" runat="server" DelimiterCharacters=""
                                                                Enabled="True" ServiceMethod="Getstaffcode" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search1"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="panelbackground">
                                                            </asp:AutoCompleteExtender>
                                                            <td>
                                                                <asp:Button ID="btn_gostaff" runat="server" CssClass="textbox1 textbox btn1" Text="Go"
                                                                    OnClick="btn_gostaff_Click" />
                                                            </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div style="margin-left: 377px;">
                                                    <asp:Label ID="lbl_totalstaffcount" runat="server" ForeColor="Green"></asp:Label></div>
                                                <br />
                                                <center>
                                                    <asp:Label ID="ermsg" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                                                        Width="600" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                                                        BorderStyle="Double" OnCellClick="fsstaff_CellClick">
                                                        <CommandBar BackColor="Control" ButtonType="PushButton" Visible="false">
                                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                        </CommandBar>
                                                        <Sheets>
                                                            <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="CadetBlue">
                                                            </FarPoint:SheetView>
                                                        </Sheets>
                                                    </FarPoint:FpSpread>
                                                </center>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <br />
                                        <center>
                                            <asp:Button runat="server" ID="btnstaffadd" CssClass="btn1 textbox textbox1" Text="Ok"
                                                OnClick="btnstaffadd_Click" />
                                            <asp:Button runat="server" ID="btnexitpop" Text="Exit" CssClass="btn1 textbox textbox1"
                                                OnClick="exitpop_Click" />
                                        </center>
                                    </div>
                                </div>
                            </center>
                            <center>
                                <div id="popwindow2" runat="server" visible="false" style="height: 50em; z-index: 100000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0;">
                                    <asp:ImageButton ID="imgbtn2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 25px; margin-left: 407px;"
                                        OnClick="imgbtn2close_Click" />
                                    <br />
                                    <br />
                                    <div style="background-color: White; height: 550px; width: 840px; border: 5px solid #0CA6CA;
                                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                        <br />
                                        <center>
                                            <div>
                                                <span style="color: #008000; font-weight: bold">Select the Student</span></div>
                                            <br />
                                        </center>
                                        <table class="maintablestyle">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_pop2collgname" Text="College Name" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2collgname" runat="server" CssClass="textbox ddlheight5 textbox1"
                                                        AutoPostBack="true" onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2degre" Text="Degree" runat="server" Width="60px"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2degre" runat="server" CssClass="textbox ddlheight2 textbox1"
                                                        OnSelectedIndexChanged="ddl_pop2degre_SelectedIndexChanged" AutoPostBack="true"
                                                        onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2batchyr" Text="Batch Year" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2batchyear" runat="server" CssClass="textbox ddlheight2 textbox1"
                                                        AutoPostBack="true" onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_pop2branch" Text="Branch" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2branch" runat="server" CssClass="textbox ddlheight5 textbox1"
                                                        AutoPostBack="true" onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2sex" Text="Sex" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2sex" runat="server" CssClass="textbox ddlheight2 textbox1"
                                                        AutoPostBack="true" onfocus="return myFunction1(this)">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_pop2studenttype" Text="Student Type" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddl_pop2studenttype" runat="server" CssClass="textbox textbox1 ddlheight2"
                                                        AutoPostBack="true" onfocus="return myFunction1(this)">
                                                        <asp:ListItem Value="Hostler','Day Scholar">Both</asp:ListItem>
                                                        <asp:ListItem Value="Hostler">Hostler</asp:ListItem>
                                                        <asp:ListItem Value="Day Scholar">Day Scholar</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_pop2go" Text="Go" CssClass="textbox btn1" runat="server" OnClick="btn_pop2go_Click" />
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                        <center>
                                            <br />
                                            <div>
                                                <asp:Label ID="lblpop2error" runat="server" ForeColor="Red" Visible="false">
                                                </asp:Label>
                                            </div>
                                        </center>
                                        <div style="width: 250px; float: right;">
                                            <asp:Label ID="lblcounttxt" runat="server" ForeColor="Red" Visible="false">
                                            </asp:Label>
                                            <asp:Label ID="lblcount" runat="server" ForeColor="Red" Visible="false">
                                            </asp:Label>
                                        </div>
                                        <br />
                                        <br />
                                        <center>
                                            <%--<div id="div2" runat="server" style="overflow: auto; width: 780px; height: 260px;
                            border: 0px solid #999999; border-radius: 5px; background-color: White; box-shadow: 0px 0px 8px #999999;">--%>
                                            <FarPoint:FpSpread ID="fproll" runat="server" Visible="false" Style="overflow: auto;
                                                height: 500px; border: 0px solid #999999; border-radius: 5px; background-color: White;
                                                box-shadow: 0px 0px 8px #999999;">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                            <%-- </div>--%>
                                        </center>
                                        <br />
                                        <asp:Button ID="btn_pop2ok" Text="Ok" runat="server" CssClass="textbox btn2" OnClick="btn_pop2ok_Click" />
                                        <asp:Button ID="btn_pop2exit" Text="Exit" runat="server" CssClass="textbox btn2"
                                            OnClick="btn_pop2exit_Click" />
                                    </div>
                                </div>
                                <%--***************** stud***********8888--%>
                            </center>
                            <%-- *************************************************Event Request**************************--%>
                            <%--*******end of div******--%>
                            <center>
                                <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                                            height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                            margin-top: 200px; border-radius: 10px;">
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lbl_description111" runat="server" Text="Description" Font-Bold="true"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                                            margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                                                        <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </center>
                                </div>
                            </center>
                            <center>
                                <div id="imgdiv2" runat="server" visible="false" style="height: 150em; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 500px;
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
                                                                <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="imgdivcnfm" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="pnl2cnfm" runat="server" class="table" style="background-color: White; height: 120px;
                                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                                            border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_alertconfm" runat="server" Text="Are You Want To Delete This Record"
                                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="btn_errorclose_cnfm" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_cnfm_Click" Text="ok" runat="server" />
                                                                <asp:Button ID="btn_errorclose_cncl" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_cncl_Click" Text="Cancel" runat="server" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="imgdivcnfm2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="pnl2cnfm2" runat="server" class="table" style="background-color: White;
                                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                            margin-top: 200px; border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_alertconfm2" runat="server" Text="Are You Want To Delete This Record"
                                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="btn_errorclose_cnfm2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_cnfm2_Click" Text="ok" runat="server" />
                                                                <asp:Button ID="btn_errorclose_cncl2" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_cncl2_Click" Text="Cancel" runat="server" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="imgdivcnfm3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="pnl2cnfm3" runat="server" class="table" style="background-color: White;
                                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                            margin-top: 200px; border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_alertconfm3" runat="server" Text="Are You Want To Delete This Record"
                                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="btn_errorclose_cnfm3" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_cnfm3_Click" Text="ok" runat="server" />
                                                                <asp:Button ID="btn_errorclose_cncl3" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_errorclose_cncl3_Click" Text="Cancel" runat="server" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="Div3" runat="server" visible="false" style="height: 200%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <center>
                                        <div id="Div4" runat="server" class="table" style="background-color: White; height: 198px;
                                            width: 530px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 551px;
                                            border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="Label12" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="bt_closedalter" runat="server" Text="Ok" CssClass="btn1 textbox1 textbox "
                                                                    OnClick="bt_closedalter_Clik" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="bb" runat="server" visible="false" style="height: 200%; z-index: 1000; width: 100%;
                                    background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;">
                                    <center>
                                        <div id="Div7" runat="server" class="table" style="background-color: White; height: 134px;
                                            width: 335px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 242px;
                                            border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="Label17" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="Button2" runat="server" Text="Ok" CssClass="btn1 textbox1 textbox "
                                                                    OnClick="bb_close_Clik" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <div id="divleavedis" runat="server" visible="false" style="height: 200%; z-index: 1000;
                                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                    left: 0px;">
                                    <asp:ImageButton ID="img_divleavedis" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                        Style="height: 30px; width: 30px; position: absolute; margin-top: 302px; margin-left: 467px;"
                                        OnClick="img_divleavedis_Click" />
                                    <br />
                                    <br />
                                    <center>
                                        <div id="Div6" runat="server" class="table div1" style="background-color: White;
                                            height: 550px; width: 960px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                            margin-top: 280px; border-radius: 10px;">
                                            <center>
                                                <table style="height: 100px; width: 100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_leavedis" runat="server" Text="" Style="color: Green;" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lbl_leavedetail" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <div style="width: 933px; height: 450px; overflow: auto;">
                                                                <asp:GridView ID="gridView3" runat="server" AutoGenerateColumns="false" GridLines="Both"
                                                                    RowStyle-HorizontalAlign="Right">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_sno" runat="server" Width="50px" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="From Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_fromdate" runat="server" Width="80px" Text='<%#Eval("Dummy1") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_todate" runat="server" Width="80px" Text='<%#Eval("Dummy2") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Leave Count" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_fullhalf" runat="server" Width="80px" Text='<%#Eval("Dummy6") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="HalfDay Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_fullhalf" runat="server" Width="80px" Text='<%#Eval("Dummy8") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Morning/Evening" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_mrngeve" runat="server" Width="80px" Text='<%#Eval("Dummy7") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Reason" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_reason" runat="server" Width="200px" Text='<%#Eval("Dummy3") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Last Approval Staff" HeaderStyle-BackColor="#0CA6CA"
                                                                            HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_lstappstaff" runat="server" Width="200px" Text='<%#Eval("Dummy4") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Approval Remarks" HeaderStyle-BackColor="#0CA6CA"
                                                                            HeaderStyle-HorizontalAlign="center">
                                                                            <ItemTemplate>
                                                                                <center>
                                                                                    <asp:Label ID="lbl_remark" runat="server" Width="200px" Text='<%#Eval("Dummy5") %>'></asp:Label>
                                                                                </center>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="White" />
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <center>
                                                                <asp:Button ID="btn_leavedisclose" CssClass=" textbox btn2 comm" Style="height: 28px;
                                                                    width: 65px;" OnClick="btn_leavedisclose_Click" Text="Close" runat="server" />
                                                            </center>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                                    </center>
                                </div>
                                <%--deparemnt auto search--%>
                                <center>
                                    <div id="divalterstaff" runat="server" visible="false" style="height: 200%; z-index: 1000;
                                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                                        left: 0px;">
                                        <asp:ImageButton ID="imgalterstafcls" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                            Style="height: 30px; width: 30px; position: absolute; margin-top: 302px; margin-left: 467px;"
                                            OnClick="imgalterstafcls_Click" />
                                        <br />
                                        <br />
                                        <center>
                                            <div id="Div8" runat="server" class="table div1" style="background-color: White;
                                                height: 550px; width: 960px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                                margin-top: 280px; border-radius: 10px;">
                                                <div style="width: 960px; overflow: auto;">
                                                    <center>
                                                        <span id="Span1" class="fontstyleheaderrr" runat="server" visible="true" style="color: #008000;">
                                                            Alternate Subject Details</span></center>
                                                    <br />
                                                    <asp:Label ID="lblaltmsg" runat="server" Visible="false"></asp:Label>
                                                    <asp:GridView ID="gdalterStaff" runat="server" Visible="true" AutoGenerateColumns="false"
                                                        GridLines="Both" Width="600px" OnDataBound="gdalterStaff_databound" OnRowDataBound="gdalterStaff_rowdatabound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblno" ReadOnly="true" runat="server" Text='<%#Eval("Sno") %>' Width="30px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbselect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Year" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblyear" ReadOnly="true" runat="server" Text='<%#Eval("sfyear") %>'
                                                                        Width="50px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dept" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldepts" ReadOnly="true" runat="server" Text='<%#Eval("sfdepts") %>'
                                                                        Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sem" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsemester" ReadOnly="true" runat="server" Text='<%#Eval("sfsemester") %>'
                                                                        Width="30px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sec" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsection" ReadOnly="true" runat="server" Text='<%#Eval("sfsection") %>'
                                                                        Width="30px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbldate" ReadOnly="true" runat="server" Text='<%#Eval("sfdate") %>'
                                                                        Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Hour" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblhr" ReadOnly="true" runat="server" Text='<%#Eval("sfhour") %>'
                                                                        Width="40px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Staff Code" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstfcode" ReadOnly="true" runat="server" Text='<%#Eval("sfcode") %>'
                                                                        Width="100px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblstfname" ReadOnly="true" runat="server" Text='<%#Eval("sfname") %>'
                                                                        Width="150px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Subject" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsub" ReadOnly="true" runat="server" Text='<%#Eval("sfsubject") %>'
                                                                        Width="225PX"></asp:Label>
                                                                    <asp:Label ID="lblsubcode" ReadOnly="true" Visible="false" runat="server" Text='<%#Eval("sfsubjectcode") %>'
                                                                        Width="225PX"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Alter Staff" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblaltstf" ReadOnly="true" runat="server" Text='<%#Eval("altsfcode") %>'
                                                                        Width="150px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <div id="divdept" runat="server" visible="false">
                                                    <table>
                                                        <tr>
                                                            <td colspan="5">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            Department
                                                                        </td>
                                                                        <td>
                                                                            <asp:UpdatePanel ID="updept" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:TextBox ID="txt_depts" runat="server" CssClass="textbox textbox1" ReadOnly="true">--Select--</asp:TextBox>
                                                                                    <asp:Panel ID="pnldept" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                                                                        <asp:CheckBox ID="cb_depts" runat="server" AutoPostBack="true" OnCheckedChanged="cbl_depts_CheckedChanged"
                                                                                            Text="Select All" />
                                                                                        <asp:CheckBoxList ID="cbl_depts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_depts_selectedchanged">
                                                                                        </asp:CheckBoxList>
                                                                                    </asp:Panel>
                                                                                    <asp:PopupControlExtender ID="pnlextnder" runat="server" PopupControlID="pnldept"
                                                                                        TargetControlID="txt_depts" Position="Bottom">
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
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5">
                                                                <table>
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
                                                                            <asp:DropDownList ID="ddlstaffid" runat="server" CssClass="ddlheight5 textbox textbox1">
                                                                            </asp:DropDownList>
                                                                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlstaffid_SelectedIndexChanged"--%>
                                                                        </td>
                                                                        <td>
                                                                            Search
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_staffid" runat="server" CssClass="textbox textbox1" Style="margin-left: 5px;
                                                                                height: 18px; width: 210px;" placeholder="Staff ID"></asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="ftext_rollno" runat="server" TargetControlID="txt_staffid"
                                                                                FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars="- ">
                                                                            </asp:FilteredTextBoxExtender>
                                                                            <asp:AutoCompleteExtender ID="acext_rollno" runat="server" DelimiterCharacters=""
                                                                                Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_staffid"
                                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                CompletionListItemCssClass="panelbackground">
                                                                            </asp:AutoCompleteExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnstaffok" runat="server" Text="OK" CssClass=" textbox btn2 comm"
                                                                                OnClick=" btnstaffok_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <td>
                                                                    <asp:Button ID="btnaltstfSave" runat="server" Text="Save" CssClass=" textbox btn2 comm"
                                                                        OnClick="btnaltstfSave_Click" />
                                                                </td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </center>
                                    </div>
                                </center>
                            </center>
                        </div>
                    </center>
                   
                </div>
                 
                <div id="divaltalert" runat="server" visible="false" style="height: 200%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="Div9" runat="server" class="table" style="background-color: White; height: 140px;
                            width: 250px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 551px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblaltalertmsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnaltalert" runat="server" Text="Ok" CssClass="btn1 textbox1 textbox "
                                                    OnClick="btnaltalert_Clik" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
                <%-- Loading Image--%>
                <center>
                    <div id="divImageLoading" runat="server" style="height: 300em; z-index: 100000; width: 100%;
                        background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
                        display: none;">
                        <center>
                            <img src="../images/loader.gif" style="margin-top: 320px; height: 50px; border-radius: 10px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: Medium; font-weight: bold; color: Black;">
                                Processing Please Wait...</span>
                        </center>
                    </div>
                </center>
<%-- alert about aletered hours--%>
                 <center>
        <div id="divPopAlertNEW" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_delete_Alter_Hour" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btn_delete_Alter_Hour_Click"
                                            Text="Yes" runat="server" />
                                    </center>
                                </td>
                                <td>
                                <center>
                                        <asp:Button ID="btn_close_alter_alert" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" Visible="true"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btn_close_alter_alert_Click"
                                            Text="No" runat="server" />
                                </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>

          <div id="divok" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divoknew" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Lblok" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="Btnok" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="Btnok_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>

    </center>
<%-- alert for successfull deletion of alter hours--%>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopupAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblpopupAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnpopupAlertMsgCloseNEW" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnpopupAlertMsgCloseNEW_Click"
                                            Text="Yes" runat="server" />
 <asp:Button ID="btnpopup_No" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnpopup_No_Click"
                                            Text="No" runat="server" />

                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>

    
            </ContentTemplate>
        </asp:UpdatePanel>
      
                                 <input type="hidden" runat="server" id="hid" />

        <script type="text/javascript">



            function checkindiv(txt1) {
                $.ajax({
                    type: "POST",
                    url: "Request.aspx/Checkindividual",
                    data: '{Roll_No: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessindiv,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }

            function OnSuccessindiv(response) {

                var mesg2 = $("#indimsg")[0];

                switch (response.d) {
                    case "0":

                        mesg2.style.color = "red";
                        mesg2.innerHTML = "Staff not exist";
                        break;
                    case "1":

                        mesg2.style.color = "green";
                        mesg2.innerHTML = "Available";

                        break;
                    case "error":
                        mesg2.style.color = "red";
                        mesg2.innerHTML = "Error occurred";
                        break;
                }
            }

            function checkdepartment(txt1) {

                $.ajax({
                    type: "POST",
                    url: "Request.aspx/Checkdept",
                    data: '{Roll_No: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessdept,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function OnSuccessdept(response) {
                var mesg = $("#deptmsg")[0];

                switch (response.d) {
                    case "0":

                        mesg.style.color = "red";
                        mesg.innerHTML = "Dept not exist";

                        break;
                    case "1":

                        mesg.style.color = "green";
                        mesg.innerHTML = "Available";

                        break;
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error occurred";
                        break;
                }
            }

            function checkEmail(id) {

                var filter = /^([a-z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    id.value = "";
                    email.focus;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }






            function checkEmail(id) {

                var filter = /^([a-z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                if (!filter.test(id.value)) {
                    id.style.borderColor = 'Red';
                    id.value = "";
                    email.focus;
                }
                else {
                    id.style.borderColor = '#c4c4c4';
                }
            }



            /* function leavereason(id) {
            var value1 = id.value;

            if (value1.trim().toUpperCase() == "OTHERS") {
            var idval = document.getElementById("<%=txtleavereason.ClientID %>");
            idval.style.display = "block";

            }
            else {
            var idval = document.getElementById("<%=txtleavereason.ClientID %>");
            idval.style.display = "none";
            }
            }*/

            function reason(id) {
                var value1 = id.value;

                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_ddlgatepassreson.ClientID %>");
                    idval.style.display = "block";

                }
                else {
                    var idval = document.getElementById("<%=txt_ddlgatepassreson.ClientID %>");
                    idval.style.display = "none";
                }
            }

            function req_by(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_gatepassreq.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_gatepassreq.ClientID %>");
                    idval.style.display = "none";
                }
            }


            function reqmode(id) {
                var value1 = id.value;
                if (value1.trim().toUpperCase() == "OTHERS") {
                    var idval = document.getElementById("<%=txt_gatepassreqmode.ClientID %>");
                    idval.style.display = "block";
                }
                else {
                    var idval = document.getElementById("<%=txt_gatepassreqmode.ClientID %>");
                    idval.style.display = "none";
                }
            }
            function getdet(txt1) {

                $.ajax({
                    type: "POST",
                    url: "Request.aspx/getData1",
                    data: '{VenContactName: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        bindss(response.d);
                    },
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function bindss(Employees) {
                var VenContactDesig = Employees[0].VenContactDesig;
                document.getElementById('<%=ddl_designation.ClientID %>').value = VenContactDesig;
                var VenContactDept = Employees[0].VenContactDept;
                document.getElementById('<%=ddl_department.ClientID %>').value = VenContactDept;
                var VendorPhoneNo = Employees[0].VendorPhoneNo;
                document.getElementById('<%=txt_visitorph.ClientID %>').value = VendorPhoneNo;
                var VendorMobileNo = Employees[0].VendorMobileNo;
                document.getElementById('<%=txt_visitormob.ClientID %>').value = VendorMobileNo;
                var VendorEmail = Employees[0].VendorEmail;
                document.getElementById('<%=txt_visitoremail.ClientID %>').value = VendorEmail;
            }
            function valid5() {
                idval4 = document.getElementById("<%=txt_visitorpurpose.ClientID %>").value;
                if (idval4.trim() == "") {
                    idval4 = document.getElementById("<%=txt_visitorpurpose.ClientID %>");
                    idval4.style.borderColor = 'Red';
                    empty = "E";
                    if (empty.trim() != "") {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }


            function change3() {
                var idval = document.getElementById("<%=txt_to1.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            function change31() {
                var idval = document.getElementById("<%=txt_to1.ClientID %>");
                idval.style.display = "none";
                document.getElementById('<%=txt_to1.ClientID %>').value = "";

                return false;
            }
            function change4() {
                var idval = document.getElementById("<%=txt_cc1.ClientID %>");
                idval.style.display = "block";

                return false;
            }
            function change41() {
                var idval = document.getElementById("<%=txt_cc1.ClientID %>");
                idval.style.display = "none";
                document.getElementById('<%=txt_cc1.ClientID %>').value = "";
                return false;
            }
            function change5() {
                var idval = document.getElementById("<%=txt_indiv1.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            function change51() {
                var idval = document.getElementById("<%=txt_indiv1.ClientID %>");
                idval.style.display = "none";
                document.getElementById('<%=txt_indiv1.ClientID %>').value = "";
                return false;
            }
            function change6() {
                var idval = document.getElementById("<%=txt_cc2.ClientID %>");
                idval.style.display = "block";
                return false;
            }
            function change61() {
                var idval = document.getElementById("<%=txt_cc2.ClientID %>");
                idval.style.display = "none";
                document.getElementById('<%=txt_cc2.ClientID %>').value = "";
                return false;
            }
            function checkchange1(id) {
                if (cb_dept.checked == true) {
                    var idval = document.getElementById("<%=div_dept.ClientID %>");
                    idval.style.display = "block";
                    return false;
                }
                if (cb_dept.checked == false) {
                    var idval = document.getElementById("<%=div_dept.ClientID %>");
                    idval.style.display = "none";
                    return false;
                }
            }
            function checkchange2() {
                if (cb_individual.checked == true) {
                    var idval = document.getElementById("<%=div_indiv.ClientID %>");
                    idval.style.display = "block";
                    return false;
                }
                else {
                    var idval = document.getElementById("<%=div_indiv.ClientID %>");
                    idval.style.display = "none";
                    return false;
                }
            }
            function myFunction(x) {

                x.style.borderColor = "#c4c4c4";
            }


            function rbchange_leave1(id) {

                var value1 = id.value;
                alter(value1);
                if (value1.trim().toUpperCase() == "Half Day") {
                    var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                    idval.style.display = "block";

                    var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                    idval.style.display = "block";
                }

                else {
                    var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                    idval.style.display = "none";

                    var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                    idval.style.display = "none";
                }
            }

            function rbchange_leave(id) {
                if (rdlist1.checked == true) {

                    var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                    idval.style.display = "block";
                    var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                    idval1.style.display = "block";
                    var idval2 = document.getElementById("<%=txt_to.ClientID %>");
                    idval2.style.display = "none";
                    var idval3 = document.getElementById("<%=lbl_to.ClientID %>");
                    idval3.style.display = "none";
                    return false;
                }
                if (rdlist1.checked == false) {
                    var idval = document.getElementById("<%=ddl_sess.ClientID %>");
                    idval.style.display = "none";
                    var idval1 = document.getElementById("<%=lbl_sess.ClientID %>");
                    idval1.style.display = "none";
                    var idval2 = document.getElementById("<%=txt_to.ClientID %>");
                    idval2.style.display = "block";
                    var idval3 = document.getElementById("<%=lbl_to.ClientID %>");
                    idval3.style.display = "block";
                    return false;
                }

            }

            function rbtime(id) {
                if (rdlist.checked == true) {
                    var idval2 = document.getElementById("<%=txt_to.ClientID %>");
                    idval2.style.display = "block";
                    var idval3 = document.getElementById("<%=lbl_to.ClientID %>");
                    idval3.style.display = "block";
                    rbchange_leave();
                    return false;
                }
            }


            function checkrno(txt1) {
                $.ajax({
                    type: "POST",
                    url: "Request.aspx/Checkstaffcode",
                    data: '{code: "' + txt1 + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccess,
                    failure: function (response) {
                        alert(response);
                    }
                });
            }
            function OnSuccess(response) {
                var mesg = $("#rnomsg")[0];

                switch (response.d) {
                    case "0":

                        mesg.style.color = "red";
                        mesg.innerHTML = "RollNo not exist";
                        studentclear();
                        break;
                    case "1":
                        get();
                        mesg.style.color = "green";
                        mesg.innerHTML = "Available";
                        break;
                    case "error":
                        mesg.style.color = "red";
                        mesg.innerHTML = "Error occurred";
                        break;
                }
            }    


        </script>  
        
         <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpSave">
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
    </body>
</asp:Content>
