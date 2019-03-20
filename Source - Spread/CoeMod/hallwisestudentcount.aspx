<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="hallwisestudentcount.aspx.cs" Inherits="hallwisestudentcount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblmessage1').innerHTML = "";
        }

        function buttoncheck() {
            var date = document.getElementById('<%=ddlDate.ClientID%>').value;
            var hall = document.getElementById('<%=ddlhall.ClientID%>').value;
            if (date == "All") {
                alert("Please Select Date");
                return false;
            }
            if (hall == "") {
                alert("Please Select Hall");
                return false;
            }
            else {
                return true;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <br />
    <center>
        <asp:Label ID="Label1" runat="server" Text="Hallwise Student Strength" Font-Names="Book Antiqua" Font-Size="Large" Font-Bold="true"
            ForeColor="green"></asp:Label></center>
 <br />
 <center>
        <table style="width:700px; height:50px; background-color:#0CA6CA;">
        <tr>
        <td>
            <asp:Label ID="Label20" runat="server"
                Width="127px" Text="Year and Month" CssClass="fontbold"></asp:Label>
                </td>
                <td>
            <asp:DropDownList ID="ddlYear" 
                runat="server" CssClass="fontbold" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
            </asp:DropDownList>
            </td>
            <td>
            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="fontbold" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
            </asp:DropDownList>
            </td>
            <td>
            <asp:Label ID="Label2" runat="server" Text="Date"  CssClass="fontbold"></asp:Label>
                </td>
                <td>
            <asp:DropDownList ID="ddlDate" runat="server"  CssClass="fontbold" Width="101px" AutoPostBack="True" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
            </asp:DropDownList>
            </td>
            <td>
            <asp:Label ID="Label3" runat="server" Text="Session" CssClass="fontbold" ></asp:Label>
                </td>
                <td>
            <asp:DropDownList ID="ddlSession" runat="server" CssClass="fontbold" Width="68px" AutoPostBack="True" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
            </asp:DropDownList>

</td>
            <td>
            <asp:Label ID="Label5" runat="server" Text="Type"  CssClass="fontbold"></asp:Label>
                </td>
                <td>
            <asp:DropDownList ID="ddltype" runat="server" CssClass="fontbold"  Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddltype_SelectedIndexChanged">
            </asp:DropDownList>
            </td>
            <td>
            <asp:Label ID="Label4" runat="server" Text="Hall No" S Width="59px" CssClass="fontbold"></asp:Label>
            </td>
            <td>
            <asp:DropDownList ID="ddlhall" runat="server" Enabled="false"  CssClass="fontbold" Width="90px" AutoPostBack="True" OnSelectedIndexChanged="ddlhall_SelectedIndexChanged">
            </asp:DropDownList>
            </td>
            <td>
            <asp:CheckBox ID="chkconsolidate" runat="server" Text="Consolidate" OnCheckedChanged="chkconsolidate_checkedchange"
                AutoPostBack="true" Width="120px" 
                CssClass="fontbold" />
                </td>
                <td>
            <asp:Button ID="btngo" runat="server" Text="GO"  CssClass="fontbold" OnClick="btngo_Click" />
                </td>
                </tr>
                </table>
                <table style="width:900px; height:50px; background-color:#0CA6CA;">
                <tr>
                <td>
            <asp:Label ID="Label6" runat="server" Text="SMS Content"  Width="100px" CssClass="fontbold"></asp:Label>
                </td>
                <td>
            <asp:TextBox ID="txtsms" runat="server" CssClass="fontbold"  Width="799px" Height="42px" MaxLength="150" TextMode="MultiLine"
                placeholder="Roll No=$ROLLNO$, Regno=$REGNO$, Name=$NAME$, Subject Name=$SUBJECT$,SUBJECT CODE=$SCODE$,DATE=$DATE$, SESSION=$SESSION$, Room No=$ROOM$, SEAT NO=$SEAT$"></asp:TextBox>
                </td>
                <td>

            <asp:Button ID="btnsms" runat="server" Text="Send" OnClick="btnsms_click" CssClass="fontbold"
               /></td>
       </tr>
       </table>
   
   </center>
    <br />
    <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" CssClass="fontbold"
        Style="top: 311px; left: 24px; position: absolute;" Visible="false"></asp:Label>
    <br />
    
    <FarPoint:FpSpread ID="Fpseating" Visible="false" runat="server" autopostback="true"
        Style="height: auto; width: auto" Width="980px">
        <Sheets>
            <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" GridLineColor="Gray">
            </FarPoint:SheetView>
        </Sheets>
    </FarPoint:FpSpread>
    <br />
    <asp:Label ID="lblmessage1" runat="server" CssClass="fontbold" ForeColor="#FF3300"
        Text="" Visible="False">
    </asp:Label>
    <br />
    <asp:Label ID="lblexcsea" runat="server" Visible="false" Text="Report Name" CssClass="fontbold"></asp:Label>
    <asp:TextBox ID="txtexseat" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
        CssClass="fontbold" Visible="false" onkeypress="display()"></asp:TextBox>
    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexseat"
        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
    </asp:FilteredTextBoxExtender>
    <asp:Button ID="Excel_seating" runat="server" Visible="false" Text="Export Excel"
        CssClass="fontbold" OnClick="Excelseating_click" />
    <asp:Button ID="Print_seating" runat="server" CssClass="fontbold" Visible="false"
        Text="Print" OnClick="printseating_click" />
    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    <br />
    <br />
</asp:Content>

