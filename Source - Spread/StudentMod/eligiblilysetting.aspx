<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/StudentMod/StudentSubSiteMaster.master" CodeFile="eligiblilysetting.aspx.cs" Inherits="StudentMod_eligiblilysetting" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
        #printCommonPdf
        {
        }
        .printclass
        {
            display: none;
        }
        .fontbold
        {}
    </style>
       <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">


        function showhide() {
            var div = document.getElementById("Div3");
            if (div.style.display !== "none") {
                div.style.display = "none";
            }
            else {
                div.style.display = "block";
            }
        }  
        </script>
        

    <script type="text/javascript">
        function myfunction() {
            var first = document.getElementById("DropDownList1").value;

            var answer = +first;
            var foumla = answer;
        }
</script>




         
   

   
   
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <center>
        <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Eligibility Mark
            Seatting </span>
        <div style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" visible="true">
            <table class="maintablestyle" style="height: auto; width: 1096px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="height: 18px; width: 10px"></asp:Label>
                            </td>
                            <td colspan="2">
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"
                             Style="">
                        </asp:DropDownList>
                    </td>
                    <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td class="style64">
                            <asp:DropDownList ID="ddlbatch" CssClass="cursorptr" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" CssClass="cursorptr" runat="server" AutoPostBack="true"
                                Width="100px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbranch" CssClass="cursorptr" runat="server" AutoPostBack="true"
                                 Height="25px" Width="191px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                           
         
                     <td>
                            &nbsp;</td>
                    </td>
                      <td>
                      <asp:Label ID="Label1" runat="server" Text=" Vocational" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                   </td>
                    <td>

                        <asp:RadioButton ID="Radioformat1" runat="server"  AutoPostBack="true"  CssClass="fontbold" GroupName="format"
                            Width="47px" Text="YES"   OnCheckedChanged="Radioformat1_CheckedChanged"
                             />
                    
                        <asp:RadioButton ID="Radioformat2" runat="server" AutoPostBack="true"  CssClass="fontbold"
                            Width="45px" GroupName="format"  Checked="true" OnCheckedChanged="Radioformat2_CheckedChanged"
                            Text="NO" />
                    </td>

                 

                    <td>
                        <asp:Button ID="btnMissingStudent" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            Text="GO" Style="width: auto;
                            height: auto;" onclick="btnMissingStudent_Click" />






                            <asp:Label ID="errormsg" runat="server" Text="Label" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </td>
                   
                    
                          
                    
                </tr>
                </table>
                </div>

        </center>
                  <table>
                        <tr align="right">
                            <td>
                 <asp:Button ID="Button1" runat="server" CssClass="btnapprove1" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Set" Width="99px" 
                                    Style="margin-top: 691px; margin-left: 205px;
                                    position: absolute; border: 2px solid orange; top: 94px; left: 23px; height: 31px;" 
                                    OnClick="btnsave_Click" />
                                <asp:Button ID="Button3" runat="server" CssClass="btnapprove1" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Back" Width="99px " 
                                    
                                    Style="margin-top: 670px; margin-left: 315px;
                                    position: absolute; border: 2px solid orange; top: 125px; left: 23px; height: 24px;"  />
                                    </td>
                        </tr>
                    </table>
                           
        


                            <center>

                                                 

