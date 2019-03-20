<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="Student_Academic_record.aspx.cs" Inherits="Student_Academic_record" %>


<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
 <style type="text/css">
        .style11
        {
            width: 68px;
            height: 2px;
        }
        .style14
        {
            height: 2px;
            width: 73px;
        }
        .style33
        {
            height: 2px;
            width: 65px;
        }
        .style34
        {
            height: 2px;
        }
        .style35
        {
            height: 2px;
            width: 138px;
        }
        .style36
        {
            height: 2px;
            width: 54px;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .style37
        {
           
            
            height: 21px;
            width: 249px;
        }
    </style>
  
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_lblerr').innerHTML = "";

            }
        </script>
       
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
           
           <br />
                
                <center>
                <asp:Label ID="Label1" runat="server" Text=" Student Academic Record " Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
                
                </center>
                <br />
             
        </div>
    
            
           <center> 
            
            
                <table style="width:700px; height:70px; background-color:#0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Style=" "></asp:Label>
                                </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="100px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                AutoPostBack="True" Style=" ">
                            </asp:DropDownList>
                        </td>

                        <td >
                            <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                               "></asp:Label>
                            
                        </td>
                        <td>
                            
                            <asp:DropDownList ID="ddlBatch" runat="server" Height="21px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                Style=" "
                                Width="71px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            
                        </td>
                        <td >
                                <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Style="
                                    ">
                                </asp:Label>
                        </td>
                        <td >
                            
                            
                            <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Style="
                                  " Width="93px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            
                        </td>
                        <td >
                            <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                                "></asp:Label>
                        </td>
                        <td >
                            
                            
                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" Style="
                                 width: 200px;"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            
                            
                        </td>
                        <td >
                            
                            
                            <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style="
                               "></asp:Label>
                            
                            <asp:LinkButton ID="LinkButton4" runat="server" BackColor="White" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Small" ForeColor="Blue" OnClick="LinkButton4_Click"
                                Style="height: 17px; width: 161px;
                                bottom: 331px;" Enabled="False" Visible="False">OverAll Best Performance</asp:LinkButton>
                            
                            
                        </td>
                        <td >
                            
                            
                            <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged" Style="
                                   width: 48px;"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td >
                            <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                Font-Names="Book Antiqua" Style=" 
                                height: 21px; width: 27px"></asp:Label>
                        </td>
                        <td>
                            
                            <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                                Style="
                                width: 42px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            
                        </td>
                        </tr>
                    <tr>
                        <td >
                            <asp:Label ID="lblExamMonth" runat="server" Text="ExamMonth" Font-Bold="True" Font-Size="Medium"
            Style="  "
            Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td >
                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" Font-Bold="True"
            Font-Names="Book Antiqua" Style=" 
            width: 60px;" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
        </asp:DropDownList>
                        </td>
                        <td >
                            
                            <asp:Label ID="lblExamYear" runat="server" Text="ExamYear" Style="
            " Font-Bold="True" Font-Size="Medium"
            Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Font-Bold="True"
            Style=" width: 60px;" Font-Names="Book Antiqua"
            Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
        </asp:DropDownList>
                        </td>
                        <td>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Style="text-align: center;
            "
            Text="Go" Width="40px" Height="28px" Font-Bold="True" Visible="true" Font-Names="Book Antiqua"
            Font-Size="Medium" />
                        </td>

                      
                        <td>
                          <asp:Label ID="lblerror" runat="server" Text="Label" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Style="color: Red;  "></asp:Label>
        
                        </td>

                    </tr>
                </table>
                </center>
                <center>
                    <asp:Panel ID="newpnl" runat="server" Style="">
                        <table>
                            <tr>
                                <td>
                                    <center>
                                        
                                        <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="1px" Height="600" Width="600" Visible="true" HorizontalScrollBarPolicy="Never"
                                            VerticalScrollBarPolicy="Never">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark" ButtonType="PushButton">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </center>
                                    <%--hai--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px" />
                                    <asp:Button ID="BtnPrint" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        OnClick="btnprintmaster_Click" Font-Size="Medium" Text="Print" Width="127px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblerr" runat="server" Text="" Visible="false" ForeColor="Red" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
           
            <%-- Hidden by srinath 12/3/2014
                <asp:Image ID="Image1" runat="server" 
            style="top: 373px; left: 229px;  height: 16px; width: 14px" />--%>
           
            
           
            
            <asp:Button ID="Button2" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="Button2_Click" Style=" 
                height: 33px; width: 54px" Enabled="False" Visible="False" />
            
            
            
            
            <%-- <asp:Label ID="lblnospread" runat="server" Text="No Records Found" 
            Visible="False" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" 
            Font-Size="Medium" 
            style="top: 330px; left: 7px;  height: 20px; width: 215px" ></asp:Label>--%>
            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="#FF3300" Style="" Visible="False"
                CssClass="style37"></asp:Label>
            
            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                Style=" "></asp:Label>
            
            <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="
                 "></asp:Label>
            
            <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                Height="24px" Width="58px" Style=" ">
            </asp:DropDownList>
            
            <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                FilterType="Numbers" />
            
            <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search" Visible="False"
               Font-Names="Book Antiqua" Font-Size="Medium" Style="" ></asp:Label>
            
            <asp:TextBox ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True"
                OnTextChanged="TextBoxpage_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Height="17px" Style=" 
                width: 34px;"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                FilterType="Numbers" />
            
            <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="
                 "></asp:Label>
            
            <%-- <asp:Button ID="Button4" runat="server" 
            style="top: 166px; left: 901px;  height: 23px; width: 56px" 
            Text="Button" />--%>
            
            
            
            
            
            <%-- </form>--%>
            <asp:Panel ID="Panel5" runat="server">
                <center>
                    <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Height="600" Width="600" HorizontalScrollBarPolicy="Never"
                        VerticalScrollBarPolicy="Never" Visible="false" Style="
                        height: 600px; ">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonType="PushButton" ShowPDFButton="True" ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            </asp:Panel>
            <center>
                <FarPoint:FpSpread ID="FpEntry" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="1px" Width="900px" Style="height: 167px;
                    " Visible="False">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" AllowSort="true">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                
            </center>
    </body>
    </html>
</asp:Content>

