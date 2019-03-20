<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Passing_Board_Report.aspx.cs" Inherits="Passing_Board_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="Script1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblmsg').innerHTML = "";

        }
        </script>
    
    
    <center>
    <br />
        <div style="width: 960px; height: 30px; margin: 0 auto;  text-align: right;">
            <center>
                           
              <asp:Label ID="lbl" runat="server" Text="Passing Board Report " Font-Bold="true"
                    Font-Names="Bood Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
                           
                           
               <%-- <asp:LinkButton ID="home" runat="server" Font-Size="Medium" PostBackUrl="~/Default_login.aspx"
                    ForeColor="White">Back</asp:LinkButton>
                
                <asp:LinkButton ID="back" runat="server" Font-Size="Medium" PostBackUrl="~/Default_login.aspx"
                    ForeColor="White">Home</asp:LinkButton>
                
                <asp:LinkButton ID="log" runat="server" Font-Size="Medium" ForeColor="White" OnClick="log_OnClick">Logout</asp:LinkButton>--%>
            </center>
        </div>
        <br />
        <div id="div1" runat="server" style="width: 940px; height: 60px; background-color: -webkit-border-radius: 10px;
            -moz-border-radius: 10px; padding: 10px; margin: 0 auto; background-color:#0CA6CA; ;">
            <table id="First" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lblclg" runat="server" Text="College" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="65px" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" AutoPostBack="true" Width="150px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblyrmon" runat="server" Text="Year and Month" font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Width="130px" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlyear" runat="server" AutoPostBack="true" Width="90px" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlyear_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="true" Width="100px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="50px" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" AutoPostBack="true" Width="100px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlbatch_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldeg" runat="server" Text="Degree" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true"  Width="60"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" AutoPostBack="true" Width="90px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddldegree_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Text="Dept" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="50px" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldept" runat="server" AutoPostBack="true" Width="130px" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddldept_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Sem" font-name="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="50px" ></asp:Label>
                        <asp:DropDownList ID="ddlsem" runat="server" AutoPostBack="true" Width="70px" Font-Names="Book Antiqua"
                            Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlsem_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Width="70px" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubject" runat="server" AutoPostBack="true" Width="110px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="25px" OnSelectedIndexChanged="ddlsubject_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblrepttype" runat="server" Text="Report Type" font-name="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Width="100px" ></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlreptype" runat="server" AutoPostBack="true" Width="100px"
                            Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" Height="28px" OnSelectedIndexChanged="lblrepttype_OnSelectedIndexChanged">
                            <asp:ListItem>Before Evaluation</asp:ListItem>
                            <asp:ListItem>After Evaluation</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        
                        <asp:Button ID="btngo" runat="server" Text="Go" Width="45px" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" OnClick="btngo_OnClick" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    
    <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="800px" ActiveSheetViewIndex="0"
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
                SelectionForeColor="White">
            </FarPoint:SheetView>
        </Sheets>
        <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
            VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
            Font-Strikeout="False" Font-Underline="False">
        </TitleInfo>
    </FarPoint:FpSpread>
    <%--<asp:GridView ID="Passinggrid" runat="server" CellPadding="4" Width="980px" Height="100px"
            Visible="false" CssClass="Dropdown_Txt_Box" HeaderStyle-Font-Size="Medium" Font-Bold="True"
            Font-Names="Book Antiqua" OnRowDataBound="Passinggrid_RowDataBound" ForeColor="#333333">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="serialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'>></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
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
        
        <asp:GridView ID="Passinggrid2" runat="server" CellPadding="4" Width="980px" Height="100px"
            Visible="false" CssClass="Dropdown_Txt_Box" HeaderStyle-Font-Size="Medium" Font-Bold="True"
            Font-Names="Book Antiqua" OnRowDataBound="Passinggrid2_RowDataBound" ForeColor="#333333">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <center>
                            <asp:Label ID="serialno" runat="server" Text='<%#Container.DataItemIndex+1 %>'>></asp:Label>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
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
        </asp:GridView>--%>
    <center>
        <table>
            <td>
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,." InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
                <asp:Button ID="Excel" runat="server" Text="Export Excel" Visible="false" Font-Size="Medium"
                    Font-Bold="true" Font-Names="Book Antiqua" OnClick="Excel_OnClick" />
            </td>
            <td>
                <asp:Button ID="Print" runat="server" Text="Print" Visible="false" Font-Size="Medium"
                    Font-Bold="true" Font-Names="Book Antiqua" OnClick="Print_OnClick" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            </td>
        </table>
    </center>
    <asp:Label ID="lblmsg" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="true"
        Visible="false" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
</asp:Content>

