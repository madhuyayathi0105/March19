<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Common_Subjectwise_Result.aspx.cs" Inherits="Common_Subjectwise_Result" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">

     function validation() {
         var error = "";

         var testname = document.getElementById('<%=txttest.ClientID %>').value;
         var subname = document.getElementById('<%=txtsubject.ClientID %>').value;

         if (testname == "---Select---") {
             error += "Please Select Test Name \n";
         }
         if (subname == "---Select---") {
             error += "Please Select Subject \n";
         }
         if (error.trim() == "") {
             return true;
         }
         else {
             alert(error);
             return false
         }

         var subname = document.getElementById('<%=txtsubject.ClientID %>').value;
         if (subname == "---Select---") {
             error += "Please Select Subject Name \n";
         }
         if (error.trim() == "") {
             return true;
         }
         else {
             alert(error);
             return false
         }

         var testname = document.getElementById('<%=txttest.ClientID %>').value;

         if (testname == "---Select---") {
             alert("Please Select Test Name");
             return false;
         }
         else {
             return true;
         }
     }

     function displayraj() {

         document.getElementById('MainContent_lblerrmsgxl').innerHTML = "";

     }

     function setMouseOverColor(id) {
         id.style.textDecoration = 'underline';
     }
    </script>
    <style type="text/css">
        .style1
        {
            height: 182px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:ScriptManager ID="Script1" runat="server">
    </asp:ScriptManager>
 <br />
    <center>
      <asp:Label ID="lbl" runat="server" Text="Common Subjectwise Result Analysis" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label> </center>
   <br /><center>
   <div style="width:800px; height:70px; background-color:#0CA6CA;"
>
        <table style="float: left;">
            <tr>
                <td>
                    <asp:Label ID="lblmonth" runat="server" Text="Exam Month" font-name="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlexm" runat="server" AutoPostBack="true" Width="70px" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlexm_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblyear" runat="server" Text="Year" font-name="Book Antiqua" Font-Size="Medium"
                        Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" Width="80px" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlyear_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Subjtype" runat="server" Text="Subject Type" font-name="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" Style="font-weight: 600;
                        " Height="16px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsubtype" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                        Font-Bold="true" Font-Size="Medium"
                        Height="25px" Width="90px" OnSelectedIndexChanged="ddlsubtype_OnSelectedIndexChanged">
                        <asp:ListItem>Common</asp:ListItem>
                        <asp:ListItem>General</asp:ListItem>
                    </asp:DropDownList>
                </td>
            
               <td>
                    <asp:Label ID="lblsubject" runat="server" Text="Subject Name" font-name="Book Antiqua"
                       Font-Size="Medium" Font-Bold="true" ></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtsubject" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="---Select---"
                               Width="152px"></asp:TextBox>
                            <asp:Panel ID="pnlsubject" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                Width="200px">
                                <asp:CheckBox ID="cbsubject" runat="server" Text="SelectAll" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="cbsubject_OnCheckedChanged" />
                                <asp:CheckBoxList ID="cblsubject" runat="server" Font-Size="Small" AutoPostBack="True"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Height="150px" OnSelectedIndexChanged="cblsubject_OnSelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubject"
                                PopupControlID="pnlsubject" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
       
        <table id="second" runat="server" style="float: left;">
            <tr>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" font-name="Book Antiqua" Font-Size="Medium"
                        Visible="false" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" Width="90px"
                        Visible="false" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                        Height="25px" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldept" runat="server" Text="Dept" font-name="Book Antiqua" Font-Size="Medium"
                        Visible="false" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldept" runat="server" AutoPostBack="true" Width="90px" Font-Names="Book Antiqua"
                        Visible="false" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddldept_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblTestname" runat="server" Text="Test Name " font-name="Book Antiqua"
                        Visible="false" Font-Size="Medium" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txttest" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                Visible="false" ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium"
                                Text="---Select---"></asp:TextBox>
                            <asp:Panel ID="pnltest" runat="server" CssClass="MultipleSelectionDDL" Height="200"
                                Visible="false" style="position:absolute;" Width="200px">
                                <asp:CheckBox ID="cbtest" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                    Visible="false" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnCheckedChanged="cbtest_OnCheckedChanged" />
                                <asp:CheckBoxList ID="cbltest" runat="server" Font-Size="Small" AutoPostBack="True"
                                    Visible="false" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                    Height="150px" OnSelectedIndexChanged="cbltest_OnSelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txttest"
                                PopupControlID="pnltest" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:RadioButtonList ID="Rbtn" runat="server" CellSpacing="0" AutoPostBack="true"
                        RepeatDirection="Horizontal" Font-Bold="true" Font-Size="Large"
                        font-name="Book Antiqua" OnSelectedIndexChanged="Rbtn_OnSelectedIndexChanged">
                        <asp:ListItem Value="1">CAM wise</asp:ListItem>
                        <asp:ListItem Value="2">University wise</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Width="50px" Height="30px"
                        OnClientClick="return validation()" OnClick="btngo_OnClick" />
                </td>
            </tr>
        </table>
        </div>
   </center>
    <br />
    <%------------------------------------->>>CommonGrid--%>
    <asp:Label ID="lblmsg" runat="server" Visible="false" Text="No Record Found" ForeColor="Red"
        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
    <asp:Label ID="Label2" runat="server" Visible="false" Text="" ForeColor="Red" Font-Bold="true"
        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
    <center>
        <asp:GridView ID="Commongrid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            CellPadding="4" HeaderStyle-BackColor="CadetBlue" HeaderStyle-Font-Size="Medium"
            CssClass="Dropdown_Txt_Box" Width="930px" ForeColor="#333333" OnRowDataBound="Commongrid_OnRowDataBound"
            OnRowCommand="Commongrid_OnRowCommand">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Size="Medium" Font-Bold="True" ForeColor="Control">
            </HeaderStyle>
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
        <br />
        <asp:Label ID="Label3" runat="server" Visible="false" Text="" ForeColor="Red" Font-Bold="true"
            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
    </center>
    <br />
    <%----------------------------------->>>CommonClickGrid--%>
    <center>
        <asp:Label ID="Label1" runat="server" Font-Size="Large" ForeColor="Brown" Font-Bold="True"
            Font-Names="Book Antiqua" Text="" Visible="true"></asp:Label>
        <br />
        <br />
        <asp:GridView ID="CommonClick" runat="server" CellPadding="4" CssClass="Dropdown_Txt_Box"
            HeaderStyle-Font-Size="Medium" OnRowDataBound="CommonClick_OnRowDataBound" Font-Bold="True"
            Font-Names="Book Antiqua" OnDataBound="CommonClick_OnDataBound" ForeColor="#333333"
            Width="600px">
            <%-- <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="serialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'>></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>--%>
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="Control" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
    </center>
    <br />
    <%------------------------------------->>>GeneralGrid--%>
    <center>
        <asp:GridView ID="GenaralGrid" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            AutoGenerateColumns="true" CellPadding="4" CssClass="Dropdown_Txt_Box" Width="800px"
            ForeColor="#333333" OnDataBound="GenaralGrid_OnDataBound" OnRowDataBound="GenaralGrid_OnRowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <%--<asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="serialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'>></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Select">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkboxSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkboxSelectAll_OnCheckedChanged" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="cbSelect" runat="server"></asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="Control" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F7F7F7" />
            <SortedAscendingHeaderStyle BackColor="#487575" />
            <SortedDescendingCellStyle BackColor="#E5E5E5" />
            <SortedDescendingHeaderStyle BackColor="#275353" />
            <SortedAscendingCellStyle BackColor="#F8FAFA"></SortedAscendingCellStyle>
            <SortedAscendingHeaderStyle BackColor="#246B61"></SortedAscendingHeaderStyle>
            <SortedDescendingCellStyle BackColor="#D4DFE1"></SortedDescendingCellStyle>
            <SortedDescendingHeaderStyle BackColor="#15524A"></SortedDescendingHeaderStyle>
        </asp:GridView>
        <br />
        <asp:Label ID="lblmsg2" runat="server" Visible="false" Text="No Records Found" ForeColor="Red"
            Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
        <br />
        <center>
            <asp:Chart ID="Genaralchart" runat="server" Width="700px" Height="500px" Visible="False"
                Enabled="False">
                <Series>
                    <asp:Series Name="Series1" XValueMember="SubName" YValueMembers="Pass%" IsValueShownAsLabel="true"
                        YValuesPerPoint="2" ChartArea="ChartArea1" ChartType="Column">
                    </asp:Series>
                </Series>
                <Titles>
                    <%-- <asp:Title Alignment="TopCenter" Text="External Wise Report" TextStyle="Shadow" Font="Trebuchet MS, 20.25pt"></asp:Title>--%>
                    <asp:Title Docking="Bottom" Text="SUBJECT NAME" Font="15.15pt">
                    </asp:Title>
                    <asp:Title Docking="Left" Text="                                               PASS %"
                        Font="15.15pt">
                    </asp:Title>
                </Titles>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                        <AxisY LineColor="Black">
                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                            <MajorGrid LineColor="#e6e6e6" />
                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                        </AxisY>
                        <AxisX LineColor="White">
                            <LabelStyle Font="Trebuchet MS, 8.25pt" />
                            <MajorGrid LineColor="#e6e6e6" />
                            <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                        </AxisX>
                    </asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </center>
        <%------------------------------------->>>GeneralReportGrid--%>
        <center>
            <asp:GridView ID="Generalreportgrid" runat="server" CellPadding="4" Font-Bold="True"
                Font-Names="Book Antiqua" ForeColor="#333333" OnRowDataBound="Generalreportgrid_RowDataBound"
                Width="600px" CssClass="Dropdown_Txt_Box" HeaderStyle-Font-Size="Medium">
                <%--<Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <center>
                                <asp:Label ID="serialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'>></asp:Label>
                            </center>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>--%>
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="Control" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            <%--<FarPoint:FpSpread ID="Fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                    Visible="false" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                    CssClass="font14">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>--%>
        </center>
        <%--<table>
            &nbsp; &nbsp; &nbsp; &nbsp;
            <tr>
                <td>--%>
    </center>
    <center>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="BtnReport" runat="server" Text="Report" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnClick="BtnReport_OnClick" Visible="false" Style="width: 90px;
                            height: 30px" />
                        <asp:Button ID="Excel" runat="server" Text="Export Excel" Visible="false" Font-Size="Medium"
                            Font-Bold="true" Font-Names="Book Antiqua" OnClick="Excel_OnClick" />
                        <asp:Button ID="Print" runat="server" Text="Print" Visible="false" Font-Size="Medium"
                            Font-Bold="true" Font-Names="Book Antiqua" OnClick="Print_OnClick" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <br />
    <center>
        <FarPoint:FpSpread ID="Fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
            Visible="false" BorderWidth="1px" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
            CssClass="font14">
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
        <br />
    </center>
    <asp:Label ID="lblerrmsgxl" runat="server" Visible="false" Width="385px" Height="20px"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="" ForeColor="Red"></asp:Label>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblexportxl" runat="server" Visible="false" Width="100px" Height="20px"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Report Name"
                    ForeColor="Black"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Height="20px" Width="180px"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" onkeypress="displayraj()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                    InvalidChars="/\">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="g1btnexcel" runat="server" OnClick="g1btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                    Style="margin-left: 6px;" />
                <asp:Button ID="g1btnprint" runat="server" OnClick="g1btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>

