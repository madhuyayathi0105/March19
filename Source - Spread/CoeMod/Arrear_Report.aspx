<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Arrear_Report.aspx.cs" Inherits="Arrear_Report"
    EnableEventValidation="false" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <head>
        <title></title>
        <style type="text/css">
            .gvRow
            {
                margin-right: 0px;
                margin-top: 330px;
            }
            
            .gvHeader th
            {
                padding: 3px;
                background-color: #008080;
                
                border: 1px solid black;
                font-family: Book Antiqua;
                font-size: medium;
                margin-left: 0px;
            }
            .gvRow td
            {
                background-color: #F0FFFF;
                font-family: Book Antiqua;
                font-size: medium;
                padding: 3px;
                border: 1px solid black;
            }
            .gvAltRow td
            {
                font-family: Book Antiqua;
                font-size: medium;
                border: 1px solid black;
                background-color: #CFECEC;
            }
            
            .top
            {
                font-size: medium;
                font-family: Book Antiqua;
                color: black;
            }
            .head
            {
                
                font-family: Book Antiqua;
                font-size: medium;
                
                top: 80px;
                position: absolute;
                font-weight: bold;
                width: 950px;
                height: 25px;
                left: 15px;
            }
            .mainbatch
            {
                background-color: #0CA6CA;;
                width: 950px;
                position: absolute;
                height: 130px;
                top: 120px;
                left: 15px;
                font-family: Book Antiqua;
                font-size: medium;
                font-weight: bold;
                color: black;
            }
        </style>
        <script type="text/javascript">

            function checkvalidate() {

                var checkvalidation = document.getElementById('<%=txt_batch.ClientID%>').value;
                var checkvalidation1 = document.getElementById('<%=txt_degree.ClientID%>').value;
                var checkvalidation2 = document.getElementById('<%=txt_branch.ClientID%>').value;

                if (checkvalidation == "--Select--") {
                    alert("Please Select Batch");
                    return false;
                }
                else if (checkvalidation1 == "--Select--") {

                    alert("Please Select Degree");
                    return false;
                }
                else if (checkvalidation2 == "--Select--") {

                    alert("Please Select Department");
                    return false;
                }
                else {
                    return true;
                }
            }
            function ChangeRowColor(row) {
                if (previousRow == row)
                    return;

                else if (previousRow != null)
                    var color = row.style.backgroundColor;

                if (previousRow != null) {

                    alert(color)

                    if (color == "bisque") {
                        previousRow.style.backgroundColor = "white";
                    }
                    else if (color == "white") {
                        previousRow.style.backgroundColor = "bisque";
                    }
                }

                row.style.backgroundColor = "#ffffda";
                previousRow = row;
            }
        </script>
    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <br />
            <asp:Label ID="lbl_head" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                ForeColor="Green" Font-Size="Large" Text="Arrear Report" /></center>
        <br />
        <center>
            <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="Iblcollege" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" runat="server">  </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropcollege" Width="160px" Height="25px" Font-Names="Book Antiqua"
                            Font-Bold="true" AutoPostBack="true" Font-Size="Medium" runat="server" OnSelectedIndexChanged="dropcollege_SelectedIndexChanged">
                            <asp:ListItem>--Select--</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Iblbatch" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updpan_batch" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                    Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="pbatch" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" Height="200" Width="175" ScrollBars="Auto"
                                    Style="">
                                    <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                                    <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                                    PopupControlID="pbatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Degree"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updpan_degree" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                    Font-Bold="true" runat="server" ReadOnly="true" Width="126px">--Select--</asp:TextBox>
                                <asp:Panel ID="pdegree" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" Height="200" Width="175" ScrollBars="Auto"
                                    Style="">
                                    <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                                        Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                                        AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                                    PopupControlID="pdegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <%--  <Triggers>
                                <asp:PostBackTrigger ControlID="Chklst_degree" />
                            </Triggers>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chk_degree" />
                            </Triggers>--%>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Department"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updpan_branch" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                    runat="server" ReadOnly="true" Width="125px">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel3" runat="server" CssClass="MultipleSelectionDDL" BackColor="White"
                                    BorderColor="Black" BorderStyle="Solid" Height="200" Width="175" ScrollBars="Auto"
                                    Style="">
                                    <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chk_branchchanged"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklst_branch" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                        runat="server" OnSelectedIndexChanged="chklst_branchselected" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_branch"
                                    PopupControlID="Panel3" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                            <%--       <Triggers>
                                <asp:PostBackTrigger ControlID="chklst_branch" />
                            </Triggers>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="chk_branch" />
                            </Triggers>--%>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="IblSem" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Sem"> </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropsem" Width="100px" AutoPostBack="true" Height="25px" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" runat="server" OnSelectedIndexChanged="dropsem_selected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Iblsec" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Sec"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropsec" runat="server" AutoPostBack="true" Width="105px" Height="25px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" OnSelectedIndexChanged="dropsec_selected">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="IblReport" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                            Font-Size="Medium" Text="Report Type"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="dropReport" Width="130px" AutoPostBack="true" Font-Names="Book Antiqua"
                            OnSelectedIndexChanged="dropReport_selected" Font-Bold="true" Font-Size="Medium"
                            runat="server">
                            <asp:ListItem>General</asp:ListItem>
                            <asp:ListItem>Lateral Entry</asp:ListItem>
                            <asp:ListItem>Hostel Students</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="button" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                            OnClick="buttongo" OnClientClick=" return checkvalidate()" />
                        <asp:Label ID="lblError" runat="server" Font-Size="Large" ForeColor="Red" Style="position: absolute;
                            left: -10px; top: 327px;" Font-Bold="True" Width="200px" Font-Names="Book Antiqua"
                            Text="" Visible="true"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButton ID="radiooverall" runat="server" S Visible="true" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" Text="Overall"
                            AutoPostBack="True" Width="100px" OnCheckedChanged="radio1_checked" />
                    </td>
                    <td>
                        <asp:RadioButton ID="radiostudent" runat="server" Font-Bold="True" Visible="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GroupName="format" Text="Studentwise"
                            AutoPostBack="True" OnCheckedChanged="radio2_Checked" Width="150px" />
                    </td>
                </tr>
            </table>
        </center>
        <asp:GridView ID="gridviewreport" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
            runat="server" Visible="false" Width="950px" GridLines="Both" HeaderStyle-CssClass="gvHeader"
            CssClass="gvRow" OnRowCommand="gridview_onselectedchanged" AlternatingRowStyle-CssClass="gvAltRow"
            AutoGenerateColumns="false" OnDataBound="bindbound" OnRowDataBound="databoud"
            Style="margin-left: -9px; margin: right:200px; margin-top: 150px;">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <asp:Label ID="Iblserial" runat="server" Text='<%#Eval("sno")%>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Font-Size="Medium"></ControlStyle>
                    <HeaderStyle HorizontalAlign="center" Font-Bold="True" Font-Size="Medium" />
                    <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Batch" SortExpression="Batch">
                    <ItemTemplate>
                        <asp:Label ID="lblbatch" Width="80px" runat="server" Text='<%#Eval("batch_Year")%>' />
                    </ItemTemplate>
                    <ControlStyle Font-Size="Medium"></ControlStyle>
                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="80px">
                    </HeaderStyle>
                    <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Degree/Dept">
                    <ItemTemplate>
                        <asp:Label ID="lblacronym" Width="100px" runat="server" Text='<%#Eval("Department")%>' />
                        <asp:Label ID="quotaid" runat="server" Visible="false" Text='<%#Eval("degreecode")%>' />
                    </ItemTemplate>
                    <ControlStyle Font-Size="Medium"></ControlStyle>
                    <HeaderStyle HorizontalAlign="center" Font-Bold="True" Font-Size="Medium" Width="80px">
                    </HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Quota" HeaderStyle-Width="80px" ItemStyle-Width="100px"
                    ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium"
                    SortExpression="Quota">
                    <ItemTemplate>
                        <asp:Label ID="lbltextval" Width="157px" runat="server" Text='<%#Eval("Quota")%>' />
                        <asp:Label ID="quotaid12" Visible="false" runat="server" Text='<%#Eval("textcode")%>'></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Font-Size="Medium"></ControlStyle>
                    <HeaderStyle HorizontalAlign="Center" Font-Size="Medium" Width="80px"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left" Width="80px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="All Clear" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="Iblallclear" Font-Underline="true" runat="server" Text='<%#Eval("All Clear")%>'></asp:Label>
                        <asp:Label ID="rollpass" runat="server" Visible="false" Text='<%#Eval("rollpass")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="One Arrear" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="Iblonearrear" Font-Underline="true" runat="server" Text='<%#Eval("One Arrear")%>'></asp:Label>
                        <asp:Label ID="name" runat="server" Visible="false" Text='<%#Eval("roll_no")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Two Arrears" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="Ibltwoarrear" Font-Underline="true" runat="server" Text='<%#Eval("Two Arrear")%>'></asp:Label>
                        <asp:Label ID="name1" runat="server" Visible="false" Text='<%#Eval("roll_no1")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="3 & Above Arrears" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="Iblthreearrear" Font-Underline="true" runat="server" Text='<%#Eval("3 & Above Arrear")%>'></asp:Label>
                        <asp:Label ID="name2" runat="server" Visible="false" Text='<%#Eval("roll_no2")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pass Percentage(%)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="percentage" runat="server" Text='<%#Eval("Pass Percentage")%>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="Ibl" Text="Total" Visible="true" runat="server"></asp:Label></FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button ID="Excel" Text="Export Excel" runat="server" Width="120px" Height="35px"
            Style="position: absolute; right: 145px;" Font-Names="Book Antiqua" Font-Bold="true"
            ForeColor="Black" Font-Size="Medium" Visible="false" OnClick="Exportexcel_click" />
        <asp:Button ID="print" Text="Print" runat="server" Visible="false" Width="100px"
            Style="position: absolute; right: 40px;" Font-Bold="true" Font-Names="Book Antiqua"
            ForeColor="Black" Font-Size="Medium" Height="35px" OnClick="btnPrint_Click" />
        </div>
        <br />
        <br />
        <br />
        <br />
        <div style="margin-left: 0px; margin: right:200px; margin-top: 50px;">
            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                Visible="false" BorderWidth="0.5" autopostback="true" Height="200" Width="950px">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <div>
            <asp:Label ID="Label2" runat="server" Font-Size="Medium" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Text="" Visible="false"></asp:Label>
            <asp:Label ID="Label1" runat="server" Font-Size="Large" ForeColor="Brown" Font-Bold="True"
                Font-Names="Book Antiqua" Text="" Visible="true"></asp:Label>
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                Visible="false" BorderWidth="0.5" autopostback="true">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <div>
            <asp:Button ID="btnexcel" Text="Export Excel" runat="server" Width="120px" Height="35px"
                Style="position: absolute; right: 145px;" Font-Names="Book Antiqua" Font-Bold="true"
                ForeColor="Black" Font-Size="Medium" Visible="false" OnClick="buttonexcel" />
            <asp:Button ID="btnprint" Text="Print" runat="server" Visible="false" Width="100px"
                Style="position: absolute; right: 40px;" Font-Bold="true" Font-Names="Book Antiqua"
                ForeColor="Black" Font-Size="Medium" Height="35px" OnClick="Buttonprint" />
            <br />
            <asp:Button ID="excelspread" Text="Export Excel" runat="server" Width="120px" Height="35px"
                Style="position: absolute; right: 140px;" Font-Names="Book Antiqua" Font-Bold="true"
                ForeColor="Black" Font-Size="Medium" Visible="false" OnClick="spread_excel" />
            <asp:Button ID="pdf" Text="Print" runat="server" Width="120px" Height="35px" Style="position: absolute;
                right: 10px;" Font-Names="Book Antiqua" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                Visible="false" OnClick="Buttonprint1" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </div>
        </div>
    </body>
    </html>
</asp:Content>
