<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="projectbook.aspx.cs" Inherits="LibraryMod_projectbook" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
    </style>
    <style>
        .fontblack
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: Black;
        }
        .fontcolorb
        {
            color: Green;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">

        function valid() {
            var idval = "";
            var empty = "";
            var id = "";
            var value1 = "";
            id = document.getElementById("<%=ddlprojectlibrary.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlprojectlibrary.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtaccno.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtaccno.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=Txttltle.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txttltle.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            id = document.getElementById("<%=Txtrollnumber.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=Txtrollnumber.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddlstatus.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddlstatus.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=txtname.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txtname.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            id = document.getElementById("<%=ddldepartment.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=ddldepartment.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }

            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <center>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Label ID="backvolume" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                        position: relative;" Text="Project Book" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                </div>
                <div>
                    <table class="maintablestyle" style="margin: 0px; font-family: Book Antiqua; font-weight: bold;
                        margin-bottom: 0px; margin-top: 8px; position: relative;" width="854px">
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College:" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                            <asp:ListItem Text="All"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbllib" runat="server" Text="Library:" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddllibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="170px" AutoPostBack="True" OnSelectedIndexChanged="ddllibrary_SelectedIndexChanged">
                                            <asp:ListItem Text="All"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblsearchby" runat="server" Text="SearchBy:" CssClass="commonHeaderFont">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlsearchby" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="170px" AutoPostBack="true" OnSelectedIndexChanged="ddlsearchby_SelectedIndexChanged">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Access No " Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Roll No" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Title" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Name" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="Department" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_searchby" runat="server" CssClass="textbox txtheight2" Visible="false"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Getsearch" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchby"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddldept" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="170px" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddldept_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanelbtn1" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatepanelbtn2" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btnadd" runat="server" ImageUrl="~/LibImages/Add.jpg" OnClick="btnadd_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                            <asp:GridView ID="grdProBook" Width="1000px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                ShowHeader="true" Font-Names="Book Antiqua" toGenerateColumns="false" OnRowCreated="grdProBook_OnRowCreated"
                                OnSelectedIndexChanged="grdProBook_SelectedIndexChanged">
                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label></center>
                            <center>
                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                    CssClass="textbox textbox1 txtheight5" onkeypress="return ClearPrint1()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                                <asp:ImageButton ID="btn_Excel" runat="server" ImageUrl="~/LibImages/export to excel.jpg" Visible="false"
                                    OnClick="btnExcel_Click" />
                                <asp:ImageButton ID="btn_printmaster" runat="server" ImageUrl="~/LibImages/Print White.jpg" Visible="false"
                                    OnClick="btn_printmaster_Click" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrolhed2" Visible="false" />
                            </center>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdProBook" />
                <asp:PostBackTrigger ControlID="btn_Excel" />
                <asp:PostBackTrigger ControlID="btn_printmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div id="divPopAlertprojectbook" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 8px; margin-left: 364px;"
                        OnClick="btnclose_Click" />
                    <br />
                    <div id="divPopAlertContent" runat="server" style="background-color: White; height: auto;
                        font-family: Book Antiqua; font-weight: bold; width: 776px; border: 5px solid #0CA6CA;
                        border-top: 30px solid #0CA6CA; border-radius: 10px;">
                        <br />
                        <center>
                            <table style="height: 100px; width: auto;">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Label ID="lblnonbook" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                    position: relative;" Text="Project Book Entry" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                                <tr>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblliry" runat="server" Text="Library:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlprojectlibrary" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddlprojectlibrary_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lblaccno" runat="server" Text="Access No:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtaccno" runat="server" Width="170px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="Chk_MultipleCopies" runat="server" Text="Multiple Copies" AutoPostBack="True"
                                            OnCheckedChanged="Chk_MultipleCopies_CheckedChange" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txt_MultipleCopies" runat="server" Width="130px" Height="20px" CssClass="textbox txtheight2"
                                            Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblaccssdate" runat="server" Text="Access Date" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_accessdate2" runat="server" CssClass="textbox txtheight2" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txt_accessdate2" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <span style="color: Red;">*</span>
                                        <asp:Label ID="lbltitle" runat="server" Text="Title:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txttltle" runat="server" Width="170px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblguidename" runat="server" Text="Guide Name:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Txtguidename" runat="server" Width="170px" Height="20px" CssClass="textbox txtheight2"></asp:TextBox>
                                        <asp:Button ID="Button1" runat="server" Text="?" OnClick="btnstaff_Click" Width="28px" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkview" Text="ViewStudent" Font-Name="Book Antiqua" Font-Size="11pt"
                                            OnClick="lnkviwestudent_Click" runat="server" Width="90px" />
                                    </td>
                                    <td align="left">
                                    </td>
                                </tr>
                            </table>
                            <fieldset style="width: 602px; height: 114px;">
                                <table style="height: 100px; width: 500px;">
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblrollnumner" runat="server" Text="RollNo/RegiseterNo:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtrollnumber" runat="server" AutoPostBack="true" Width="170px"
                                                Height="20px" CssClass="textbox txtheight2" OnTextChanged="txt_rollno_TextChanged"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnrollno" runat="server" Text="?" OnClick="btnrollno_Click" Width="28px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="" CssClass="commonHeaderFont" Visible="false"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lblname" runat="server" Text="Name:" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtname" runat="server" AutoPostBack="true" Width="170px" Height="20px"
                                                CssClass="textbox textbox1"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label7" runat="server" Text="" CssClass="commonHeaderFont" Visible="false"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color: Red;">*</span>
                                            <asp:Label ID="lbldepartment" runat="server" Text="Department:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updatepanel7" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddldepartment" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                        Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updatepanelbtn3" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="btnaddproject" runat="server" ImageUrl="~/LibImages/AddWhite.jpg"
                                                        OnClick="btnaddproject_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="Label8" runat="server" Text="" CssClass="commonHeaderFont" Visible="false"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <table style="height: 100px; width: 45px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Month & Year" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="textbox txtheight2" onchange="return checkDate()"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TextBox2" runat="server"
                                            Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstatus" runat="server" Text="Status:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel10" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddlstatus_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Subject:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlsubj" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Width="178px" AutoPostBack="True" Height="29px">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Language:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanel9" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlmedium" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                    Height="29px" Width="178px" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblreemark" runat="server" Text="Remark:" CssClass="commonHeaderFont"
                                            Font-Names=" Book antiqua">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtremark" runat="server" AutoPostBack="true" Width="170px" Height="20px"
                                            CssClass="textbox txtheight2"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanelbtn4" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="Btnsave" runat="server" ImageUrl="~/LibImages/save.jpg" OnClick="btnsave_Click"
                                                    OnClientClick="return valid()" />
                                                <asp:ImageButton ID="Btnup" runat="server" ImageUrl="~/LibImages/update (2).jpg"
                                                    OnClick="Btnup_Click" Visible="true" />
                                                <asp:ImageButton ID="Btndele" runat="server" ImageUrl="~/LibImages/delete.jpg" OnClick="Btndele_Click"
                                                    Visible="true" />
                                                <asp:ImageButton ID="Btnclose" runat="server" ImageUrl="~/LibImages/close.jpg" OnClick="btnclose_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </center>
                        </center>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <center>
                    <div id="divpoprollnumber" runat="server" class="popupstyle popupheight1" visible="false"
                        style="height: 300em; font-family: Book Antiqua; font-weight: bold;">
                        <asp:ImageButton ID="imgbtn_popclose" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 367px;"
                            OnClick="Btnselectrollnoclose_Click" />
                        <br />
                        <center>
                            <div id="div3" runat="server" style="background-color: White; width: 800px; height: 600px;
                                border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                                <br />
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <div>
                                        <asp:Label ID="lblrollnumber" runat="server" Style="margin: 0px; margin-top: 15px;
                                            margin-bottom: 15px; position: relative;" Text="Select Roll Number" ForeColor="Green"
                                            CssClass="fontstyleheader"></asp:Label>
                                    </div>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbatchyear" runat="server" Text="Batch Year:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlbatchyear" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddlbatchyear_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldegree" runat="server" Text="Degree:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddldegree" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblbranch" runat="server" Text="Branch:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlbranch" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblsemyear" runat="server" Text="Sem/Year:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlsemester" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsection" runat="server" Text="Section:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:DropDownList ID="ddlsection" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                                Width="178px" AutoPostBack="True" Height="29px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblrollno" runat="server" Text="Roll No:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:TextBox ID="txtrollno" runat="server" AutoPostBack="true" Width="170px" Height="20px"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblregno" runat="server" Text="Reg No:" CssClass="commonHeaderFont"
                                                Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:TextBox ID="txtreg" runat="server" AutoPostBack="true" Width="170px" Height="20px"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Name:" CssClass="commonHeaderFont" Font-Names=" Book antiqua">
                                            </asp:Label>
                                            <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="true" Width="170px" Height="20px"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="updatepanelbtn5" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="Btnselectrollno" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                                        OnClick="Btnselectrollnogo_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divTreeView" runat="server" align="left" style="overflow: auto; width: 760px;
                                    height: 350px; border-radius: 10px;">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="HiddenField2" runat="server" Value="-1" />
                                                <asp:GridView ID="grdStudent" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                                    Font-Names="book antiqua" togeneratecolumns="true" AllowPaging="true" PageSize="50"
                                                    OnSelectedIndexChanged="grdStudent_onselectedindexchanged" OnRowCreated="grdStudent_OnRowCreated"
                                                    Width="980px">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                                </asp:Label></center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="updatepanelbtn6" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="btnstaffexit" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                                        OnClick="btnstaffexit_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </center>
                    </div>
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
            <ContentTemplate>
                <div id="divstafflist" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: 20px; margin-left: 320px;"
                        OnClick="Btnstafflistclose_Click" />
                    <br />
                    <center>
                        <div id="divstafflist1" runat="server" style="background-color: White; width: 700px;
                            height: 700px; border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <span style="top: 10px; bottom: 20px; text-align: center; color: Green; font-size: large;
                                    position: relative; font-weight: bold;">Staff List</span>
                            </center>
                            <br />
                            <table width="500px" style="font-family: Book Antiqua; font-weight: bold;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="College" CssClass="commonHeaderFont">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlcolle" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Style="width: 146px; margin-left: 18px;" AutoPostBack="True" OnSelectedIndexChanged="ddlcolle_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldep" runat="server" Text="Department" CssClass="commonHeaderFont">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldep" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddldep_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsearstaf" runat="server" CssClass="commonheaderFont" Text="Search By"></asp:Label>
                                        <asp:DropDownList ID="ddlsearstaff" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                            Width="150px" OnSelectedIndexChanged="ddlsearstaff_selectedindex_changed" AutoPostBack="true">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem>Staff Name</asp:ListItem>
                                            <asp:ListItem>Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtsearstaff" runat="server" Width="150px" Visible="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="updatepanelbtn7" runat="server">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="btnseargo" runat="server" ImageUrl="~/LibImages/GoWhite.jpg"
                                                    OnClick="btnseargo_click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <div id="div2" runat="server" visible="false" style="height: 500px; width: 600px;
                                overflow: auto;">
                                <center>
                                    <asp:HiddenField ID="HiddenField3" runat="server" Value="-1" />
                                    <asp:GridView ID="grdStaff" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        Font-Names="book antiqua" togeneratecolumns="true" OnSelectedIndexChanged="grdStaff_onselectedindexchanged"
                                        OnRowCreated="grdStaff_OnRowCreated" Width="550px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl_sno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="<%#Container.DataItemIndex+1 %>" Visible="true">
                                                    </asp:Label></center>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0ca6ca" ForeColor="black" />
                                    </asp:GridView>
                                </center>
                            </div>
                            </br>
                            <asp:UpdatePanel ID="updatepanelbtn8" runat="server">
                                <ContentTemplate>
                                    <asp:ImageButton ID="btnex" Visible="false" runat="server" ImageUrl="~/LibImages/save (2).jpg"
                                        OnClick="btn_ex_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
            <ContentTemplate>
                <div id="divPoplinlkprojectbook" runat="server" class="popupstyle popupheight1" visible="false"
                    style="height: 300em; font-family: Book Antiqua;">
                    <br />
                    <center>
                        <div id="div1" runat="server" style="background-color: White; height: 500px; width: 600px;
                            border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;
                            margin-left: 150px">
                            <br />
                            <center>
                                <div id="ViewStu" runat="server" style="height: 340px; width: 500px; overflow: auto;">
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                                    <asp:GridView ID="GrdViewStu" Width="400px" runat="server" ShowFooter="false" AutoGenerateColumns="true"
                                        Font-Names="Book Antiqua" toGenerateColumns="false">
                                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                    </asp:GridView>
                                </div>
                            </center>
                            <center>
                                <asp:UpdatePanel ID="updatepanelbtn9" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="Btnclose1" runat="server" ImageUrl="~/LibImages/close.jpg" OnClick="Btnclose1_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel17" runat="server">
            <ContentTemplate>
                <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; right: 0%;">
                    <center>
                        <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <center>
                                                <asp:UpdatePanel ID="updatepanel18" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnPopAlertClose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                            OnClick="btnPopAlertClose_Click" />
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
    <center>
        <asp:UpdatePanel ID="UpdatePanel16" runat="server">
            <ContentTemplate>
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 100px;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 50px;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
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
                                                <asp:UpdatePanel ID="updatepanelbtn10" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnerrclose" runat="server" ImageUrl="~/LibImages/ok.jpg" OnClick="btnerrclose_Click" />
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
    <%--progressBar for go and add--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updatepanelbtn1">
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
    <%--progressBar for save and Exit--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updatepanelbtn4">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <%--progressBar for Add Student--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="updatepanelbtn3">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="updatepanelbtn8">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender4" runat="server" TargetControlID="UpdateProgress4"
            PopupControlID="UpdateProgress4">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="updatepanelbtn7">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
