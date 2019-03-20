<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Input_Events.ascx.cs" Inherits="Usercontrols_Input_Events" %>
<link href="../Styles/Input_Events.css" rel="stylesheet" type="text/css" />

<%--<link href="../styles/Input_Events.css" rel="stylesheet" type="text/css" />
--%>
<div class="body">
<script>
    $(document).ready(function () {
        $('#tabs div').hide();
        $('#tabs div:first').show();
        $('#tabs ul li:first').addClass('active');

        $('#tabs ul li a').click(function () {
            $('#tabs ul li').removeClass('active');
            $(this).parent().addClass('active');
            var currentTab = $(this).attr('href');
            $('#tabs div').hide();
            $(currentTab).show();
            return false;
        });
    });
</script>

<div id="tabs-1">
    <div class="top-empty">
    </div>
    <div class="input-events">
        <table style=" background-color:lightblue; border-color:Black; border-width:1px; border-style:solid;">
            <tr>
               
                <td class="coll-lbl">
                    <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="22px" Width="55px"></asp:Label>
                </td>
                <td class="coll-ddl">
                    <asp:DropDownList ID="ddlcollege" runat="server"  Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" style=" text-align:left" 
                        Height="22px" Width="100px">
                    </asp:DropDownList>
                </td>
                <td class="batch-lbl">
                    <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Height="22px" Width="40px"></asp:Label>
                </td>
                <td class="batch-ddl">
                    <asp:DropDownList ID="ddlBatch"  runat="server" AutoPostBack="true"  Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"  OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" Height="22px"
                        Width="70px" >
                    </asp:DropDownList>
                </td>
                <td class="degree-lbl">
                    <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Height="22px" Width="55px">
                    </asp:Label>
                </td>
                <td class="degree-ddl">
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="22px"
                        Width="70px" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td class="branch-lbl">
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Height="22px" Width="55px"></asp:Label>
                </td>
                <td class="branch-ddl">
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                        Height="22px" Width="250px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td class="sem-lbl">
                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Height="22px" Width="34px"></asp:Label>
                </td>
                <td class="sem-ddl">
                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged"
                        Height="22px" Width="50px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td class="sec-lbl">
                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua" Height="22px" Width="26px"></asp:Label>
                </td>
                <td class="sem-ddl">
                    <asp:DropDownList ID="ddlSec" runat="server" Height="22px" Width="50px" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlSec_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
   
</div>
