<%@ Page Title="" Language="C#" MasterPageFile="~/OfficeMOD/OfficeSubSiteMaster.master" AutoEventWireup="true" CodeFile="parents_meet.aspx.cs" Inherits="parents_meet" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <style type="text/css">
        .style1
        {
            height: 238px;
        }
        .font
        {
        }
       
        
    </style>
    <script type="text/javascript">
        function rcity() {

            document.getElementById('<%=btnpointsadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnpointsremove.ClientID%>').style.display = 'block';


        }
        function display() {

            document.getElementById('MainContent_lblerrex').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
   <br /><center>
    <asp:Label ID="Label31" runat="server" Text="Parents Meet " Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Green"></asp:Label></center> <br />
   <center>
    <table style="width:700px; height:70px; background-color:#0CA6CA;">
         <tr>
              <td>
                    <asp:Label ID="lbl_start_date" runat="server" CssClass="font" Font-Bold="true" Font-Names="Book Antiqua"   Font-Size="Medium" Text="From Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbstart_date" runat="server" AutoPostBack="true" CssClass="font"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" OnTextChanged="tbstart_date_OnTextChanged"
                        Width="83px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="tbstart_date">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lbl_end_date" runat="server" CssClass="font" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="To Date"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbend_date" runat="server" AutoPostBack="true" CssClass="font" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" OnTextChanged="tbend_date_OnTextChanged"
                        Width="83px"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="tbend_date">
                    </asp:CalendarExtender>
                </td>
                <td>
                <asp:RadioButton ID="rbvisited" runat="server"  Text="Visited" GroupName="lang" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"  />
                </td>
                <td>
                <asp:RadioButton ID="rbnotvis" runat="server" Text="Not Visited"   GroupName="lang" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium" />
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btngo_click" />
                </td>
            </tr>
      </table>
      </center>
                        
    <asp:Panel ID="panel2" runat="server">
        <br />
        
        <asp:Label ID="lblerroe" runat="server" CssClass="font" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Text="" Visible="false"></asp:Label>
        <br />


         <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                                  
                                     <%-- <FarPoint:FpSpread ID="Fpspread" runat="server" AutoPostBack="false"   BorderColor="Black" CssClass="cur"
                                            BorderStyle="Solid" BorderWidth="1px"   OnButtonCommand="Fpspread1_Command"   OnPreRender="Fpspread1_PreRender" OnCellClick="Fpspread1_CellClick" 
                                            Height="300" Width="623" Visible="False">
                                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                            ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" AutoPostBack="false"  >
                                            </FarPoint:SheetView>
                                        </Sheets>
                                    </FarPoint:FpSpread>--%>

                                      <FarPoint:FpSpread ID="Fpspread" runat="server" AutoPostBack="false" Height="300"  Width="650px" OnButtonCommand="Fpspread1_Command"   OnPreRender="Fpspread1_PreRender" OnCellClick="Fpspread1_CellClick" >
            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                ButtonShadowColor="ControlDark" ButtonType="PushButton" Visible="false">
            </CommandBar>
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" AutoPostBack="false">
                </FarPoint:SheetView>
            </Sheets>
        </FarPoint:FpSpread>
                                      </ContentTemplate>
                    </asp:UpdatePanel>
       
    </asp:Panel>
     <br />
                     
                 
                    <asp:Button ID="btnmeet" runat="server" Text="Action Taken" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnClick="btnmeet_Click" />
                                    <asp:TextBox ID="txtfoc" runat="server" Height="0px" Width="0px" style="Opacity:0;"></asp:TextBox>


              <asp:Panel ID="pnlmsgboxupdate1" runat="server"  CssClass="modalPopup" BorderColor="Black" BorderWidth="1Px" 
          BackColor="#ffffcc" Style="  top:195px; position:absolute; left:250px; width: 650px;  height: 242px;  " >


            

    <table  style=" margin-left: 60px;margin-top: 15px;">
       
        <tr>
            <td>
             <asp:Label ID="lblrepodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Date" ></asp:Label>
            </td>
            <td></td>
            <td>
                <asp:TextBox ID="txtrepodate" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="" Style=" height: 19px;
    width: 119px;">
                                 </asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="txtrepodate">
                    </asp:CalendarExtender>
            </td>
            <td></td>
        
        </tr>

        <tr  >
         <td>
             <asp:Label ID="lblpoints" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Points Discussed" ></asp:Label>
            </td>
            <td>
          
             <asp:Button ID="btnpointsadd" runat="server" Text="+" Style="display: none; top:80px;left:160px; 
                 position:absolute;"
                                                        Font-Names="Book Antiqua" Font-Size="Small" Height="21px" OnClick="btnpointsadd_Click"/> 
                                                   
                                             
                                                    

            </td>
            <td>                                   

                <asp:DropDownList ID="ddlpoints" runat="server" Font-Bold="true" 
                    Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="125px">
                </asp:DropDownList>
                  </td>     
            <td>
            
                <asp:Button ID="btnpointsremove" runat="server" Font-Names="Book Antiqua" 
                    Font-Size="Small" Height="21px" OnClick="btnpointsremove_Click" 
                    Style="display: none; top:80px;left:324px; position:absolute;" Text="-" />
            </td>

        </tr>
        
        <tr>
        <td>
         <asp:Label ID="lblstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Staff"></asp:Label>
        </td>
        <td>
        </td>
        <td>
        <asp:TextBox ID="txtstaff" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="" Style="
                                 height: 20px; width:106px "   
                                 ></asp:TextBox>
                                 <asp:Button ID="btnstaff" runat="server" Text="?" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"  OnClick="btnstaff_click" />
        </td>
        
         
       

        
         <td>
         <asp:TextBox ID="txtstaff_co" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="" Style=" opacity:0;
                                 height: 0;  width:0;"   
                                 ></asp:TextBox>
        </td>
       
        
        </tr>
        <tr>
        <td>
         <asp:Label ID="lblaction" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Action"></asp:Label>
        
        </td>
        <td></td>
        <td>
        <asp:CheckBox ID="d" runat="server" Text="Dismissal" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" /> 
                                          <asp:CheckBox ID="w" runat="server" Text="Warning" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" /> 
                                          <asp:CheckBox ID="s" runat="server" Text="Suspension" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="s_CheckedChanged" /> 

                                          <asp:CheckBox ID="f" runat="server" Text="Fine" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="f_CheckedChanged" AutoPostBack="true" /> 
                                        <asp:TextBox ID="txt_amt" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="" placeholder="Amount" Visible="false"  Style="
                                 height: 18px; width:67px "   
                                 ></asp:TextBox>
                                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txt_amt" FilterType="Numbers"
    ValidChars="" />
                                        </td>
                                        <td></td>

        </tr>
       <br />
       <br />
        <tr>
        <td>

                                        </td>
                                        <td></td>

                                        <td>
                                                 <asp:Button ID="btnok" runat="server" Text="OK" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"  OnClick="btnok_click" /> &nbsp;&nbsp;
                                             <asp:Button ID="btncncl" runat="server" Text="Cancel" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"  OnClick="btncncl_click" />
                                        </td>
                                        <td></td>
        </tr>
          </table>
         
  <table style=" position:absolute; margin-top:-32px; margin-left:335px;">
   <tr>
        <td>  </td>
        <td>

        <asp:Label ID="lblstartdt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="false" Text="Start Date"></asp:Label>
        </td>
        <td>
         <asp:TextBox ID="txtstrdate" runat="server" Font-Bold="True"  AutoPostBack="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=""  Visible="false" OnTextChanged="txtstrdate_OnTextChanged"  Style="
                                 height: 18px; width:67px "   
                                 ></asp:TextBox>
                                   <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="txtstrdate">
                    </asp:CalendarExtender>
        </td>
        <td>
        <asp:Label ID="lblsusdays" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="false" Text="Days"></asp:Label>
          <asp:TextBox ID="txtsusdays" runat="server" Font-Bold="True"  Font-Names="Book Antiqua"
                                Font-Size="Medium" Text=""  Visible="false"  Style="
                                 height: 18px; width:67px; margin-left: 5px; "   
                                 ></asp:TextBox>
        </td>
        </tr>
  </table>
   <table>
        <tr>
        <td> <asp:Label ID="lblmessage" runat="server" Text="" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label></td>
          
        </tr>
       
  </table>

</asp:Panel>

 <asp:Panel ID="panelref" runat="server" Visible="False" Style="width: 200px; 
                                            height: 100px; top:299px; left: 407px; position:absolute;" BorderStyle="Solid" Font-Bold="true"
                                            BorderWidth="1px" BackColor="#CCCCCC" Font-Names="Book Antiqua" Font-Size="Medium">
                                            <center>
                                                <caption runat="server" id="capref" style="height: 16px; top: 16px; font-weight:bold; font-variant: Medium-caps">
                                                </caption>
                                                <br />
                                                 <asp:TextBox ID="txt_ref" Width="151px" Height="18px" runat="server"></asp:TextBox>
                                          
                                                  </center>
                                    
                                    <table>
                                    <tr>
                                    <td>
                                                  </td>   
                                           </tr>
                                           <tr>
                                           <td>
                                              &nbsp;&nbsp;&nbsp;  &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; 
                                               <asp:Button ID="btnaddref" Width="50px" runat="server" Text="Add" OnClick="btnaddref_Click" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                                    </td>
                                                    <td>
                                                &nbsp;
                                                <asp:Button ID="btnexitref" Width="50px" runat="server" Text="Exit" OnClick="btnexitref_Click" Font-Bold="true"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" />
                                                    </td>
                                                    </tr>
                                          </table>
                                        </asp:Panel>

   <asp:Panel ID="panel8" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false" BorderWidth="2px"
            style="background-color: AliceBlue; border-color: Black; border-width: 2px; border-style: solid; position: fixed; width: 520px; height: 440px; left: 250px; top: 99px;">
            <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                font-size: Small; font-weight: bold">
                <br />
                <asp:Label ID="Label19" runat="server" Text=" Staff List" Style="width: 150px; position: absolute;
                    left: 166px; top: 4px;"></asp:Label>
                <%-- <caption style="top: 20px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Staff List
                    </caption>--%>
                <br />
                <br />
               <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                    <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="College" Style="width: 150px; position: absolute;
                                left: -41px; top: 30px;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" Style="width: 150px;
                                position: absolute; left: 70px; top: 30px;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDepartment" runat="server" Text="Department" Style="width: 150px;
                                position: absolute; left: 237px; top: 30px;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldepratstaff" runat="server" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                Style="width: 150px; position: absolute; left: 360px; top: 30px;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label20" runat="server" Text="Staff Type" Style="width: 150px; position: absolute;
                                left: -41px; top: 65px;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_stftype" runat="server" Width="150px" OnSelectedIndexChanged="ddl_stftype_SelectedIndexChanged"
                                AutoPostBack="true" Style="width: 150px; position: absolute; left: 70px; top: 65px;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label21" runat="server" Text="Designation" Style="width: 150px; position: absolute;
                                left: 237px; top: 65px;"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_design" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddl_design_SelectedIndexChanged" Style="width: 150px;
                                position: absolute; left: 360px; top: 65px;">
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
                <div style="width: 510px; position: absolute; top: 95px;">
                    <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                        Width="510" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                        OnUpdateCommand="fsstaff_UpdateCommand" OnCellClick="fsstaff_CellClick">
                        <CommandBar BackColor="Control" ButtonType="PushButton">
                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
                </ContentTemplate>
                    </asp:UpdatePanel>
                <fieldset style="width: 160px; position: absolute;  padding: 4px 1em 19px; left: 328px; height: 9px; top: 388px;">
                    <asp:Button runat="server" ID="btnstaffadd" OnClick="btnstaffadd_Click" Width="75px" />
                    <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px" />
                </fieldset>
        </asp:Panel>  
</asp:Content>