<table id="div" class="maintablestyle" runat="server" visible="false"  style="height: auto; width: auto;">
                <tr>
                    <td>

                                                   <div id="Div2" runat="server" visible="true" style="width: 200px; height: 300px;
                                                background-color: #F8F8F8; margin: 5px; border: 2px lightblue solid; -webkit-border-radius: 10px;
                                                -moz-border-radius: 10px; border-radius: 10px; padding: 10px; margin: 0 auto;
                                                float: left;">
                                                     <asp:GridView ID="grdsubjectDetails" runat="server" AutoGenerateColumns="false" 
                                                           CellPadding="4" CellSpacing="4"  HorizontalAlign="Center" Height="302px" 
                                                           Width="188px"  GridLines="None" onselectedindexchanged="grdsubjectDetails_SelectedIndexChanged"
                                                            >
                <AlternatingRowStyle BackColor="White"   />
                <HeaderStyle BackColor="#507C7D1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"></HeaderStyle>
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:TemplateField>
                     <HeaderTemplate  >   
                     <center>
                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);"  oncheckedchanged="CheckBox1_CheckedChanged"  />  
                            </center>
                        </HeaderTemplate>
                        <ItemTemplate >
                            <asp:CheckBox ID="cbSelect" AutoPostBack="true" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="textval" HeaderText="Subjectes" ItemStyle-Width="30" />

                    
                    
                </Columns>
            </asp:GridView> 
                   
                   
                                                  
                       


                                                    </div>



                                                    </td>
                                                 


                                                    <TD>
                                                    
                                                    <div id="Div1" runat="server" visible="true" style="width: 400px; height: 300px;
                                                background-color: #F8F8F8; margin: 5px; border: 2px lightblue solid; -webkit-border-radius: 10px;
                                                -moz-border-radius: 10px; border-radius: 10px; padding: 10px; margin: 0 auto;
                                                float: left;">


                                              



                                                <div style="width: 400px; height: 300px; overflow-y: scroll; float: left;">


                                                  <asp:Label ID="la"  runat="server" Visible="false" Text="Label"></asp:Label>  


                                                     <asp:GridView ID="GridView1"  runat="server" AutoGenerateColumns="false" 
                                                         CellPadding="4" CellSpacing="4" HorizontalAlign="Center" Height="274px" 
                                                         Width="386px" GridLines="None" >
                <AlternatingRowStyle BackColor="White" />
                <HeaderStyle BackColor="#507C7D1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left"></HeaderStyle>
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:TemplateField>

                     <HeaderTemplate>   
                            <asp:CheckBox ID="chkAllSelect1"  runat="server" onclick="CheckAll1(this);" />  
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbSelect2" runat="server" />
                        </ItemTemplate>

                    </asp:TemplateField>
                 <%--    <asp:TemplateField>--%>
                    <%-- <HeaderTemplate>   
                           
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label  ID="comunity1" runat="server" Text="Label"></asp:Label>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
--%>
                    
                   
                    <asp:BoundField DataField="textval" HeaderText="community" ItemStyle-Width="30" />

                    <asp:TemplateField>

                    <HeaderTemplate>   
                           
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            
                        </ItemTemplate>
                    </asp:TemplateField>


                    
                    
                </Columns>
            </asp:GridView> 



           
                   
                                                             </div>

                                                      </div>

                                                      </TD>
                                                 
                                                         </tr>

                                                      </table>




                                                      

 <div id="droup" runat="server" visible="false" style="width: 100%; margin: 0px; margin-bottom: 10px; margin-top: 10px;" >
 <td>
                                                  
  
                            <asp:DropDownList ID="DropDownList1"  AutoPostBack="true" 
         CssClass="cursorptr" runat="server"
                                Width="100px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px" 
         onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                            </asp:DropDownList>

                            <asp:DropDownList ID="DropDownList2" AutoPostBack="true"
         CssClass="cursorptr" runat="server" 
                                Width="100px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px" 
         onselectedindexchanged="DropDownList2_SelectedIndexChanged">
                                <asp:ListItem Value="0">seleted</asp:ListItem>
                                <asp:ListItem Value="1">+</asp:ListItem>
                                <asp:ListItem Value="2">-</asp:ListItem>
                                <asp:ListItem Value="3">/</asp:ListItem>
                                <asp:ListItem Value="4">*</asp:ListItem>
                                <asp:ListItem Value="5">(</asp:ListItem>
                                <asp:ListItem Value="6">)</asp:ListItem>
                            </asp:DropDownList>


                           

                         

                             <asp:DropDownList ID="DropDownList3" AutoPostBack="true"
         CssClass="cursorptr" runat="server" 
                                Width="100px" Font-Names="Book Antiqua" 
                                Font-Size="Medium" Font-Bold="True" Height="25px" 
         onselectedindexchanged="DropDownList3_SelectedIndexChanged">
                                <asp:ListItem Value="0">seleted</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="7">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="0">0</asp:ListItem>
                            </asp:DropDownList>
                            

                              <asp:TextBox ID="foumla"  runat="server" Visible="false" Width="342px"></asp:TextBox>


                                <asp:TextBox ID="TextBox2" runat="server" Width="342px"></asp:TextBox>

                              <asp:Button ID="Clr" runat="server" Text="Clear"  OnClick="clrfomula" />


                              <br />
                              
                        </td>


    
                               




                        </div>


                                                      

                       
                  


                                
                                                     <%--  <asp:Button ID="Button1"  CssClass="textbox textbox1 commonHeaderFont"  runat="server" Text="Back"   Style="width: auto;
                            height: auto; margin-left:100px" />        <asp:Button ID="btnsave" 
        CssClass="textbox textbox1 commonHeaderFont"  runat="server"
                            Text="Save" Style="width: auto;
                            height: auto; margin-left:100px" onclick="btnsave_Click" />--%>    
                    
                            </td>    
                                                      

</center>
                                               
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
          
                                                                                <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 157px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
  
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                            
                    
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtsubject" runat="server" Width="216px"></asp:TextBox>
                                    
                                    <br />
                                         <asp:TextBox ID="txtcommunity" runat="server" Width="219px"></asp:TextBox>
                                         <br />

                                       



</td>
<tr>
   <center>

                            
                        <asp:Button ID="Button2" CssClass="textbox textbox1 commonHeaderFont" OnClick="btnPopAlertcancel_Click" runat="server"
                            Text="cancle" Style="height: auto; width: auto; height: auto;"/>
                               
                                             <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />


                                           
                                         </td>
                                    </center>
                            </tr>
                        </table>
                    </center>
                </div>
                                                                                    
            </center>
        </div>
    </center>
  


        </asp:Content>
