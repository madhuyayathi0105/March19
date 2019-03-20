<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master" AutoEventWireup="true" CodeFile="subjectwise_report.aspx.cs" Inherits="subjectwise_report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">

     function display() {
         document.getElementById('MainContent_lblerror').innerHTML = "";
     }

     function validation() {

         var term = document.getElementById('<%=txtterm.ClientID %>').value;

         if (term == "-Select-") {
             alert("Please Select Term");
             return false;
         }
         else {
             return true;
         }
     }
    </script>
    <style type="text/css">
        .vertical-text
        {
            transform: rotate(90deg);
            transform-origin: left top 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center style="margin-top:-110px; ">
                <div style="width: 1016px; height: 26px;  margin: 159px auto 159px -28px;
                    padding-left: auto; padding-right: auto; background-color: Teal; text-align: right;  margin-left:5px; ">
                    <center>
                       
                        <asp:Label ID="lbl" runat="server" Text="Subjectwise Report" Font-Bold="true" Font-Names="Bood Antiqua"
                            Font-Size="Large" ForeColor="Azure"></asp:Label>
                        
                       <%-- <asp:LinkButton ID="back" runat="server" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 70px; position: absolute;" PostBackUrl="~/Default_login.aspx" ForeColor="White">Back</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="home" runat="server" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 99px; position: absolute;" PostBackUrl="~/Default_login.aspx" ForeColor="White">Home</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="log" runat="server" OnClick="go_Click" Font-Bold="true" Style="margin-top: 4px;
                            margin-left: 136px; position: absolute;" ForeColor="White">Logout</asp:LinkButton>--%>
                    </center>
                </div>
            </center>
            <div style="width: 996px; height: 65px; background-color: -webkit-border-radius: 10px;
                -moz-border-radius: 10px; padding: 10px; padding-left: auto; padding-right: auto;
                margin: -159px  auto -159px -28px; margin-left:5px; background-color: #219DA5;">
                <center>
                    <asp:UpdatePanel ID="upd" runat="server">
                        <ContentTemplate>
                            <table style="margin-left: -101px; margin-top: -220px; position: absolute; height: 50px;
                                width: 600px; margin-bottom: 0px; line-height: 27px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblschool" runat="server" Width="46px" Height="20px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="position: absolute; left: 99px; top: 225px;"
                                            Font-Size="Medium" Text="School" ForeColor="#ffffff"></asp:Label>
                                        <asp:DropDownList ID="ddschool" runat="server" Width="185px" Height="25px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="position: absolute; left: 158px; top: 225px;"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddschool_OnSelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblyear" runat="server" Width="50px" Height="20px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="position: absolute; left: 351px; top: 225px;"
                                            Font-Size="Medium" Text="Year" ForeColor="#ffffff"></asp:Label>
                                        <asp:DropDownList ID="dropyear" runat="server" Width="59px" Height="25px" Font-Bold="True"
                                            OnSelectedIndexChanged="dropyear_SelectedIndexChanged" Font-Names="Book Antiqua"
                                            Style="position: absolute; left: 393px; top: 225px;" Font-Size="Medium" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblschooltype" runat="server" Width="125px" Height="20px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="position: absolute; left: 460px; top: 225px;"
                                            Font-Size="Medium" Text="School Type" ForeColor="#ffffff"></asp:Label>
                                        <asp:DropDownList ID="ddschooltype" runat="server" Width="80px" Height="25px" AutoPostBack="true"
                                            OnSelectedIndexChanged="dropschooltype_SelectedIndexChanged" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="position: absolute; left: 559px; top: 225px;"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstandard" runat="server" Width="37px" Height="20px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Style="position: absolute; left: 648px; top: 225px;"
                                            Font-Size="Medium" Text="Standard" ForeColor="#ffffff"></asp:Label>
                                        <asp:DropDownList ID="ddstandard" runat="server" Width="110px" Height="25px" AutoPostBack="true"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddstandard_SelectedIndexChanged"
                                            Style="position: absolute; left: 725px; top: 225px;" Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblterm" runat="server" Font-Color="white" Width="100px" Height="20px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="position: absolute; left: 845px;
                                            top: 225px;" Font-Size="Medium" Text="Term" ForeColor="#ffffff"></asp:Label>
                                        <%-- <asp:DropDownList ID="dropterm" runat="server" Width="55px" Height="25px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Style="position: absolute; left: 892px; top: 225px;"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="dropterm_OnSelectedIndexChanged">
                                </asp:DropDownList>--%>
                                        <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>--%>
                                        <asp:TextBox ID="txtterm" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                            Visible="true" ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="-Select-"
                                            Width="80px" Style="position: absolute; left: 892px; top: 225px;"></asp:TextBox>
                                        <asp:Panel ID="pnlterm" runat="server" CssClass="MultipleSelectionDDL" Height="117"
                                            Style="position: absolute;" Width="105px">
                                            <asp:CheckBox ID="cbterm" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbterm_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblterm" runat="server" Font-Size="Small" AutoPostBack="True"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblterm_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <br />
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtterm"
                                            PopupControlID="pnlterm" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="Iblsec" runat="server" Style="position: absolute; left: 980px; width: 40px;
                                            top: 225px; color: white;" Font-Size="Medium" Font-Bold="true" Text="Sec"></asp:Label>
                                        <%-- <asp:DropDownList ID="dropsec" runat="server" Width="44px" Height="25px" Style="position: absolute;
                                    left: 1013px; top: 225px;" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="dropsec_OnSelectedIndexChanged"
                                    Font-Bold="true" Font-Size="Medium">
                                </asp:DropDownList>--%>
                                        <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>--%>
                                        <asp:TextBox ID="txtsec" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="True"
                                            ReadOnly="true" Font-Names="Book Antiqua" Font-Size="Medium" Text="-Select-"
                                            Style="position: absolute; left: 1006px; top: 225px;" Width="94px"></asp:TextBox>
                                        <asp:Panel ID="pnlsec" runat="server" CssClass="MultipleSelectionDDL" BackColor="White" BorderColor="Black" 
BorderStyle="Solid" Height="200"
                    Width="175" 
                                    ScrollBars="Auto" Style="">
                                            <asp:CheckBox ID="cbsec" runat="server" Text="SelectAll" AutoPostBack="True" Font-Bold="True"
                                                ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbsec_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblsec" runat="server" Font-Size="Small" AutoPostBack="True"
                                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblsec_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <br />
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsec"
                                            PopupControlID="pnlsec" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Font-Color="white" Width="100px" Height="20px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="position: absolute; left: 99px;
                                            top: 258px;" Font-Size="Medium" Text="Subject" ForeColor="#ffffff"></asp:Label>
                                        <asp:DropDownList ID="ddlsubject" runat="server" OnSelectedIndexChanged="ddlsubject_OnSelectedIndexChanged"
                                            Width="185px" Height="25px" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                                            Style="position: absolute; left: 159px; top: 260px;" Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btngo" runat="server" Style="background-color: silver; border: 2px solid white;
                                            color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                            height: 27px; margin-left: 290px; margin-top: 212px; position: absolute; width: 42px;"
                                            Text="Go" OnClick="btngo_Click" OnClientClick="return validation()" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="dropyear" />
                        </Triggers>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ddschooltype" />
                        </Triggers>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ddstandard" />
                        </Triggers>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="ddlsubject" />
                        </Triggers>
                    </asp:UpdatePanel>
                </center>
            </div>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
            <center>
                <table>
                    <tr>
                        <td>
                            &nbsp;
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                                CssClass="stylefp">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                </table>
            </center>
            <table>
                <%--<div>
                <asp:Label ID="lblerrormsg" runat="server" Text="" Width="302px" Style="margin-left: -22px;
                    position: absolute; margin-top: 159px;" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
            </div>
            <br />
            <br />
            <div style="margin-top: 140px; margin-left: -19px;">
                <center>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="true" VerticalScrollBarPolicy="Always" HorizontalScrollBarPolicy="Always">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
            </div>--%>
                <table id="prntrpt" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblerror" runat="server" Text="" Width="250px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="true" ForeColor="#FF3300"></asp:Label>
                            <br />
                            <asp:Label ID="lblexportxl" runat="server" Visible="false" Width="100px" Height="20px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Report Name"
                                ForeColor="Black"></asp:Label>
                            <asp:TextBox ID="txtexcell" runat="server" Visible="false" Height="20px" Width="180px"
                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcell"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_OnClick" Visible="false"
                                Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Style="margin-left: 6px;" />
                            <asp:Button ID="btnprint" runat="server" OnClick="btnprint_OnClick" Visible="false"
                                Text="Print" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

