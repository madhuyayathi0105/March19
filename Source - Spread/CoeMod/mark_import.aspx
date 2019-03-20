<%@ Page Title="" Language="C#" MasterPageFile="~/CoeMod/COESubSiteMaster.master" AutoEventWireup="true" CodeFile="mark_import.aspx.cs" Inherits="mark_import" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
  <html>
  <title></title>
<script type ="text/javascript" >

    function Fp_Marks_ActiveCellChanged(event) {

        var spread = document.getElementById('<%=Fp_Marks.ClientID%>');
        var activeRow = spread.GetActiveRow();
        var activeCol = spread.GetActiveCol();
        var value = spread.GetValue(activeRow, activeCol);
        var note = spread.GetCellByRowCol(activeRow, activeCol);
        var notevar = note.title;
        var marks = notevar.split('-'); //Get Header like I,E,T
        var head = marks[0];
        var minintmark = "";
        var minextmark = "";
        var mintotal = "";
        var intcol = "";
        var extcol = "";
        var totacol = "";
        var resultcol = "";

        if (activeCol >= 4) {
            var intmark = 0;
            var extmark = 0;
            var totmark = 0;
            if (head == "I") {
                intcol = parseInt(activeCol);
                extcol = parseInt(activeCol) + parseInt(1);
                totacol = parseInt(activeCol) + parseInt(2);
                resultcol = parseInt(activeCol) + parseInt(3);
            }
            if (head == "E") {
                intcol = parseInt(activeCol) - parseInt(1);
                extcol = parseInt(activeCol);
                totacol = parseInt(activeCol) + parseInt(1);
                resultcol = parseInt(activeCol) + parseInt(2);
            }
            if (head == "T") {
                intcol = parseInt(activeCol) - parseInt(2);
                extcol = parseInt(activeCol) - parseInt(1);
                totacol = parseInt(activeCol);
                resultcol = parseInt(activeCol) + parseInt(1);
            }
            intmark = spread.GetValue(activeRow, intcol);
            extmark = spread.GetValue(activeRow, extcol);
            totmark = parseFloat(intmark) + parseFloat(extmark);
            spread.SetValue(activeRow, totacol, totmark, true); //Set Total
            if (intmark == -1 && extmark > -1) {
                spread.SetValue(activeRow, totacol, extmark, true);
            } else if (extmark == -1 && intmark > -1) {
                spread.SetValue(activeRow, totacol, intmark, true);
            } else if (extmark == -1 && intmark == -1) {
                spread.SetValue(activeRow, totacol, 0, true);
            }


            var resultnote = spread.GetCellByRowCol(activeRow, resultcol); //Get Minimum Marks From Result Header Note.
            var result = resultnote.title;
            var minimummark = result.split('-');
            minintmark = minimummark[1];
            minextmark = minimummark[2];
            mintotal = minimummark[3];



            //Set Pass or Fail
            var cell = spread.GetCellByRowCol(activeRow, resultcol);
            cell.removeAttribute("FpCellType");
            cell.setAttribute("disabled", "false");
            if (parseFloat(intmark) != -1 && parseFloat(extmark) != -1) {
                if (parseFloat(intmark) >= parseFloat(minintmark) && parseFloat(extmark) >= parseFloat(minextmark) && parseFloat(totmark) >= parseFloat(mintotal)) {

                    spread.SetValue(activeRow, resultcol, "Pass", true);
                }
                else {
                    spread.SetValue(activeRow, resultcol, "Fail", true);
                }
            } else {
                spread.SetValue(activeRow, resultcol, "AAA", true);
            }
            cell.setAttribute("FpCellType", "readonly");
        }
    }
</script>
<body>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
    <asp:Label ID="Label2" runat="server" Text=" Marks Import" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>

    <br />
    <center>
    <table style="width:700px; height:70px; background-color:#0CA6CA;">
    <tr>
    <td>
        <asp:Label ID="lblbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Batch"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_batch" runat="server" AutoPostBack="true"
            onselectedindexchanged="ddl_batch_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua">
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
        <asp:DropDownList ID="ddl_dept" runat="server" AutoPostBack="true" Width="120px"
            onselectedindexchanged="ddl_dept_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua"> 
        </asp:DropDownList>
       </td>
       <td>
   
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Exam Month"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_exmonth" runat="server" AutoPostBack="true" Width="90px"
            onselectedindexchanged="ddl_exmonth_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua">
        </asp:DropDownList>
       </td> 
  <td>
        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Exam Year" Width="90px"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_exyear" runat="server" AutoPostBack="true" Width="55px"
        onselectedindexchanged="ddl_exyear_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua">
            
        </asp:DropDownList>
        </td>
       </tr>
       <tr> 
    <td>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Type"></asp:Label>
