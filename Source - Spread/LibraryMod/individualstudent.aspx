<%@ Page Title="" Language="C#" MasterPageFile="~/LibraryMod/LibraryMaster.master"
    AutoEventWireup="true" CodeFile="individualstudent.aspx.cs" Inherits="LibraryMod_individualstudent" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
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

        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%= grdindividual.ClientID %>");
            //variable to contain the cell of the grid
            var cell;
            //var gridrow = grid.rows.length;
            //alert(gridrow);
            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column

                    cell = grid.rows[i].cells[1];
                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell 
                            //checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
       
    </script>
    <center>
        <div>
            <asp:Label ID="Label4" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                position: relative;" Text="Individual Student Card Details" ForeColor="Green"
                CssClass="fontstyleheader"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="color: Black; font-family: Book Antiqua; font-weight: bold; height: 145px;
                    width: 960px; margin: 0px; margin-top: 15px; margin-bottom: 15px; position: relative;
                    text-align: left;" class="maintablestyle">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College" Style="margin-left: 5px;"></asp:Label>
                                <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="dropdown commonHeaderFont"
                                    Style="height: 22px; width: 146px; margin-left: 33px;" AutoPostBack="true" OnSelectedIndexChanged="ddl_collegename_OnSelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbllibrary" runat="server" Text="Library" CssClass="commonHeaderFont"
                                    Style="margin-left: 10px;">
                                </asp:Label>
                                <asp:DropDownList ID="ddlLibrary" runat="server" CssClass="dropdown commonHeaderFont"
                                    Style="height: 22px; width: 146px; margin-left: 47px;" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbook" runat="server" Text="Book Type" CssClass="commonHeaderFont"
                                    Style="margin-left: 10px;">
                                </asp:Label>
                                <asp:DropDownList ID="ddlbooktype" runat="server" CssClass="dropdown commonHeaderFont"
                                    Style="height: 22px; width: 146px;" AutoPostBack="True">
                                    <%--OnSelectedIndexChanged="ddlbooktype_SelectedIndexChanged"--%>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <fieldset style="border: 2px solid #ffffff; height: 5px; margin-left: 8px; width: 150px;">
                                    <asp:UpdatePanel ID="UpdatePanel9" style="width: 150px; margin-top: -9px;" runat="server">
                                        <ContentTemplate>
                                            <asp:RadioButtonList ID="rblstaff" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="rblstaffstudent_Selected" Enabled="True">
                                                <asp:ListItem Text="Student" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <fieldset id="Student" runat="server" style="width: 915px; border: 2px solid #ffffff;
                                    height: 13px;">
                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Style="margin-left: -8px;" CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlBatch" runat="server" CssClass="dropdown commonHeaderFont"
                                        Style="height: 22px; width: 146px; margin-left: 46px;" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="commonHeaderFont"
                                        Style="margin-left: 11px;">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddldegree" runat="server" CssClass="dropdown commonHeaderFont"
                                        Style="height: 22px; width: 146px; margin-left: 51px;" AutoPostBack="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Style="margin-left: 10px;"
                                        CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlbranch" runat="server" CssClass="dropdown commonHeaderFont"
                                        Style="height: 22px; width: 146px; margin-left: 28px;" AutoPostBack="True">
                                    </asp:DropDownList>
                                </fieldset>
                                <fieldset id="staff" runat="server" style="width: 915px; border: 2px solid #ffffff;
                                    height: 13px;">
                                    <asp:Label ID="lblDepartment" runat="server" Style="margin-left: -8px;" Text="Department"
                                        CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="dropdown commonHeaderFont"
                                        Style="height: 22px; width: 146px;" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblStaffCat" runat="server" Text="Staff Category" Style="margin-left: 10px;"
                                        CssClass="commonHeaderFont">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlStaffCat" runat="server" CssClass="dropdown commonHeaderFont"
                                        Style="height: 22px; width: 146px; margin-left: -3px;" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblStaffType" runat="server" Text="Staff Type" CssClass="commonHeaderFont"
                                        Style="margin-left: 10px;">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlStaffType" runat="server" CssClass="dropdown commonHeaderFont"
                                        Style="height: 22px; width: 146px;" AutoPostBack="True">
                                    </asp:DropDownList>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblcardtype" runat="server" Style="margin-left: 5px" Text="Card Type"
                                    CssClass="commonHeaderFont">
                                </asp:Label>
                                <asp:DropDownList ID="ddlcard" runat="server" CssClass="dropdown commonHeaderFont"
                                    Style="height: 22px; width: 146px; margin-left: 11px;" AutoPostBack="True" OnSelectedIndexChanged="ddlcardtype_SelectedIndexChanged">
                                    <asp:ListItem Text="General"></asp:ListItem>
                                    <asp:ListItem Text="Individual"></asp:ListItem>
                                    <asp:ListItem Text="Merit"></asp:ListItem>
                                    <asp:ListItem Text="Book Bank"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblcategory" runat="server" Text="Category" Style="margin-left: 10px;"
                                    CssClass="commonHeaderFont">
                                </asp:Label>
                                <asp:DropDownList ID="ddlcategory" runat="server" Enabled="false" CssClass="dropdown commonHeaderFont"
                                    Style="height: 22px; width: 146px; margin-left: 37px;" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblcard" runat="server" Style="margin-left: 10px;">Card Category</asp:Label>
                                        <asp:Button ID="Btnaddd" runat="server" CssClass="textbox btn2" Text="+" OnClick="btnadd_Click"
                                            Style="width: 27px; height: 27px;" />
                                        <asp:DropDownList ID="ddl_CardCatogery" runat="server" AutoPostBack="true" Style="height: 27px;
                                            width: 146px;" CssClass="textbox textbox1 ddlheight4">
                                            <%--OnSelectedIndexChanged="ddlCardCategory_SelectedIndexChanged"--%>
                                        </asp:DropDownList>
                                        <asp:Button ID="Btndelete" runat="server" CssClass="textbox btn2" Text="-" OnClick="btndel_Click"
                                            Style="width: 27px; height: 27px;" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>
                                        <asp:ImageButton ID="btngo" runat="server" ImageUrl="~/LibImages/Go.jpg" OnClick="btngo_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <asp:UpdatePanel ID="UpComButton" runat="server">
                                <ContentTemplate>
                                    <td colspan="5" runat="server" align="right">
                                        <asp:ImageButton ID="btngene" runat="server" ImageUrl="~/LibImages/generate card.jpg"
                                            OnClick="btngenerate_Click" />
                                        <asp:ImageButton ID="btnDeletecard" runat="server" ImageUrl="~/LibImages/Delete card card.jpg"
                                            OnClick="btnDeleteCard_Click" />
                                        <asp:ImageButton ID="BtnUpdateRenew" runat="server" ImageUrl="~/LibImages/update renewal days.jpg"
                                            OnClick="btnUpdateRenew_Click" />
                                        <asp:ImageButton ID="BtnAllowBkBnk" runat="server" ImageUrl="~/LibImages/Allow Book.jpg"
                                            OnClick="BtnAllowBkBnk_Click" />
                                        <asp:ImageButton ID="BtnRemoveBkBnk" runat="server" ImageUrl="~/LibImages/removal book bank.jpg"
                                            OnClick="BtnRemoveBkBnk_Click" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="divspread" runat="server" visible="false" style="overflow: auto; background-color: White;
                    border-radius: 10px;">
                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                    <asp:GridView ID="grdindividual" Width="1000px" runat="server" ShowFooter="false"
                        AutoGenerateColumns="false" Font-Names="Book Antiqua" toGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" OnRowDataBound="grdindividual_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="allchk" runat="server" Text="Select All" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="selectchk" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_roll_no" runat="server" Text='<%#Eval("roll_no") %>' Width="80px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_Name" runat="server" Text='<%#Eval("name") %>' Style="text-align: left;"
                                            Width="250px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="General" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_General" runat="server" Text='<%#Eval("General") %>' Style="text-align: right;"
                                            Width="60px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Individual" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_Individual" runat="server" Text='<%#Eval("individual") %>' Style="text-align: right;"
                                            Width="60px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Merit" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_Merit" runat="server" Text='<%#Eval("merit") %>' Style="text-align: right;"
                                            Width="60px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Book" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_Book" runat="server" Text='<%#Eval("book") %>' Style="text-align: right;"
                                            Width="60px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <center>
                                        <asp:Label ID="lbl_Total" runat="server" Text='<%#Eval("total") %>' Style="text-align: right;"
                                            Width="60px"></asp:Label>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cards to Add/Delete" HeaderStyle-BackColor="#0CA6CA"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_AddOrDel" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("AddOrDel") %>'
                                            Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                    <asp:FilteredTextBoxExtender ID="filterextenderAddOrDel" runat="server" TargetControlID="txt_AddOrDel"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Renewal Days" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_renewdays" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("RenewDays") %>'
                                            Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                    <asp:FilteredTextBoxExtender ID="filterextenderRenewDays" runat="server" TargetControlID="txt_renewdays"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Days" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_DueDays" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("DueDays") %>'
                                            Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox></center>
                                    <asp:FilteredTextBoxExtender ID="filterextenderDueDays" runat="server" TargetControlID="txt_DueDays"
                                        FilterType="Numbers,Custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Week Fine" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_weekfine" runat="server" Checked='<%#Eval("FineType").ToString()=="1"?true:false %>'>
                                    </asp:CheckBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fine" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_Fine" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("Fine") %>'
                                            Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterextenderFine" runat="server" TargetControlID="txt_Fine"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fine(OverNight Issue)" HeaderStyle-BackColor="#0CA6CA"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <center>
                                        <asp:TextBox ID="txt_FineOver" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("FineOver") %>'
                                            Height="15px" Width="60px" Style="text-align: right;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="filterextenderFineOver" runat="server" TargetControlID="txt_FineOver"
                                            FilterType="Numbers,Custom" ValidChars=".">
                                        </asp:FilteredTextBoxExtender>
                                    </center>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="grdindividual" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="NewCardCatogery" runat="server" visible="false" style="height: 100%; z-index: 10000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="DivCard" runat="server" visible="false" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 35px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblNewCardCatogery" runat="server" Text="Card Catogery" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt_CardCatogery" runat="server" Width="200px" Style="font-family: 'Book Antiqua';
                                            margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox><%--TextMode="MultiLine"--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpCatSave" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btn_NewCardCatogerySave" runat="server" Text="Add" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btn_NewCardCatogerySave_Click" />
                                                <asp:Button ID="btn_NewCardcatogeryExit" runat="server" Text="Exit" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="textbox btn1 textbox1" OnClick="btn_NewCardcatogeryExit_Click" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <br />
                    <asp:Label ID="lblErrNewCardCatoger" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
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
                                            <asp:Label ID="lblAlertMsgNEW" runat="server" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:ImageButton ID="btnPopAlertCloseNEW" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                    OnClick="btnPopAlertClose_Click" />
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
    <%--Card Generation Popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div id="Surediv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="LblGen" runat="server" Text="Do You Want To Generate Card" Style="color: Red;"
                                                Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpSureYes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnSure_yes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btnSure_yes_Click" />
                                                        <asp:ImageButton ID="btnSure_no" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btnSure_no_Click" />
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
    <%-- Delete Confirmation popup--%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <div id="sureDivDel" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divDel" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbldel" runat="server" Text="Are you Sure to Delete Card for the selected one? "
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="UpSureDel" runat="server">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="btnDelYes" runat="server" ImageUrl="~/LibImages/yes.jpg" OnClick="btnDelYes_Click" />
                                                        <asp:ImageButton ID="btnDelNo" runat="server" ImageUrl="~/LibImages/no.jpg" OnClick="btnDelNo_Click" />
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
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
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
                                                <asp:ImageButton ID="btn_errorclose" runat="server" ImageUrl="~/LibImages/ok.jpg"
                                                    OnClick="btn_errorclose_Click" />
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
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    <%--Progress bar for UpComButton--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpComButton">
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
    <%--Progress bar for UpCatSave--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpCatSave">
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
    <%--Progress bar for UpSureYes--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpSureYes">
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
    <%--Progress bar for UpSureDel--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpSureDel">
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
    <%--Progress bar for UpdatePanel8--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpdatePanel8">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender6" runat="server" TargetControlID="UpdateProgress6"
            PopupControlID="UpdateProgress6">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>
