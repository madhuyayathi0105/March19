<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="ConsolidateReport.aspx.cs" Inherits="ConsolidateReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .gvRow
        {
            margin-right: 0px;
            margin-top: 325px;
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
            padding: 3px;
            border: 1px solid black;
            background-color: #CFECEC;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
  <br />  <center>
        <asp:Label ID="lbl" runat="server" Text="Subjectwise Mark & Grade Report" Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>  
    </center>
    <br />
     <center>
            <table style="width:1000px; height:70px; background-color:#0CA6CA;">
                <tr>
                    <td>
                        <asp:Label ID="lblschool" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="School" ></asp:Label>
                            </td>
                            <td>
                        <asp:DropDownList ID="ddschool" runat="server" Width="266px" Height="25px" Font-Bold="True"
                            Font-Names="Book Antiqua"  Font-Size="Medium" OnSelectedIndexChanged="ddschool_OnSelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblyear" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua"  Font-Size="Medium" Text="Year"></asp:Label>
                            </td>
                            <td>
                        <asp:DropDownList ID="dropyear" runat="server" Width="59px" Height="25px" Font-Bold="True"
                            OnSelectedIndexChanged="dropyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                             Font-Size="Medium" AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblschooltype" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="School Type"></asp:Label>
                            </td>
                            <td>
                        <asp:DropDownList ID="ddschooltype" runat="server" Width="80px" Height="25px" AutoPostBack="true"
                            OnSelectedIndexChanged="dropschooltype_SelectedIndexChanged" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblstandard" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Standard"> </asp:Label>
                            </td>
                            <td>
                        <asp:DropDownList ID="ddstandard" runat="server" Width="110px" Height="25px" AutoPostBack="true"
                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddstandard_SelectedIndexChanged" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                   
                            </tr>
                            <tr>
                             <td>
                        <asp:Label ID="lblterm" runat="server" Font-Color="Black" Font-Bold="True" Font-Names="Book Antiqua" 
                            Font-Size="Medium" Text="Term" ></asp:Label>
                            </td>
                      <td>  <asp:DropDownList ID="dropterm" runat="server" Width="35px" Height="25px" Font-Bold="True"
                            Font-Names="Book Antiqua"  Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropterm_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Iblsec" runat="server"  Font-Size="Medium" Font-Names="Book Antiqua"  Font-Bold="true" Text="Sec"></asp:Label>
                        </td>
                       <td>
                        <asp:DropDownList ID="dropsec" runat="server" Width="44px" Height="25px"  Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblreportdisplay" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Report Display"></asp:Label> </td>
                       <td>
                        <asp:DropDownList ID="dropreportdisplay" runat="server" OnSelectedIndexChanged="dropreportdisplay_SelectedIndexChanged" Width="70px" Height="25px" Font-Bold="True" Font-Names="Book Antiqua"  Font-Size="Medium" AutoPostBack="true">
                            <asp:ListItem Value="0" Selected="True">Mark</asp:ListItem>
                            <asp:ListItem Value="1" Selected="False">Grade</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubname" runat="server" Font-Bold="True"
                            Font-Names="Book Antiqua"  Font-Size="Medium" Text="Subject Name"></asp:Label></td>
                     <td>
                        <asp:DropDownList ID="dropsubname" runat="server" OnSelectedIndexChanged="dropsubname_SelectedIndexChanged" Width="178px" Height="25px" Font-Bold="True" Font-Names="Book Antiqua"  Font-Size="Medium" AutoPostBack="true">
                        </asp:DropDownList>
                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txttestname" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="97px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style="position: absolute;
                                    left: 680px; top: 260px;" Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="testnamepanel" runat="server" CssClass="MultipleSelectionDDL" Width="188px"
                                    Height="150px">
                                    <asp:CheckBox ID="chcktestname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        OnCheckedChanged="checktestname_CheckedChanged" Font-Size="Medium" Text="Select All"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="chcklisttestname" runat="server" Font-Size="Medium" Font-Bold="True"
                                        OnSelectedIndexChanged="cheklisttestname_SelectedIndexChanged" Font-Names="Book Antiqua"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txttestname"
                                    PopupControlID="testnamepanel" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
               
           <td>
                <asp:Button ID="btngo" runat="server" Width="42px" Height="27px" Font-Bold="True"
                    Font-Names="Book Antiqua"  Font-Size="Medium" ForeColor="Black"
                    Text="Go" OnClick="btngo_Click" /></td> </tr>
            </table>
          
        </center>
 <br />

    <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="position: absolute;
        left: 15px; top: 277px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
        Visible="true" ForeColor="#FF3300"></asp:Label>
    <%--  <div>
    <asp:Label ID="lblcalc" runat="server" Text="Calculation Max Mark" Width="500px" Style="position: absolute;
        left: 29px; top: 293px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
        Visible="true" ForeColor="Black"></asp:Label>
    <asp:TextBox ID="txtcalc" runat="server" BorderColor="Teal" Width="30px" Style="position: absolute;
        left: 201px; top: 290px; height:18px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
        Visible="true" onkeypress="return isNumberKey(event)" MaxLength="3"></asp:TextBox>
    </div>--%>
    <br />
    <%-- <asp:GridView ID="reportgrid1" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
        HeaderStyle-BackColor="Teal" AlternatingRowStyle-CssClass="gvAltRow" HeaderStyle-CssClass="gvHeader"
        OnRowDataBound="reportgrid1_RowDataBound" OnDataBound="reportgrid1_DataBound"
        Style="margin-top: -4px; width: 1560px; margin-left: -27px;">
    </asp:GridView>--%>
    <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="900px" ActiveSheetViewIndex="0"
        currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
        EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
        <CommandBar BackColor="Control" ButtonType="PushButton">
            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
        </CommandBar>
        <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
            Font-Underline="False" />
        <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
            Font-Underline="False" />
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                SelectionForeColor="Black">
            </FarPoint:SheetView>
        </Sheets>
        <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
            VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
            Font-Strikeout="False" Font-Underline="False">
        </TitleInfo>
    </FarPoint:FpSpread>
    <table>
        <tr>
            <td>
                <asp:Label ID="lblexportxl" runat="server" Visible="false" Width="95px" Height="20px"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Export Excel"
                    ForeColor="Black"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Visible="false" Height="20px" Width="180px"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                    InvalidChars="/\">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="g1btnexcel" runat="server" OnClick="g1btnexcel_OnClick" Visible="false"
                    Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                    Style="margin-left: 6px;" />
            </td>
            <td>
                <asp:Button ID="g1btnprint" runat="server" OnClick="g1btnprint_OnClick" Visible="false"
                    Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>