</td>
<td>
        <asp:DropDownList ID="ddl_operation" runat="server" AutoPostBack="true" Width="75px"
        onselectedindexchanged="ddl_operation_SelectedIndexChanged"  Font-Bold="True" Font-Names="Book Antiqua">        
        <asp:ListItem>Import</asp:ListItem>
        <asp:ListItem>Entry</asp:ListItem>
        </asp:DropDownList>
   </td>
   <td>

        <asp:DropDownList ID="ddl_type" runat="server"  Width="75px"  Font-Bold="True" Font-Names="Book Antiqua" >
           
               
        <asp:ListItem>Mark</asp:ListItem>
        <asp:ListItem>Grade</asp:ListItem>
        </asp:DropDownList>
   </td>
   <td>
        <asp:Label ID="lbl_choose" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Visible="false" Text="Select File"></asp:Label>
</td>
<td>
         <asp:FileUpload ID="File_Upload" ForeColor="Red" Visible="false" runat="server" />
   </td>
   <td>
        <asp:Button ID="Btn_go" runat="server" Text="Go" Width="100px" Font-Names="Book Antiqua"
            Font-Size="Medium" onclick="Btn_go_Click" />
   </td>
   <td>
        <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" onclick="LinkButton3_Click" Width="125px">Header Settings</asp:LinkButton>
            </td>
</tr>    
     </table>  

        </center>
   
    <asp:Label ID="lbl_msg" runat="server"  Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" ></asp:Label>
   

   <div style="width: 1000px; position: absolute; top: 300px;">
  <FarPoint:FpSpread ID="Fp_Marks" runat="server"  
                                     Width="970" Height="200" Visible="False">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false"
                                        ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" 
                                        ButtonType="PushButton" ShowPDFButton="True" >
                                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView AutoPostBack="false" SheetName="Sheet1" Visible="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                    <ClientEvents EditStopped="Fp_Marks_ActiveCellChanged"  />
                                    <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" 
                                        HorizontalAlign="Center" VerticalAlign="NotSet">
                                    </TitleInfo>
                                </FarPoint:FpSpread>
                                 
                 <FarPoint:FpSpread ID="Fp_Grade" runat="server"  
                                     Width="970" Height="80" Visible="False" oncellclick="Fp_Grade_CellClick" OnUpdateCommand="Fp_Grade_UpdateCommand">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false"
                                        ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark" 
                                        ButtonType="PushButton" ShowPDFButton="True" >
                                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView AutoPostBack="false" SheetName="Sheet1" Visible="true">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                    
                                    <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" 
                                        HorizontalAlign="Center" VerticalAlign="NotSet">
                                    </TitleInfo>
                                </FarPoint:FpSpread>

    <div style="height:30px;"></div>
     <div style="width: 1000px; position: absolute; left: 438px;">
        <asp:Button ID="Btn_save" runat="server" OnClick="Btn_save_Click" Text="Save" />
        <asp:Button ID="Btn_Delete" runat="server" Text="Delete" 
             onclick="Btn_Delete_Click" Visible="False" />
    </div>
    </div>
   
                                  
   


    <div style="position: absolute; margin-top: 220px; left: 222px;">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel3" runat="server" Visible="false" CssClass="modalPopup" Style=" background-color:lightgray; border-style:solid; border-width:1px; height:700; width:700;">
        <div>
        <table>
        <tr>

        <td>
             <FarPoint:FpSpread ID="FpSpread1" runat="server" oncellclick="FpSpread1_CellClick">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton" >
                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                    </FarPoint:SheetView>
                </Sheets>                
                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                    Font-Size="X-Large">
                </TitleInfo>
            </FarPoint:FpSpread>
        </td>
        <td>
        <div>
            <asp:Button ID="Btn_Move" runat="server" Text=">" OnClick="Btn_Move_Click" 
                Height="27px" Width="29px"/>
         </div>
         <div style="height:10px;"></div>
         <div>
            <asp:Button ID="Btn_Moveall" runat="server" Text=">>" OnClick="Btn_Moveall_Click"/>
            </div>
            <div style="height:10px;"></div>
            <div>
            <asp:Button ID="Btn_Remove" runat="server" Text="<" OnClick="Btn_Remove_Click"/>
         </div>
         <div style="height:10px;"></div>
         <div>
            <asp:Button ID="Btn_Removeall" runat="server" Text="<<" 
                 OnClick="Btn_Removeall_Click" Height="25px" Width="22px"/>
            </div>
        </td>
        <td>
        <FarPoint:FpSpread ID="FpSpread2" runat="server" oncellclick="FpSpread2_CellClick">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark" ButtonType="PushButton" >
                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                    </FarPoint:SheetView>
                </Sheets>
                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                    Font-Size="X-Large">
                </TitleInfo>
            </FarPoint:FpSpread>
        </td>
        
        </tr>
        </table>
        </div>

        <div style="margin-left:125px;">
            <asp:Button ID="Btn_ok" runat="server" Text="Ok" Width="75px" OnClick="Btn_ok_Click" />
            <asp:Button ID="Btn_cancel" runat="server" Text="Cancel" Width="75px" OnClick="Btn_cancel_Click" />
        </div>

        </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
 </body>
     </html>
</asp:Content>

