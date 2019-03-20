<%@ Page Title="Studentwise Consolidate Report" Language="C#" AutoEventWireup="true" CodeFile="StudentwiseConsolidateReport.aspx.cs"
    Inherits="CoeMod_StudentwiseConsolidateReport" MasterPageFile="~/CoeMod/COESubSiteMaster.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
    <asp:Label ID="Label2" runat="server" Text=" Studentwise Consolidate Report" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
     </center>
    <br />
    <center>
    <table style="width:700px; height:70px; background-color:#0CA6CA;">
    <tr>
    <td>
        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="College"></asp:Label>
    </td>
    <td>
        <asp:DropDownList ID="ddl_college" runat="server" AutoPostBack="true"
          onselectedindexchanged="ddl_college_SelectedIndexChanged"   Font-Bold="True" Font-Names="Book Antiqua">
        </asp:DropDownList> 
       </td>
    <td>
        <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Batch"></asp:Label>
    </td>
    <td>
        <asp:DropDownList ID="ddl_batch" runat="server" AutoPostBack="true"
          onselectedindexchanged="ddl_batch_SelectedIndexChanged"   Font-Bold="True" Font-Names="Book Antiqua">
        </asp:DropDownList>   
       </td>
       <td>
        <asp:Label ID="lbldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
       Font-Size="Medium" Text="Degree"></asp:Label>
</td> 
   <td>
        <asp:DropDownList ID="ddl_degree" runat="server" AutoPostBack="true"
               onselectedindexchanged="ddl_degree_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua">
        </asp:DropDownList>       
        </td>
  <td>
        <asp:Label ID="lblcourse" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Branch"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_branch" runat="server" AutoPostBack="true" Width="120px"
          onselectedindexchanged="ddl_branch_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua"> 
        </asp:DropDownList>  
       </td>
       </tr>
       <tr>
        <td>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Section"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_section" runat="server" AutoPostBack="true" Width="120px"
            Font-Bold="True" Font-Names="Book Antiqua"> 
        </asp:DropDownList> <%-- onselectedindexchanged="ddl_section_SelectedIndexChanged"--%>
       </td>
        <td>
        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Exam Year" Width="90px"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_exyear" runat="server" AutoPostBack="true" Width="55px"
       onselectedindexchanged="ddl_exyear_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
            
        </asp:DropDownList> 
        </td>
       <td>
   
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Exam Month"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_exmonth" runat="server" AutoPostBack="true" Width="90px"
            Font-Bold="True" Font-Names="Book Antiqua">
        </asp:DropDownList> 
       </td> 
 
      
       
       <td>
         
        <asp:Button ID="Btn_go" runat="server" Text="Go" Width="100px" Font-Names="Book Antiqua"
          onclick="Btn_go_Click"   Font-Size="Medium" />
          </td>
          <td>
            <asp:Button ID="Btn_print" runat="server" Text="Print" Width="100px" Font-Names="Book Antiqua"
            Font-Size="Medium" onclick="Btn_print_Click"/>
   </td>
      
       </tr>
       </table>
       </center>
    <div style="margin-top: 25px">
        <center>
            <div id="divgrid" visible="false" runat="server">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>' Visible="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAll_Checked"  />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="gridcb" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No">
                            <ItemTemplate>
                                <asp:Label ID="lblgridrollno" runat="server" Text='<%# Bind("Roll_No") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reg No">
                            <ItemTemplate>
                                <asp:Label ID="lblgridregno" runat="server" Text='<%# Bind("Reg_No") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                      
                        <asp:TemplateField HeaderText="Admission No">
                            <ItemTemplate>
                                <asp:Label ID="lblgridadmissionno" runat="server" Text='<%# Bind("Roll_Admit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                             <asp:TemplateField HeaderText="Student Type">
                            <ItemTemplate>
                                <asp:Label ID="lblgridstudenttype" runat="server" Text='<%# Bind("Stud_Type") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="Lblgridname" runat="server" Text='<%# Bind("Stud_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="250px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
            
            </div>
        </center>
    </div>
    <div>
      <center>
            <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                left: 0%;">
                <center>
                    <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
                                        <center>
                                            <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
                </div>
                  </center>
            </div>
    <asp:Label ID="lbl_msg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
        Font-Size="Medium" ForeColor="Red"></asp:Label>
</asp:Content>
