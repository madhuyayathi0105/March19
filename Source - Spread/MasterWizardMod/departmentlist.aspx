<%@ Page Language="C#" MasterPageFile="~/MasterWizardMod/MasterWizard.master" AutoEventWireup="true"
    CodeFile="departmentlist.aspx.cs" Inherits="departmentlist" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green;">Department List</span></div>
        </center>
    </div>
    <center>
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <div>
                    <center>
                        <table class="maintablestyle" style="height: auto; font-family: Book Antiqua; font-weight: bold;
                            margin-left: 0px; margin-top: 10px; margin-bottom: 10px; padding: 6px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblclg" runat="server" Text="College" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlclg" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlcollege_selectedindexchange"
                                        Style="margin-left: 18px" Font-Bold="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldepartlist" runat="server" Text="Department Type" Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddldepartmenttype" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Font-Bold="true" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddldepartmenttype_selectedindexchange"
                                        Style="margin-left: 10px">
                                        <asp:ListItem Value="0">Other</asp:ListItem>
                                        <asp:ListItem Value="1">Academic</asp:ListItem>
                                        <asp:ListItem Value="2">All</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtsearch" runat="server" CssClass="textbox txtheight2"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getsearchby" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtsearch"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3px">
                                    <asp:Label ID="lblfromyr" runat="server" Text="Year From"></asp:Label>
                                    <asp:DropDownList ID="ddlfromyr" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Font-Bold="true" Width="85px" OnSelectedIndexChanged="ddlfromyr_selectedindexchange"
                                        Style="margin-left: 0px">
                                    </asp:DropDownList>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To" Style="margin-left: 25px;"></asp:Label>
                                    <asp:DropDownList ID="ddltoyear" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                        Font-Bold="true" Width="85px" OnSelectedIndexChanged="ddltoyear_selectedindexchange"
                                        Style="margin-left: 10px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upgo" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btngo" runat="server" Text="Go" Width="46px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="color: Black; position: relative; margin-left: -289px"
                                                OnClick="btnGo_Click" />
                                            <asp:Button ID="btnaddnew" runat="server" Text="Add New" Width="99px" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style="color: Black; position: relative;"
                                                OnClick="btnaddnew_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="upgrd" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 40px;
                    width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="true" OnRowCreated="OnRowCreated1"
                    OnRowDataBound="gridview1_OnRowDataBound" OnDataBound="gridview1_DataBound" OnSelectedIndexChanged="SelectedIndexChanged1"  BackColor="AliceBlue">
                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="updatepanel9" runat="server">
            <ContentTemplate>
                <div id="divPopAlertbackvolume" runat="server" visible="false" style="height: 550em;
                    z-index: 100; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0%; font-family: Book Antiqua; font-weight: bold">
                    <center>
                        <div id="divPopAlertback" runat="server" class="table" style="background-color: White;
                            height: 467px; width: 39%; border: 5px solid #0CA6CA; margin-right: auto; margin-left: auto;
                            border-top: 25px solid #0CA6CA; left: 31%; right: 39%; top: 20%; padding: 5px;
                            z-index: 1000; position: fixed; border-radius: 10px; font-family: Book Antiqua;
                            font-weight: bold">
                            <center>
                                <asp:Label ID="lblnonbook" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                    position: relative;" Text="Department Information" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                            </center>
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px; margin-top: 10px; margin-left: 7px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldepart" runat="server" Text="Department Type" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <fieldset id="studdetail" runat="server" style="height: 18px; width: 178px;">
                                                <asp:RadioButtonList ID="rbacademic" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Enabled="true" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True">Academic</asp:ListItem>
                                                    <asp:ListItem>Other</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                         <asp:Label ID="lblimport" runat="server" Text="*" ForeColor="Red" Font-Bold="true" style="margin-left: -8px;"></asp:Label>
                                            <asp:Label ID="lbldepartname" runat="server" Text="Department Name" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="lbldept_code" runat="server" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtdepartname" runat="server" Style="height: 15px; width: 305px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblyrofintro" runat="server" Text="Year Of Introduction" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtyrofintro" runat="server" Style="height: 15px; width: 88px;" MaxLength="4"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtyrofintro"
                                                        FilterType="numbers" ></asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblacronym" runat="server" Text="Acronym" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtarconym" runat="server" Style="height: 15px; width: 157px;text-transform:uppercase;" CssClass="textbox txtheight2" MaxLength="10"></asp:TextBox>
                                            <asp:CheckBox ID="cbgroup" runat="server" Text="Group" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbldean" runat="server" Text="Dean Name" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Txtdean" runat="server" Style="height: 15px; width: 185px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:Button ID="btndean" runat="server" Text="?" Width="35px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="color: red; position: relative; background-color: lightsalmon;"
                                                OnClick="btndean_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblhodname" runat="server" Text="HOD Name" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txthod" runat="server" Style="height: 15px; width: 185px;" CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:Button ID="btnhod" runat="server" Text="?" Width="35px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="color: red; position: relative; background-color: lightsalmon;"
                                                OnClick="btnhod_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblcordinate" runat="server" Text="Coordinator Name" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtcordinator" runat="server" Style="height: 15px; width: 185px;"
                                                CssClass="textbox txtheight2"></asp:TextBox>
                                            <asp:Button ID="btncoordinator" runat="server" Text="?" Width="35px" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style="color: red; position: relative;
                                                background-color: lightsalmon;" OnClick="btncoordinator_click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2px">
                                            <asp:LinkButton ID="lnkroomalloc" ForeColor="Blue" Text="Room Allocation Settings"
                                                Font-Name="Book Antiqua" OnClick="lnkroomalloc_Click" runat="server" Style="margin-left: 171px"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblbuildacr" runat="server" Text="Building Acronym" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtbuilacr" runat="server" Style="height: 15px; width: 305px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblflooracr" runat="server" Text="Floor Acronym" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfloor" runat="server" Style="height: 15px; width: 305px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblroom" runat="server" Text="Room Acronym" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtroom" runat="server" Style="height: 15px; width: 305px;" CssClass="textbox txtheight2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2px" style="margin-left: 150px">
                                            <asp:UpdatePanel ID="upsave" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnsave" runat="server" Text="Save" Width="69px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Style="color: Black; position: relative;
                                                        margin-left: 159px; margin-top: 30px;" OnClick="btnsave_click" />
                                                    <asp:Button ID="btndelete" runat="server" Text="Delete" Width="69px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Style="color: Black; position: relative;
                                                        margin-left: -1px" OnClick="btndelete_click" />
                                                    <asp:Button ID="btnexit" runat="server" Text="Exit" Width="69px" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Style="color: Black; position: relative;
                                                        margin-left: 1px" OnClick="btnexit_click" /></ContentTemplate>
                                            </asp:UpdatePanel>
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
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="div4" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div5" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label3" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnaltok" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnaltok_Click"
                                                Text="Ok" runat="server" />
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
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="div1" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label2" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnyes" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnyes_Click"
                                                Text="Yes" runat="server" />
                                                 <asp:Button ID="btnno" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnno_Click"
                                                Text="No" runat="server" />
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
    <%-- Stafflookup--%>
    <center>
        <asp:UpdatePanel ID="uppanel" runat="server">
            <ContentTemplate>
                <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                    BorderWidth="2px" Style="left: 30%; top: 23%; right: 30%; position: absolute;"
                    Height="530px" Width="523px">
                    <asp:ImageButton ID="ImageButton5" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 30px; width: 30px; position: absolute; margin-top: -8px; margin-left: 237px;"
                        OnClick="imagebtnpopclose5_Click" />
                    <div class="PopupHeaderrstud2" id="Div16" style="text-align: center; font-family: Book Antiqua;
                        font-size: Small; font-weight: bold;">
                        <br />
                        <center>
                            <asp:Label ID="lblselstaf" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                position: relative;" Text="Select Staff" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                        </center>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege1" runat="server" Width="150px" OnSelectedIndexChanged="ddlcollege1_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="Labelstaffalert" runat="server" Text="No Record Found!" Font-Bold="true"
                            ForeColor="Red"></asp:Label>
                        <br />
                        <div id="divstaff" runat="server" style="overflow: auto; border: 1px solid Gray;
                            width: 439px; height: 280px; margin-left: 37px">
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="-1" />
                            <asp:GridView runat="server" ID="gviewstaff" AutoGenerateColumns="false" Style="height: 300;
                                width: 439px; overflow: auto;" OnRowCreated="OnRowCreated" OnSelectedIndexChanged="SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DisplayIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstaff" runat="server" Text='<%#Eval("Staff_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblname" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                <RowStyle ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                            </asp:GridView>
                        </div>
                        <br />
                        <center>
                            <div id="staffok" runat="server" visible="false">
                                <asp:Button ID="btn_staffok" runat="server" CssClass="textbox btn2" Text="Ok" OnClick="btn_staffok_Click" />
                                <asp:Button ID="btn_staffexit" runat="server" CssClass="textbox btn2" Text="Exit"
                                    OnClick="btn_staffexit_Click" />
                            </div>
                        </center>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upgo">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upsave">
            <ProgressTemplate>
                <div style="height: 40px; width: 150px;">
                    <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                    <br />
                    <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                        Processing Please Wait...</span>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>

    <center>
        <asp:UpdatePanel ID="updatepanel3" runat="server">
            <ContentTemplate>
                <div id="divroomalloc" runat="server" visible="false" style="height: 550em; z-index: 100;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%; font-family: Book Antiqua; font-weight: bold">
                    <center>
                        <div id="divroomalloc1" runat="server" class="table" style="background-color: White;
                            height: 87%; width: 61%; border: 5px solid #0CA6CA; margin-right: auto; margin-left: auto;
                            border-top: 25px solid #0CA6CA; left: 19%; right: 39%; top: 5%; padding: 5px;
                            z-index: 1000; position: fixed; border-radius: 10px; font-family: Book Antiqua;
                            font-weight: bold">
                            <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                                Style="height: 30px; width: 30px; position: absolute; margin-top: -31px; margin-left: 412px;"
                                OnClick="imagebtnpopclose1_Click" />
                            <center>
                                <asp:Label ID="Label1" runat="server" Style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                                    position: relative;" Text="Room Selection" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
                            </center>
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                        <div style=" height:300px; width:800px; overflow:auto;">
                                            <asp:HiddenField ID="HiddenField2" runat="server" Value="-1" />
                                            <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 2px;
                                                width: 500px;" Font-Names="Times New Roman" AutoGenerateColumns="true" OnRowCreated="OnRowCreated2"
                                                OnRowDataBound="gridview2_OnRowDataBound" OnDataBound="gridview2_DataBound" OnSelectedIndexChanged="SelectedIndexChanged_gridview2">
                                                <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                            </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td align="center">
                                    <asp:Label ID="lblerrormsg" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style=" height:300px; width:800px; overflow:auto;">
                                                <asp:GridView ID="GridView3" runat="server" Height="300px" Style="margin-bottom: 15px; margin-top: 2px;
                                                    width: 800px;" Font-Names="Times New Roman" AutoGenerateColumns="false" Visible="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No">
                                                            <ItemTemplate>
                                                                <%#Container.DisplayIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Room Acronymn">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblroomAcronymn" runat="server" Text='<%#Eval("roomAcronymn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Room Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblroomtype" runat="server" Text='<%#Eval("roomtype") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Building Acronymn">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblbuildacronym" runat="server" Text='<%#Eval("buildacronym") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Floor Acronymn">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblflooracronymn" runat="server" Text='<%#Eval("flooracronymn") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbselect" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td align="center">
                                    <asp:Button ID="btnoknew" runat="server" OnClick="btnoknew_OnClick" Text="Ok" Height="25px"  CssClass="textbox textbox1" Width="80px" />
                                     <asp:Button ID="btnclosenew" runat="server" OnClick="btnclosenew_OnClick" Text="Exit"  Height="25px"  CssClass="textbox textbox1" Width="80px" />
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
</asp:Content>
